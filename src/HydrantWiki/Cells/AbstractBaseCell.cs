using HydrantWiki.Controls;
using Xamarin.Forms;

namespace HydrantWiki.Cells
{
    public abstract class AbstractBaseCell : ViewCell
    {
        private StackLayout m_Layout;
        private HWLabel m_lblCellText;

        protected AbstractBaseCell (string bindToProperty)
        {
            m_Layout = new StackLayout ();
            m_Layout.Padding = new Thickness (0, 5);

            m_lblCellText = new HWLabel ();
            m_lblCellText.SetBinding (Label.TextProperty, bindToProperty);
            m_Layout.Children.Add (m_lblCellText);

            View = m_Layout;
        }

    }
}

