using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class MessageSender
    {
        public float IkSensorLeft { get; set; }
        public float IkSensorRight { get; set; }
        public float UzSensorLeft { get; set; }
        public float UzSensorLeftSide { get; set; }
        public float UzSensorForward { get; set; }
        public float UzSensorRightSide { get; set; }
        public float UzSensorRight { get; set; }
        public bool IsStart { get; set; }

        public MessageSender(float ikSensorLeft, float ikSensorRight, float uzSensorLeft, float uzSensorLeftSide, float uzSensorForward,
            float uzSensorRightSide, float uzSensorRight, bool isStart)
        {
            IkSensorLeft = ikSensorLeft;
            IkSensorRight = ikSensorRight;
            UzSensorLeft = uzSensorLeft;
            UzSensorLeftSide = uzSensorLeftSide;
            UzSensorForward = uzSensorForward;
            UzSensorRightSide = uzSensorRightSide;
            UzSensorRight = uzSensorRight;
            IsStart = isStart;
        }
    }
}
