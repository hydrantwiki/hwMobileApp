using System;
using HydrantWiki.Cells;
using Xamarin.Forms;

namespace HydrantWiki.Controls
{
    public class ReviewTagHydrantsListView : AbstractListView
    {
        public ReviewTagHydrantsListView()
        {
            ItemTemplate = new DataTemplate(typeof(ReviewTagHydrantCell));
            HasUnevenRows = true;
        }
    }
}
