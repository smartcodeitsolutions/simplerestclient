//------------------------------------------------------------------------------
// <copyright file="RestToolWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace SimpleRestClient
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for RestToolWindowControl.
    /// </summary>
    public partial class RestToolWindowControl : UserControl
    {
        static HttpClient httpClient = new HttpClient();
        string selectedMethod = "GET";
        HttpMethod choosedMethod = HttpMethod.Get;


        public static bool SetAllowUnsafeHeaderParsing20()
        {
            //Get the assembly that contains the internal class
            Assembly aNetAssembly = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                      BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });

                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, true);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestToolWindowControl"/> class.
        /// </summary>
        public RestToolWindowControl()
        {
            this.InitializeComponent();
            SetAllowUnsafeHeaderParsing20();
            
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Add a new Request Message
            HttpRequestMessage requestMessage = new HttpRequestMessage(choosedMethod, urlTextBox.Text.Trim());
            string data;
            string headerString = string.Empty;
            string headerValues = string.Empty;
            // Add our custom headers
            requestMessage.Headers.Add("User-Agent", "User-Agent-Here");


            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Send the request to the server
            try
            {
                HttpResponseMessage r = await httpClient.SendAsync(requestMessage);
                data = await r.Content.ReadAsStringAsync();
                responseTextBox.Text = data;
                foreach (var header in r.Content.Headers)
                {
                    headerValues = string.Empty;
                    foreach (var item in header.Value)
                    {
                        headerValues += item;
                    }

                    headerString += $"{header.Key}: {headerValues}\n";
                }

                foreach (var header in r.Headers)
                {
                    headerValues = string.Empty;
                    foreach (var item in header.Value)
                    {
                        headerValues += item;
                    }

                    headerString += $"{header.Key}: {headerValues}\n";
                }

                headersTextBox.Text = headerString;
            }
            catch (Exception ex)
            {
                MessageBox.Show("The operation cannot be completed." + ex.Message);
            }
        
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked.Value)
                //selectedMethod = "GET"; 
                choosedMethod = HttpMethod.Get;
        }

        private void radioButton_Copy_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked.Value)
                //selectedMethod = "POST";
                choosedMethod = HttpMethod.Post;
        }

        private void radioButton_Copy1_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked.Value)
                //selectedMethod = "PUT";
                choosedMethod = HttpMethod.Put;
        }

        private void radioButton_Copy2_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked.Value)
                //selectedMethod = "DELETE";
                choosedMethod = HttpMethod.Delete;
        }

        private void radioButton_Copy3_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked.Value)
                //selectedMethod = "OPTIONS";
                choosedMethod = HttpMethod.Options;
        }
    }
}