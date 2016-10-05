using Xamarin.Forms;

namespace HydrantWiki.Controls
{
    public class HWButtonBar : ContentView
    {
        private StackLayout m_Buttons;

        public HWButtonBar()
        {
            m_Buttons = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Padding = 0,
                Spacing = 0
            };

            if (Device.Idiom == TargetIdiom.Phone)
            {
                m_Buttons.Margin = new Thickness(0, 0, 0, 0);
            } else {
                m_Buttons.Margin = new Thickness(0, 0, 0, 0);
            }

            Content = m_Buttons;
        }

        public void Add(Button button)
        {
            m_Buttons.Children.Add(button);
        }

        public HWButton Add(string _text, LayoutOptions _horizontalOptions, int _width = 100)
        {
            HWButton button = new HWButton()
            {
                Text = _text,
                Margin = new Thickness(5, 0, 5, 0),
                HorizontalOptions = _horizontalOptions,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = _width
            };

            m_Buttons.Children.Add(button);

            return button;
        }
    }
}

