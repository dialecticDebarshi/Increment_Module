using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StaffType.Api.Interface;
using StaffType.Api.Models;
using TenantCompany.Models;

namespace StaffType.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncrementEligibilityAPIController : Controller
    {
        private IIncrementEligibility iIncrementEligibility;
        public IncrementEligibilityAPIController(IIncrementEligibility incrementEligibilityrepo)
        {
            iIncrementEligibility = incrementEligibilityrepo;
        }



        [HttpGet]
        [Route("GetIncrementEligibility")]
        public IEnumerable<IncrementEligibilityAPIModel> GetIncrementEligibility()
        {
            return iIncrementEligibility.GetIncrementEligibility();
        }

        [HttpGet]
        [Route("GetTableForm")]
        public List<SelectListItem> GetTableForm()
        {
            List<SelectListItem> dt = iIncrementEligibility.GetTableForm();
            return dt;

        }
        [HttpGet]
        [Route("GetFieldForm/{ApplicationDataTable_Master_KEY}")]
        public string GetFieldForm(int ApplicationDataTable_Master_KEY)
        {
            return iIncrementEligibility.GetFieldForm(ApplicationDataTable_Master_KEY);


        }

        [HttpPost]
        [Route("SaveIncrementEligibility")]
        public int SaveIncrementEligibility(IncrementEligibilityAPIModel model)
        {
            if (model.MAST_INCREMENT_ELIGIBILITY_KEY == 0)
            {
                return iIncrementEligibility.SaveIncrementEligibility(model, "INSERT");
            }
            else 
            
            {
                return iIncrementEligibility.SaveIncrementEligibility(model, "UPDATE");
            }
            //return iIncrementEligibility.SaveIncrementEligibility(model);
        }



        [HttpGet]
        [Route("EditIncrementEligibility/{id}")]
        public IEnumerable<IncrementEligibilityAPIModel> EditIncrementEligibility(int id)
        {
            return iIncrementEligibility.GetIncrementEligibility(id);

        }

        [HttpGet]
        [Route("DeleteIncrementEligibility/{id}")]
        public int DeleteIncrementEligibility(int id)
        {
            IncrementEligibilityAPIModel Model = new IncrementEligibilityAPIModel();
            // MIUI.REC_TYPE = "DELETE";

            Model.MAST_INCREMENT_ELIGIBILITY_KEY = id;

            int r = iIncrementEligibility.DeleteIncrementEligibility(Model,"DELETE");
            return r;

        }


    }
}

