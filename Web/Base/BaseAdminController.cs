using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using db_saber.BLL;
using System.Data;
using db_saber.Model;
using System.ComponentModel;
using Aspose.Cells;
using db_saber.Tools;
using Newtonsoft.Json;
using PagedList;
using PlainElastic.Net.Queries;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;

namespace Web.Base
{
    public class BaseAdminController : Controller
    {
        #region 定义公司ID数据 
        int ComID = 1;
        #endregion
        #region 自定义变量   
        Jpush JpushService = new Jpush();

        #endregion
        #region 所有action 调用 身份验证
        /// <summary>
        /// 不需要需要登录的action等验证
        /// </summary>
        public Dictionary<string, List<string>> nologinaction
        {
            get
            {
                if (Session["_admin_login_action"] == null)
                {
                    Dictionary<string, List<string>> temp = new Dictionary<string, List<string>>();
                    temp["login"] = new List<string>();
                    temp["common"] = new List<string>();
                    Session["_admin_login_action"] = temp;
                }
                return Session["_admin_login_action"] as Dictionary<string, List<string>>;
            }
        }
        //所有action 调用 身份验证
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string Area = RouteData.DataTokens["area"].ToString();
            string controller = RouteData.Values["controller"].ToString().ToLower();
            string action = RouteData.Values["action"].ToString().ToLower();
            bool blnNoRight = false;
            string agent = filterContext.HttpContext.Request.UserAgent.ToLower();
            int isWeixin = agent.IndexOf("micromessenger");
            //如果当前没在微信内，又要进来，那就先登录
            //if (isWeixin == -1 && Area == "WeChat")
            //{
            //    filterContext.Result = new RedirectResult("/Admin");
            //    return;
            //}
            if (!nologinaction.Keys.Contains(controller))
            {
                if (Area == "Admin")
                {
                    
                }
                if (controller == "login")
                    blnNoRight = false;
            }
            if (controller == "app")
            {
                blnNoRight = false;
            }
            //无权限跳转
            if (blnNoRight && Area == "Admin")
            {
                //Response.Write(string.Format("<script>window.top.location.href='/Login?returnurl={0}';</script>", Server.UrlEncode(Request.Url.ToString())));
                //Response.Redirect(string.Format("/Login?returnurl={0}", Server.UrlEncode(Request.Url.ToString())));
                //Response.End();
                filterContext.Result = new RedirectResult("/Admin/Login");
                return;
            }

        }
        #endregion
        
        #region 自定义方法汇总
        public string ReturnMsg(string code, string errmsg)
        {
            return "{\"status\":\"" + code + "\",\"errmsg\":\"" + errmsg + "\"}";
        }
        public void ReturnR(string msg, string jumppage)
        {
            Response.Write("<script>alert('" + msg + "');location.href='" + jumppage + "';</script>");
        }
        /// <summary>
        ///  处理返回信息
        /// </summary>
        /// <param name="ToJsonData"></param>
        /// <returns></returns>
        public string ToJson(object ToJsonData)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(ToJsonData);
        }
        #region 自定义返回类
        /// <summary>
        /// 基础类
        /// </summary>
        public class ReturnJson
        {
            /// <summary>
            /// 返回错误码
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 返回错误信息
            /// </summary>
            public string Errmsg { get; set; }
        }
        public class ReturnObjJson : ReturnJson
        {
            public object Data { get; set; }
        }
        /// <summary>
        ///  返回HTML时
        /// </summary>
        public class ReturnHtml : ReturnJson
        {
            public string HtmlInfo { get; set; }
        }
        public class ReturnMod<T> : ReturnJson
        {
            public T Mod { get; set; }
        }
        /// <summary>
        /// 返回基础信息
        /// </summary>
        public class ReturnObject : ReturnJson
        {
            public int PageCount { get; set; }
            public int ItemCount { get; set; }
            public object GetModel { get; set; }
            public object GetInList { get; set; }
            public string Model { get; set; }
        }
        /// <summary>
        /// 带链接的基础信息
        /// </summary>
        public class ReturnObjectByLink : ReturnJson
        {
            public object GetModel { get; set; }

            public string GetLink { get; set; }
        }
        /// <summary>
        /// 自定义错误日志
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="value"></param>
        public void AddMyErrorLog(string Type, string value)
        {
            Sys_Error_log ErrorLog = new Sys_Error_log();
            ErrorLog.ErrType = Type;
            ErrorLog.ErrMsg = value;
            ErrorLog.Date = DateTime.Now;
            new Sys_Error_logService().Insert(ErrorLog);
        }
        /// <summary>
        ///  获取分页数量
        /// </summary>
        /// <param name="ItemCount"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public int GetPageCount(int ItemCount, int PageNum)
        {
            return Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(ItemCount) / PageNum));
        }
        #endregion

        #endregion
        
        #region 日志类信息操作
        /// <summary>
        ///填写一条报错数据
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Context"></param>
        public static void AddErrorlog(string Type, string Context)
        {
            Sys_Error_logService LogService = new Sys_Error_logService();
            Sys_Error_log ErrLog = new Sys_Error_log();
            ErrLog.ErrType = Type;
            ErrLog.ErrMsg = Context;
            ErrLog.Date = DateTime.Now; 
            LogService.Insert(ErrLog);
        }
    
        #endregion

        public static bool IsEquals<T>(T obj1, T obj2)
        {
            Type type1 = obj1.GetType();
            Type type2 = obj2.GetType();

            System.Reflection.PropertyInfo[] properties1 = type1.GetProperties();
            System.Reflection.PropertyInfo[] properties2 = type2.GetProperties();

            bool IsMatch = true;
            for (int i = 0; i < properties1.Length; i++)
            {
                string s = properties1[i].DeclaringType.Name;
                if (properties1[i].GetValue(obj1, null).ToString() != properties2[i].GetValue(obj2, null).ToString())
                {
                    IsMatch = false;
                    break;
                }
            }

            return IsMatch;
        }


        public List<ChangeItem> GetChangeItem<T>(T newmod, T oldmod, string[] ExcludeList,string ID)
        {
            List<ChangeItem> Changelist = new  List<ChangeItem>();
            try
            {
                foreach (var mItem in typeof(T).GetProperties())
                {
                    if (ExcludeList.Contains(mItem.Name))
                    {
                        continue;
                    }
                    else
                    {
                        object olditem = mItem.GetValue(oldmod, new object[] { });
                        object newitem = mItem.GetValue(newmod, new object[] { });
                        //判断是否赋值
                        if (newitem != null)
                        {
                            //判断新得到的值是否为NULL
                            if (newitem != olditem)
                            {
                                //判断数值是否改变  如果改变则直接将新值赋到上面
                                ChangeItem ChangeItem = new ChangeItem();
                                ChangeItem.ID = ID;
                                ChangeItem.Field = mItem.Name;
                                ChangeItem.OldItem = olditem;
                                ChangeItem.NewItem = newitem;
                                Changelist.Add(ChangeItem);
                                //mItem.SetValue(oldmod, mItem.GetValue(newmod, new object[] { }), null);
                            }
                        }
                    }
                } 
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Changelist;
        }

        /// <summary>
        /// 反射实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pTargetObjSrc"></param>
        /// <param name="pTargetObjDest"></param>
        public void EntityToEntity<T>(T newmod, ref T oldmod, string[] ExcludeList)
        {
            try
            {
                foreach (var mItem in typeof(T).GetProperties())
                {
                    if (ExcludeList.Contains(mItem.Name))
                    {
                        continue;
                    }
                    else
                    {
                        object olditem = mItem.GetValue(oldmod, new object[] { });
                        object newitem = mItem.GetValue(newmod, new object[] { });
                        //判断是否赋值
                        if (newitem != null)
                        {
                            //判断新得到的值是否为NULL
                            if (newitem != olditem)
                            {
                                //判断数值是否改变  如果改变则直接将新值赋到上面
                                mItem.SetValue(oldmod, mItem.GetValue(newmod, new object[] { }), null);
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        /// <summary>
        /// 反射实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pTargetObjSrc"></param>
        /// <param name="pTargetObjDest"></param>
        public void EntityToEntity<T>(T newmod, ref T oldmod)
        {
            try
            {
                foreach (var mItem in typeof(T).GetProperties())
                {
                    if (mItem.Name == "Addtime" || mItem.Name == "Adduser" || mItem.Name == "ID" || mItem.Name == "AdduserName")
                    {
                        continue;
                    }
                    else if (mItem.Name == "Restoretime" || mItem.Name == "Backtime")
                    {
                        continue;
                    }
                    else
                    {
                        object olditem = mItem.GetValue(oldmod, new object[] { });
                        object newitem = mItem.GetValue(newmod, new object[] { });
                        //判断是否赋值
                        if (newitem != null)
                        {
                            //判断新得到的值是否为NULL
                            if (newitem != olditem)
                            {
                                //判断数值是否改变  如果改变则直接将新值赋到上面
                                mItem.SetValue(oldmod, mItem.GetValue(newmod, new object[] { }), null);
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        /// <summary>
        /// 反射实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pTargetObjSrc"></param>
        /// <param name="pTargetObjDest"></param>
        public void EntityToEntityUpdateNull<T>(T newmod, ref T oldmod)
        {
            try
            {
                foreach (var mItem in typeof(T).GetProperties())
                {
                    if (mItem.Name == "Addtime" || mItem.Name == "Adduser" || mItem.Name == "ID" || mItem.Name == "FirstTime")
                    {
                        continue;
                    }
                    else if (mItem.Name == "Changetime" || mItem.Name == "Restoretime" || mItem.Name == "Backtime")
                    {
                        continue;
                    }
                    else
                    {
                        object olditem = mItem.GetValue(oldmod, new object[] { });
                        object newitem = mItem.GetValue(newmod, new object[] { });
                        //判断是否赋值

                        //判断新得到的值是否为NULL
                        if (newitem != olditem)
                        {
                            //判断数值是否改变  如果改变则直接将新值赋到上面
                            mItem.SetValue(oldmod, mItem.GetValue(newmod, new object[] { }), null);
                        }
                    }
                }
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        
        
        
        
        
        #region  SignalR Send
        public void SignalRSend(string ConnectionId, string value)
        {
            SignalRChat.ChatHub.ChatTicker.Instance.GlobalContext.Clients.Client(ConnectionId).SendMessage(value); //.SendData(ConnectionId, DateTime.Now.ToString());
        }
        #endregion
        #region Elasticsearch
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <param name="ID"></param>
        /// <param name="IndexName"></param>
        /// <param name="IndexType"></param>
        public void CreateDataIndex<T>(T Model,string ID,string IndexName,string IndexType)
        {
            var result = Amy.Toolkit.PlainElastic.ElasticSearchHelper.Intance.Index(IndexName, IndexType, ID, Model);
        }
        public void DeleteDataIndex(string ID, string IndexName, string IndexType)
        {
            Amy.Toolkit.PlainElastic.ElasticSearchHelper.Intance.Delete(IndexName, IndexType, ID);
        }
     

        #endregion
        
    }
}
 