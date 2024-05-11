using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.JSInterop.Implementation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using TenantCompany.Models;

namespace TenantCompany.Controllers
{
    public class IncrementController : Controller
    {
        public readonly string BaseUrl;
        public readonly string BaseUrl1;
        public readonly string BaseUrl2;
        public readonly string BaseUrlAuth;
        public readonly IConfiguration _configuration;
        private readonly TenantCompanyProfileController _tenantCompanyProfile;
        public IncrementController(IConfiguration configuration) 
        {
            _configuration=configuration;
            BaseUrl = configuration["BaseUrl"];
            BaseUrl1 = configuration["BaseUrlFinance&Commercial"];
            BaseUrl2 = configuration["BaseUrlStatury"];
            _tenantCompanyProfile = new TenantCompanyProfileController(configuration);
            BaseUrlAuth = configuration["BaseUrlAuth"];

        }
        public async Task<IActionResult> Increment()
       {
            string urlPara = "GetEmployeeHeader";

            try
            {

                using (var httpClient = new HttpClient())
                {

                    HttpResponseMessage response = await httpClient.GetAsync(BaseUrl + urlPara);


                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        List<Increment> lst = JsonConvert.DeserializeObject<List<Increment>>(data);
                        ViewBag.EmployeeDetails = lst;
                        //var DesignationList = await FetchDesignation();
                        //ViewBag.DesignationList = new SelectList(DesignationList, "Value", "Text");


                        var Stafftype = await GetStafftype();
                        ViewBag.stafftype = new SelectList(Stafftype, "Value", "Text");

                        var costCenterList = await GetCompany_Names();
                        ViewBag.company = new SelectList(costCenterList, "Value", "Text");
                       





                            // Warehouse

                            return View();

                    }
                     
                        else { return View(); }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return View();
        }

        public async Task<JsonResult> GetEmployeeData(Increment model)
        {
            try
            {
                string urlPara = "";
                string jsonses = HttpContext.Session.GetString("UserData");

                VM_UserLoginResponse loginresponseData2 = JsonConvert.DeserializeObject<VM_UserLoginResponse>(jsonses);

                if(loginresponseData2.Employee_Master_Key==1)
                {
                     urlPara = "GetEmployeeDataApproval";
                }
                else
                {
                     urlPara = "GetEmployeeData";
                }
                
                //var dataToSerialize = new { Company_Key = model.Company_Key, Costcenter_Key=model.Costcenter_Key, Division_Key=model.Division_Key, WareHouse_Key =model.WareHouse_Key, stafftypeid =model.stafftypeid};
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {

                    HttpResponseMessage rMessage = await httpClient.PostAsync(BaseUrl+urlPara,content);
                    if(rMessage.IsSuccessStatusCode)
                    {
                        var data = await rMessage.Content.ReadAsStringAsync();
                        return Json(data);
                    }
                    else
                    {
                        return Json("error");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<JsonResult> IncrementSave([FromBody] IncrementUIModel model)
        {
            try
            {
                //decimal m = model.NetCTCPA;

                string UrlPara = "SaveIncrementById";
                using(var httpClient = new HttpClient())
                {
                    var content = JsonConvert.SerializeObject(model);
                    StringContent contentString = new StringContent(content, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await httpClient.PostAsync(BaseUrl + UrlPara, contentString);
                    if(resp.IsSuccessStatusCode)
                    {
                        var response = await resp.Content.ReadAsStringAsync();
                        return Json(response);
                    }
                }
                return Json("");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        //  CC,DD,WW
        #region RB SECTION
        // == RB SECTION 

        public async Task<IActionResult> IncrementRB()
        {
            //string urlPara = "GetEmployeeHeaderRB";
            string jsonses = HttpContext.Session.GetString("UserData");
            dynamic userData = JsonConvert.DeserializeObject<dynamic>(jsonses);

            string EmployeeMasterlkey = userData.employee_Master_Key;
            
            //string urlPara = "GetEmployeeDataRB/"+ EmployeeMasterlkey;
            //string urlPara = "GetEmployeeHeaderRB";

            int moduleID = 35;
            //try
            //{


            //    using (var httpClient = new HttpClient())
            //    {

            //        HttpResponseMessage response = await httpClient.GetAsync(BaseUrl + urlPara);

            //        //var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            //        //HttpResponseMessage rMessage = await httpClient.PostAsync(BaseUrl + urlPara, content);
            //            if (response.IsSuccessStatusCode)
            //            {
            //                var data = await response.Content.ReadAsStringAsync();
            //                 //var EmployeeDataRB = await GetEmployeeDataRB(EmployeeMasterlkey);
            //                return View();
            //            }
            //            else
            //            {
            //                return Json("error");
            //            }
            //        }
            //    }

            //catch (Exception ex)
            //{
            //    throw ex;

            //}
            return View();
        }



        public async Task<JsonResult> GetEmployeeDataRB()
        {
            try
            {
               
                string jsonses = HttpContext.Session.GetString("UserData");
                dynamic userData = JsonConvert.DeserializeObject<dynamic>(jsonses);
                dynamic userData1 = JsonConvert.DeserializeObject<dynamic>(jsonses);
                string EmployeeMasterlkey = "";
                EmployeeMasterlkey = userData.employee_Master_Key;
                string userTypeList = "";
                userTypeList = userData1.UserTypeList;
                JArray userTypeList1 = JArray.Parse(userTypeList);
                string reportingBossValue = "0";
              
                foreach (var userType in userTypeList1)
                {
                    if (userType["text"].ToString() == "Reporting Boss")
                    {
                        reportingBossValue = userType["value"].ToString();
                        break; 
                    }
                }
               
                string urlPara = $"GetEmployeeDataDP/{reportingBossValue}";
                string urlPara1 = $"GetEmployeeDataRB/{EmployeeMasterlkey}/{userData.Company}";
                using (var httpClient = new HttpClient())
                    {

                         HttpResponseMessage rMessage;
                   
                        // You may want to handle this case differently
                     rMessage = await httpClient.GetAsync(BaseUrl + urlPara1);
                   
                    

                    if (rMessage != null && rMessage.IsSuccessStatusCode)
                    {
                        var data = await rMessage.Content.ReadAsStringAsync();
                        return Json(data);
                    }
                    else
                    {
                        return Json("error");
                    }

                }



                //var dataToSerialize = new { Company_Key = model.Company_Key, Costcenter_Key=model.Costcenter_Key, Division_Key=model.Division_Key, WareHouse_Key =model.WareHouse_Key, stafftypeid =model.stafftypeid};
                //var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                return Json("");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<JsonResult> SaveIncrementRB([FromBody] List<IncrementRB> model)
        {
            string UrlParemetrs = "SaveIncrementRB";


			// ====MD====

			string SessionData = HttpContext.Session.GetString("UserData");

			VM_UserLoginResponse loginresponseData2 = JsonConvert.DeserializeObject<VM_UserLoginResponse>(SessionData);

			if (loginresponseData2.Employee_Master_Key == 1)
			{
				foreach (var item in model)
				{
					item.USER_KEY = 1;
				}
			}
			else
			{
				foreach (var item in model)
				{
					item.USER_KEY = loginresponseData2.Employee_Master_Key;
				}
			}
			

            // =======MD========
			try
            {
                using (var httpClient = new HttpClient())
                {
                    var content = JsonConvert.SerializeObject(model);
                    StringContent contentString = new StringContent(content, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await httpClient.PostAsync(BaseUrl + UrlParemetrs, contentString);
                    if (resp.IsSuccessStatusCode)
                    {
                        var response = await resp.Content.ReadAsStringAsync();
                        return Json(response);
                    }

                    return Json("");
                }
            }
            catch (Exception ex)
            { 
                throw ex;
            }       
            

        }



        // ==
        #endregion
        public async Task<List<SelectListItem>> GetCompany_Names()
        {
            string urlParameters = "FETCH_COMPANY_LIST";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(BaseUrl1 + urlParameters);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var Companylist = JsonConvert.DeserializeObject<List<SelectListItem>>(responseData);

                        // Add the "Select" option to the beginning of the list
                        //groupHeadList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
                        Companylist.Insert(0, new SelectListItem { Text = "-- Select --", Value = "0" });
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




        [HttpPost]
        public async Task<JsonResult> GetCostCentersForFC(int Company_Key)
        {


            string urlparameters = "GetCostCenterList/";


            try
            {
                using (var httpClient = new HttpClient())
                {


                    string apiurl = BaseUrl1 + urlparameters;
                    HttpResponseMessage response = await httpClient.GetAsync(apiurl + Company_Key);



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
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve CompanyList.");
            }


        }

        public async Task<JsonResult> GetDivisionsForFC(int Costcenter_Key)
        {
            string urlparameters = "GetDivisionList/" + Costcenter_Key.ToString();
            var httpclient = new HttpClient();
            //var dataToSerialize = new { EmployeeReportingBoss = model.EmployeeReportingBoss };

            Finance_CommercialUIModel obj = new Finance_CommercialUIModel();
            obj.Costcenter_Key = Costcenter_Key;

            //var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            var response = await httpclient.GetAsync(BaseUrl1 + urlparameters);
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


        public async Task<JsonResult> GetWarehouseForFC(int DivisionKey, int CompanyKey)
        {


            string urlparameters = "GetWarehouseCheckboxes/" + DivisionKey + "/" + Convert.ToString(CompanyKey); 
            var httpclient = new HttpClient();
            //var dataToSerialize = new { EmployeeReportingBoss = model.EmployeeReportingBoss };



            var response = await httpclient.GetAsync(BaseUrl + urlparameters);
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
        public async Task<List<SelectListItem>> GetStafftype()
        {
            string urlParameters = "fetchtstafftype";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(BaseUrl+urlParameters);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var stafflist = JsonConvert.DeserializeObject<List<SelectListItem>>(responseData);

                        // Add the "Select" option to the beginning of the list
                        //groupHeadList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
                        stafflist.Insert(0, new SelectListItem { Text = "-- Select --", Value = "0" });
                        return stafflist;

                        
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

        
        public async Task<JsonResult> FetchDesignation(string Warehouse)
        {
            string urlParameters = "GetdesignationInc/" + Warehouse;
            try
            {
                var designationList = new List<SelectListItem>();

                var warehouseArray = Warehouse.Split(',');

                using (var httpClient = new HttpClient())
                {
                    foreach (var warehouse in warehouseArray)
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(BaseUrl + urlParameters);
                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();
                            //var designationItems = JsonConvert.DeserializeObject<List<SelectListItem>>(responseData);
                            //designationList.AddRange(designationItems);
                            return Json(responseData);
                        }
                        else
                        {
                            return Json("Error");
                            // Handle the error response
                            throw new Exception($"Failed to retrieve designation list for warehouse: {warehouse}. Status code: {response.StatusCode}");
                        }
                    }
                }

                // Set ViewBag with the designation list
                //ViewBag.DesignationList = designationList;
                //        DesignationList.Insert(0, new SelectListItem { Text = "-- Select --", Value = "0" });
                        //return Companylist;
               return Json("");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
      

        public IActionResult Grid()
        {
            return View();
        }
        public async Task<JsonResult> ValidateNewDeduction(ValidateNewDeduction model)
        {
            try
            {
                string urlPara = "EmployeeNewDeductionData";
                //var dataToSerialize = new { Company_Key = model.Company_Key, Costcenter_Key=model.Costcenter_Key, Division_Key=model.Division_Key, WareHouse_Key =model.WareHouse_Key, stafftypeid =model.stafftypeid};
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {

                    HttpResponseMessage rMessage = await httpClient.PostAsync(BaseUrl + urlPara, content);
                    if (rMessage.IsSuccessStatusCode)
                    {
                        var data = await rMessage.Content.ReadAsStringAsync();
                        //var dataValidation = JsonConvert.DeserializeObject<List<SelectListItem>>(responseData);
                        return Json(data);
                    }
                    else
                    {
                        return Json("error");
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Error");
            }
        }

        //public JsonResult  staffs()
        //{

        //    Root root = new Root();
        //    List<Datum> data = new List<Datum>();
        //    data.Add(new Datum
        //    {
        //        age = "30",
        //        DT_RowId = "1",
        //        email = "test@gmail.com",
        //        extn = "txt",
        //        first_name = "samir",
        //        last_name = "Sen",
        //        office = "p29",
        //        position = "Staff",
        //        salary = "10000",
        //        start_date = "2024-02-01"
        //    }
        //   );

        //    data.Add(new Datum
        //    {
        //        age = "30",
        //        DT_RowId = "1",
        //        email = "test@gmail.com",
        //        extn = "txt",
        //        first_name = "samir",
        //        last_name = "Sen",
        //        office = "p29",
        //        position = "Staff",
        //        salary = "10000",
        //        start_date = "2024-02-01"
        //    }
        //  );
        //    data.Add(new Datum
        //    {
        //        age = "30",
        //        DT_RowId = "1",
        //        email = "test@gmail.com",
        //        extn = "txt",
        //        first_name = "samir",
        //        last_name = "Sen",
        //        office = "p29",
        //        position = "Staff",
        //        salary = "10000",
        //        start_date = "2024-02-01"
        //    }
        //  );

        //    root.data= data;

        //    return Json( root );
        //}

        public async Task<JsonResult> SaveIncrement(Save_Increment model)
        {
            try
            {
                string jsonses = HttpContext.Session.GetString("UserData");

                VM_UserLoginResponse loginresponseData2 = JsonConvert.DeserializeObject<VM_UserLoginResponse>(jsonses);
                model.EMPLOYEE_MASTER_KEY = loginresponseData2.Employee_Master_Key;
                string urlPara = "SaveEmployeeIncrement";
                //var dataToSerialize = new { Company_Key = model.Company_Key, Costcenter_Key=model.Costcenter_Key, Division_Key=model.Division_Key, WareHouse_Key =model.WareHouse_Key, stafftypeid =model.stafftypeid};
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {

                    HttpResponseMessage rMessage = await httpClient.PostAsync(BaseUrl + urlPara, content);
                    if (rMessage.IsSuccessStatusCode)
                    {
                        var data = await rMessage.Content.ReadAsStringAsync();
                        //var dataValidation = JsonConvert.DeserializeObject<List<SelectListItem>>(responseData);
                        return Json(data);
                    }
                    else
                    {
                        return Json("error");
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Error");
            }
        }

        public async Task<IActionResult> IncrementApproval()
        {

            string urlPara = "GetEmployeeHeader";

            try
            {

                using (var httpClient = new HttpClient())
                {

                    HttpResponseMessage response = await httpClient.GetAsync(BaseUrl + urlPara);


                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        List<Increment> lst = JsonConvert.DeserializeObject<List<Increment>>(data);
                        ViewBag.EmployeeDetails = lst;
                        //var DesignationList = await FetchDesignation();
                        //ViewBag.DesignationList = new SelectList(DesignationList, "Value", "Text");


                        var Stafftype = await GetStafftype();
                        ViewBag.stafftype = new SelectList(Stafftype, "Value", "Text");

                        var costCenterList = await GetCompany_Names();
                        ViewBag.company = new SelectList(costCenterList, "Value", "Text");






                        // Warehouse

                        return View();

                    }

                    else { return View(); }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return View();
        }

        [HttpPost]
        public async Task<JsonResult> IncrementFinalApproval(IncrementApproveData model)
        {
            try
            {
                string urlPara = "IncrementFinalApproval";
                

                

                //var dataToSerialize = new { Company_Key = model.Company_Key, Costcenter_Key=model.Costcenter_Key, Division_Key=model.Division_Key, WareHouse_Key =model.WareHouse_Key, stafftypeid =model.stafftypeid};
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {

                    HttpResponseMessage rMessage = await httpClient.PostAsync(BaseUrl + urlPara, content);
                    if (rMessage.IsSuccessStatusCode)
                    {
                        var data = await rMessage.Content.ReadAsStringAsync();
                        return Json(data);
                    }
                    else
                    {
                        return Json("error");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }





    
    

    public class Datum
    {
        public string DT_RowId { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string position { get; set; }
        public string email { get; set; }
        public string office { get; set; }
        public string extn { get; set; }
        public string age { get; set; }
        public string salary { get; set; }
        public string start_date { get; set; }
    }

    public class Root
    {
        public List<Datum> data { get; set; }
    }
}
