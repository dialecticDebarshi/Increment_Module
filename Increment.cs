using System.ComponentModel.DataAnnotations;

namespace TenantCompany.Models
{
    public class Increment
    {

        
        [Required(ErrorMessage = "Please Select Company")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid company.")]


        public int Company_Key { get; set; }
        //public string? Companyname { get; set; } 
        public int Costcenter_Key { get; set; }
        //public string?  Costcentername { get; set; }

        public int Division_Key { get; set; }
        //public string? Divisionname { get; set; }
        //public int WareHouse_Key { get; set; }

        public string? Warehousename { get; set; }

        public int stafftypeid { get; set; }    

        //public string? staffname { get; set; }  
        //public int Designationid { get; set; }    

        public string? Designationname { get; set; }
        public DateTime EffectiveDate { get; set; }
    

    }

    public class ValidateNewDeduction
    {
        public int MastHrdDraftPersonnelKey { get;set; }
        public int StaffTypeId { get;set; }
        public int CompanyKey { get;set; }
        public int MastIncrement_Key { get; set; }
        public int WarehouseKey { get; set; }
        public int MastSalaryTemplateKey { get;set; }
        public List<string> salaryHead { get; set; }
        public List<string> Amount { get; set; }
        public List<string> StatutoryHeadKey { get; set; }
        public List<string> DtlsStatutoryHeadKey { get; set; }
    }

    public class IncrementRB
    {
        public int Mast_Increment_key { get; set; }

        public int employeeMasterKey { get; set; }

        public decimal REVISED_NET_SALARY_TOTAL { get; set; }

        public decimal INCREMENTED_AMOUNT { get; set; }

		// ADDED 24-04-2024
		public decimal FINAL_INCREMENTED_AMOUNT { get; set; }

		public int USER_KEY { get; set; }
	}
    public class Save_Increment
    {
        public int MAST_HRD_DRAFT_PERSONNEL_KEY { get; set; }
        public int EMPLOYEE_MASTER_KEY { get; set; }
        public int MAST_INCREMENT_KEY { get; set; }
        public decimal GROSS { get; set; }
        public decimal EMPLOYEE_DEDUCTION { get; set; }
        public decimal EMPLOYER_DEDUCTION { get; set; }
        public decimal NET_SALARY { get; set; }
        public decimal NET_SALARY_OTHERS { get; set; }
        public decimal OTHERS_TOTAL { get; set; }
        public int COMPANY_KEY { get; set; }
        public string DATA_SALARY { get; set; }
        public string DATA_OTHERS { get; set; }
        public DateTime EFFECTIVE_DATE { get; set; }

    }
    public class IncrementApproveData
    {
        public int MAST_HRD_DRAFT_PERSONNEL_KEY { get; set; }
        public int EMPLOYEE_MASTER_KEY { get; set; }
        public int MAST_INCREMENT_KEY { get; set; }
    }

}
