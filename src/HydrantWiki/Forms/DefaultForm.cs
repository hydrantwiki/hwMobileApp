using System;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class DefaultForm : AbstractPage
    {
        private RecentTagsListView m_lstRecentTags;
        private HWLabel m_lblTags;

        public DefaultForm() : base("Home")
        {
            m_lblTags = new HWLabel
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Text = "Loading Tag Count",
                Margin = new Thickness(0, 10, 0, 10)
            };
            OutsideLayout.Children.Add(m_lblTags);

            m_lstRecentTags = new RecentTagsListView();
            OutsideLayout.Children.Add(m_lstRecentTags);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //TODO load tags
            LoadTagCount();
        }

        private void LoadTagCount()
        {
            try
            {
                var response = HWManager.GetInstance().ApiManager.GetMyTagCount(HydrantWikiApp.User);

                if (response.Success)
                {
                    HWManager.GetInstance().SettingManager.SetTagCount(response.TagCount);
                    m_lblTags.Text = string.Format("Total Tags Collected - {0}", response.TagCount);

                    return;
                }

            }
            catch (Exception ex)
            {
                //TODO - Log exception
            }

            //Failed to get the count so pull the cached value
            int count = HWManager.GetInstance().SettingManager.GetTagCount();
            m_lblTags.Text = string.Format("Total Tags Collected - {0} (Cached)", count);
        }
    }
}

