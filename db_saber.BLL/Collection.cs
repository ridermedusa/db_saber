using db_saber.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace db_saber.BLL
{
    public  class Collection
    {
        public string WebYuMing
        {
            get { return ConfigurationManager.AppSettings["bgdomain"]; }
        } 
        public void StartCollectionRWTF()
        {
            string html = GetHtml("http://www.romaway.com/JList.shtml");
            //根据读取的网页获取第一页第一个详情的数据信息

            string pattern = "<a id=\"grdNewsList_ctl02_lnkNews\" href=\"JShowNews-(.*?).shtml\" target=\"_blank\">详细查看</a>";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m = r.Match(html);
            if (m.Success)
            {
                //string value = m.Groups[1].Value;
                ////当前获取到了文章最大值ID，开始进入对应页进行数据采集
                //int ArtID = 1;
                ////获取数据库内已获取的起点ID
                //Mpr_Article_New VaNewMod = ArticleService.FindByParam(s => s.RomawayID != null).OrderByDescending(s => s.RomawayID).FirstOrDefault();
                //if (VaNewMod != null)
                //{
                //    ArtID = VaNewMod.RomawayID.Value;
                //}
                //int MaxID = Convert.ToInt32(value);
                //while (ArtID <= MaxID)
                //{
                //    string GetHtmlArt = GetWebHtml("http://www.romaway.com/Dshownews.aspx?ID=" + ArtID);
                //    //尝试获取几个ID
                //    string titlematch = GetMatchData("<span id=\"lbtitle\">(.*?)</span>", GetHtmlArt);
                //    string sourcematch = GetMatchData("<span id=\"lbsource\">(.*?)</span>", GetHtmlArt);
                //    string datematch = GetMatchData("<span id=\"lbdate\">(.*?)</span>", GetHtmlArt);
                //    string nrmatch = GetMatchData("(?is)<span id=\"lbnr\" class=\"nr\">(.*?)</span>", GetHtmlArt);
                //    if (!string.IsNullOrEmpty(titlematch))
                //    {
                //        if (ArticleService.GetModel(s => s.RomawayID == ArtID) == null)
                //        {
                //            //写入数据库
                //            Mpr_Article_New NewMod = new Mpr_Article_New();
                //            NewMod.ID = Guid.NewGuid().ToString("N");
                //            NewMod.Type = 0;
                //            NewMod.Title = titlematch;
                //            //将正文中可能的图片信息全部替换(?i)(?<=<img\b[^>]*?src=(['""]?))(?!http://)[^'""]+(?=\1)
                //            string TrueContext = Regex.Replace(nrmatch, @"(?i)(?<=<img\b[^>]*?src=(['""]?))(?!http://)[^'""]+(?=\1)", "http://www.romaway.com$0");
                //            NewMod.ArtContent = TrueContext;
                //            NewMod.Addtime = Convert.ToDateTime(datematch);
                //            NewMod.Status = 1;
                //            NewMod.Source = sourcematch;
                //            NewMod.Adduser = 1;
                //            NewMod.RomawayID = ArtID;
                //            ArticleService.Insert(NewMod);
                //            AddErrorLog("同步百家财富信息", ArtID + "存在，已同步");
                //        }
                //        else
                //        {
                //            AddErrorLog("同步百家财富信息", ArtID + "已存在，无需同步");
                //        }
                //    }
                //    else
                //    {
                //        AddErrorLog("同步百家财富信息", ArtID + "不存在，未同步");
                //    }
                //    Mpr_ThreadInfo ThreadModEX = ThreadService.GetModel(s => s.ThreadName == "MyThread");
                //    if (ThreadModEX != null)
                //    {
                //        ThreadModEX.IsStart = 1;
                //        ThreadModEX.LastChangeTime = DateTime.Now;
                //        ThreadService.Update(ThreadModEX);
                //    }
                //    ArtID++;
                //}
            }
            else {
              
            }
        }
        public void AddErrorLog(string Type, string Context)
        { 
            Sys_Error_log Mod = new Sys_Error_log();
            Mod.ErrType = Type;
            Mod.ErrMsg = Context;
            Mod.Date = DateTime.Now; 
            new Sys_Error_logService().Insert(Mod);
        }
        public string GetMatchData(string Match, string html)
        {
            Regex r = new Regex(Match, RegexOptions.IgnoreCase);
            Match m = r.Match(html);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        public string GetHtml(string url)
        {
            WebClient wc = new WebClient();
            //下载页面源文件并将其转换成UTF8编码格式的STRING
            string mainData = Encoding.UTF8.GetString(wc.DownloadData(string.Format(url)));
            return mainData;
        }
        public string GetWebHtml(string url)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //发起请求
            //httpRequest.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            httpRequest.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            httpRequest.KeepAlive = true;
            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            httpRequest.Method = "GET";
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            int status = (int)httpResponse.StatusCode;
            StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            return result;
        }

    }
}
