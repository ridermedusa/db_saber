using db_saber.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_saber.DAL
{

    public partial class Sys_MasterRepository : RepositoryBase<Sys_Master>
    {
        public List<Sys_Master> GetPage(int StartNo, int pageIndex, int pageSize,ref int totalCount)
        {
            //var q = (from o in context.Sys_Master
            //         where o.post_no >= StartNo
            //         select o.user_name).Distinct();

            var t = from o in context.Sys_Master
                    where o.post_no >= StartNo
                    select o;
            totalCount = t.Count();
            t = t.OrderBy(s => s.post_no).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return t.ToList();
        }
    }
}
