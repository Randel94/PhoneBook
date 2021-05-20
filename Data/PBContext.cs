using Microsoft.EntityFrameworkCore;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Data
{
    public class PBContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public PBContext(DbContextOptions<PBContext> options) : base (options)
        {
            Database.EnsureCreated();
        }
    }
}
