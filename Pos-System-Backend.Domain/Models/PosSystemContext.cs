using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Pos_System_Backend.Domain.Models
{
    public partial class PosSystemContext : DbContext
    {
        public PosSystemContext()
        {
        }

        public PosSystemContext(DbContextOptions<PosSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<BrandAccount> BrandAccounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Collection> Collections { get; set; } = null!;
        public virtual DbSet<CollectionProduct> CollectionProducts { get; set; } = null!;
        public virtual DbSet<DateReport> DateReports { get; set; } = null!;
        public virtual DbSet<ExtraCategory> ExtraCategories { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<MenuProduct> MenuProducts { get; set; } = null!;
        public virtual DbSet<MenuStore> MenuStores { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderSource> OrderSources { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentType> PaymentTypes { get; set; } = null!;
        public virtual DbSet<Pos> Pos { get; set; } = null!;
        public virtual DbSet<PosSession> PosSessions { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
        public virtual DbSet<StoreAccount> StoreAccounts { get; set; } = null!;
        public virtual DbSet<Table> Tables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=pos-system-dev.cmzdrlsusaac.ap-southeast-1.rds.amazonaws.com,6969;User Id=possystem;Password=3w^N&Sp775B5;Database=PosSystem");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Password).HasMaxLength(64);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");

                entity.HasIndex(e => e.Code, "UQ_Area_Code")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Area_Store");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(256);

                entity.Property(e => e.Email).HasMaxLength(254);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BrandAccount>(entity =>
            {
                entity.ToTable("BrandAccount");

                entity.HasIndex(e => e.AccountId, "UX_BrandAccount_Account")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.BrandAccount)
                    .HasForeignKey<BrandAccount>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BrandAccount_Account");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.BrandAccounts)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BrandAccount_Brand");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.HasIndex(e => e.Code, "UX_Category_Code")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.Type).HasMaxLength(20);
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_Collection_Id");

                entity.ToTable("Collection");

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(20);
            });

            modelBuilder.Entity<CollectionProduct>(entity =>
            {
                entity.ToTable("CollectionProduct");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CollectionCode).HasMaxLength(20);

                entity.Property(e => e.ProductCode).HasMaxLength(20);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.CollectionCodeNavigation)
                    .WithMany(p => p.CollectionProducts)
                    .HasForeignKey(d => d.CollectionCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectionProduct_Collection");

                entity.HasOne(d => d.ProductCodeNavigation)
                    .WithMany(p => p.CollectionProducts)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(d => d.ProductCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectionProdcut_Product");
            });

            modelBuilder.Entity<DateReport>(entity =>
            {
                entity.ToTable("DateReport");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.DateReports)
                    .HasForeignKey(d => d.PosId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DataReport_Pos");
            });

            modelBuilder.Entity<ExtraCategory>(entity =>
            {
                entity.ToTable("ExtraCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ExtraCategoryCode).HasMaxLength(20);

                entity.Property(e => e.ProductCategoryCode).HasMaxLength(20);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.ExtraCategoryCodeNavigation)
                    .WithMany(p => p.ExtraCategoryExtraCategoryCodeNavigations)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(d => d.ExtraCategoryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ProductCategoryCodeNavigation)
                    .WithMany(p => p.ExtraCategoryProductCategoryCodeNavigations)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(d => d.ProductCategoryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.HasIndex(e => e.Code, "UX_Menu_Code")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<MenuProduct>(entity =>
            {
                entity.ToTable("MenuProduct");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MenuProducts)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuProduct_Menu");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.MenuProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuProduct_Product");
            });

            modelBuilder.Entity<MenuStore>(entity =>
            {
                entity.ToTable("MenuStore");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MenuStores)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuStore_Menu");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.MenuStores)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuStore_Store");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.HasIndex(e => e.InvoiceId, "UQ_Order_InvoiceID")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FeeDescription).HasMaxLength(50);

                entity.Property(e => e.InvoiceId)
                    .HasMaxLength(50)
                    .HasColumnName("InvoiceID");

                entity.Property(e => e.OrderType).HasMaxLength(20);

                entity.Property(e => e.Vat).HasColumnName("VAT");

                entity.Property(e => e.Vatamount).HasColumnName("VATAmount");

                entity.HasOne(d => d.CheckInPersonNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CheckInPerson)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Account");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Order)
                    .HasForeignKey<Order>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_OrderSource");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Session");

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.TableId)
                    .HasConstraintName("FK_Order_Table");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Notes).HasMaxLength(200);

                entity.Property(e => e.ProductCode).HasMaxLength(20);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.ProductCodeNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(d => d.ProductCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Product");
            });

            modelBuilder.Entity<OrderSource>(entity =>
            {
                entity.ToTable("OrderSource");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(95);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Payment)
                    .HasForeignKey<Payment>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_PaymentType");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Order");
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.ToTable("PaymentType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Pos>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Location).HasMaxLength(50);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Pos)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POS_Area");
            });

            modelBuilder.Entity<PosSession>(entity =>
            {
                entity.ToTable("PosSession");

                entity.HasIndex(e => new { e.SessionId, e.PosId }, "UQ_PosID_SessionId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.PosSessions)
                    .HasForeignKey(d => d.PosId)
                    .HasConstraintName("FK_PosSession_Pos");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.PosSessions)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("FK_PosSession_SessionIdS");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.Code, "UX_Product_Code")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CategoryCode).HasMaxLength(20);

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PicUrl).HasColumnName("PicURL");

                entity.Property(e => e.Size).HasMaxLength(10);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.Type).HasMaxLength(15);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Brand");

                entity.HasOne(d => d.CategoryCodeNavigation)
                    .WithMany(p => p.Products)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(d => d.CategoryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");

                entity.HasOne(d => d.ParentProduct)
                    .WithMany(p => p.InverseParentProduct)
                    .HasForeignKey(d => d.ParentProductId)
                    .HasConstraintName("FK_Product_Product");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Session");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.HasIndex(e => e.Code, "UX_Store_StoreCode")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.Email).HasMaxLength(254);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName).HasMaxLength(30);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Store_Brand");
            });

            modelBuilder.Entity<StoreAccount>(entity =>
            {
                entity.ToTable("StoreAccount");

                entity.HasIndex(e => e.AccountId, "UX_StoreAccount_AccountId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.StoreAccount)
                    .HasForeignKey<StoreAccount>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreAccount_Account");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreAccounts)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreAccount_Store");
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.ToTable("Table");

                entity.HasIndex(e => e.Code, "UQ_Table_Code")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(15);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Tables)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_Area");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
