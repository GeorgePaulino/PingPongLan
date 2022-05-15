using System;
using Microsoft.Xna.Framework.Input;

namespace PingPong{
    public static class Utilities
    {
        public static Keys[][] keys = new Keys[][]{ new Keys[]{Keys.W, Keys.S}, new Keys[]{Keys.Up, Keys.Down}};
        public static int BaseVelocity = 200;
        public static Random Random => new Random(Guid.NewGuid().GetHashCode());
        public static int[] ScreenBounds = new int[]{800, 500};
    }
}