using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teleg_training.DBEntities;

namespace Teleg_training
{
    public class ProgramListContext:DbContext
    {
        public ProgramListContext()
        {
        }
        public DbSet<DBProduct> Products { get; set; }
        public DbSet<DBAuthor> Authors { get; set; }
        public DbSet<DBProgramList> ProgramLists { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TGBotProgram;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBAuthor>()
                .HasMany(o => o.ProgramLists)
                .WithOne(o => o.Author)
                .HasForeignKey(o => o.AuthorId)
                .IsRequired();
            modelBuilder.Entity<DBProgramList>()
                .HasIndex(u => u.Name).IsUnique();
        }
    } 
}
