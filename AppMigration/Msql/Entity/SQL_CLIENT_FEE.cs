using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMigration.Msql.Entity
{
    [Table("CLIENT_FEE", Schema = "dbo")]
    public class SQL_CLIENT_FEE
    {
        [Key]
        public int CLIENT_FEE_ID {get; set; }
        public string? CLIENT_CODE {get; set; }
        public string? CUSTOMER_CODE { get; set; }

        [Required(ErrorMessage = "Fee is required")]
        public double FEE_COST {get; set; }

        [Required(ErrorMessage = "Tax is required")]
        public int TAX_COST {get; set; }
        public bool ISACTIVE { get; set; }
        public string? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
    }
}
