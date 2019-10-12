using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WSB_SEM5_Kalkulator.Model;

namespace WSB_SEM5_Kalkulator.Components
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public delegate bool AuthorizeLoginDelegate(LoginWindow dialog);
        public bool IsLogged { get; private set; }

        public AuthorizeLoginDelegate OnAuthorizeLogin;

        public string Login => LoginField.Text;
        public string Password => PasswordField.Password;


        public LoginWindow()
        {
            Log.Write(GetType());

            InitializeComponent();

            IsLogged = false;

            Activated += OnActivated;

            LoginBtn.Click += OnButtonClick;
            CancelBtn.Click += OnButtonClick;
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button instancja = sender as Button;

                Log.Write(GetType(), "OnButtonClick", "Przycisk = <{0}>", instancja.Name);

                switch (instancja.Name)
                {
                    case "LoginBtn":
                        AuthorizeLoginDialog(); break;

                    case "CancelBtn":
                        CancelLoginDialog(); break;
                }
            }
        }

        public void Clear()
        {
            Log.Write(GetType(), "Clear");

            LoginField.Text = "";
            PasswordField.Password = "";

            LoginField.Focus();
        }

        private void AuthorizeLoginDialog()
        {
            Log.Write(GetType(), "AuthorizeLoginDialog");

            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Pole <Login> nie może być puste!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                LoginField.Focus();
            }
            else if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Pole <Hasło> nie może być puste!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordField.Focus();
            }
            else
            {
                IsLogged = OnAuthorizeLogin?.Invoke(this) ?? false;

                if (IsLogged)
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("Błędny login lub hasło!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    Clear();
                }

            }


        }

        private void CancelLoginDialog()
        {
            Log.Write(GetType(), "CancelLoginDialog");

            Close();
        }

        private void OnActivated(object sender, EventArgs e)
        {
            Log.Write(GetType(), "OnActivated");

            if (sender is LoginWindow)
            {
                LoginWindow loginWindow = sender as LoginWindow;

                loginWindow.Activated -= OnActivated;

                loginWindow.LoginField.Focus();
            }
        }
    }
}
