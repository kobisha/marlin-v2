using Marlin.sqlite.Models;
using Microsoft.EntityFrameworkCore;


namespace Marlin.sqlite.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        // Define your DbSet properties here

        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<OrderHeaders> OrderHeaders { get; set; }
        public DbSet<Catalogues> Catalogues { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<InvoiceHeader> InvoiceHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistory { get; set; }
        public DbSet<PriceList> PriceList { get; set; }
        public DbSet<AccessProfiles> AccessProfiles { get; set; }
        public DbSet<AccessSettings> AccessSettings { get; set; }
        public DbSet<AccountSettings> AccountSettings { get; set; }
        public DbSet<ConnectionSettings> ConnectionSettings { get; set; }
        public DbSet<ErrorCodes> ErrorCodes { get; set; }
        public DbSet<ExchangeLog> ExchangeLog { get; set; }
        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<PositionName> PositionName { get; set; }
        public DbSet<Shops> Shops { get; set; }
        public DbSet<UserPositions> UserPositions { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<ProductsByCategories> ProductsByCategories { get; set; }
        public DbSet<Barcodes> Barcodes { get; set; }
        public DbSet<ProductCategories> ProductCategories { get; set; }
        public DbSet<OrderHeaders> HeadersData { get; set; }
        public DbSet<OrderDetails> DetailsData { get; set; }
        public DbSet<StocksOfProducts> ProductsStocks { get; set; }

        public DbSet<ServiceLevels> ServiceLevels { get; set; }

        public DbSet<temTable> Table { get; set; }
        public DbSet<OrderFront> frontOrders { get; set; }
        public DbSet<OrderDetailsFront> frontDetails { get; set; }

        public DbSet<SLAByCategory> sLAByCategories { get; set; }
        public DbSet<SLAByOrder> sLAByOrders { get; set; }
        public DbSet<SLAByProducts> SLAByProducts { get; set; }
        public DbSet<SLAByShops> SLAByShops { get; set; }
        public DbSet<SLAByVendors> SLAByVendors { get; set; }
        public DbSet<RetroBonusHeader> RetroBonusHeaders { get; set; }
        public DbSet<RetroBonusDetails> RetroBonusDetails { get; set; }
        public DbSet<RetroBonusPlanRanges> RetroBonusPlanRanges { get; set; }
        public DbSet<RBFront> RBFronts { get; set; }
        public DbSet<RetroBonusResultFront> RetroBonusResults { get; set; }
        public DbSet<invoicfront> Results { get; set; }
        public DbSet<invdetails> invdetails { get; set; }
        public DbSet<OrderStatusResult> OrderStatusResults { get; set; }
        public DbSet<AccountData> AccountData { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<RetroBonusConditionFront> retroBonusConditionFronts { get; set; }
        public DbSet<AccountRelations> AccountRelations { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvoiceHeader>().HasKey(e => e.InvoiceID);
            
            modelBuilder.Entity<InvoiceDetail>()         
                        .HasOne(e => e.InvoiceHeader)
                .WithMany(h => h.InvoiceDetails).HasForeignKey(e => e.InvoiceID)
                        ;
            /*modelBuilder.Ignore<temTable>();
            modelBuilder.Ignore<OrderFront>();
            modelBuilder.Ignore<RBFront>();
            modelBuilder.Ignore<OrderDetailsFront>();
            modelBuilder.Ignore<SLAByOrder>();
            modelBuilder.Ignore<SLAByOrder>();
            modelBuilder.Ignore<SLAByCategory>();
            modelBuilder.Ignore<SLAByProducts>();
            modelBuilder.Ignore<SLAByShops>();
            modelBuilder.Ignore<SLAByVendors>();
            modelBuilder.Ignore<RetroBonusResultFront>();
            modelBuilder.Ignore<invoicfront>();
            modelBuilder.Ignore<invdetails>();
            modelBuilder.Ignore<OrderStatusResult>();*/
           // modelBuilder.Entity<InvoiceHeader>().HasKey(e => new { e.ID, e.OrderID }); // Define the composite primary key
           

            // Add other model configurations here


            base.OnModelCreating(modelBuilder);
        }



        
        
           
        



    }

}

