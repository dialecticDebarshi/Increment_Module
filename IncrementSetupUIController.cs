using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using TenantCompany.Models;

namespace TenantCompany.Controllers
{
    public class IncrementSetupUIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string BaseUrl;
        string urlParameters = "";
        private readonly TenantCompanyProfileController _tenantCompanyProfile;
        private readonly string BaseUrlAuth;

        public IncrementSetupUIController(IConfiguration configuration)
        {
            _tenantCompanyProfile = new TenantCompanyProfileController(configuration);

            BaseUrlAuth = configuration["BaseUrlAuth"];
            _configuration = configuration;
            BaseUrl = configuration["BaseUrlIncrementSetup"];
        }
        public async Task< IActionResult> Index()
        {
            int moduleID = 95;
            string jsonses = HttpContext.Session.GetString("UserData");
            VM_UserLoginResponse loginresponseData = new VM_UserLoginResponse();
            loginresponseData = JsonConvert.DeserializeObject<VM_UserLoginResponse>(jsonses);
            string ActionIds = await _tenantCompanyProfile.AcID(BaseUrlAuth, loginresponseData.Company, loginresponseData.DesignationId, loginresponseData.AppID, loginresponseData.Employee_Master_Key, moduleID);
            string[] actionIdArray = ActionIds.Split(',').Select(s => s.Trim()).ToArray();
            ViewBag.MyArray = actionIdArray;
            //string urlParameters = "Getsetup";
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(BaseUrl);
                var Companylist = await GetCompany_Names();
                ViewBag.CompanyList = new SelectList(Companylist, "Value", "Text");

                var ApplicableComponent = await GetApplicableComponent();
                ViewBag.ApplicableComponent = new SelectList(ApplicableComponent, "Value", "Text");

                await FetchEligibilty();
                if (response.IsSuccessStatusCode)
                {
                    
                    
                    var data = await response.Content.ReadAsStringAsync();
                    List<IncrementSetupUIModel> lst = JsonConvert.DeserializeObject<List<IncrementSetupUIModel>>(data);
                    ViewBag.inc = lst;
                    
                    // Pass the data to the View.
                    return View();
                }
                else
                {
                    // Handle the API error if needed.
                    // You can show an error page or return an error message.
                    return View("Error");
                }
            }
         
        }
        public async Task<List<SelectListItem>> GetCompany_Names()
        {
            string urlParameters = "GetCompany";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(BaseUrl + urlParameters);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var Companylist = JsonConvert.DeserializeObject<List<SelectListItem>>(responseData);

                        // Add the "Select" option to the beginning of the list
                        //groupHeadList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
                        Companylist.Insert(0, new SelectListItem { Text = "-- Select --", Value = "" });
                        return Companylist;
                    }
                    else
                    {
                        // Handle the error response
                        throw new Exception("Failed to retrieve CompanyList.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<JsonResult> GetCostCenters(IncrementSetupUIModel model)
        {
            string urlparameters = "GetCostCenterList";
            var httpclient = new HttpClient();
            //var dataToSerialize = new { EmployeeReportingBoss = model.EmployeeReportingBoss };

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await httpclient.PostAsync(BaseUrl + urlparameters, content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                //var result = new { responseData };

                return Json(responseData);
            }
            else
            {
                return Json("Error");
            }
        }


        public async Task<JsonResult> GetDivisions(IncrementSetupUIModel model)
        {
            string urlparameters = "GetDivisionList";
            var httpclient = new HttpClient();
            //var dataToSerialize = new { EmployeeReportingBoss = model.EmployeeReportingBoss };

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await httpclient.PostAsync(BaseUrl + urlparameters, content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                //var result = new { responseData };

                return Json(responseData);
            }
            else
            {
                return Json("Error");
            }
        }




        public async Task<JsonResult> GetWarehouse(int DivisionKey, int CompanyKey)
        {

            IncrementSetupUIModel model = new IncrementSetupUIModel();

            model.Companykey = CompanyKey;
            model.DivisionKey = DivisionKey;
            string urlparameters = "GetWarehouseList";
            var httpclient = new HttpClient();
            //var dataToSerialize = new { EmployeeReportingBoss = model.EmployeeReportingBoss };

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await httpclient.PostAsync(BaseUrl + urlparameters, content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                //var result = new { responseData };

                return Json(responseData);
            }
            else
            {
                return Json("Error");
            }
        }








        public async Task<JsonResult> FetchDisignations( int Companykey,int Costcenterkey,int DivisionKey,int WarehouseKey,int SetupKey)
        {

            string urlparametrs = "GetDesignations";
            var httpclient = new HttpClient();


            var dataToSerialize = new { Companykey = Companykey, Costcenterkey = Costcenterkey, DivisionKey = DivisionKey, WarehouseKey = WarehouseKey , SetupKey = SetupKey };

            var content = new StringContent(JsonConvert.SerializeObject(dataToSerialize), Encoding.UTF8, "application/json");

            var response = await httpclient.PostAsync(BaseUrl + urlparametrs, content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();



                return Json(responseData);
            }
            else
            {
                return Json("Error");
            }

        }

        public async Task<List<SelectListItem>> GetApplicableComponent()
        {
            string urlParameters = "GetApplicableComponent";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(BaseUrl + urlParameters);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var Companylist = JsonConvert.DeserializeObject<List<SelectListItem>>(responseData);

                        // Add the "Select" option to the beginning of the list
                        //groupHeadList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
                        Companylist.Insert(0, new SelectListItem { Text = "-- Select --", Value = "" });
                        return Companylist;
                    }
                    else
                    {
                        // Handle the error response
                        throw new Exception("Failed to retrieve CompanyList.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<JsonResult> FetchEligibilty()
        {

            string urlparametrs = "GetEligibility";
            var httpclient = new HttpClient();


            //var dataToSerialize = new { Companykey = Companykey, Costcenterkey = Costcenterkey, DivisionKey = DivisionKey, WarehouseKey = WarehouseKey };

            //var content = new StringContent(JsonConvert.SerializeObject(dataToSerialize), Encoding.UTF8, "application/json");

            var response = await httpclient.GetAsync(BaseUrl + urlparametrs);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();



                return Json(responseData);
            }
            else
            {
                return Json("Error");
            }

        }

        [HttpPost]
        public async Task<JsonResult> SaveIncrement_setup([FromBody] List<IncrementSetupUIModel> model)
        {
            try
            {
               


                string urlparameters = "SaveIncrementSetup";
                var httpclient = new HttpClient();

                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await httpclient.PostAsync(BaseUrl + urlparameters, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var R = new JsonResult(responseData);
                    return R;
                }
                else
                {
                    return Json("Error");
                }







            }
            catch (Exception ex)
            {
                // Log the exception or return a specific error message
                return Json("An error occurred: " + ex.Message);
            }

        }



        public async Task<IActionResult> GetIncrementSetupByID(int id)
        {

            string urlParameters = "GetByid/" + id;

            using (var httpClient = new HttpClient())
            {
                // Construct the URL with the ID parameter
                string apiUrl = BaseUrl + urlParameters;

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the API response to your desired data model or use dynamic if the structure is unknown.
                    var data = await response.Content.ReadAsStringAsync();
                    //var info = JsonConvert.DeserializeObject<StaffTypeUIModel>(data);
                   
                   List<IncrementSetupUIModel> lst = JsonConvert.DeserializeObject<List<IncrementSetupUIModel>>(data);

                    // Return the JSON response directly as JSON data
                   
                    return Json(lst);
                }
                else
                {
                    // Handle the API error if needed.
                    // You can show an error page or return an error message.
                    return Json("Error");
                }
            }
        }
        public async Task<IActionResult> Deletesetup(int id)
        {
            string msg = "";
            string urlParameters = "DeleteSetup/" + id;
            using (var httpClient = new HttpClient())
            {
                string apiUrl = BaseUrl + urlParameters;
                StringContent content = new StringContent(id.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the API response to your desired data model or use dynamic if the structure is unknown.
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (int.TryParse(responseContent, out int apiResponse))
                    {
                        if (apiResponse > 0)
                        {
                            msg = "success";
                        }
                        else
                        {
                            msg = "Fail";
                        }

                        return Json(msg);
                    }
                    else
                    {
                        return Json("Error");
                    }
                }
                else
                {
                    return Json("Error");
                }
            }

        }

    }
}
