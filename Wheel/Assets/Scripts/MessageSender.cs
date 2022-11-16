using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class MessageSender
    {
        public float IKSensorLeft { get; set; }
        public float IKSensorRight { get; set; }
        public float UZSensorLeft { get; set; }
        public float UZSensorLeftSide { get; set; }
        public float UZSensorForward { get; set; }
        public float UZSensorRightSide { get; set; }
        public float UZSensorRight { get; set; }
        public bool IsStart { get; set; }

        public MessageSender(float ikSensorLeft, float ikSensorRight, float uzSensorLeft, float uzSensorLeftSide, float uzSensorForward,
            float uzSensorRightSide, float uzSensorRight, bool isStart)
        {
            IKSensorLeft = ikSensorLeft;
            IKSensorRight = ikSensorRight;
            UZSensorLeft = uzSensorLeft;
            UZSensorLeftSide = uzSensorLeftSide;
            UZSensorForward = uzSensorForward;
            UZSensorRightSide = uzSensorRightSide;
            UZSensorRight = uzSensorRight;
            IsStart = isStart;
        }
    }
}
