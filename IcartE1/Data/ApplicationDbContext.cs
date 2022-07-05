using IcartE.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IcartE1.Data;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using IcartE1.Models;

namespace IcartE1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ListItem>().HasKey(li => new { li.ListId, li.ProductId });
            builder.Entity<OrderItem>().HasKey(oi => new { oi.OrderId, oi.ProductId });
            builder.Entity<Sales>().HasKey(s => new { s.ProductId, s.BranchId, s.Date });

            builder.Entity<Vendor>().HasData(
                new Vendor { Id = 1, Name = "Pepsi Co", Email = "orders@pepsi.com", PhoneNumber = "01201196290" },
                new Vendor { Id = 2, Name = "Juhayna", Email = "orders@juhayna.com", PhoneNumber = "01201296290" }
                );

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Title = "Soft Drinks", ImageUrl = "placeholder" },
                new Category { Id = 2, Title = "Dairy", ImageUrl = "placeholder" }
                );

            builder.Entity<Product>().HasData(
                new Product { Id = 1, Title = "Pepsi Can 330 ML", Description = "placeholder description", Price = 4.99f, CategoryId = 1, VendorId = 1 },
                new Product { Id = 2, Title = "7UP Can 330 ML", Description = "placeholder description", Price = 4.99f, CategoryId = 1, VendorId = 1 },
                new Product { Id = 3, Title = "Juhayna Full Cream 1L", Description = "placeholder description", Price = 27.99f, CategoryId = 2, VendorId = 2 }
                );

            builder.Entity<Branch>().HasData(
                new Branch { Id = 1, Title = "Test Branch", Latitude = 1.0, Longitude = 2.0 }
                );

            builder.Entity<Batch>().HasData(
                new Batch { Id = 1, ArrivalDate = new DateTime(2022, 2, 26), ExpiryDate = new DateTime(2022, 11, 26), Quantity = 1000, ProductId = 3, BranchId = 1 },
                new Batch { Id = 2, ArrivalDate = new DateTime(2022, 2, 26), ExpiryDate = new DateTime(2023, 2, 26), Quantity = 1000, ProductId = 2, BranchId = 1 },
                new Batch { Id = 3, ArrivalDate = new DateTime(2022, 2, 26), ExpiryDate = new DateTime(2023, 2, 26), Quantity = 1000, ProductId = 3, BranchId = 1 },
                new Batch { Id = 4, ArrivalDate = new DateTime(2022, 4, 26), ExpiryDate = new DateTime(2023, 4, 26), Quantity = 1500, ProductId = 2, BranchId = 1 },
                new Batch { Id = 5, ArrivalDate = new DateTime(2022, 2, 26), ExpiryDate = new DateTime(2023, 3, 26), Quantity = 500, ProductId = 1, BranchId = 1 }

                );



        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }
        public DbSet<CustomerPayment> CustomerPayment { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}
