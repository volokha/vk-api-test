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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace VK_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //wb.Visibility = Visibility.Visible;
            Vk.AccessToken = null;
            //wb.Navigate(String.Format("https://oauth.vk.com/authorize?client_id={0}&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=wall_type=token&v=5.52", ConfigurationSettings.AppSettings["VKAppId"], ConfigurationSettings.AppSettings["VKScope"], ConfigurationSettings.AppSettings["VKRedirectUri"]));
            wb.Navigate(String.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&display=page&response_type=token", ConfigurationSettings.AppSettings["VKAppId"], "wall", ConfigurationSettings.AppSettings["VKRedirectUri"]));
            box2.Text = (String.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&display=page&response_type=token", ConfigurationSettings.AppSettings["VKAppId"], "wall", ConfigurationSettings.AppSettings["VKRedirectUri"]));
            wb.Navigated += new NavigatedEventHandler(HandleNavigated);
            
        }
        private void HandleNavigated(object sender, NavigationEventArgs e)
        {
            var clearUriFragment = e.Uri.Fragment.Replace("#", "").Trim();
            var parameters = HttpUtility.ParseQueryString(clearUriFragment);
            Vk.AccessToken = parameters.Get("access_token");
            Vk.UserId = parameters.Get("user_id");
            infoBox.Text = Vk.AccessToken;
            //box2.Text = Vk.UserId;
            
            if (Vk.AccessToken != null && Vk.UserId != null)
            {
                Console.WriteLine("KEK");
                wb.Visibility = Visibility.Hidden;
            }
            
        }

        private string VkRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var responseText = reader.ReadToEnd();
            return responseText;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
           // var response = VkRequest(String.Format("https://api.vk.com/method/users.get?uids={0}", Vk.UserId.ToString()));
           //// box2.Text = VkRequest(String.Format("https://api.vk.com/method/users.get?uids={0}", Vk.UserId.ToString()));
           // var users = JsonConvert.DeserializeObject<VkUsers>(response);
           // box2.Text = users.response[0].first_name;

            //popitka2
            //string reqStr = string.Format(
            //"https://api.vkontakte.ru/method/wall.post?owner_id={0}=&access_token={1}&message={2}",
            //Vk.UserId, Vk.AccessToken, "Posted via Vk API");
            //var result = VkRequest(reqStr);

            Console.WriteLine(wallPost(message:"Posted via VKAPI"));

            //Console.WriteLine(VkRequest(String.Format("https://api.vk.com/method/account.setOffline?&access_token={0}", Vk.AccessToken)));
        }
        
        private void infoBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public string wallPost( int friends_only = 0, int from_group = 1, string message = "", string attachments = "", long publish_date = 0)                    //пост на стенку
        {
            if (message == "" && attachments == "") return "Error: message and attachments is empty!";                //не вызывать API, если msg and attach пустые

            string request_path = "https://api.vk.com/method/wall.post?";     //путь обращения к vk.api
            request_path += "owner_id=" + Vk.UserId;
            request_path += "&friends_only=" + friends_only;
            request_path += "&from_group=" + from_group;
            if (message != string.Empty) request_path += "&message=" + message;
            if (attachments != string.Empty) request_path += "&attachments=" + attachments;
            if (publish_date != 0) request_path += "&publish_date=" + publish_date;
            request_path += "&v=5.21";
            request_path += "&access_token=" + Vk.AccessToken;                                                          //токен (задается в конструкторе)

            return VkRequest(request_path);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            wb.Navigate("https://login.vk.com/?act=logout&hash=14466908cac58bbe4b&_origin=http://vk.com");
        }


    }
}
