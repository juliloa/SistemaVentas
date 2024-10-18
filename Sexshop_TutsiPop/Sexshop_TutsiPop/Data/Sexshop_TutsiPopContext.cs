using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sexshop_TutsiPop.Models;

namespace Sexshop_TutsiPop.Data
{
    public class Sexshop_TutsiPopContext : DbContext
    {
        public Sexshop_TutsiPopContext (DbContextOptions<Sexshop_TutsiPopContext> options)
            : base(options)
        {
        }

        public DbSet<Sexshop_TutsiPop.Models.Usuarios> Usuarios { get; set; } = default!;
    }
}
