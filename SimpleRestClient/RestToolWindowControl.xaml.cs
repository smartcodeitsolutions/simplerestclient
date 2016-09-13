//------------------------------------------------------------------------------
// <copyright file="RestToolWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace SimpleRestClient
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for RestToolWindowControl.
    /// </summary>
    public partial class RestToolWindowControl : UserControl
    {
        static HttpClient httpClient = new HttpClient();
        /// <summary>
        /// Initializes a new instance of the <see cref="RestToolWindowControl"/> class.
        /// </summary>
        public RestToolWindowControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "RestToolWindow");
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpResponseMessage r;
                string data;
                switch (((ListBoxItem)methodComboBox.SelectedValue).Content.ToString())
                {
                    case "POST":

                        r = await httpClient.PostAsync(urlTextBox.Text.Trim(), new StringContent(sendTextBox.Text.Trim(), Encoding.UTF8, "application/json"));
                        data = await r.Content.ReadAsStringAsync();
                        responseTextBox.Text = data;
                        break;
                    case "GET":
                        r = await httpClient.GetAsync(urlTextBox.Text.Trim(), HttpCompletionOption.ResponseContentRead);
                        data = await r.Content.ReadAsStringAsync();
                        responseTextBox.Text = data;
                        break;
                    case "PUT":
                        r = await httpClient.PutAsync(urlTextBox.Text.Trim(), new StringContent(sendTextBox.Text.Trim(), Encoding.UTF8, "application/json"));
                        data = await r.Content.ReadAsStringAsync();
                        responseTextBox.Text = data;
                        break;
                    case "DELETE":
                        r = await httpClient.DeleteAsync(urlTextBox.Text.Trim());
                        data = await r.Content.ReadAsStringAsync();
                        responseTextBox.Text = data;
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("The operation cannot be completed.");
            }

        }
    }
}