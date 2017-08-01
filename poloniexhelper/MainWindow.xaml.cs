using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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

namespace poloniexhelper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



            string json = @"{
  'Email': 'james@example.com',
  'Active': true,
  'CreatedDate': '2013-01-20T00:00:00Z',
  'Roles': [
    'User',
    'Admin'
  ]
}";

            Account account = JsonConvert.DeserializeObject<Account>(json);
            MessageBox.Show(account.Email);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tb2.Text = "";
            string para = tb1.Text.ToUpper();


            int unixTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds - 310;



            WebClient client = new WebClient();
            var url = "https://poloniex.com/public?command=returnOrderBook&currencyPair=BTC_" + para + "&depth=1";
            var response = client.DownloadString(url);
            //  var  last_upd = System.Text.Encoding.UTF8.GetString(response);
            //  var trim = last_upd.Substring(1, last_upd.Length - 2);

            var m = JsonConvert.DeserializeObject<returnChartData>(response);


            //  

            //        tb2.Text = m.asks[0][0].ToString();


            // tb3.Text

            double balance = Convert.ToDouble((pok1.Text).Replace('.', ','));
            pok2.Text = (balance * 0.1).ToString("0.00000000");

            double dvaproc = Convert.ToDouble((m.asks[0][0]).Replace('.', ','));
            tb2.Text = dvaproc.ToString("0.00000000");
            pok4.Text = (dvaproc * 1.02).ToString("0.00000000");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string site = "https://poloniex.com/tradingApi";
            string key = "G8PXBGX4-U5J21BS2-0265V2OK-WLTEL86R";
            string secret = "8c5bf988ae9dab073ba61ae3f5901d7f5c9119f3a0934e41a5af90bd44c145a472843ab927ddab1c12e0a50fcb78c5d3c287093c891e0b6f62ccec361740a594";
            string unixTime = (((DateTime.Now - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString()).Replace(",", "");
            string command = "command=returnBalances&nonce=" + unixTime + "1";
            string hash = BitConverter.ToString(hmacSHA256(command, secret)).Replace("-", "").ToLower();













            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);


            req.Headers.Add("Key: " + key);
            req.Headers.Add("Sign: " + hash);

            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = command.Length;

        
            using (var writer = new StreamWriter(req.GetRequestStream()))
            {
                writer.Write(command);
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                MessageBox.Show(stream.ReadToEnd());
            }





        }

        static byte[] hmacSHA256(String data, String key)
        {
            using (HMACSHA512 hmac = new HMACSHA512(Encoding.ASCII.GetBytes(key)))
            {
                return hmac.ComputeHash(Encoding.ASCII.GetBytes(data));
            }
        }
    }


    class returnChartData
    {

        public IList<IList<string>> asks;
        public IList<IList<string>> bids;
        public string isFrozen;
        public string seq;


    }



    public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
    }
}
