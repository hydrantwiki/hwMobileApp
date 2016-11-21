using System;
using System.Threading.Tasks;
using HydrantWiki.Constants;
using HydrantWiki.Interfaces;
using HydrantWiki.Managers;
using HydrantWiki.Objects;
using Xamarin.Forms;

namespace HydrantWiki.Forms
{
    public class MainForm : MasterDetailPage, IMainForm
    {
        public bool FiredAppStarted = false;

        public MainForm()
        {
            Title = DisplayConstants.AppName;

            var menuPage = new MenuPage();
            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuOption);
            Master = menuPage;

            Detail = new HWNavigationPage(new DefaultForm());
        }

        public Task<bool> ShowAlert(string _title, string _message, string _accept, string _cancel)
        {
            return DisplayAlert(_title, _message, _accept, _cancel);
        }

        private void NavigateTo(MenuOption menu)
        {
            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

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

        public void PushModal(Page _form)
        {
            Navigation.PushModalAsync(_form);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!HWManager.GetInstance().AreUserCredentialsCached())
            {
                LoginForm login = new LoginForm();
                Navigation.PushModalAsync(login);
            }
        }
    }
}

