using API.Base;
using API.Context;
using API.Models;
using API.Models.VM;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        public IConfiguration _config;
        private readonly MyContext context;
        public AccountRepository(IConfiguration config, MyContext context) : base(context)
        {
            this._config = config;
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

        public ResponseObj doAuth(LoginVM obj)
        {
            ResponseObj objResp = new ResponseObj();
            List<Role> listRoles = new List<Role>();
            var idToken = string.Empty;

            Employee empObj = (from emp in context.Employees
                               where emp.EmployeeEmail == obj.Email
                               select emp).FirstOrDefault();

            Customer custObj = (from cust in context.Customers
                                where cust.CustomerEmail == obj.Email
                                select cust).FirstOrDefault();

            string id = "";
            string name = "";
            string phone = "";

            if (empObj != null)
            {
                id = empObj.EmployeeId;
                name = empObj.EmployeeName;
            }
            else if (custObj != null)
            {
                id = custObj.CustomerId;
                name = custObj.CustomerName;
                phone = custObj.CustomerPhone;
            }
            else
            {
                objResp.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objResp.message = "Email tidak terdaftar.";
                objResp.data = null;

                return objResp;
            }

            Account accObj = (from acc in context.Accounts
                              where acc.Id == id
                              select acc).FirstOrDefault();

            if (!BC.Verify(obj.Password, accObj.Password))
            {
                objResp.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objResp.message = "Password salah.";
                objResp.data = null;
            }
            else
            {
                listRoles = (from a in context.Accounts
                             join b in context.AccountRoles
                             on a.Id equals b.AccountId
                             join c in context.Roles
                             on b.RoleId equals c.RoleId
                             where a.Id == accObj.Id
                             select c).ToList();

                var claims = new List<Claim>();
                claims.Add(new Claim("Id", id));
                claims.Add(new Claim("Name", name));
                claims.Add(new Claim("Email", obj.Email));
                if (custObj != null)
                {
                    claims.Add(new Claim("Phone", phone));
                }

                foreach (var roles in listRoles)
                {
                    claims.Add(new Claim("roles", roles.RoleName));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signIn
                    );

                idToken = new JwtSecurityTokenHandler().WriteToken(token);
                claims.Add(new Claim("TokenSecurity", idToken.ToString()));

                objResp.statusCode = Convert.ToInt32(HttpStatusCode.OK);
                objResp.message = "Berhasil login.";
                objResp.data = idToken;
            }

            return objResp;
        }

        public ResponseObj ForgetPassword(ForgetPasswordVM obj)
        {
            ResponseObj objResp = new ResponseObj();
            EmailService es = new EmailService();

            Employee empObj = (from emp in context.Employees
                               where emp.EmployeeEmail == obj.Email
                               select emp).FirstOrDefault();

            Customer custObj = (from cust in context.Customers
                                where cust.CustomerEmail == obj.Email
                                select cust).FirstOrDefault();

            string id = "";

            if (empObj != null)
            {
                id = empObj.EmployeeId;
            }
            else if (custObj != null)
            {
                id = custObj.CustomerId;
            }
            else
            {
                objResp.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objResp.message = "Email tidak terdaftar.";
                objResp.data = null;

                return objResp;
            }

            string OTP = GenerateOTP();

            Account accObj = (from acc in context.Accounts
                              where acc.Id == id
                              select acc).FirstOrDefault();

            accObj.Password = BC.HashPassword(OTP);

            context.SaveChanges();

            es.Send(es.EmailSender, obj.Email, "Request Change Password", "Hello, you recently request to change password. If you're not requesting this, you can ignore this email. Your OTP is " + OTP + " .");

            objResp.statusCode = Convert.ToInt32(HttpStatusCode.OK);
            objResp.message = "Success Request Change Password";
            objResp.data = null;

            return objResp;
        }

        public ResponseObj ChangePassword(ChangePasswordVM obj)
        {
            ResponseObj objResp = new ResponseObj();

            Employee empObj = (from emp in context.Employees
                               where emp.EmployeeEmail == obj.Email
                               select emp).FirstOrDefault();

            Customer custObj = (from cust in context.Customers
                                where cust.CustomerEmail == obj.Email
                                select cust).FirstOrDefault();

            string id = "";

            if (empObj != null)
            {
                id = empObj.EmployeeId;
            }
            else if (custObj != null)
            {
                id = custObj.CustomerId;
            }
            else
            {
                objResp.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objResp.message = "Email tidak terdaftar.";
                objResp.data = null;

                return objResp;
            }

            Account accObj = (from acc in context.Accounts
                              where acc.Id == id
                              select acc).FirstOrDefault();

            if (!BC.Verify(obj.OTP, accObj.Password))
            {
                objResp.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objResp.message = "OTP yang dimasukkan salah.";
                objResp.data = null;

                return objResp;
            }
            else if (obj.NewPassword != obj.RetryPassword)
            {
                objResp.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objResp.message = "Password yang dimasukkan tidak sesuai. Harap ulangi.";
                objResp.data = null;

                return objResp;
            }
            else
            {
                accObj.Password = BC.HashPassword(obj.NewPassword);

                context.SaveChanges();

                objResp.statusCode = Convert.ToInt32(HttpStatusCode.OK);
                objResp.message = "Password berhasil diganti.";
                objResp.data = null;

                return objResp;
            }
        }
    }
}
