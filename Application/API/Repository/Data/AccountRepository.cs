using API.Base;
using API.Context;
using API.Models;
using API.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext context;
        public AccountRepository(MyContext context) : base(context)
        {
            this.context = context;
        }

        private string GenerateOTP(int length = 10)
        {
            //Referensi : https://www.aspsnippets.com/Articles/Generate-Unique-Random-OTP-One-Time-Password-in-ASPNet-using-C-and-VBNet.aspx
            string OTP = String.Empty;

            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numbers = "1234567890";
            string characters = numbers + alphabets;

            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (OTP.IndexOf(character) != -1);
                OTP += character;
            }

            return OTP;
        }

        private string GenerateID(int roleID)
        {
            string count;
            int last_id;
            string new_count;

            if (roleID == 4)
            {
                count = (from s in context.Customers
                         orderby s.CustomerId
                         select s.CustomerId).LastOrDefault();
            } else
            {
                count = (from s in context.Employees
                         orderby s.EmployeeId
                         select s.EmployeeId).LastOrDefault();
            } 

            if (count == null)
            {
                last_id = 1;
            }
            else
            {
                last_id = Convert.ToInt32(count.Substring(count.Length - 4)) + 1;
            }

            if (last_id < 10)
            {
                new_count = "000" + last_id;
            }
            else if (last_id < 100)
            {
                new_count = "00" + last_id;
            }
            else if (last_id < 1000)
            {
                new_count = "0" + last_id;
            }
            else
            {
                new_count = last_id.ToString();
            }

            return new_count;
        }
        public int Register(RegisterVM register)
        {
            if(register.RoleID == 4)
            {
                var CustomerID = "COS" + DateTime.Now.ToString("MMddyy") + GenerateID(register.RoleID);
                string password = GenerateOTP();
                var password_enc = BC.HashPassword(password);
                Customer customer = new Customer
                {
                    CustomerId = CustomerID,
                    CustomerName = register.Name,
                    CustomerEmail = register.Email,
                    CustomerPhone = register.Phone,
                    
                };
                Account account = new Account
                {
                    Id = CustomerID,
                    Password = password_enc
                };
                AccountRole accountRole = new AccountRole
                {
                    AccountId = CustomerID,
                    RoleId = register.RoleID
                };

                context.Customers.Add(customer);
                context.Accounts.Add(account);
                context.AccountRoles.Add(accountRole);
                var result = context.SaveChanges();

                EmailService email = new EmailService();
                email.Send(email.EmailSender, register.Email, "Your password", "Your password is " + password);
                return result;
            }
            else
            {
                var employeeID = "EMP" + DateTime.Now.ToString("MMddyy") + GenerateID(register.RoleID);
                string password = GenerateOTP();
                var password_enc = BC.HashPassword(password);
                Employee employee = new Employee
                {
                    EmployeeId = employeeID,
                    EmployeeName = register.Name,
                    EmployeeEmail = register.Email,
                    TeamLeadId = register.TeamLeadID
                };
                Account account= new Account
                {
                    Id = employeeID,
                    Password = password_enc
                };

                AccountRole accountRole = new AccountRole
                {
                    AccountId = employeeID,
                    RoleId = register.RoleID
                };

                context.Employees.Add(employee);
                context.Accounts.Add(account);
                context.AccountRoles.Add(accountRole);
                var result = context.SaveChanges();

                EmailService email = new EmailService();
                email.Send(email.EmailSender, register.Email, "Your password", "Your password is " + password);
                return result;
            }
        }
    }
}
