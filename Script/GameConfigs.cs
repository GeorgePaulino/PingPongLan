using System;
using Microsoft.Xna.Framework.Input;

namespace PingPong{
    public static class Utilities
    {
        public static Keys[][] keys = new Keys[][]{ new Keys[]{Keys.Up, Keys.Down}, new Keys[]{Keys.W, Keys.S}};
        public static int BaseVelocity = 200;
        public static Random Random => new Random(Guid.NewGuid().GetHashCode());
        public static int[] ScreenBounds = new int[]{800, 500};
    }
}