using AppMigration.Postgree.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppMigration.Postgree
{
    public class PgDbContext(DbContextOptions<PgDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        public DbSet<BRANCH> Branches { get; set; }
        public DbSet<CUSTOMER> Customers { get; set; }
        public DbSet<CLIENT> Clients { get; set; }
        public DbSet<CLIENT_ACCOUNT> ClientAccounts { get; set; }
        public DbSet<CLIENT_FEE> ClientFees { get; set; }


        public DbSet<ACCESS> Access { get; set; }
        public DbSet<ACCESSROLES> AccessRoles { get; set; }
        public DbSet<AHUACCOUNT> AhuAccounts { get; set; }
        public DbSet<NOTARIS> Notaris { get; set; }
        public DbSet<POSTS> Posts { get; set; }
        public DbSet<INVOICE> Invoices { get; set; }
        public DbSet<INVOICE_HISTORY> InvoiceHistories { get; set; }
        public DbSet<PNBP> Pnbps { get; set; }
        public DbSet<DOCUMENTS> Documents { get; set; }
        public DbSet<DOCUMENTS_CONFIG> DocumentsConfig { get; set; }
        public DbSet<DOCUMENTS_FILE> DocumentsFile { get; set; }
        public DbSet<DOCUMENTS_IMPORT> DocumentsImport { get; set; }
        public DbSet<HISTORY_CERTIFICATE> HistoriesCertificates { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Ensure base method is called


            builder.Entity<ApplicationUser>(entity =>
            {
                // datetimeoffset → timestamptz (BENAR)
                entity.Property(e => e.LockoutEnd)
                      .HasColumnType("timestamp with time zone");

                // datetime → timestamp without time zone (WAJIB EXPLICIT)
                entity.Property(e => e.CREATED_DATE)
                      .HasColumnType("timestamp without time zone");

                entity.Property(e => e.MODIFIED_DATE)
                      .HasColumnType("timestamp without time zone");
            });


            builder.Entity<CLIENT_ACCOUNT>(entity =>
            {
                entity.Property(e => e.CREATED_DATE)
                      .HasColumnType("timestamp without time zone");

                entity.Property(e => e.MODIFIED_DATE)
                      .HasColumnType("timestamp without time zone");
            });

            builder.Entity<CLIENT_FEE>(entity =>
            {
                entity.Property(e => e.CREATED_DATE)
                      .HasColumnType("timestamp without time zone");

                entity.Property(e => e.MODIFIED_DATE)
                      .HasColumnType("timestamp without time zone");
            });

            builder.Entity<ACCESSROLES>(e =>
            {
                e.Property(x => x.CREATED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.MODIFIED_DATE)
                 .HasColumnType("timestamp without time zone");
            });

            builder.Entity<AHUACCOUNT>(e =>
            {
                e.Property(x => x.CREATED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.MODIFIED_DATE)
                 .HasColumnType("timestamp without time zone");
            });


            builder.Entity<DOCUMENTS>(e =>
            {
                e.Property(x => x.TGL_SERTIFIKAT)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.JAM_MINUTA)
                 .HasColumnType("time without time zone");
                e.Property(x => x.TANGGAL_AKTA)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.TGLLAHIR_PEMBERIFIDUSIA)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.TGLLAHIR_PASANGAN)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.TGLLAHIR_DEBITUR)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.TANGGAL_ORDER)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.TANGGAL_KONTRAK)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.TANGGAL_AWAL_TENOR)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.TANGGAL_AKHIR_TENOR)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.DELETED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.Tanggal_Description_1)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.Tanggal_Description_2)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.Tanggal_Description_3)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.Tanggal_Description_4)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.Tanggal_Description_5)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.INSERT_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.AHU_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.CERTIFICATE_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.CREATED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.MODIFIED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.DUPLICATED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.DELETED_DATE)
                 .HasColumnType("timestamp without time zone");
            });

            builder.Entity<DOCUMENTS_FILE>(e =>
            {
                e.Property(x => x.CREATED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.MODIFIED_DATE)
                 .HasColumnType("timestamp without time zone");
            });

            builder.Entity<DOCUMENTS_IMPORT>(e =>
            {
                e.Property(x => x.CREATED_DATE)
                 .HasColumnType("timestamp without time zone");
            });

            builder.Entity<HISTORY_CERTIFICATE>(e =>
            {
                e.Property(x => x.CREATED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.MODIFIED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.WAKTU_DAFTAR)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.TANGGAL_AKTA)
                 .HasColumnType("timestamp without time zone");
            });


            builder.Entity<INVOICE>(e =>
            {
                e.Property(x => x.START_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.END_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.INVOICE_DATE)
                 .HasColumnType("timestamp without time zone");
            });

            builder.Entity<INVOICE_HISTORY>(e =>
            {
                e.Property(x => x.CREATED_DATE)
                 .HasColumnType("timestamp without time zone");
            });

            builder.Entity<NOTARIS>(e =>
            {
                e.Property(x => x.CREATED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.MODIFIED_DATE)
                 .HasColumnType("timestamp without time zone");
            });

            builder.Entity<PNBP>(e =>
            {
                e.Property(x => x.CREATED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.MODIFIED_DATE)
                 .HasColumnType("timestamp without time zone");
            });

            builder.Entity<POSTS>(e =>
            {
                e.Property(x => x.CREATED_DATE)
                 .HasColumnType("timestamp without time zone");
                e.Property(x => x.MODIFIED_DATE)
                 .HasColumnType("timestamp without time zone");
            });
        }
    }
}
