using System.Collections.Generic;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Collisions;

namespace PingPong
{
    public class PingPongScreen : GameScreen
    {
        private PadleEntity[] _padles = new PadleEntity[2];
        private BallEntity _ball;

        private int[] _scores = new int[2];
        public void AddScore(int index) => _scores[index] += 1;

        // 0 - Normal | 1 - Server | 2 - Client
        private int _gameMode;
        private NetPeer _conn;

        private CollisionComponent _collisionComponent;
        private List<IEntity> _entities = new List<IEntity>();
        private ParticleController _particleController = new ParticleController();

        private Texture2D _ballTex;
        private Texture2D _fieldTex;
        
        private SpriteFont _scoreFont;
        private SpriteFont _hintFont;

        private Song backgroundSong;

        private SoundEffect _ballImpact;
        private SoundEffect _pointMarked;

        private new MainGame Game => (MainGame)base.Game;
        public PingPongScreen(MainGame game, int gameMode, string ip = null) : base(game) 
        {
            if(gameMode == 1)
            {
                var config = new NetPeerConfiguration("Ping Pong") { Port = 1490 };
                _conn = new NetServer(config);
                _conn.Start();
            }
            else if(gameMode == 2)
            {    
                var config = new NetPeerConfiguration("Ping Pong");
                _conn = new NetClient(config);
                _conn.Start();
                _conn.Connect(host: ip, port: 1490);
            }
            this._gameMode = gameMode;
        }

        public override void Initialize()
        {
            Game.screen = 2;
            Game.graphics.PreferredBackBufferWidth = Utilities.screenBounds[0];
            Game.graphics.PreferredBackBufferHeight = Utilities.screenBounds[1];
            Game.graphics.ApplyChanges();

            _collisionComponent = new CollisionComponent(new RectangleF(0, 0, Utilities.screenBounds[0], Utilities.screenBounds[1]));

            _padles[0] = new PadleEntity(new RectangleF(new Point2(50, 190), new Size2(20, 120)), Color.Red);
            _padles[1] = new PadleEntity(new RectangleF(new Point2(750, 190), new Size2(20, 120)), Color.Blue);
            _ball = new BallEntity(new CircleF(new Point2(395, 245), 10), Color.Green);
            _entities.Add(_padles[0]);
            _entities.Add(_padles[1]);
            _entities.Add(_ball);

            foreach(var e in _entities) { _collisionComponent.Insert(e); }
        }

        public override void LoadContent() 
        {
            _particleController.LoadContent(Game.GraphicsDevice);
            
            _padles[0].texture = Content.Load<Texture2D>("sprites/paddle1");
            _padles[1].texture = Content.Load<Texture2D>("sprites/paddle2");
            
            _ballTex = Content.Load<Texture2D>("sprites/ball");
            _ballImpact = Content.Load<SoundEffect>("sounds/impact");
            _ball.texture = _ballTex;
            _ball.impactSoundEffect = _ballImpact;

            _fieldTex = Content.Load<Texture2D>("sprites/field");

            _scoreFont = Content.Load<SpriteFont>("Score");
            _hintFont = Content.Load<SpriteFont>("Hint");

            _pointMarked = Content.Load<SoundEffect>("sounds/point");

            backgroundSong = Content.Load<Song>("songs/gameBG");
            MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3f;
        }

        public override void Update(GameTime gameTime) 
        {
            if(_gameMode == 0)
            {
                _padles[0].motions[0] = Keyboard.GetState().IsKeyDown(Utilities.keys[0][0]);
                _padles[0].motions[1] = Keyboard.GetState().IsKeyDown(Utilities.keys[0][1]);
                _padles[1].motions[0] = Keyboard.GetState().IsKeyDown(Utilities.keys[1][0]);
                _padles[1].motions[1] = Keyboard.GetState().IsKeyDown(Utilities.keys[1][1]);
            }
            else
            {
                if(_conn.ConnectionsCount == 0) return;
                if(_gameMode == 1) ServerUpdate();
                else ClientUpdate();
            }

            _particleController.Update(gameTime);
            foreach(var e in _entities) { e.Update(gameTime); }
            _collisionComponent.Update(gameTime);
            if(_ball.markPoint != 0)
            {
                _pointMarked.Play(0.2f, 1, 1);
                AddScore(_ball.markPoint - 1);
                _particleController.AddParticle(_ball.markPoint == 1, _ball.Bounds.Position);
                _entities.Remove(_ball);
                _collisionComponent.Remove(_ball);
                _ball = new BallEntity(new CircleF(new Point2(395, 245), 10), Color.Green, false);
                _ball.texture = _ballTex;
                _ball.impactSoundEffect = _ballImpact;
                _entities.Add(_ball);
                _collisionComponent.Insert(_ball);
            }
        }

        void ClientUpdate()
        {
            var send = _conn.CreateMessage();
            send.WriteAllFields(new ClientMessage(){up = Keyboard.GetState().IsKeyDown(Utilities.keys[1][0]), 
                down = Keyboard.GetState().IsKeyDown(Utilities.keys[1][1])});
            ((NetClient)_conn).SendMessage(send, NetDeliveryMethod.ReliableOrdered);
            
            NetIncomingMessage message;
            while ((message = _conn.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        var data = new ServerMessage();
                        message.ReadAllFields(data);
                        _padles[0].prePositionedValue = new Point2(data.pad1X, data.pad1Y);
                        _padles[1].prePositionedValue = new Point2(data.pad2X, data.pad2Y);
                        _ball.prePositionedValue = new Point2(data.ballX, data.ballY);
                        _scores[0] = data.score1;
                        _scores[1] = data.score2;
                        _ball.markPoint = data.win;
                        break;
                    default: break;
                }
            }
        }
        void ServerUpdate()
        {
            var send = _conn.CreateMessage();
            send.WriteAllFields(new ServerMessage(){pad1X = _padles[0].Bounds.Position.X, pad1Y = _padles[0].Bounds.Position.Y,
                pad2X = _padles[1].Bounds.Position.X, pad2Y = _padles[1].Bounds.Position.Y,
                ballX = _ball.Bounds.Position.X, ballY = _ball.Bounds.Position.Y,
                score1 = _scores[0], score2 = _scores[1], win = _ball.markPoint});
            ((NetServer)_conn).SendToAll(send, NetDeliveryMethod.ReliableOrdered);

            _padles[0].motions[0] = Keyboard.GetState().IsKeyDown(Utilities.keys[0][0]);
            _padles[0].motions[1] = Keyboard.GetState().IsKeyDown(Utilities.keys[0][1]);
            NetIncomingMessage message;
            while ((message = _conn.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        var data = new ClientMessage();
                        message.ReadAllFields(data);
                        _padles[1].motions[0] = data.up;
                        _padles[1].motions[1] = data.down;
                        break;
                    default: break;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            if(_gameMode != 0)
            {
                if(_conn.ConnectionsCount == 0) 
                {
                    string msg = _gameMode == 1 ? "Waiting Cliente... (Press ESC)" : "Searching Server... (Press ESC)" ;
                    Color color = _gameMode == 1 ? Color.IndianRed : Color.SkyBlue;
                    Game.spriteBatch.Begin();
                    Game.spriteBatch.DrawString(_scoreFont, msg, new Vector2(50, 50), color);
                    Game.spriteBatch.End();
                    return;
                }
            }

            _particleController.Draw(Game.spriteBatch);
            Game.spriteBatch.Begin();
            Game.spriteBatch.Draw(_fieldTex, new Rectangle(-5, -3, 800, 500), Color.White);
            Game.spriteBatch.DrawString(_scoreFont, _scores[1].ToString(), new Vector2(150, 20), Color.White);
            Game.spriteBatch.DrawString(_scoreFont, _scores[0].ToString(), new Vector2(650, 20), Color.White);
            Game.spriteBatch.DrawString(_hintFont, "W", new Vector2(150, 100), Color.IndianRed);
            Game.spriteBatch.DrawString(_hintFont, "S", new Vector2(150, 400), Color.IndianRed);
            Game.spriteBatch.DrawString(_hintFont, "UP", new Vector2(650, 100), Color.SkyBlue);
            Game.spriteBatch.DrawString(_hintFont, "DOWN", new Vector2(650, 400), Color.SkyBlue);
            _padles[0].Draw(Game.spriteBatch);
            _padles[1].Draw(Game.spriteBatch);
            _ball.Draw(Game.spriteBatch);
            
            Game.spriteBatch.End();
        }

        public override void UnloadContent()
        {
            if(_gameMode != 0) _conn.Shutdown("");
        }

    }
}