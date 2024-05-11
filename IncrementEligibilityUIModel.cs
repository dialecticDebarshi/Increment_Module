using System.ComponentModel.DataAnnotations;

namespace TenantCompany.Models
{
    public class IncrementEligibilityUIModel
    {
        public int MAST_INCREMENT_ELIGIBILITY_KEY { get; set; }
       
        [Required(ErrorMessage = "Please enter eligibility name")]
        [StringLength(50, ErrorMessage = "BUDGET CATEGORY NAME must be at most 50 characters long.")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "BUDGET CATEGORY NAME must contain only letters.")]
        public string ELIGIBILITY_NAME { get; set; }
        public int ApplicationDataTable_Master_KEY { get; set; }
        [Required(ErrorMessage = "Please select a field form.")]
        public int ApplicationDataTable_Dtls_KEY { get; set; }
        public string? Table_Name { get; set; }
        [Required(ErrorMessage = "Please select a table form.")]
        public int TableId { get; set; }
        public string? ColumnName { get; set; }
    }
}


