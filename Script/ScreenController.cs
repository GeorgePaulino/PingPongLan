using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Microsoft.Xna.Framework;

namespace PingPong
{
    public static class ScreenController
    {
        private static ScreenManager _screenManager;
        private static MainGame _game;

        public static void Set(MainGame game)
        {
            _screenManager = new ScreenManager();
            game.Components.Add(_screenManager);
            _game = game;
        }

        public static void LoadPingPongScreen() => _screenManager.LoadScreen(new PingPongScreen(_game, 0), new FadeTransition(_game.GraphicsDevice, Color.Black));
        public static void LoadTitlecreen() => _screenManager.LoadScreen(new TitleScreen(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
        public static void LoadServerScreen() => _screenManager.LoadScreen(new PingPongScreen(_game, 1), new FadeTransition(_game.GraphicsDevice, Color.Black));
        public static void LoadClientScreen(string ip) => _screenManager.LoadScreen(new PingPongScreen(_game, 2, ip), new FadeTransition(_game.GraphicsDevice, Color.Black));
    }
}