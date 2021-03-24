using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Navigation;

namespace FloorSweep.Monitor.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _tokenUrl;
        private string _challenge = "fIlpoFwEhSGtAAIpyiPfGlq8ti7DjT2o6sTolTxmkdfdsfdsfdsfs";

        public MainWindow()
        {
            InitializeComponent();
        }

        class Handler : HttpClientHandler
        {
            public Handler()
            {
                ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true;
            }
        }
        private async void OnActivated(object sender, EventArgs e)
        {
            try
            {
                var client = new HttpClient(new Handler());
                var result = await client.GetAsync("https://floorsweep.com:8443/auth/realms/master/.well-known/openid-configuration");
                dynamic json = JObject.Parse(await result.Content.ReadAsStringAsync());
                var authUrl = (string)json.authorization_endpoint;
                _tokenUrl = (string)json.token_endpoint;
                var sha = Base64UrlEncoder.Encode(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(_challenge)));
                var url = $"{authUrl}?response_type=code&client_id=robot-client&redirect_uri=https%3A%2F%2Flocalhost%2FMonitorToken.html&scope=monitor-view%20openid&state=TW9uIE1hciAyMiAyMDIxIDE2OjE3OjM2IEdNVCswMTAwIChNaWRkZW4tRXVyb3Blc2Ugc3RhbmRhYXJkdGlqZCk%3D&realm=master&code_challenge={sha}&code_challenge_method=S256";
                //var loginScreen = await client.GetAsync(url);
                //webBrowser.Navigate(url);
                Process.Start(new ProcessStartInfo {FileName = url,UseShellExecute=true });
            }
            catch
            {

            }
        }

        private async void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            
            if (e.Uri.GetLeftPart(UriPartial.Path) == "https://localhost/MonitorToken.html")
            {
                var queries = HttpUtility.ParseQueryString(e.Uri.Query);
                if (queries.AllKeys.Contains("code"))
                {
                    var client = new HttpClient(new Handler());
                    var code = queries.GetValues("code").FirstOrDefault();
                    if (code != null)
                    {
                        var content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                            { "code_verifier",_challenge },
                        {"code", code},
                        { "redirect_uri","https://localhost/MonitorToken.html"},
                        { "client_secret","9f3e6864-aca6-4c3b-930b-0d8267acc656"},
                        { "client_id" , "robot-client" },
                        { "grant_type", "authorization_code" } });
                        var tokenResult = await client.PostAsync(_tokenUrl, content);
                        dynamic tokenresp = JObject.Parse(await tokenResult.Content.ReadAsStringAsync());
                        
                    }


                    e.Cancel = true;
                }
            }
        }
    }
}
