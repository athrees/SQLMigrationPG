using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMigration.Msql.Entity
{
    [Table("DOCUMENTS_IMPORT", Schema = "dbo")]
    public class SQL_DOCUMENTS_IMPORT
    {
        [Key]
        public int ID { get; set; }
        public string? CUSTOMER_CODE { get; set; }
        public string? FILE_PATH { get; set; }
        public string? FILE_NAME_SERVER { get; set; }
        public string? FILE_NAME_CLIENT { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string? CREATED_BY { get; set; }
    }
}
