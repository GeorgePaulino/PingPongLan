using System.Numerics;
using Lidgren.Network;
namespace PingPong
{
    public class ClientMessage
    {
        public bool up;
        public bool down;
    }
    public class MsgData
    {
        //public Vector2[] paddlePos = new Vector2[2];
        public float pad1X;
        public float pad1Y;
        public float pad2X;
        public float pad2Y;
        public float ballX;
        public float ballY;
        public int score1;
        public int score2;
        public int win;
    }
}