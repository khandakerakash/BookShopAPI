using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Api.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookCategory>()       // THIS IS FIRST
                .HasOne(u => u.Book).WithMany(u => u.BookCategories).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookCategory>()       // THIS IS FIRST
                .HasOne(u => u.Category).WithMany(u => u.BookCategories).IsRequired().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });
            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
