using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMigration.Msql.Entity
{
    [Table("DOCUMENTS_FILE", Schema = "dbo")]
    public class SQL_DOCUMENTS_FILE
    {
        [Key]
        public int FILE_ID { get; set; }

        [Required(ErrorMessage = "Document ID Name is required")]
        public int DOCUMENT_ID { get; set; }

        public string? CUSTOMER_CODE { get; set; }
        public string? DOCUMENT_TYPE { get; set; }
        public string? FILE_PATH { get; set; }
        public string? FILE_NAME_SERVER { get; set; }
        public string? FILE_NAME_CLIENT { get; set; }
        public string? MIME_TYPE { get; set; }
        public int? SIZE_BYTES { get; set; }

        public bool ISACTIVE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string? CREATED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
    }
}
