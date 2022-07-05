using API.Context;
using API.Models;
using API.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRolesRepository : GeneralRepository<MyContext, AccountRole, string>
    {
        private readonly MyContext context;
        public AccountRolesRepository(MyContext context) : base(context)
        {
            this.context = context;
        }

        public List<Employee> GetTeamLead()
        {
            var result = (from ar in context.AccountRoles
                          join e in context.Employees
                          on ar.AccountId equals e.EmployeeId
                          where ar.RoleId == 2
                          select e).ToList<Employee>();
            return result;
        }

        public List<Employee> GetEmployee(TicketOwnerVM ownerVM)
        {
            var result = (from e in context.Employees
                          where e.TeamLeadId == ownerVM.AccountId
                          select e).ToList<Employee>();
            return result;
        }
    }
}
