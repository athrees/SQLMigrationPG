using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMigration.Msql.Entity
{
    [Table("DOCUMENTS_CONFIG", Schema = "dbo")]
    public class SQL_DOCUMENTS_CONFIG
    {
        [Key]
        public int CONFIG_ID { get; set; }


        [Required(ErrorMessage = "Header or Title Name is required")]
        [Display(Name = "Header or Title Name")]
        public string? HEADER_NAME { get; set; }


        [Required(ErrorMessage = "Column Name is required")]
        [Display(Name = "Column Name")]
        public string? COLUMN_NAME { get; set; }

        [Required(ErrorMessage = "Client Name is required")]
        [Display(Name = "Client")]
        public string? CLIENT_CODE { get; set; }


        [Required(ErrorMessage = "Customer Name is required")]
        [Display(Name = "Customer")]
        public string? CUSTOMER_CODE { get; set; }


        public int? SEQ_DATA { get; set; }
        public int? SEQ_VIEW { get; set; }
        public int? SEQ_EXPORT { get; set; }
        public int? SEQ_IMPORT { get; set; }
        public int? SEQ_INVOICE { get; set; }
    }
}
