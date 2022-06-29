using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        #region DbSet
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            if (!ob.IsConfigured)
            {
                ob.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {

            mb.Entity<AccountRole>()
                .HasKey("AccountId", "RoleId");
            mb.Entity<AccountRole>()
                .HasOne(a => a.Account)
                .WithMany(b => b.AccountRoles)
                .HasForeignKey(b => b.AccountId);
            mb.Entity<AccountRole>()
                .HasOne(a => a.Role)
                .WithMany(b => b.AccountRoles)
                .HasForeignKey(b => b.RoleId);

            mb.Entity<Comment>()
                .HasOne(a => a.Account)
                .WithMany(b => b.Comments)
                .HasForeignKey(b => b.AccountId);
            mb.Entity<Comment>()
                .HasOne(a => a.Ticket)
                .WithMany(b => b.Comments)
                .HasForeignKey(b => b.TicketId);

            mb.Entity<Ticket>()
                .HasOne(a => a.Customer)
                .WithMany(b => b.Tickets)
                .HasForeignKey(b => b.CustomerId);
            mb.Entity<Ticket>()
                .HasOne(a => a.Employee)
                .WithMany(b => b.Tickets)
                .HasForeignKey(b => b.TeamLeadId)
                .HasForeignKey(b => b.EmployeeId);
            
        }
    }
}
