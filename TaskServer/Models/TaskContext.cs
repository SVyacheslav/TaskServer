using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskServer.Models
{
    public class TaskContext : DbContext
    {
        public DbSet<Assignment> Assignments { get; set; }
       
        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
   
}
