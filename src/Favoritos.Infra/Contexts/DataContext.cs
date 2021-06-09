using Favoritos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Favoritos.Infra.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<ClienteItem> Clientes { get; set; }
        public DbSet<FavoritoItem> Favoritos { get; set; }
        public DbSet<LoginItem> Logins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClienteItem>().HasIndex(a => a.Email).IsUnique();
            //modelBuilder.Entity<TodoItem>().ToTable("Todo");
            //modelBuilder.Entity<TodoItem>().Property(x => x.Id);
            //modelBuilder.Entity<TodoItem>().Property(x => x.User).HasMaxLength(120).HasColumnType("varchar(120)");
            //modelBuilder.Entity<TodoItem>().Property(x => x.Title).HasMaxLength(160).HasColumnType("varchar(160)");
            //modelBuilder.Entity<TodoItem>().Property(x => x.Done).HasColumnType("bit");
            //modelBuilder.Entity<TodoItem>().Property(x => x.Date);
            //modelBuilder.Entity<TodoItem>().HasIndex(b => b.User);
        }


    }
}