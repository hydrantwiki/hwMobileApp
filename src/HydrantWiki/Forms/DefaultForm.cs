using HydrantWiki.Controls;
using HydrantWiki.Managers;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class DefaultForm : AbstractPage
    {
        private RecentTagsListView m_lstRecentTags;

        public DefaultForm() : base("Home")
        {
            m_lstRecentTags = new RecentTagsListView();

            OutsideLayout.Children.Add(m_lstRecentTags);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //TODO load tags
        }
    }
}

