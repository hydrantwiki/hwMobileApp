using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HydrantWiki.Constants;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class DefaultForm : AbstractPage
    {
        private RecentTagsListView m_lstRecentTags;
        private HWLabel m_lblTags;
        private HWFormButton m_btnTagHydrant;
        private HWLabel m_lblRecent;

        public DefaultForm() : base(DisplayConstants.FormHome)
        {
            m_lblTags = new HWLabel
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Text = DisplayConstants.LoadingTagCount,
                Margin = new Thickness(0, 10, 0, 10)
            };
            OutsideLayout.Children.Add(m_lblTags);

            m_btnTagHydrant = new HWFormButton
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = DisplayConstants.TagHydrant,
                Margin = new Thickness(10, 10, 10, 10)
            };
            m_btnTagHydrant.Clicked += TagHydrant_Clicked;
            OutsideLayout.Children.Add(m_btnTagHydrant);

            m_lblRecent = new HWLabel
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = DisplayConstants.MyRecentTags,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Margin = new Thickness(0, 10, 0, 10)

            };
            OutsideLayout.Children.Add(m_lblRecent);

            m_lstRecentTags = new RecentTagsListView();
            OutsideLayout.Children.Add(m_lstRecentTags);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Factory.StartNew(() => LoadTagCount());

            Task.Factory.StartNew(() => LoadRecentTags());
        }

        void TagHydrant_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new TagHydrant());
        }

        private void LoadRecentTags()
        {
            List<Tag> tags = HWManager.GetInstance().GetRecentTags();

            Device.BeginInvokeOnMainThread(() =>
            {
                m_lstRecentTags.ItemsSource = tags;
            });
        }

        private void LoadTagCount()
        {
            HWManager manager = HWManager.GetInstance();

            try
            {
                if (HydrantWikiApp.User != null
                    && manager.PlatformManager.HasNetworkConnectivity)
                {
                    var response = HWManager.GetInstance().ApiManager.GetMyTagCount(HydrantWikiApp.User);

                    if (response != null
                        && response.Success)
                    {
                        manager.SettingManager.SetTagCount(response.TagCount);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            m_lblTags.Text = string.Format(DisplayConstants.TotalTagsCollected, response.TagCount);
                        });

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                manager.ApiManager.Log(LogLevels.Exception, ex.ToString());
            }

            //Failed to get the count so pull the cached value
            int count = HWManager.GetInstance().SettingManager.GetTagCount();

            Device.BeginInvokeOnMainThread(() =>
            {
                m_lblTags.Text = string.Format(DisplayConstants.TotalTagsCollectedCached, count);
            });
        }
    }
}

