import asyncio
import websockets
import json

p = i = d = speed = time = error = error_old = power_l = power_r = steer = 0

def get_params():
    with open('start_params.json', 'r') as f:
        obj = json.load(f)

    global p, i, d, speed, time
    p = obj['p']
    i = obj['i']
    d = obj['d']
    speed = obj['speed']
    time = obj['time']

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
    
def cheker(data):
    if(data['IKSensorLeft'] != 0.0 or data['IKSensorRight'] != 0.0):
        pid_controller(data['IKSensorLeft'], data['IKSensorRight'])



async def main(websocket):
    get_params()
    while True:
        msg = await websocket.recv()
        data = json.loads(msg)
        cheker(data)
        await websocket.send(f"{power_l} {power_r}")
        print(data)



start_server = websockets.serve(main, 'localhost', 8080)

asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever()