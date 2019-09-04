using Microsoft.EntityFrameworkCore;
using PaymentGateway.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    public class MerchantUsernameDbContext : DbContext
    {  

        public MerchantUsernameDbContext(DbContextOptions<MerchantUsernameDbContext> options) : base(options)
        {

        }

        public DbSet<MerchantUsername> MerchantUsername { get; set; }

    }
}
