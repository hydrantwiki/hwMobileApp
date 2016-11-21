using System.Collections.Generic;
using System.Threading.Tasks;
using HydrantWiki.Constants;
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

        public ReviewTagsForm() : base(DisplayConstants.FormReviewTags)
        {
            m_lstTags = new ReviewTagListView
            {
                RowHeight = 100
            };
            m_lstTags.RefreshCommand = new Command(StartRefresh);
            m_lstTags.ItemSelected += TagSelected;

            OutsideLayout.Children.Add(m_lstTags);

            HWManager.GetInstance().ApiManager.Log(LogLevels.Info,
                   string.Format("Review Tags viewed by {0}", HydrantWikiApp.User.Username));
        }

        private void StartRefresh()
        {
            Task.Factory.StartNew(() => Refresh());
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

            ReviewTagForm form = new ReviewTagForm();
            form.SetTag(tag);

            Navigation.PushModalAsync(form);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            StartRefresh();
        }
    }
}
