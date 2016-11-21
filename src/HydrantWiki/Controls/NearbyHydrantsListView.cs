using HydrantWiki.Cells;
using Xamarin.Forms;

namespace HydrantWiki.Controls
{
    public class NearbyHydrantsListView : AbstractListView
    {
        public NearbyHydrantsListView()
        {
            ItemTemplate = new DataTemplate(typeof(HydrantCell));
            HasUnevenRows = true;
        }
    }
}
