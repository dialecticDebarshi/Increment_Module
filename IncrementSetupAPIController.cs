using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffType.Api.Interface;
using StaffType.Api.Models;

namespace StaffType.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncrementSetupAPIController : ControllerBase
    {
        I_IncrementSetup I_IncrementSetup;
        public IncrementSetupAPIController(I_IncrementSetup i_IncrementSetup)
        {
            I_IncrementSetup = i_IncrementSetup;
        }
        [HttpGet]
        [Route("GetCompany")]
        public List<object> GetCompany()
        {
           return I_IncrementSetup.GetCompanyNames();
        }
        [HttpPost]
        [Route("GetCostCenterList")]
        public string GetCostCenterList(IncrementSetupAPIModel model)
        {
            return I_IncrementSetup.GetCostCenterList(model.Companykey);
        }
        [HttpPost]
        [Route("GetDivisionList")]
        public string GetDivisionList(IncrementSetupAPIModel model)
        {
            return I_IncrementSetup.GetDivisionList(model.Costcenterkey);
        }
        [HttpPost]
        [Route("GetWarehouseList")]
        public string GetWarehouseList(IncrementSetupAPIModel model)
        {
            return I_IncrementSetup.GetWarehouseList(model.DivisionKey, model.Companykey);
        }
        [HttpPost]
        [Route("GetDesignations")]
        public string GetDesignations(IncrementSetupAPIModel model)
        {
            return I_IncrementSetup.GetDesignationsCostcenter_Division_Warehouse(model.Companykey, model.Costcenterkey, model. DivisionKey, model.WarehouseKey,Convert.ToInt32(model.SetupKey));
        }

        [HttpGet]
        [Route("GetApplicableComponent")]
        public List<object> GetApplicableComponent()
        {
            return I_IncrementSetup.GetApplicableComponent();
        }

        [HttpGet]
        [Route("GetEligibility")]
        public string GetEligibility()
        {
            return I_IncrementSetup.LoadEligibility();
        }
        [HttpPost]
        [Route("SaveIncrementSetup")]
        public int SaveIncrementSetup(List<IncrementSetupAPIModel> model)
        {
            return I_IncrementSetup.SaveincrementSetup(model);

        }
        [HttpGet]
    
        public List<IncrementSetupAPIModel> Get()
        {
            var r = I_IncrementSetup.getallincrementsetup();
            return r;
        }
        [HttpGet]
        [Route("GetByid/{INCREMENT_SETUP_KEY}")]
        public List<IncrementSetupAPIModel> GetByid(int INCREMENT_SETUP_KEY)
        {
            var r = I_IncrementSetup.getallincrementsetupByid(INCREMENT_SETUP_KEY);
            return r;
        }
        [HttpPost]
        [Route("DeleteSetup/{id}")]
        public int DeleteSetup(int id)
        {

            return I_IncrementSetup.Deletesetup(id);
        }

    }
}
