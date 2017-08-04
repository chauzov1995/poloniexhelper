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
            List<string> valute = new List<string>() { "ETH", "XEM", "NXT", "XRP", "LTC", "DGB", "BTS", "DASH", "ETC", "STRAT", "STR", "SC", "GNT", "XMR", "FCT", "ZEC", "LBC", "MAID", "LSK", "ARDR", "STEEM", "DOGE", "GAME", "REP", "BURST", "SYS", "CLAM", "BCY", "VRC", "DCR", "BCN", "EMC2", "PPC", "NAUT", "AMP", "GNO", "XBC", "PASC", "VIA", "VTC", "XCP", "FLDC", "POT", "OMNI", "NXC", "BTM", "SJCX", "BTCD", "NEOS", "EXP", "BLK", "GRC", "BELA", "PINK", "NMC", "FLO", "RADS", "XPM", "HUC", "XVC", "NOTE", "NAV", "SBD", "RIC" };
            for (int i = 0; i < valute.Count; i++)
            {
                valuteall.Add(new valure() { name = valute[i], ordersozd = "" });

            }



            string zapros = zpros_poloniex("command=returnOpenOrders&currencyPair=all");
            Dictionary<string, List<asdasd>> values = JsonConvert.DeserializeObject<Dictionary<string, List<asdasd>>>(zapros);
            // MessageBox.Show(zapros);
            // bb.Text = zapros;
            // MessageBox.Show(values["BTC_ETH"][0].rate);


            zapros = zpros_poloniex("command=returnBalances");
            Dictionary<string, string> values2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(zapros);
            MessageBox.Show(zapros);

            for (int i = 0; i < valuteall.Count(); i++)
            {


                if ((values["BTC_" + valuteall[i].name]).Count > 0)
                {
                    valuteall[i].rate = values["BTC_" + valuteall[i].name][0].rate;
                    valuteall[i].ordersozd = "#FF00E00A";
                    valuteall[i].sost = "3";
                    valuteall[i].orderNumber = values["BTC_" + valuteall[i].name][0].orderNumber;
                }
                else
                {
                    if ((values2[valuteall[i].name]) != "0.00000000")
                    {

                        valuteall[i].amount = values2[valuteall[i].name];
                        valuteall[i].ordersozd = "#FF0FA8FF";
                        valuteall[i].sost = "2";
                    }
                    else
                    {
                        //ещё 1 условие для выставленоого ордера 1 но такого не должно быть

                        //  valuteall[i].amount = values2[valuteall[i].name];
                        //  valuteall[i].ordersozd = "#FF0FA8FF";
                        valuteall[i].sost = "0";
                    }
                }


            }


            lb1.ItemsSource = valuteall;







        }

        class asdasd
        {

            public string rate { get; set; }
            public string orderNumber { get; set; }
        }

        public class valure
        {
            public string name { get; set; }
            public string ordersozd { get; set; }
            public string rate { get; set; }
            public string sost { get; set; }//0- нет ничего, 1-ордер покупка, 2-валюта куплена но не созд ордер, 3-ордер продажа
            public string orderNumber { get; set; }
            public string amount { get; set; }



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


            string zapros = zpros_poloniex("command=buy&currencyPair=BTC_FCT&rate=" + rate + "&amount=" + amount);

            MessageBox.Show("Валюта " + para + "\r\nКуплено по цене " + rate + "\r\nВ колличестве  " + amount + "\r\nНа сумму " + totalstring);
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

            obnovitb();

        }


        void obnovitb()
        {

            string para = (lb1.SelectedItem as valure).name;
            tb1.Text = para;

            if ((lb1.SelectedItem as valure).sost == "3")
            {

                //ордер за сколько продаю
                string zapros = zpros_poloniex("command=returnOpenOrders&currencyPair=BTC_" + para);
                var m = JsonConvert.DeserializeObject<List<order>>(zapros);
                ordertb1.Text = m[0].rate;

                //ордер куплен был за сколько
                zapros = zpros_poloniex("command=returnTradeHistory&currencyPair=BTC_" + para + "&start=1501552278");
                m = JsonConvert.DeserializeObject<List<order>>(zapros);
                orderkupl.Text = m[0].rate;


            }

            if ((lb1.SelectedItem as valure).sost == "2")
            {
                //ордер куплен был за сколько
                string zapros = zpros_poloniex("command=returnTradeHistory&currencyPair=BTC_" + para + "&start=1501552278");
                var m = JsonConvert.DeserializeObject<List<order>>(zapros);
                orderkupl.Text = m[0].rate;

                double pokupka2per = Convert.ToDouble((m[0].rate).Replace('.', ','));
                pokupka2per *= 1.02;

                zaskopok.Text = MyToString(pokupka2per);

                //  MessageBox.Show("asdas");



            }


            if ((lb1.SelectedItem as valure).sost == "0")
            {
                WebClient client = new WebClient();
                var url = "https://poloniex.com/public?command=returnOrderBook&currencyPair=BTC_" + para + "&depth=1";
                var response = client.DownloadString(url);
                //  var  last_upd = System.Text.Encoding.UTF8.GetString(response);
                //  var trim = last_upd.Substring(1, last_upd.Length - 2);

                var m = JsonConvert.DeserializeObject<returnChartData>(response);




                zenapokupk.Text = m.asks[0][0];

                double balance = Convert.ToDouble((pok1.Text).Replace('.', ','));

                double total = balance * 0.1;//10 проц от всей суммы
                stavka10per.Text = MyToString(total);






            }



        }

        class order
        {
            public string rate { get; set; }
            public string type { get; set; }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string zapros = zpros_poloniex("command=returnOpenOrders&currencyPair=all");




            string[] stringSeparators = new string[] { @""":[{""" };

            string[] splitkod = zapros.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

            string asdasd = splitkod[0].Split('_').Last();



            MessageBox.Show(asdasd.ToString());


        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            string orderNumber = (lb1.SelectedItem as valure).orderNumber;
            //  MessageBox.Show(orderNumber);
            string zapros = zpros_poloniex("command=cancelOrder&orderNumber=" + orderNumber);


            MessageBox.Show(zapros);

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            //продажа
            string para = tb1.Text.ToUpper();


            // double price = pokupkazena;
            // double total = balance * 0.1;//10 проц от всей суммы



            string rate = zaskopok.Text;
            string amount = (lb1.SelectedItem as valure).amount;
            // string totalstring = MyToString(total);


            string zapros = zpros_poloniex("command=sell&currencyPair=BTC_" + para + "&rate=" + rate + "&amount=" + amount);

            MessageBox.Show("Валюта " + para + "\r\nКуплено по цене " + rate + "\r\nВ колличестве  " + amount + "\r\nНа сумму ");
            MessageBox.Show(zapros);

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {






            //покупка

            string para = tb1.Text.ToUpper();
            double price = Convert.ToDouble((zenapokupk.Text).Replace('.', ','));
            double total = Convert.ToDouble((stavka10per.Text).Replace('.', ','));



            string rate = zenapokupk.Text;
            string amount = MyToString(total / price);
            string totalstring = stavka10per.Text;


            string zapros = zpros_poloniex("command=buy&currencyPair=BTC_" + para + "&rate=" + rate + "&amount=" + amount);

            MessageBox.Show("Валюта " + para + "\r\nКуплено по цене " + rate + "\r\nВ колличестве  " + amount + "\r\nНа сумму " + totalstring);
            MessageBox.Show(zapros);




        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            obnovitb();
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
    class returnOpenOrders
    {


        public IList<string> BTC_ETH { get; set; }


    }
}
