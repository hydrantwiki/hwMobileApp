using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HydrantWiki.Controls;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using HydrantWiki.ResponseObjects;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class ReviewTagsForm : AbstractPage
    {
        private ReviewTagListView m_lstTags;

        public ReviewTagsForm() : base("Review Tags")
        {
            m_lstTags = new ReviewTagListView
            {
                RowHeight = 100
            };
            m_lstTags.RefreshCommand = new Command(StartRefresh);
            m_lstTags.ItemSelected += TagSelected;

            OutsideLayout.Children.Add(m_lstTags);

            StartRefresh();
        }

        private void StartRefresh()
        {
            Task t = Task.Factory.StartNew(() => Refresh());
        }

        private void Refresh()
        {
            List<TagToReview> tags = GetTagsToReview();

            Device.BeginInvokeOnMainThread(() =>
            {
                m_lstTags.ItemsSource = tags;
                m_lstTags.EndRefresh();
            });
        }


        private List<TagToReview> GetTagsToReview()
        {
            TagsToReviewResponse response = HWManager.GetInstance().ApiManager.GetTagsToReview(HydrantWikiApp.User);

            if (response.Success)
            {
                return response.Tags;
            }

            return null;
        }

        void TagSelected(object sender, SelectedItemChangedEventArgs e)
        {
            TagToReview tag = (TagToReview)e.SelectedItem;


        }
    }
}
