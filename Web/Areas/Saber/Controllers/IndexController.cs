using db_saber.BLL;
using db_saber.DAL;
using db_saber.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions; 
using System.Web;
using System.Web.Mvc;
using Web.Base;

namespace Web.Areas.Saber.Controllers
{
    public class IndexController : BaseAdminController
    {
        //
        // GET: /Saber/Index/ 
        Sys_MasterService MenuService = new Sys_MasterService();
        Sys_MasterRepository Sys_MasterDal = new Sys_MasterRepository();
        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            ViewBag.pageIndex = pageIndex;
            int pageSize = 5;
            int totalCount = 0;
            List<Sys_Master> Pglist = Sys_MasterDal.GetPage(1481, pageIndex, pageSize, ref totalCount);
            ViewBag.totalCount = totalCount;
            var personsAsIPagedList = new StaticPagedList<Sys_Master>(Pglist, pageIndex, pageSize, totalCount);
            return View(personsAsIPagedList); 
        }



        public void CreateGetHtml()
        {
            Sys_MasterService MasterService = new Sys_MasterService();
            for (int i = 1; i <= 53; i++)
            {

                string html = new Collection().GetHtml("https://tieba.baidu.com/p/1065980221?pn=" + i);
                string pattern = "<div class=\"l_post l_post_bright j_l_post clearfix  \"  data-field='(.*?)' >";
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection m = r.Matches(html);
                if (m.Count > 0)
                {
                    foreach (Match item in m)
                    {
                        string value = HtmlDecode(item.Groups[1].Value);
                        try
                        {
                            db_SaberMaster Master = Newtonsoft.Json.JsonConvert.DeserializeObject<db_SaberMaster>(value);
                            //开始生成数据
                            Sys_Master MasterVa = MasterService.GetModel(s => s.post_no == Master.content.post_no);
                            if (MasterVa == null)
                            {
                                //开始新建数据
                                MasterVa = new Sys_Master();
                                MasterVa.ID = Guid.NewGuid().ToString("N");
                                MasterVa.DataField = value;
                                MasterVa.user_id = Master.author.user_id;
                                MasterVa.user_name = Unescape(Master.author.user_name);
                                MasterVa.props = Newtonsoft.Json.JsonConvert.SerializeObject(Master.author.props);
                                MasterVa.post_id = Master.content.post_id;
                                MasterVa.is_anonym = Master.content.is_anonym;
                                MasterVa.forum_id = Master.content.forum_id;
                                MasterVa.thread_id = Master.content.thread_id;
                                MasterVa.contenttext = Unescape(Master.content.content);
                                MasterVa.post_no = Master.content.post_no;
                                MasterVa.type = Master.content.type;
                                MasterVa.comment_num = Master.content.comment_num;
                                MasterVa.post_index = Master.content.post_index;
                                MasterVa.pb_tpoint = Master.content.pb_tpoint;
                                MasterService.Insert(MasterVa);
                            }
                            else
                            {
                                AddErrorlog("数据采集失败", "数据已成功");
                            }
                        }
                        catch (Exception ex)
                        {
                            AddErrorlog("数据采集失败", value);
                        }

                    }
                }
            }
        }
        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlDecode(string html)
        {
           // return Regex.Unescape(html);
            return HttpUtility.HtmlDecode(html);//System.Net.WebUtility.HtmlDecode(html);  
        }
        public static string Unescape(string html)
        {
           return Regex.Unescape(html);
        }
    }
}
