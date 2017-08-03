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

     





            List<valure> valuteall = new List<valure>();
            List<string> valute=new List<string>(){ "ETH", "XEM", "NXT", "XRP", "LTC", "DGB", "BTS", "DASH", "ETC", "STRAT", "STR", "SC", "GNT", "XMR", "FCT", "ZEC", "LBC", "MAID", "LSK", "ARDR", "STEEM", "DOGE", "GAME", "REP", "BURST", "SYS", "CLAM", "BCY", "VRC", "DCR", "BCN", "EMC2", "PPC", "NAUT", "AMP", "GNO", "XBC", "PASC", "VIA", "VTC", "XCP", "FLDC", "POT", "OMNI", "NXC", "BTM", "SJCX", "BTCD", "NEOS", "EXP", "BLK", "GRC", "BELA", "PINK", "NMC", "FLO", "RADS", "XPM", "HUC", "XVC", "NOTE", "NAV", "SBD", "RIC" };
            for(int i = 0; i < valute.Count; i++)
            {
                valuteall.Add(new valure() { name = valute[i], ordersozd = "" });

            }


            string zapros = zpros_poloniex("command=returnOpenOrders&currencyPair=all");
            string[] stringSeparators = new string[] { @""":[{""" };
            string[] splitkod = zapros.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            string asdasd = splitkod[0].Split('_').Last();

          //  valuteall.Find(x => x.name.Equals(asdasd)).ordersozd = "zel";

            MessageBox.Show(valuteall[3].name.ToString());






            lb1.ItemsSource = valuteall;
            MessageBox.Show(valute[0]);

        }

        public class valure
        {
            public string name;
            public string ordersozd;


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


            // 

            double balance = Convert.ToDouble((pok1.Text).Replace('.', ','));
            pok2.Text = (balance * 0.1).ToString("0.00000000");

            double dvaproc = Convert.ToDouble((m.asks[0][0]).Replace('.', ','));
            tb2.Text = dvaproc.ToString("0.00000000");
            pok4.Text = (dvaproc * 1.02).ToString("0.00000000");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            //покупка
            double price = 0.00703233;
            double total = 0.00010011;

            string amount = (Math.Floor((total / price) * 100000000) / 100000000).ToString().Replace(',', '.');
            MessageBox.Show(amount);
            string zapros = zpros_poloniex("command=buy&currencyPair=BTC_FCT&rate=0.00703233&amount=" + amount);
            MessageBox.Show(zapros);


        }


        string zpros_poloniex(string str)
        {

            string site = "https://poloniex.com/tradingApi";
            string key = "G8PXBGX4-U5J21BS2-0265V2OK-WLTEL86R";
            string secret = "8c5bf988ae9dab073ba61ae3f5901d7f5c9119f3a0934e41a5af90bd44c145a472843ab927ddab1c12e0a50fcb78c5d3c287093c891e0b6f62ccec361740a594";
            string unixTime = (((DateTime.Now - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString()).Replace(",", "");
            string command = str + "&nonce=" + unixTime + "1";
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
                return stream.ReadToEnd();
            }



        }





        static byte[] hmacSHA256(String data, String key)
        {
            using (HMACSHA512 hmac = new HMACSHA512(Encoding.ASCII.GetBytes(key)))
            {
                return hmac.ComputeHash(Encoding.ASCII.GetBytes(data));
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            tb2.Text = "";
            string para = tb1.Text.ToUpper();


            //   int unixTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds - 310;



            WebClient client = new WebClient();
            var url = "https://poloniex.com/public?command=returnOrderBook&currencyPair=BTC_" + para + "&depth=1";
            var response = client.DownloadString(url);
            //  var  last_upd = System.Text.Encoding.UTF8.GetString(response);
            //  var trim = last_upd.Substring(1, last_upd.Length - 2);

            var m = JsonConvert.DeserializeObject<returnChartData>(response);


            //  

            //        tb2.Text = m.asks[0][0].ToString();


            // 

            double balance = Convert.ToDouble((pok1.Text).Replace('.', ','));
            double pokupkazena = Convert.ToDouble((m.asks[0][0]).Replace('.', ','));
            double prodajazena = pokupkazena * 1.02;

            pok2.Text = MyToString(balance);
            tb2.Text = MyToString(pokupkazena);
            pok4.Text = MyToString(prodajazena);






            //покупка
            double price = pokupkazena;
            double total = balance * 0.1;//10 проц от всей суммы



            string rate = MyToString(price);
            string amount = MyToString(total / price);
            string totalstring = MyToString(total);
            

            string zapros = zpros_poloniex("command=buy&currencyPair=BTC_FCT&rate="+rate+"&amount=" + amount);

            MessageBox.Show("Валюта "+ para + "\r\nКуплено по цене "+ rate + "\r\nВ колличестве  "+ amount+"\r\nНа сумму "+ totalstring);
            MessageBox.Show(zapros);



        }

        string MyToString(double zifr)
        {

            return (Math.Floor(zifr * 100000000) / 100000000).ToString().Replace(',', '.');

        }

        class currencyPair
        {
            public string asks;
            public string bids;

        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb1.Text = ((sender as ListBox).SelectedValue.ToString());
          
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string zapros = zpros_poloniex("command=returnOpenOrders&currencyPair=all");
        



            string[] stringSeparators = new string[] { @""":[{""" };

            string[] splitkod = zapros.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

            string asdasd = splitkod[0].Split('_').Last();

            

            MessageBox.Show(asdasd.ToString());

           
        }
    }


    class returnChartData
    {

        public IList<IList<string>> asks;
        public IList<IList<string>> bids;
        //    public string 1CR;
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
