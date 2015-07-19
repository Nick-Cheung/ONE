using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using System.Text;
using Windows.Storage;

namespace One
{
    

   
    class ViewModel
    {
         
        public bool isDataLoad;  //判断数据是否获得
        public bool isDataGet;   //判断数据是否正确处理
        public IRandomAccessStream stream;

        private string Homepage; 
        private string Content;
        private string Question;

        private HttpClient httpClient;
        private string date;


        //主页、文章和问题的api
        private const string HomepageLink = @"http://211.152.49.184:7001/OneForWeb/one/getHpinfo?strDate=";
        //private const string ContentLink = "http://211.152.49.184:7001/OneForWeb/one/getOneContentInfo?strDate=";
        private const string QuestionLink = @"http://211.152.49.184:7001/OneForWeb/one/getOneQuestionInfo?strDate=";
        private const string ContentLink = @"http://onewp.sinaapp.com/";
        

        public ViewModel()
        {

        }

        //构造函数，初始化数据
        public ViewModel(string _date)
        {
            httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 2560000;
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            one = new ONE();
            date = _date;
            one.date = _date;
            isDataLoad = true;
            isDataGet = true;
            getdatelist();
            
        }

        public ONE one { get; set; }

        //加载数据
        public async Task<bool> LoadData()
        {
                 
                //获取api上的Json数据
                Homepage = await GetDataAsync(HomepageLink + one.date);
                Content = await GetDataAsync(ContentLink + one.date + "/content.html");
                Question = await GetDataAsync(QuestionLink + one.date);

                if (isDataGet == true)
                {
                    //处理获得的Json数据
                    LoadHomepage(Homepage);
                    LoadContent(Content);
                    LoadQuestion(Question);
                }
                else
                {
                    return false;
                }
            
            if(isDataLoad == true)
            {
                //加载图片
                bool res = await LoadImage(one.HomepagestrOriginalImgUrl);
            }
            else
            {
                return false;
            }

            return true;
        }


        //获取最近八天的list列表
        private void getdatelist()
        {
            
            DateTime dt = DateTime.Now;
            string datetime;
            for(int i = 0; i > -9; i--)
            {
                datetime = dt.AddDays(i).ToString("yyyy-MM-dd");
                if (datetime != date)
                {
                    one.datelist.Add(datetime);
                }               
            }
        }


        //处理获得的Homepage的Json数据
        private void LoadHomepage(string data)
        {
            JsonObject obj = JsonObject.Parse(data);
            string result = obj.GetNamedString("result");
            if(result == "SUCCESS")
            {
                
                JsonObject hpentity = obj.GetNamedObject("hpEntity");
                one.HomepagestrAuthor = hpentity.GetNamedString("strAuthor");
                one.HomepagestrContent = "       " + hpentity.GetNamedString("strContent");
                one.HomepagestrHpTitle = hpentity.GetNamedString("strHpTitle");
                one.HomepagestrOriginalImgUrl = hpentity.GetNamedString("strOriginalImgUrl");
            }
            else
            {              
                isDataLoad = false;
            }

        }


        //处理获得的Content的Json数据
        private void LoadContent(string data)
        {
            int start_title = data.IndexOf("<h3>");
            int end_title = data.IndexOf("</h3>");
            one.ContentstrContTitle = data.Substring(start_title + 4, end_title - start_title - 4);

            int start_content = data.IndexOf("<p>");
            int end_content = data.IndexOf("</p>");
            string temp_content = data.Substring(start_content + 3, end_content - start_content - 3);

            int start_introduce = temp_content.IndexOf("<b>");
            int end_introduce = temp_content.IndexOf("</b>");

            one.ContentstrContent = temp_content.Substring(0, start_introduce).Replace("<br><br><br>", "\n\n      ").Replace("<br><br>", "\n\n      ").Replace("<br>", "\n      ");
            one.ContentstrContAuthorIntroduce = temp_content.Substring(start_introduce + 3, end_introduce - start_introduce - 3);

            string temp_author = data.Substring(end_title, start_content - end_title);
            int start_author = temp_author.IndexOf("<div>");
            int end_author = temp_author.IndexOf("</div>");
         
            one.ContentstrContAuthor = temp_author.Substring(start_author + 5, end_author - start_author - 5);

        }
        

        //处理获得的Question的Json数据
        private void LoadQuestion(string data)
        {
            JsonObject obj = JsonObject.Parse(data);
            string result = obj.GetNamedString("result");
            if (result == "SUCCESS")
            {
                JsonObject questionAdEntity = obj.GetNamedObject("questionAdEntity");
                one.strQuestionTitle = questionAdEntity.GetNamedString("strQuestionTitle");
                one.strQuestionContent = questionAdEntity.GetNamedString("strQuestionContent");
                one.strAnswerTitle = questionAdEntity.GetNamedString("strAnswerTitle");
                string temp = questionAdEntity.GetNamedString("strAnswerContent");
                temp = temp.Replace("<br><br>", "\n\n");
                temp = temp.Replace("<br>", "\n");
                temp = temp.Replace("<i>", " ");
                temp = temp.Replace("</i>", " ");
                one.strAnswerContent =temp.Replace("<br><br><br>", "\n");
            }
            else
            {
                isDataLoad = false;
            }

        }


        //异步加载图片
        private async Task<bool> LoadImage(string uriString)
        {
            try
            {
                var rass = RandomAccessStreamReference.CreateFromUri(new Uri(uriString));
                stream = await rass.OpenReadAsync();
                one.Homepageimage = new BitmapImage();
                one.Homepageimage.SetSource(stream);
                return true;
            }
            catch(Exception)
            {
                isDataLoad = false;
                return false;
            }

        }


         //异步获取api远程数据
        private async Task<String> GetDataAsync(string uriString)
        {

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(uriString);
                response.EnsureSuccessStatusCode();

                string res = await response.Content.ReadAsStringAsync();
                
                return res;
            }
            catch(Exception)
            {
                isDataGet = false;
                return string.Empty;
            }            
        }
       
    }
}
