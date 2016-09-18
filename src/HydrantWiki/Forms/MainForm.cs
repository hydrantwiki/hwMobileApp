using System;
using System.Threading.Tasks;
using ATMobile.Forms;
using HydrantWiki.Constants;
using HydrantWiki.Interfaces;
using HydrantWiki.Objects;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class MainForm : MasterDetailPage, IMainForm
    {
        public bool FiredAppStarted = false;
        private App m_App;

        public MainForm(App _app)
        {
            m_App = _app;

            Title = "ArcheryTrack";


            var menuPage = new MenuPage();
            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as Objects.MenuOption);
            Master = menuPage;

            Detail = new NavigationPage(new DefaultForm());
        }



        public Task<bool> ShowAlert(string _title, string _message, string _accept, string _cancel)
        {
            return DisplayAlert(_title, _message, _accept, _cancel);
        }

        private void NavigateTo(MenuOption menu)
        {
            Page displayPage = null;


            if (displayPage != null)
            {
                Detail = new NavigationPage(displayPage)
                {
                    BarTextColor = Color.FromHex(UIConstants.NavBarTextColor),
                    BarBackgroundColor = Color.FromHex(UIConstants.NavBarColor)
                };

                IsPresented = false;
            }
        }



        public void PushModal(ContentPage _form)
        {
            Navigation.PushModalAsync(_form);
        }
    }
}

