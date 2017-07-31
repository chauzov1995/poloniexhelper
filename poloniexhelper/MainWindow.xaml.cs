using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


            int unixTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds- 310;



            WebClient client = new WebClient();
            var url = "https://poloniex.com/public?command=returnOrderBook&currencyPair=BTC_"+ para + "&depth=1" ;
           var  response = client.DownloadString(url);
            //  var  last_upd = System.Text.Encoding.UTF8.GetString(response);
            //  var trim = last_upd.Substring(1, last_upd.Length - 2);
           
    var m = JsonConvert.DeserializeObject<returnChartData>(response);

      
            //  
            
                tb2.Text = m.asks[0][0].ToString();


            tb3.Text
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
