using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private HWButton m_btnTagHydrant;
        private HWLabel m_lblRecent;

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

            m_btnTagHydrant = new HWButton
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Tag Hydrant",
                BackgroundColor = Color.White,
                BorderWidth = 1,
                BorderColor = Color.Black,
                Margin = new Thickness(10, 10, 10, 10)
            };
            m_btnTagHydrant.Clicked += TagHydrant_Clicked;
            OutsideLayout.Children.Add(m_btnTagHydrant);

            m_lblRecent = new HWLabel
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "My Recent Tags",
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

            var tagCountTask = Task.Factory.StartNew(() => LoadTagCount());

            var recentTagsTask = Task.Factory.StartNew(() => LoadRecentTags());
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
            try
            {
                if (HydrantWikiApp.User != null)
                {
                    var response = HWManager.GetInstance().ApiManager.GetMyTagCount(HydrantWikiApp.User);

                    if (response.Success)
                    {
                        HWManager.GetInstance().SettingManager.SetTagCount(response.TagCount);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            m_lblTags.Text = string.Format("Total Tags Collected - {0}", response.TagCount);
                        });

                        return;
                    }
                } else {
                    return;
                }

            }
            catch (Exception ex)
            {
                //TODO - Log exception
            }

            //Failed to get the count so pull the cached value
            int count = HWManager.GetInstance().SettingManager.GetTagCount();

            Device.BeginInvokeOnMainThread(() =>
            {
                m_lblTags.Text = string.Format("Total Tags Collected - {0} (Cached)", count);
            });
        }
    }
}

