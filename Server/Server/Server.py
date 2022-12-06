import asyncio
import threading
import time
import json
import websockets

p = i = d = speed = time_ = error = error_old = power_l = power_r = steer = 0
min_forward_distance = 50
min_side_distance = 20
data_from_client = ''
data_from_server = ''
is_already_moving = False
is_already_revers = False

def get_data_from_client():
    global data_from_client
    with open('data_from_client.json', 'r') as f:
        data_from_client = json.load(f)

def get_data_from_server():
    global data_from_server
    with open('data_from_server.json', 'r') as f:
        data_from_server = json.load(f)

def get_params():
    with open('start_params.json', 'r') as f:
        obj = json.load(f)

    global p, i, d, speed, time_
    p = obj['p']
    i = obj['i']
    d = obj['d']
    speed = obj['speed']
    time_ = obj['time']

def pid_controller(ik_left, ik_right):
    global p, i, d, speed, error, error_old, steer, power_l, power_r
    error = ik_left - ik_right
    steer = steer + error
    if(steer < -0.1):
        steer = -0.1
    else:
        steer = 0.1

    power_l = (speed - (p * error + d * (error - error_old) + i * steer))
    power_r = (speed + (p * error + d * (error - error_old) + i * steer))

    error_old = error

def no_stop():
    global data_from_client
    time.sleep(4.0)
    data_from_client['IsStop'] = False
    
def stop(stop_time):
    global data_from_client
    data_from_client['IsBrake'] = True
    time.sleep(stop_time)
    data_from_client['IsBrake'] = False
    data_from_client['IsMoving'] = True

def stop_moving():
    global data_from_client, is_already_moving
    time.sleep(3)
    data_from_client["IsMoving"] = False
    is_already_moving = False

def stop_revers():
    global data_from_client, is_already_revers
    time.sleep(2)
    data_from_client['IsReversing'] = False
    is_already_revers = False

def maze_move():
    global data_from_client, power_l, power_r
    
    if(data_from_client['IsStart']):
        if(data_from_client['UZSensorForward'] < 5 and not(data_from_client['IsReversing'])):
            data_from_client['IsReversing'] = True
            power_l = -700
            power_r = -700
        elif(not(data_from_client['IsReversing']) and (data_from_client['UZSensorForward'] < 15 or data_from_client['UZSensorLeftSide'] < 10 or 
                                           data_from_client['UZSensorRightSide'] < 10 or data_from_client['UZSensorLeft'] < 5 or
                                           data_from_client['UZSensorRight'] < 5)):
            if(not(data_from_client['IsStop']) and not(data_from_client['IsMoving'])):
                data_from_client['IsStop'] = True
                threading.Thread(target=no_stop, args=()).start()
                threading.Thread(target=stop, args=(1.5,)).start()
    
        if(not(data_from_client['IsReversing'])):
            if(data_from_client['UZSensorForward'] > min_forward_distance):
                if(data_from_client['UZSensorLeftSide'] < min_side_distance or data_from_client['UZSensorRightSide'] < min_side_distance):
                    pid_controller(data_from_client['UZSensorLeftSide'], data_from_client['UZSensorRightSide'])
                else:
                    pid_controller(0, 0)
            elif(data_from_client['UZSensorLeft'] > data_from_client['UZSensorRight'] or data_from_client['UZSensorLeft'] < data_from_client['UZSensorRight']):
                if(data_from_client['UZSensorLeftSide'] < min_side_distance or data_from_client['UZSensorRightSide'] < min_side_distance):
                    pid_controller(data_from_client['UZSensorLeftSide'], data_from_client['UZSensorRightSide'])
                else:
                    pid_controller(data_from_client['UZSensorLeft'], data_from_client['UZSensorRight'])
            else:
                pid_controller(data_from_client['UZSensorLeft'], data_from_client['UZSensorRight'])

            if(data_from_client['UZSensorLeftSide'] < min_side_distance or data_from_client['UZSensorRightSide'] < min_side_distance):
                pid_controller(data_from_client['UZSensorLeftSide'], data_from_client['UZSensorRightSide'])

def cheker(data):
    if(data['IKSensorLeft'] != 0.0 or data['IKSensorRight'] != 0.0):
        pid_controller(data['IKSensorLeft'], data['IKSensorRight'])
    elif(data['UZSensorLeft'] != 0.0):
        maze_move()

def allow_data(msg):
    global data_from_client
    d = json.loads(msg)
    data_from_client['IKSensorLeft'] = d['IKSensorLeft']
    data_from_client['IKSensorRight'] = d['IKSensorRight']
    data_from_client['UZSensorLeft'] = d['UZSensorLeft']
    data_from_client['UZSensorLeftSide'] = d['UZSensorLeftSide']
    data_from_client['UZSensorForward'] = d['UZSensorForward']
    data_from_client['UZSensorRightSide'] = d['UZSensorRightSide']
    data_from_client['UZSensorRight'] = d['UZSensorRight']
    data_from_client['IsStart'] = d['IsStart']

async def main(websocket):
    global data_from_client, data_from_server, power_l, power_r, is_already_moving, is_already_revers
    get_params()
    get_data_from_server()
    get_data_from_client()
    while True:
        msg = await websocket.recv()
        allow_data(msg)
        cheker(data_from_client)
        if(data_from_client['IsMoving'] and not(is_already_moving)):
            is_already_moving = True
            threading.Thread(target=stop_moving, args=()).start()

        if(not(data_from_client['IsStop']) and data_from_client['IsReversing'] and not(is_already_revers)):
            is_already_revers = True
            threading.Thread(target=stop_revers, args=()).start()

        data_from_server['IsBrake'] = data_from_client['IsBrake']
        data_from_server['powerL'] = power_l
        data_from_server['powerR'] = power_r
        await websocket.send(json.dumps(data_from_server))
        print(threading.active_count())

start_server = websockets.serve(main, 'localhost', 8080)

asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever()
