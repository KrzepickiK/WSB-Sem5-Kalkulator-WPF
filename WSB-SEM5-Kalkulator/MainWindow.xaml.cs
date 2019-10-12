using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WSB_SEM5_Kalkulator.Components;
using WSB_SEM5_Kalkulator.Helper;
using WSB_SEM5_Kalkulator.Model;

namespace WSB_SEM5_Kalkulator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _login = ResourcesFile.TestLogin;
        private readonly string _pass = ResourcesFile.TestPass;
        string operation = "";
        float num1 = 0;
        float num2 = 0;

        public MainWindow()
        {
            Log.Write(GetType());

            InitializeComponent();

            Activated += OnActivated;

            ApiToken.Click += OnButtonClick;

            btn0.Click += OnButtonClick;
            btn1.Click += OnButtonClick;
            btn2.Click += OnButtonClick;
            btn3.Click += OnButtonClick;
            btn4.Click += OnButtonClick;
            btn5.Click += OnButtonClick;
            btn6.Click += OnButtonClick;
            btn7.Click += OnButtonClick;
            btn8.Click += OnButtonClick;
            btn9.Click += OnButtonClick;


            btnPlus.Click += OnButtonClick;
            btnMinus.Click += OnButtonClick;
            btnDivide.Click += OnButtonClick;
            btnTimes.Click += OnButtonClick;
            btnEquals.Click += OnButtonClick;

            btnClear.Click += OnButtonClick;
            btnClearEntry.Click += OnButtonClick;

        }

        private void OnActivated(object sender, EventArgs e)
        {
            if (sender is MainWindow)
            {
                MainWindow Okno = sender as MainWindow;

                Log.Write(GetType(), "OnActivated", "okno aktywowane");

                Okno.Activated -= OnActivated;
            }
        }

        private void OnInitialized(object sender, EventArgs e)
        {
            Log.Write(GetType(), "OnInitialized", "okno zainicjalizowane");
            if (sender is MainWindow)
            {
                MainWindow mainWindow = sender as MainWindow;
                
                LoginWindow Dialog = new LoginWindow()
                {
                    OnAuthorizeLogin = AuthorizeLoginDelegate,
                    Title = $"Logowanie ({_login}, {_pass})"
                };

                Dialog.ShowDialog();

                if (Dialog.IsLogged)
                {
                    mainWindow.Run();
                }
                else
                {
                    mainWindow.Close();
                }
            }
        }

        private void Run()
        {
            Log.Write(GetType(), "Run");

            Title += $" wersja {ResourcesFile.AppVersion}";
            Closing += OnClosing;

            if (!string.IsNullOrEmpty(AppState.ApiToken))
            {
                Title += $" UWAGA! - Zalogowano do api!";
            }

            try
            {
                Log.Write(GetType(), "Run Try");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Log.Write(GetType(), "OnClosing", "Zamykamy okno <{0}>", sender);

            MessageBoxResult Option = MessageBox.Show("Czy zakończyć działanie programu?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            Log.Write(GetType(), "OnClosing", "Opcja <{0}>", Option);

            if (Option != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private bool AuthorizeLoginDelegate(LoginWindow sender)
        {
            Log.Write(GetType(), "AuthorizeLoginDelegate");

            if ((sender.Login.Equals(_login) && (sender.Password.Equals(_pass))) || GetAccessToken(sender.Login, sender.Password))
            {
                return true;
            }

            return false;
        }

        public bool GetAccessToken(string login, string password)
        {
            using (var client = new HttpClient())
            {
                var param = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", login },
                    { "password", password }
                };

                string postUri = $"Token?grant_type=password&username={login}&password={password}";
                client.BaseAddress = new Uri(ResourcesFile.ApiHost);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string jsonResponseString = "";
                HttpResponseMessage response = client.PostAsync(postUri, new FormUrlEncodedContent(param)).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                    throw new Exception($"Wystąpił błąd podczas wysyłania danych POST do Api\n Base: {client.BaseAddress}\n{postUri}\n. Response Status Code: {response}\n");
                }
                else
                {
                    try
                    {
                        using (HttpContent content = response.Content)
                        {
                            Task<string> result = content.ReadAsStringAsync();
                            jsonResponseString = result.Result;
                            TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonResponseString);
                            AppState.Username = tokenResponse.userName;
                            AppState.ApiToken = tokenResponse.access_token;
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;

                Log.Write(GetType(), "OnButtonClick", "Przycisk = <{0}>", btn.Name);

                switch (btn.Name)
                {
                    //Api buttons
                    case "ApiToken": ShowApiToken(); break;
                    //Number buttons
                    case "btn0": MathButtonClick(0); break;
                    case "btn1": MathButtonClick(1); break;
                    case "btn2": MathButtonClick(2); break;
                    case "btn3": MathButtonClick(3); break;
                    case "btn4": MathButtonClick(4); break;
                    case "btn5": MathButtonClick(5); break;
                    case "btn6": MathButtonClick(6); break;
                    case "btn7": MathButtonClick(7); break;
                    case "btn8": MathButtonClick(8); break;
                    case "btn9": MathButtonClick(9); break;
                    //Math operations buttons
                    case "btnPlus": OperationButtonClick("+"); break;
                    case "btnMinus": OperationButtonClick("-"); break;
                    case "btnDivide": OperationButtonClick("/"); break;
                    case "btnTimes": OperationButtonClick("*"); break;
                    case "btnEquals": OperationButtonClick("="); break;
                    //Other buttons
                    case "btnClear": OperationButtonClick("C"); break;
                    case "btnClearEntry": OperationButtonClick("CE"); break;
                }
            }
        }

        private void ShowApiToken()
        {
            Log.Write(GetType(), "ShowApiToken");

            if (string.IsNullOrEmpty(AppState.ApiToken))
            {
                MessageBox.Show("Token niedostępny!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ApiDataPopUp apiDataPopUp = new ApiDataPopUp();
                apiDataPopUp.ShowDialog();
            }
        }

        private void MathButtonClick(int number)
        {
            Log.Write(GetType(), "MathButtonClick");

            if (operation == "")
            {
                num1 = (num1 * 10) + number;
                txtDisplay.Text = num1.ToString();
            }
            else
            {
                num2 = (num2 * 10) + number;
                txtDisplay.Text = $"{num1}{operation}{num2}";
            }
        }

        private void OperationButtonClick(string op)
        {
            Log.Write(GetType(), "OperationButtonClick");

            if (op == "=")
            {
                switch (operation)
                {
                    case "+": txtDisplay.Text = (num1 + num2).ToString(); break;
                    case "-": txtDisplay.Text = (num1 - num2).ToString(); break;
                    case "*": txtDisplay.Text = (num1 * num2).ToString(); break;
                    case "/": txtDisplay.Text = (num1 / num2).ToString(); break;
                }

                operation = "";
                num1 = float.Parse(txtDisplay.Text);
                num2 = 0;
            }else if (op == "C")
            {
                txtDisplay.Text = "0";
                num1 = 0;
                num2 = 0;
                operation = "";
            }
            else
            {
                operation = op;
                txtDisplay.Text = $"{num1}{op}{num2}";
            }
            
        }

    }
}
