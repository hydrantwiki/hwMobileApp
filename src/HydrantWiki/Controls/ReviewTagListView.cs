using System;
using HydrantWiki.Cells;
using Xamarin.Forms;

namespace HydrantWiki.Controls
{
    public class ReviewTagListView : AbstractListView
    {
        public ReviewTagListView()
        {
            ItemTemplate = new DataTemplate(typeof(ReviewTagCell));
            HasUnevenRows = true;
        }
    }
}
