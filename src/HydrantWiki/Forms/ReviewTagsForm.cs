using System;
using System.Threading.Tasks;
using HydrantWiki.Controls;
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

            };
            m_lstTags.RefreshCommand = new Command(StartRefresh);
            m_lstTags.ItemSelected += TagSelected;

            OutsideLayout.Children.Add(m_lstTags);
        }

        private void StartRefresh()
        {
            Task t = Task.Factory.StartNew(() => Refresh());
        }

        private async Task Refresh()
        {
            await GetTagsToReview();

            Device.BeginInvokeOnMainThread(() =>
            {
                m_lstTags.EndRefresh();
            });
        }

        private async Task GetTagsToReview()
        {

        }

        void TagSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}
