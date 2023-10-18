using Microsoft.EntityFrameworkCore;
using RoofingAmerica.Domain.Core.Data;
using RoofingAmerica.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoofingAmerica.Infrastructure.Data
{
    public class DataContext : DbContext, IUnitOfWork
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) 
        {
        }

        public DbSet<Sale> Sales { get; set; }
    }
}
