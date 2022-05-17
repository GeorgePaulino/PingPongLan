using System.IO;
using FontStashSharp;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;

namespace PingPong
{
	public class TitleUI: Grid
	{
		private Label _titleText;
		public ImageTextButton initNormalGameBtn;
		public ImageTextButton lanUIBtn;
		private Label _aboutMe;
		public TitleUI()
		{
			BuildUI();

		}
		private void BuildUI()
		{
			
			byte[] ttfData = File.ReadAllBytes("./Data/fonts/title.ttf");
            FontSystemSettings fontSet = new FontSystemSettings{Effect = FontSystemEffect.Stroked, EffectAmount = 2};
            FontSystem font = new FontSystem(fontSet);
            font.AddFont(ttfData);

			_titleText = new Label();
			_titleText.Text = @"\c[indianRed]PING \c[skyBlue]PONG";
			_titleText.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
			_titleText.Margin = new Thickness(0, 80, 0, 0);
			_titleText.Background = new SolidBrush("#00000000");
			_titleText.Font = font.GetFont(128);

			ttfData = File.ReadAllBytes("./Data/fonts/buttons.otf");
            fontSet = new FontSystemSettings{Effect = FontSystemEffect.Stroked, EffectAmount = 2};
            font = new FontSystem(fontSet);
            font.AddFont(ttfData);

			initNormalGameBtn = UI.DefaultTextButton("Normal Game", font);
			initNormalGameBtn.Margin = new Thickness(0, 120, 0, 0);

            lanUIBtn = UI.DefaultTextButton("Lan Game", font);
			lanUIBtn.Margin = new Thickness(0, 300, 0, 0);

			_aboutMe = new Label();
			_aboutMe.Text = @"IG - george_psf";
			_aboutMe.VerticalAlignment = VerticalAlignment.Bottom;
			_aboutMe.Margin = new Thickness(2, 0, 0, 4);
			_aboutMe.Background = new SolidBrush("#00000000");
			_aboutMe.Font = font.GetFont(20);

			
			Widgets.Add(_titleText);
			Widgets.Add(initNormalGameBtn);
			Widgets.Add(lanUIBtn);
			Widgets.Add(_aboutMe);
		}
	}
}
