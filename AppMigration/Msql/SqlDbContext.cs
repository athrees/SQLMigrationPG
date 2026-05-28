using AppMigration.Msql.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppMigration.Msql
{
    public class SqlDbContext(DbContextOptions<SqlDbContext> options) : IdentityDbContext<SQL_ApplicationUser>(options)
    {

        public DbSet<SQL_CLIENT> SqlClients { get; set; }
        public DbSet<SQL_CLIENT_ACCOUNT> SqlClientAccounts { get; set; }
        public DbSet<SQL_CLIENT_FEE> SqlClientFees { get; set; }

        public DbSet<SQL_CUSTOMER> SqlCustomers { get; set; }
        public DbSet<SQL_BRANCH> SqlBranches { get; set; }

        public DbSet<SQL_NOTARIS> SqlNotaris { get; set; }

        public DbSet<SQL_INVOICE> SqlInvoices { get; set; }
        public DbSet<SQL_INVOICE_HISTORY> SqlInvoiceHistories { get; set; }

        public DbSet<SQL_PNBP> SqlPnbps { get; set; }

        public DbSet<SQL_POSTS> SqlPosts { get; set; }
        public DbSet<SQL_AHUACCOUNT> SqlAhuAccounts { get; set; } 

        public DbSet<SQL_DOCUMENTS> SqlDocuments { get; set; }
        public DbSet<SQL_DOCUMENTS_CONFIG> SqlDocumentsConfig { get; set; }
        public DbSet<SQL_DOCUMENTS_FILE> SqlDocumentsFile { get; set; }
        public DbSet<SQL_DOCUMENTS_IMPORT> SqlDocumentsImport { get; set; }

        public DbSet<SQL_HISTORY_CERTIFICATE> SqlHistoriesCertificates { get; set; }

        public DbSet<SQL_ACCESS> SqlAccess { get; set; }
        public DbSet<SQL_ACCESSROLES> SqlAccessRoles { get; set; }     
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Ensure base method is called
        }
    }
}
