using AppMigration.Models;
using AppMigration.Msql;
using AppMigration.Msql.Entity;
using AppMigration.Postgree;
using AppMigration.Postgree.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System.Net.Mail;

namespace AppMigration.Service
{
    public class ServiceJob
    {

        private readonly SqlDbContext _sqlDb;
        private readonly PgDbContext _pgDb;
        private readonly IMapper _mapper;
        private readonly MySettings _settings;

        public ServiceJob(
            SqlDbContext sqlDbContext,
            PgDbContext pgDbContext,
            IMapper mapper,
            IOptions<MySettings> options
        )
        {
            _sqlDb = sqlDbContext;
            _pgDb = pgDbContext;
            _mapper = mapper;
            _settings = options.Value;
        }


        public async Task DoWork()
        {
            const int batchSize = 2000;
            try
            {

                Console.WriteLine($"Process Migration I started at {DateTime.Now:dd-MMM-yyyy HH:mm:ss}");
                // ===== PHASE 1: CORE DATA =====
                await RunInTransactionAsync(async () =>
                {
                    await MigrateIdentityAsync(batchSize);
                    await MigrateClientsAsync(batchSize);
                    await MigrateInvoicesAsync(batchSize);
                    await ResetSequencesAsync();
                });
                Console.WriteLine($"Process Migration I completed at {DateTime.Now:dd-MMM-yyyy HH:mm:ss}");

                Console.WriteLine($"Process Migration II started at {DateTime.Now:dd-MMM-yyyy HH:mm:ss}");
                // ===== PHASE 2: DOCUMENTS =====
                await RunInTransactionAsync(async () =>
                {
                    await MigrateDocumentsAsync(batchSize);
                    await ResetDocumentsSequencesAsync();
                });

                Console.WriteLine($"Process Migration II completed at {DateTime.Now:dd-MMM-yyyy HH:mm:ss}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        private async Task MigrateIdentityAsync(int batchSize)
        {
            Console.WriteLine("Migrating Identity...");

            // USERS
            var users = await _sqlDb.Users.AsNoTracking().ToListAsync();
            var pgUsers = _mapper.Map<List<ApplicationUser>>(users);
            foreach (var b in pgUsers.Chunk(batchSize))
            {
                await _pgDb.Users.AddRangeAsync(b);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            // ROLES
            var roles = await _sqlDb.Roles.AsNoTracking().ToListAsync();
            foreach (var b in roles.Chunk(batchSize))
            {
                await _pgDb.Roles.AddRangeAsync(b);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            // USER ROLES
            var userRoles = await _sqlDb.UserRoles.AsNoTracking().ToListAsync();
            foreach (var b in userRoles.Chunk(batchSize))
            {
                await _pgDb.UserRoles.AddRangeAsync(b);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            // USER CLAIMS
            var userClaims = await _sqlDb.UserClaims.AsNoTracking().ToListAsync();
            foreach (var b in userClaims.Chunk(batchSize))
            {
                await _pgDb.UserClaims.AddRangeAsync(b);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            // USER LOGINS
            var userLogins = await _sqlDb.UserLogins.AsNoTracking().ToListAsync();
            foreach (var b in userLogins.Chunk(batchSize))
            {
                await _pgDb.UserLogins.AddRangeAsync(b);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            // ROLE CLAIMS
            var roleClaims = await _sqlDb.RoleClaims.AsNoTracking().ToListAsync();
            foreach (var b in roleClaims.Chunk(batchSize))
            {
                await _pgDb.RoleClaims.AddRangeAsync(b);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            // USER TOKENS
            var userTokens = await _sqlDb.UserTokens.AsNoTracking().ToListAsync();
            foreach (var b in userTokens.Chunk(batchSize))
            {
                await _pgDb.UserTokens.AddRangeAsync(b);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            Console.WriteLine("Identity migration done.");
        }

        private async Task MigrateClientsAsync(int batchSize)
        {
            Console.WriteLine("Migrating clients & master data...");
            async Task BulkInsertAsync<TSource, TTarget>( IQueryable<TSource> sourceQuery, DbSet<TTarget> targetSet) 
                where TSource : class 
                where TTarget : class
            {
                var sourceData = await sourceQuery.AsNoTracking().ToListAsync();
                var targetData = _mapper.Map<List<TTarget>>(sourceData);

                foreach (var batch in targetData.Chunk(1000))
                {
                    await targetSet.AddRangeAsync(batch);
                    await _pgDb.SaveChangesAsync();
                    _pgDb.ChangeTracker.Clear();
                }
            }

            await BulkInsertAsync(_sqlDb.SqlClients, _pgDb.Clients);
            await BulkInsertAsync(_sqlDb.SqlClientAccounts, _pgDb.ClientAccounts);
            await BulkInsertAsync(_sqlDb.SqlClientFees, _pgDb.ClientFees);
            await BulkInsertAsync(_sqlDb.SqlCustomers, _pgDb.Customers);
            await BulkInsertAsync(_sqlDb.SqlBranches, _pgDb.Branches);
            await BulkInsertAsync(_sqlDb.SqlNotaris, _pgDb.Notaris);

            await BulkInsertAsync(_sqlDb.SqlPnbps, _pgDb.Pnbps);
            await BulkInsertAsync(_sqlDb.SqlPosts, _pgDb.Posts);
            await BulkInsertAsync(_sqlDb.SqlAhuAccounts, _pgDb.AhuAccounts);
            await BulkInsertAsync(_sqlDb.SqlDocumentsConfig, _pgDb.DocumentsConfig);
            await BulkInsertAsync(_sqlDb.SqlDocumentsImport, _pgDb.DocumentsImport);

            Console.WriteLine("Client & master data migration done.");
        }


        private async Task MigrateInvoicesAsync(int batchSize)
        {
            Console.WriteLine("Migrating transaction...");

            var invoices = await _sqlDb.SqlInvoices.AsNoTracking().ToListAsync();
            var pgInvoices = _mapper.Map<List<INVOICE>>(invoices);
            foreach (var b in pgInvoices.Chunk(batchSize))
            {
                await _pgDb.Invoices.AddRangeAsync(b);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            var histories = await _sqlDb.SqlInvoiceHistories.AsNoTracking().ToListAsync();
            var pgHistories = _mapper.Map<List<INVOICE_HISTORY>>(histories);
            foreach (var b in pgHistories.Chunk(batchSize))
            {
                await _pgDb.InvoiceHistories.AddRangeAsync(b);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            //ACCESS

            var accesses = await _sqlDb.SqlAccess.AsNoTracking().ToListAsync();
            var pgAccesses = _mapper.Map<List<ACCESS>>(accesses);
            foreach (var batch in pgAccesses.Chunk(batchSize))
            {
                await _pgDb.Access.AddRangeAsync(batch);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            //ACCESS ROLES
            var accessRoles = await _sqlDb.SqlAccessRoles.AsNoTracking().ToListAsync();
            var pgAccessRoles = _mapper.Map<List<ACCESSROLES>>(accessRoles);
            foreach (var batch in pgAccessRoles.Chunk(batchSize))
            {
                await _pgDb.AccessRoles.AddRangeAsync(batch);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            

            Console.WriteLine("Invoice migration done.");
        }

        private async Task ResetSequencesAsync()
        {
            Console.WriteLine("Resetting PostgreSQL sequences...");

            async Task ResetAsync(string table, string column)
            {
                var sql = $@"
                            SELECT setval(
                                pg_get_serial_sequence('""{table}""', '{column}'),
                                COALESCE((SELECT MAX(""{column}"") FROM ""{table}""), 1)
                            );
                            ";
                await _pgDb.Database.ExecuteSqlRawAsync(sql);
            }


            await ResetAsync("ACCESS", "ACCESS_ID");
            await ResetAsync("NOTARIS", "NOTARIS_ID");
            await ResetAsync("PNBP", "PNBP_ID");
            await ResetAsync("POSTS", "POST_ID");

            await ResetAsync("CLIENT", "CLIENT_ID");
            await ResetAsync("CUSTOMER", "CUSTOMER_ID");
            await ResetAsync("BRANCH", "BRANCH_ID");
            await ResetAsync("INVOICE", "INVOICE_ID");
            await ResetAsync("INVOICE_HISTORY", "HISTORY_ID");
            await ResetAsync("CLIENT_ACCOUNT", "CLIENT_ACCOUNT_ID");
            await ResetAsync("CLIENT_FEE", "CLIENT_FEE_ID");
            await ResetAsync("ACCESSROLES", "ACCESS_ROLE_ID");

            await ResetAsync("AHUACCOUNT", "AHU_ID");
            await ResetAsync("DOCUMENTS_CONFIG", "CONFIG_ID");
            await ResetAsync("DOCUMENTS_IMPORT", "ID");
            await ResetAsync("INVOICE_HISTORY", "HISTORY_ID");

            Console.WriteLine("Sequence reset done.");
        }


        private async Task MigrateDocumentsAsync(int batchSize)
        {
            Console.WriteLine("Migrating Documents...");


            //DOCUMENTS
            var documents = await _sqlDb.SqlDocuments.AsNoTracking().ToListAsync();
            var pgDocuments = _mapper.Map<List<DOCUMENTS>>(documents);
            foreach (var batch in pgDocuments.Chunk(batchSize))
            {
                await _pgDb.Documents.AddRangeAsync(batch);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            //DOCUMENTS FILE
            var documentsFile = await _sqlDb.SqlDocumentsFile.AsNoTracking().ToListAsync();
            var pgDocumentsFile = _mapper.Map<List<DOCUMENTS_FILE>>(documentsFile);
            foreach (var batch in pgDocumentsFile.Chunk(batchSize))
            {
                await _pgDb.DocumentsFile.AddRangeAsync(batch);
                await _pgDb.SaveChangesAsync();
                _pgDb.ChangeTracker.Clear();
            }

            //HISTORY CERTIFICATE
            //var historiesCertificates = await _sqlDb.SqlHistoriesCertificates.AsNoTracking().ToListAsync();
            //var pgHistoriesCertificates = _mapper.Map<List<HISTORY_CERTIFICATE>>(historiesCertificates);
            //foreach (var batch in pgHistoriesCertificates.Chunk(batchSize))
            //{
            //    await _pgDb.HistoriesCertificates.AddRangeAsync(batch);
            //    await _pgDb.SaveChangesAsync();
            //    _pgDb.ChangeTracker.Clear();
            //}

            Console.WriteLine("Documents migration done.");
        }


        private async Task ResetDocumentsSequencesAsync()
        {
            Console.WriteLine("Resetting PostgreSQL sequences...");

            async Task ResetAsync(string table, string column)
            {
                var sql = $@"
                            SELECT setval(
                                pg_get_serial_sequence('""{table}""', '{column}'),
                                COALESCE((SELECT MAX(""{column}"") FROM ""{table}""), 1)
                            );
                            ";
                await _pgDb.Database.ExecuteSqlRawAsync(sql);
            }

            await ResetAsync("DOCUMENTS", "ID");
            await ResetAsync("DOCUMENTS_FILE", "FILE_ID");
            //await ResetAsync("HISTORY_CERTIFICATE", "ID");

            Console.WriteLine("Sequence reset done.");
        }


        private async Task RunInTransactionAsync(Func<Task> action)
        {
            await using var trx = await _pgDb.Database.BeginTransactionAsync();

            var oldAutoDetect = _pgDb.ChangeTracker.AutoDetectChangesEnabled;
            var oldTracking = _pgDb.ChangeTracker.QueryTrackingBehavior;

            try
            {
                _pgDb.ChangeTracker.AutoDetectChangesEnabled = false;
                _pgDb.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                await action();

                await trx.CommitAsync();
            }
            catch (Exception ex)
            {
                await trx.RollbackAsync();
                Console.WriteLine($"Migration failed: {ex}");
                throw;
            }
            finally
            {
                _pgDb.ChangeTracker.AutoDetectChangesEnabled = oldAutoDetect;
                _pgDb.ChangeTracker.QueryTrackingBehavior = oldTracking;
            }
        }

        //FOR BIG DATA

        private async Task MigrateClientsWithCopyAsync()
        {
            Console.WriteLine("▶ Start CLIENT migration with PostgreSQL COPY");

            // ⚠️ Pakai connection string PostgreSQL langsung
            await using var conn = new NpgsqlConnection(_pgDb.Database.GetConnectionString());
            await conn.OpenAsync();

            // ⚠️ COPY BINARY (PALING CEPAT)
            await using var importer = conn.BeginBinaryImport(@"
                                                COPY ""CLIENT"" (
                                                    ""CLIENT_ID"",
                                                    ""CLIENT_CODE"",
                                                    ""CLIENT_NAME"",
                                                    ""ACCOUNT_NAME""
                                                )
                                                FROM STDIN (FORMAT BINARY)
                                            ");

            int counter = 0;

            // ✅ STREAM DATA DARI SQL SERVER
            await foreach (var row in _sqlDb.SqlClients
                .AsNoTracking()
                .AsAsyncEnumerable())
            {
                importer.StartRow();

                importer.Write(row.CLIENT_ID, NpgsqlDbType.Integer);
                importer.Write(row.CLIENT_CODE, NpgsqlDbType.Text);
                importer.Write(row.CLIENT_NAME, NpgsqlDbType.Text);
                importer.Write(row.ACCOUNT_NAME, NpgsqlDbType.Timestamp);

                counter++;

                if (counter % 100_000 == 0)
                {
                    Console.WriteLine($"  Migrated {counter:N0} clients...");
                }
            }

            await importer.CompleteAsync();

            Console.WriteLine($"✔ CLIENT migration finished: {counter:N0} rows");
        }

    }
}
