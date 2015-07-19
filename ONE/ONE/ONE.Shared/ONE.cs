using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace One
{

    public class ONE
    {

        public ONE()
        {
            datelist = new List<string>();
        }

        public List<string> datelist { get; set; }

        /// <summary>
        /// 首页Json格式
        /// {"result":"SUCCESS",
        /// "hpEntity":{
        ///     "strAuthor":"光年2011&绘图\/枣",
        ///     "strContent":"。 by 河井宽次郎",
        ///     "strHpId":"604",
        ///     "strHpTitle":"VOL.587",
        ///     "strMarketTime":"2014-05-17",
        ///     "strOriginalImgUrl":"http:\/\/pic.yupoo.com\/hanapp\/DKIwiYM7\/f2Fs8.jpg",
        ///     "strThumbnailUrl":"http:\/\/pic.yupoo.com\/hanapp\/DKIwiYM7\/f2Fs8.jpg",
        /// }}
        /// </summary>
        public string date{get;set;}        //日期yyyy-mm-dd
        public string HomepagestrAuthor { get; set; }  //首页图片作者

        public string HomepagestrContent { get; set; }  //首页句子内容

        public string HomepagestrHpTitle { get; set; }  //首页期数

        public string HomepagestrOriginalImgUrl { get; set; }  //首页图片链接


        public BitmapImage Homepageimage { get; set; }

        /// <summary>
        /// {"result":"SUCCESS",
        /// "contentEntity":{
        ///     "sGW":"我相信，人生下来，以为是完整吗，其实是为了分裂。",
        ///     "strContAuthor":"周耀辉",
        ///     "strContAuthorIntroduce":"（责任编辑：薛诗汉）",
        ///     "strContDayDiffer":"5",
        ///     "strContMarketTime":"2014-05-15",
        ///     "strContTitle":"骨",
        ///     "strContent":"balabala"
        /// }}
        /// </summary>
        public string ContentstrContTitle { get; set; }   //文章标题

        public string ContentstrContAuthor { get; set; }  //文章作者

        public string ContentstrContAuthorIntroduce { get; set; }  //文章编辑

        public string ContentstrContent { get; set; }  //文章内容

        public string ContentsGW { get; set; }  //文章句子

        

        /// <summary>
        /// {"result":"SUCCESS",
        /// "questionAdEntity":{
        ///     "strAnswerContent"",
        ///     "strAnswerTitle":"网友答@一个App工作室：",
        ///     "strQuestionContent":"@一个App工作室问网友：你有哪些无聊的技能？",
        ///     "strQuestionTitle":"你有哪些无聊的技能？"
        /// }}
        /// </summary>
        public string strQuestionTitle { get; set; }  //问题标题

        public string strQuestionContent { get; set; }  //问题内容

        public string strAnswerTitle { get; set; }  //回答标题
        public string strAnswerContent { get; set; }  //回答内容

    }
}
