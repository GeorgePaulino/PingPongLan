using System.IO;
using FontStashSharp;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;

namespace PingPong
{
	public class TitleUI: Grid
	{
		public Label titleText;
		public ImageTextButton initNormalGameBtn;
		public ImageTextButton lanUIBtn;
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

			titleText = new Label();
			titleText.Text = @"\c[indianRed]PING \c[skyBlue]PONG";
			titleText.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
			titleText.Margin = new Thickness(0, 80, 0, 0);
			titleText.Background = new SolidBrush("#00000000");
			titleText.Id = "title";
			titleText.Font = font.GetFont(128);

			ttfData = File.ReadAllBytes("./Data/fonts/buttons.otf");
            fontSet = new FontSystemSettings{Effect = FontSystemEffect.Stroked, EffectAmount = 2};
            font = new FontSystem(fontSet);
            font.AddFont(ttfData);

			initNormalGameBtn = UI.DefaultTextButton("Normal Game", font);
			initNormalGameBtn.Margin = new Thickness(0, 120, 0, 0);

            lanUIBtn = UI.DefaultTextButton("Lan Game", font);
			lanUIBtn.Margin = new Thickness(0, 300, 0, 0);

			
			Widgets.Add(titleText);
			Widgets.Add(initNormalGameBtn);
			Widgets.Add(lanUIBtn);
		}
	}
}
