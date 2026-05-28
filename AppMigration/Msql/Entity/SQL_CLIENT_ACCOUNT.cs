using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMigration.Msql.Entity
{
    [Table("CLIENT_ACCOUNT", Schema = "dbo")]
    public class SQL_CLIENT_ACCOUNT
    {
        [Key]
        public int CLIENT_ACCOUNT_ID {get; set; }
        public string? CLIENT_CODE {get; set; }
        [Required(ErrorMessage = "Account Number Bank is required")]
        public string? ACCOUNT_NUMBER {get; set; }
        [Required(ErrorMessage = "Bank Name is required")]
        public string? ACCOUNT_BANK {get; set; }
        public bool ISACTIVE { get; set; }
        public string? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
    }
}
