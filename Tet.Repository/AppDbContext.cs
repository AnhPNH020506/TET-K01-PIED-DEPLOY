using Microsoft.EntityFrameworkCore;
using Tet.Repository.Entity;

namespace Tet.Repository;

public class AppDbContext : DbContext
{
    public static Guid UserId1 = Guid.NewGuid(); //seller
    public static Guid UserId2 = Guid.NewGuid(); //user
    public static Guid OrderId1 = Guid.NewGuid();
    public static Guid OrderId2 = Guid.NewGuid();
    public static Guid CategoryParentId1 = Guid.NewGuid();
    public static Guid CategoryParentId2 = Guid.NewGuid();
    public static Guid SellerId1 = Guid.NewGuid();
    public static Guid ProductId1 = Guid.NewGuid();
    public static Guid ProductId2 = Guid.NewGuid();
    public static Guid ProductId3 = Guid.NewGuid();
    public static Guid ProductId4 = Guid.NewGuid();
    public static Guid StorageId1 = Guid.NewGuid();


    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductStorage> ProductStoragess { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<ProductCategory> ProductCategoriess { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CartDetail> CartDetails { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var seededCategories = new List<Category>();
        var seededProducts = new List<Product>();
        var seededStorage = new List<Storage>();
        var seededUsers = new List<User>();
        var seededOrders = new List<Order>();
        // ==================== User Configuration ====================
        modelBuilder.Entity<User>(builder =>
        {
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            // LastName - required, max 100 characters
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // ImageUrl - nullable, max 500 characters (URL)
            builder.Property(u => u.ImageUrl)
                .HasMaxLength(500);

            // PhoneNumber - nullable, max 20 characters
            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            // HashedPassword - required, max 500 characters
            builder.Property(u => u.HashedPassword)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("User");

            // Relationship: User has one Seller (one-to-one)
            builder.HasOne(u => u.Seller)
                .WithOne(s => s.User)
                .HasForeignKey<Seller>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.Cart)
                .WithOne(s => s.User)
                .HasForeignKey<Cart>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // DeleteBehavior.Cascade: Khi một User bị xóa, thì Seller liên quan cũng sẽ bị xóa theo.
            // DeleteBehavior.Restrict: Ngăn chặn việc xóa một User nếu có Seller liên quan tồn tại.
            //(Tham chiếu tới PK tồn tại)
            // 1 Project còn Task thì không xoá được
            // DeleteBehavior.NoAction: Không thực hiện hành động gì đặc biệt khi User bị xóa. ( Gàn giống Restrict, xử lí ở DB)
            // DeleteBehavior.SetNull: Khi một User bị xóa, thì trường UserId trong bảng Seller sẽ được đặt thành NULL.
            //(Áp dụng khi trường FK cho phép NULL)

            seededUsers = new List<User>()
            {
                new()
                {
                    Id = UserId1,
                    Email = "tan182205@gmail.com",
                    FirstName = "Tan",
                    LastName = "Tran",
                    HashedPassword = "hashed_password_1",
                },
                new()
                {
                    Id = UserId2,
                    Email = "tan182206@gmail.com",
                    FirstName = "Tan",
                    LastName = "Tran",
                    HashedPassword = "hashed_password_1",
                }
            };
            for (int i = 0; i < 1000; i++)
            {
                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = $"Anh + {i} +@gmail.com",
                    FirstName = $"Anh {i}",
                    LastName = $"Anh {i}",
                    HashedPassword = $"hashed_password {i}",
                };
                seededUsers.Add(newUser);
            }

            builder.HasData(seededUsers);
        });
        modelBuilder.Entity<Seller>(builder =>
        {
            builder.Property(s => s.TaxCode).IsRequired().HasMaxLength(50);
            builder.Property(s => s.CompanyName).IsRequired().HasMaxLength(200);
            builder.Property(s => s.CompanyAddress).IsRequired().HasMaxLength(500);
            var seller = new List<Seller>()
            {
                new()
                {
                    Id = SellerId1,
                    TaxCode = "TAXCODE123",
                    CompanyName = "Tan",
                    CompanyAddress = "123 abc def",
                    UserId = UserId1,
                }
            };
            builder.HasData(seller);
        });
        modelBuilder.Entity<Category>(builder =>
        {
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

            seededCategories = new List<Category>()
            {
                new()
                {
                    Id = CategoryParentId1,
                    Name = "Áo",
                },
                new()
                {
                    Id = CategoryParentId2,
                    Name = "quần",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Áo thể thao",
                    ParentId = CategoryParentId1,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Quần dài",
                    ParentId = CategoryParentId2,
                }
            };
            for (int i = 0; i < 500; i++)
            {
                var newCategory = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Áo loại {i}",
                    ParentId = CategoryParentId1,
                };
                seededCategories.Add(newCategory);
            }

            for (int i = 0; i < 500; i++)
            {
                var newCategory = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Quần loại {i}",
                    ParentId = CategoryParentId2,
                };
                seededCategories.Add(newCategory);
            }

            builder.HasData(seededCategories);
        });
        modelBuilder.Entity<Product>(builder =>
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Price).HasColumnType("decimal(10,2)");
            builder.Property(p => p.UrlImage).HasMaxLength(500);
            //Relationship: One to Many Seller
            builder.HasOne(p => p.Seller)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Cascade);
            seededProducts = new List<Product>()
            {
                new Product()
                {
                    Id = ProductId1,
                    Name = "Áo Thun Nam",
                    Description =
                        "Áo thun nam chất liệu cotton cao cấp, thoáng mát, phù hợp cho mọi hoạt động hàng ngày.",
                    UrlImage = "https://example.com/images/ao_thun_nam.jpg",
                    Price = 199000m,
                    SellerId = SellerId1
                },
                new Product()
                {
                    Id = ProductId2,
                    Name = "Quần Jeans Nữ",
                    Description = "Quần jeans nữ dáng ôm, tôn dáng, chất liệu denim co giãn, phù hợp cho mọi dịp.",
                    UrlImage = "https://example.com/images/quan_jeans_nu.jpg",
                    Price = 399000m,
                    SellerId = SellerId1
                },
                new Product()
                {
                    Id = ProductId3,
                    Name = "Áo Sơ Mi Nam",
                    Description = "Áo sơ mi nam công sở, thiết kế hiện đại, chất liệu vải cao cấp, thoáng mát.",
                    UrlImage = "https://example.com/images/ao_so_mi_nam.jpg",
                    Price = 299000m,
                    SellerId = SellerId1
                },
                new Product()
                {
                    Id = ProductId4,
                    Name = "Chân Váy Nữ",
                    Description = "Chân váy nữ xòe, thiết kế trẻ trung, chất liệu vải mềm mại, phù hợp cho mọi dịp.",
                    UrlImage = "https://example.com/images/chan_vay_nu.jpg",
                    Price = 249000m,
                    SellerId = SellerId1
                }
            };

            builder.HasData(seededProducts);
        });


        modelBuilder.Entity<Order>(builder =>
        {
            builder.Property(o => o.TotalAmount).HasColumnType("decimal(10,2)");
            builder.Property(o => o.status).IsRequired().HasMaxLength(50);
            builder.Property(o => o.Address).IsRequired().HasMaxLength(200);
            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            var order = new List<Order>()
            {
                new Order()
                {
                    Id = OrderId1,
                    UserId = UserId2,
                    Address = "Hello ban nha",
                    TotalAmount = 100000m,
                    status = "Completed"
                },
                new Order()
                {
                    Id = OrderId2,
                    UserId = UserId2,
                    Address = "Hello ban nha 1",
                    TotalAmount = 100000m,
                    status = "Completed"
                }
            };


            builder.HasData(order);
        });
        modelBuilder.Entity<OrderDetail>(builder =>
        {
            builder.Property(o => o.OrderId).IsRequired();
            builder.Property(o => o.ProductId).IsRequired();
            builder.Property(o => o.Quantity).IsRequired();
            builder.Property(o => o.UnitPrice).HasColumnType("decimal(10,2)");
            builder.HasOne(o => o.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.Product)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            var orderDetails = new List<OrderDetail>()
            {
                new OrderDetail()
                {
                    Id = Guid.NewGuid(),
                    OrderId = OrderId1,
                    ProductId = ProductId1,
                    Quantity = 2,
                    UnitPrice = 199000m,
                },
                new OrderDetail()
                {
                    Id = Guid.NewGuid(),
                    OrderId = OrderId1,
                    ProductId = ProductId2,
                    Quantity = 1,
                    UnitPrice = 399000m,
                },
                new OrderDetail()
                {
                    Id = Guid.NewGuid(),
                    OrderId = OrderId2,
                    ProductId = ProductId3,
                    Quantity = 1,
                    UnitPrice = 299000m,
                }
            };


            builder.HasData(orderDetails);
        });
        modelBuilder.Entity<Cart>(builder =>
            {
                builder.HasOne(c => c.User)
                    .WithOne(u => u.Cart)
                    .HasForeignKey<Cart>(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
            
        );
        modelBuilder.Entity<CartDetail>(builder =>
        {
            builder.HasOne(u => u.Cart)
                .WithMany(c => c.CartDetails)
                .HasForeignKey(u => u.CartId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.Product)
                .WithMany(c => c.CartDetails)
                .HasForeignKey(u => u.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

        });
    }
}