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
        public void LoadCollisionScreen() => _screenManager.LoadScreen(new CollisionScreen(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
    }
}