using API.Context;
using API.Models;
using API.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class RoleRepository : GeneralRepository<MyContext, Role, int>
    {
        private readonly MyContext context;
        public RoleRepository(MyContext context) : base(context)
        {
            this.context = context;
        }
        
        public List<GetRoleVM> GetRole()
        {
            var result = (from r in context.Roles
                          select r).ToList();
            List<GetRoleVM> list = new List<GetRoleVM>();
            foreach (var r in result)
            {
                GetRoleVM roleVM = new GetRoleVM
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                };
                list.Add(roleVM);
            }
            return list;
        }

    }
}
