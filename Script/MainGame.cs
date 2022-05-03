using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System;
using System.IO;

namespace MonogameBase
{

    public class MainScreen : GameScreen
    {

        private new MainGame Game => (MainGame)base.Game;
        public MainScreen(MainGame game) : base(game) { }

        private Desktop _desktop;

        private Texture2D background;

        private ImageButton lollyBtn;
        private int lollyRes = 0;
        private Label tittle;

        public override void LoadContent()
        {
            base.LoadContent();

            background = Game.Content.Load<Texture2D>("background");

            string data = File.ReadAllText("./Data/xmmp/ui.xmmp");
            Project project = Project.LoadFromXml(data);
            
            Grid grid = (Grid)project.Root;

            lollyBtn = (ImageButton)project.Root.FindWidgetById("lolly");
            Texture2D lollyTex = Game.Content.Load<Texture2D>("lolly");
            Console.WriteLine(lollyTex.Width);
            lollyRes = lollyTex.Width / lollyTex.Height;
            lollyBtn.Image = new TextureRegion(lollyTex);
            lollyBtn.OverImage = new TextureRegion(Game.Content.Load<Texture2D>("lolly-h"));
            lollyBtn.PressedImage = new TextureRegion(Game.Content.Load<Texture2D>("lolly-p"));
            lollyBtn.LayoutUpdated += LollyResUpdate;
            lollyBtn.Click += LollyClicked;
            lollyBtn.MouseEntered += (sender, e) => TittleColor(sender, e, Color.Blue);
            lollyBtn.MouseLeft += (sender, e) => TittleColor(sender, e, Color.Pink);
            lollyBtn.TouchDown += (sender, e) => TittleColor(sender, e, Color.Green);
            lollyBtn.TouchUp += (sender, e) => TittleColor(sender, e, Color.Blue);
            lollyBtn.MouseEntered += delegate { Console.WriteLine("Testing"); };

            tittle = (Label)project.Root.FindWidgetById("tittle");
            byte[] ttfData = File.ReadAllBytes("./Data/fonts/PlayfairDisplay-VariableFont_wght.ttf");
            FontSystemSettings fontSett = new FontSystemSettings{Effect = FontSystemEffect.Stroked, EffectAmount = 2};
            FontSystem font = new FontSystem(fontSett);
            font.AddFont(ttfData);
            tittle.TextColor = Color.Pink;
            tittle.Font = font.GetFont(64);
            

            _desktop = new Desktop();
            _desktop.Root = grid;
            
            LollyResUpdate(null, null);
        }

        void LollyResUpdate(object sender, EventArgs e) => lollyBtn.Width = lollyRes * lollyBtn.ActualBounds.Height;
        void LollyClicked(object sender, EventArgs e) => Game.Exit();
        void TittleColor(object sender, EventArgs e, Color color) => tittle.TextColor = color;

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            Game.spriteBatch.Begin();
            Game.spriteBatch.Draw(background, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
            Game.spriteBatch.End();

            _desktop.Render();
        }
    }

    public class ScreenControl
    {
        private readonly ScreenManager _screenManager;
        private readonly MainGame _game;

        public ScreenControl(MainGame game)
        {
            _screenManager = new ScreenManager();
            game.Components.Add(_screenManager);
            this._game = game;
        }

        public void LoadMainScreen()
        {
            _screenManager.LoadScreen(new MainScreen(_game), new FadeTransition(_game.GraphicsDevice, Color.Black));
        }
    }

    public class MainGame : Game
    {
        public SpriteBatch spriteBatch;
        public GraphicsDeviceManager graphics;

        public ScreenControl screenControl;

        public MainGame()
        {
            // Preferences
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;

            // Variables
            graphics = new GraphicsDeviceManager(this);

            // Extended
            screenControl = new ScreenControl(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
            screenControl.LoadMainScreen();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            MyraEnvironment.Game = this;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
