using System;
using Globo.PIC.Domain.Entities.Cadprog;
using Microsoft.EntityFrameworkCore;

namespace Globo.PIC.Functions.context
{

    /// <summary>
    /// 
    /// </summary>
    public class CadprogDbContext : DbContext
    {
        public DbSet<Projeto> Projects { get; set; }

        public DbSet<Tarefa> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(Environment.GetEnvironmentVariable("CONNECTIONSTRING"), 
                options => options.UseOracleSQLCompatibility("11"));
                
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("GIT");

            modelBuilder.Entity<Projeto>()
                .ToTable("PROJETOS");
            modelBuilder.Entity<Projeto>(x => x.HasKey(c => c.ProjectId));

            modelBuilder.Entity<Tarefa>()
                .ToTable("TAREFAS");
            modelBuilder.Entity<Tarefa>(x => x.HasKey(c => c.TaskId));

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName().ToUpper());

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToUpper());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToUpper());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToUpper());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().ToUpper());
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
