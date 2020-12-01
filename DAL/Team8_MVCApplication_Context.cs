using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Team8_MVCApplication.Models;

namespace Team8_MVCApplication.DAL
{
    public class Team8_MVCApplication_Context : DbContext
    {
        public System.Data.Entity.DbSet<Team8_MVCApplication.Models.Profile> Profiles { get; set; }
        public Team8_MVCApplication_Context() : base("name=DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<Team8_MVCApplication.Models.CoreValueRecognitions> CoreValueRecognitions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();  // note: this is all one line!
            modelBuilder.Entity<Profile>().HasMany(t => t.GivingtheRecognition).WithRequired(a => a.personRecognizor).WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>().HasMany(t => t.BeingRecognized).WithRequired(a => a.personRecognized).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}