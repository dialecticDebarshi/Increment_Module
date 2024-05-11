

using Microsoft.AspNetCore.Mvc.Rendering;

namespace TenantCompany.Models
{
    public class IncrementUIModel
    {
        public int Mast_Hrd_Draft_Personnel_Key { get; set; }
        public decimal NetSalary { get; set; }
        public decimal NetCTCPA { get; set; }
        public decimal NetCTCPM { get; set; }
    }
}
