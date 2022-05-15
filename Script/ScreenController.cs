using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Microsoft.Xna.Framework;

namespace PingPong
{
    public class ScreenController
    {
        private readonly ScreenManager _screenManager;
        private readonly MainGame _game;

        public ScreenController(MainGame game)
        {
            _screenManager = new ScreenManager();
            game.Components.Add(_screenManager);
            this._game = game;
        }

        public void LoadPingPongScreen() => _screenManager.LoadScreen(new PingPongScreen(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
        public void LoadTitlecreen() => _screenManager.LoadScreen(new TitleScreen(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
        public void LoadServerScreen() => _screenManager.LoadScreen(new PingPongServerScreen(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
        public void LoadClientScreen() => _screenManager.LoadScreen(new PingPongClientScreen(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
    }
}