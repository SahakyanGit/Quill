using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Quill.Models;

namespace Quill
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string Password { get; set; }
        private string Login { get; set; }
        List<string> _passList = new List<string>();
        List<string> _logList = new List<string>();
        private readonly string _path = @"Resources\data.json";

        public MainWindow()
        {
            InitializeComponent();
            HideAmin();
        }

        private void HideUser()
        {
            loginLabel.Visibility = Visibility.Hidden;
            passwordLabel.Visibility = Visibility.Hidden;
            signInBtn.Visibility = Visibility.Hidden;
            signUpButton.Visibility = Visibility.Hidden;
            loginTextBox.Visibility = Visibility.Hidden;
            passwordTextBox.Visibility = Visibility.Hidden;
        }

        private void ShowUser()
        {
            loginLabel.Visibility = Visibility.Visible;
            passwordLabel.Visibility = Visibility.Visible;
            signInBtn.Visibility = Visibility.Visible;
            signUpButton.Visibility = Visibility.Visible;
            loginTextBox.Visibility = Visibility.Visible;
            passwordTextBox.Visibility = Visibility.Visible;
        }

        private void HideAmin()
        {
            lengthCheckBox.Visibility = Visibility.Hidden;
            capitalCheckBox.Visibility = Visibility.Hidden;
            lowerCheckBox.Visibility = Visibility.Hidden;
            numberCheckBox.Visibility = Visibility.Hidden;
            symbolCheckBox.Visibility = Visibility.Hidden;
            backBtn.Visibility = Visibility.Hidden;
            loginsLabel.Visibility = Visibility.Hidden;
            passwordsLabel.Visibility = Visibility.Hidden;
            loginListBox.Visibility = Visibility.Hidden;
            passwordListBox.Visibility = Visibility.Hidden;

        }

        private void ShowAdmin()
        {
            lengthCheckBox.Visibility = Visibility.Visible;
            capitalCheckBox.Visibility = Visibility.Visible;
            lowerCheckBox.Visibility = Visibility.Visible;
            numberCheckBox.Visibility = Visibility.Visible;
            symbolCheckBox.Visibility = Visibility.Visible;
            backBtn.Visibility = Visibility.Visible;
            loginListBox.Visibility = Visibility.Visible;
            passwordListBox.Visibility = Visibility.Visible;
            loginsLabel.Visibility = Visibility.Visible;
            passwordsLabel.Visibility = Visibility.Visible;
            loginListBox.Visibility = Visibility.Visible;
            passwordListBox.Visibility = Visibility.Visible;
        }

        private void passwordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Password = passwordTextBox.Text;
        }

        private void loginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Login = loginTextBox.Text;
        }

        private void signInBtn_Click(object sender, RoutedEventArgs e)
        {
            bool accountExist = false;

            try
            {
                StreamReader reader = new StreamReader(_path);
                string content = reader.ReadToEnd();
                reader.Close();

                List<Account> data = JsonConvert.DeserializeObject<List<Account>>(content);

                if (data!=null)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (Login == (string)data[i].Login && Password == (string)data[i].Password)
                        {
                            accountExist = true;
                            break;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            bool isValide = Validation();
            if (isValide)
            {
                bool admin = Admin();
                if (!admin)
                {
                    if (accountExist)
                    {
                        MessageBox.Show("Opening...");
                    }
                    else
                    {
                        MessageBox.Show("Wrong login or password.Please try again.");
                    }
                }
                else
                {
                    HideUser();
                    ShowAdmin();
                }

            }
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValide = Validation();
            bool isValidePassword = false;

            if (isValide)
            {
                isValidePassword = ValidatePassword();
            }

            if (isValidePassword)
            {
                try
                {
                    Account account = new Account();
                    account.Password = Password;
                    account.Login = Login;
                    account.Id = 1;
                    string output = "";

                    StreamReader reader = new StreamReader(_path);
                    string content = reader.ReadToEnd();
                    reader.Close();

                    List<Account> data = JsonConvert.DeserializeObject<List<Account>>(content);

                    if (data == null)
                    {

                        data = new List<Account>();
                        data.Add(account);
                        output = JsonConvert.SerializeObject(data);
                    }
                    else
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            account.Id++;
                        }
                        data.Add(account);
                        output = JsonConvert.SerializeObject(data);
                    }

                    StreamWriter writer = new StreamWriter(_path);
                    writer.Write(output);
                    writer.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                _logList.Add(Login);
                _passList.Add(Password);
            }
        }

        /// <summary>
        /// Validation for empty field.
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            bool isValide = true;
            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Please enter your Login.");
                isValide = false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Please enter your Password.");
                isValide = false;
            }

            return isValide;
        }

        /// <summary>
        /// check for admin account
        /// </summary>
        /// <returns></returns>
        private bool Admin()
        {
            bool isAdmin = Login == "admlog" && Password == "admpass";
            if (isAdmin)
            {
                try
                {
                    StreamReader reader = new StreamReader(_path);
                    string content = reader.ReadToEnd();
                    reader.Close();

                    List<Account> data = JsonConvert.DeserializeObject<List<Account>>(content);

                    if (data != null)
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            loginListBox.Items.Add(data[i].Id + ". " + data[i].Login);
                            passwordListBox.Items.Add(data[i].Id + ". " + data[i].Password);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            return isAdmin;
        }

        /// <summary>
        /// back button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            HideAmin();
            ShowUser();
            loginListBox.Items.Clear();
            passwordListBox.Items.Clear();
        }

        /// <summary>
        /// Validation for password
        /// </summary>
        /// <returns></returns>
        private bool ValidatePassword()
        {
            bool isValidLength = true;
            bool containCapital = true;
            bool containLower = true;
            bool containNumber = true;
            bool contanSymbol = true;
            bool isValidPassword = false;
            if (lengthCheckBox.IsChecked == true)
            {
                isValidLength = false;
                if (Password.Length > 8)
                {
                    isValidLength = true;
                }
            }
            if (capitalCheckBox.IsChecked == true)
            {
                containCapital = false;
                for (int i = 0; i < Password.Length; i++)
                {
                    if (Password[i] >= 65 && Password[i] <= 90)
                    {
                        containCapital = true;
                        break;
                    }
                }
            }
            if (lowerCheckBox.IsChecked == true)
            {
                containLower = false;
                for (int i = 0; i < Password.Length; i++)
                {
                    if (Password[i] >= 97 && Password[i] <= 122)
                    {
                        containLower = true;
                        break;
                    }
                }
            }
            if (numberCheckBox.IsChecked == true)
            {
                containNumber = false;
                for (int i = 0; i < Password.Length; i++)
                {
                    int value;
                    bool isNumber = int.TryParse(Password[i].ToString(), out value);
                    if (isNumber)
                    {
                        containNumber = true;
                        break;
                    }
                }
            }
            if (symbolCheckBox.IsChecked == true)
            {
                contanSymbol = false;
                for (int i = 0; i < Password.Length; i++)
                {
                    if ((Password[i] >= 33 && Password[i] <= 47) || (Password[i] >= 58 && Password[i] <= 64))
                    {
                        contanSymbol = true;
                        break;
                    }
                }
            }

            if (!isValidLength)
            {
                MessageBox.Show("Passwords must be at least 8 characters in length.");
            }
            if (!containCapital)
            {
                MessageBox.Show("Password must be include at least one uppercase letter.");
            }
            if (!containLower)
            {
                MessageBox.Show("Password must be include at least one lowercase letter.");
            }
            if (!containNumber)
            {
                MessageBox.Show("Password must be include at least one number.");
            }
            if (!contanSymbol)
            {
                MessageBox.Show("Password must be include at least one special character.");
            }

            if (isValidLength && containCapital && containLower && contanSymbol && containNumber)
            {
                isValidPassword = true;
            }

            return isValidPassword;
        }
    }
}
