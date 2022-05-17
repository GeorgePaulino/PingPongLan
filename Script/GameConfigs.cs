using System;
using Microsoft.Xna.Framework.Input;

namespace PingPong{
    public static class Utilities
    {
        public static Keys[][] keys = new Keys[][]{ new Keys[]{Keys.W, Keys.S}, new Keys[]{Keys.Up, Keys.Down}};
        public static int baseVelocity = 200;
        public static Random random => new Random(Guid.NewGuid().GetHashCode());
        public static int[] screenBounds = new int[]{800, 500};
    }
}