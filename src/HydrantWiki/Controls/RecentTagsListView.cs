using HydrantWiki.Cells;
using Xamarin.Forms;

namespace HydrantWiki.Controls
{
    public class RecentTagsListView : AbstractListView
    {
        public RecentTagsListView()
        {
            ItemTemplate = new DataTemplate(typeof(TagCell));
            HasUnevenRows = true;
        }
    }
}
