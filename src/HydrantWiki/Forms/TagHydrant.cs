using System;
using HydrantWiki.Controls;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class TagHydrant : AbstractPage
    {
        private HWHeader m_Header;
        private HWButtonBar m_ButtonLayout;
        private HWButton CancelButton;
        private HWButton SaveButton;

        private Image m_HydrantImage;

        public TagHydrant()
            : base("Tag Hydrant")
        {
            m_Header = new HWHeader("Hydrant Details")
            {
                Margin = new Thickness(0, 0, 0, 0)
            };
            OutsideLayout.Children.Add(m_Header);

            m_ButtonLayout = new HWButtonBar();
            OutsideLayout.Children.Add(m_ButtonLayout);

            SaveButton = m_ButtonLayout.Add("Save", LayoutOptions.EndAndExpand);
            SaveButton.Clicked += SaveButton_Clicked;

            CancelButton = m_ButtonLayout.Add("Cancel", LayoutOptions.StartAndExpand);
            CancelButton.Clicked += CancelButton_Clicked;

        }

        void CancelButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }

        void SaveButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
