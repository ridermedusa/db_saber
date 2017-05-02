 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using db_saber.Model;
using db_saber.DAL;
//using db_saber.Linq;

namespace db_saber.BLL
{
	
	public partial class Sys_Error_logService:SysBllBase<Sys_Error_log>
    { 
		public override RepositoryBase<Sys_Error_log> repository {set;get;}
		public Sys_Error_logService(Sys_Error_logRepository repository)
        {
            this.repository = repository;
        }
        public Sys_Error_logService()
        {
            this.repository = new Sys_Error_logRepository();
        }
    }
	
	public partial class Sys_MasterService:SysBllBase<Sys_Master>
    { 
		public override RepositoryBase<Sys_Master> repository {set;get;}
		public Sys_MasterService(Sys_MasterRepository repository)
        {
            this.repository = repository;
        }
        public Sys_MasterService()
        {
            this.repository = new Sys_MasterRepository();
        }
    }
	
	public partial class Sys_ReturnService:SysBllBase<Sys_Return>
    { 
		public override RepositoryBase<Sys_Return> repository {set;get;}
		public Sys_ReturnService(Sys_ReturnRepository repository)
        {
            this.repository = repository;
        }
        public Sys_ReturnService()
        {
            this.repository = new Sys_ReturnRepository();
        }
    }
	
}