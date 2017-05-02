using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_saber.Model
{

    public class db_SaberMaster
    {
        public authorC author { get; set; }
        public contentC content { get; set; }

        public class authorC
        {
            public string user_id { get; set; }
            public string user_name { get; set; }

            public object props { get; set; }
        }

        public class contentC
        {
            public string post_id { get; set; }
            public string is_anonym { get; set; }
            public string forum_id { get; set; }
            public string thread_id { get; set; }
            public string content { get; set; }
            public int post_no { get; set; }
            public int type { get; set; }
            public int comment_num { get; set; }
            public string props { get; set; }
            public int post_index { get; set; }
            public string pb_tpoint { get; set; }
        }
    }
     
    public class ChangeItem
    {
        public string ID { get; set; }

        public string Field { get; set; }

        public object OldItem { get; set; }
        public object NewItem { get; set; }

    }
    /// <summary>
    /// 导入联系人方法
    /// </summary>
    public class CreateLink
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNum { get; set; }
        //备注信息 
        public string Bz { get; set; }

    }

    /// <summary>
    /// 群发朋友圈类
    /// </summary>
    public class CircleOfFriendsModel
    {

        public string SendText { get; set; }

        public List<string> SendPic { get; set; }

    }
    /// <summary>
    /// 用于列表页显示  构造模型
    /// </summary>
    public class TaskByLog
    {
        public string UserPhoneID { get; set; }
        public string TaskID { get; set; }
        public string TaskType { get; set; }
        public string TaskLogDes { get; set; }
        public string TaskPhone { get; set; }
    }
}
