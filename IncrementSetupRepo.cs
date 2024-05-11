using StaffType.Api.DAL;
using StaffType.Api.Interface;
using StaffType.Api.Models;
using System.Data;
using System.Runtime.InteropServices.ComTypes;

namespace StaffType.Api.Repositoris
{
    public class IncrementSetupRepo: I_IncrementSetup
    {
        private readonly IConfiguration _configuration;
        private readonly DbAccess _dbAccess;
        public IncrementSetupRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbAccess = new DbAccess(_configuration);

        }

        #region IncrementSetup
        public List<object> GetCompanyNames()
        {
            try
            {
                string[] pname = { "@GET_REC_TYPE ", "@COMPANY_KEY ", "@COMPANY_NAME" };
                string[] pvalue = { "ALL", "0", "0" };
                DataSet ds = _dbAccess.Ds_Process("SP_GET_SYS_COMPANY", pname, pvalue);
                List<object> types = new List<object>();

                foreach (DataRow item in ds.Tables[0].Rows)
                {

                    types.Add(
                        new { value = Convert.ToInt32(item["CompanyId"]), text = item["COMPANY_NAME"].ToString() }
                        );


                }


                return types;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public string GetCostCenterList(int company_key)
        {
            //string ii = "";

            string dropdown = "<select>";
            dropdown += $"<option value='{0}'>{"--Select--"}</option>";
            try
            {
                string[] pname = { "@COMPANY_KEY", "@LABEL_KEY" };
                string[] pvalue = { company_key.ToString(), "0" };
                DataSet ds = _dbAccess.Ds_Process("SP_GET_NEW_COSTCENTERS_NAME", pname, pvalue);
                List<object> types = new List<object>();

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    string costCenterName = item["SubDivisionName"].ToString();
                    int costCenterKey = Convert.ToInt32(item["SubDivisionId"]);

                    //dropdown += "<option value=''>costCenterName</option>";
                    dropdown += $"<option value='{costCenterKey}'>{costCenterName}</option>";
                }

                dropdown += "</select>";
                return dropdown;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public string GetDivisionList(int CostCenter_Key)
        {
            //string ii = "";

            string dropdown = "<select>";
            dropdown += $"<option value='{0}'>{"--Select--"}</option>";
            try
            {
                string[] pname = { "@ID", "@CompanyId" };
                string[] pvalue = { CostCenter_Key.ToString(), "0" };
                DataSet ds = _dbAccess.Ds_Process("SP_GET_NEW_DIVISIONS_NAME", pname, pvalue);
                List<object> types = new List<object>();

                foreach (DataRow item in ds.Tables[0].Rows)
                {


                    //dropdown += "<option value=''>costCenterName</option>";
                    dropdown += $"<option value='{Convert.ToInt32(item["DIVISION_KEY"])}'>{item["DIVISION_NAME"].ToString()}</option>";
                }

                dropdown += "</select>";
                return dropdown;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string GetWarehouseList(int Division_Key, int Company_Key)
        {
            //string ii = "";

            string dropdown = "<select>";
            dropdown += $"<option value='{0}'>{"--Select--"}</option>";
            try
            {
                string[] pname = { "@GET_REC_TYPE", "@DIVISION_KEY", "@WAREHOUSE_CODE", "@WAREHOUSE_DESC", "@INITIAL", "@USER_KEY", "@COMPANY_KEY" };
                string[] pvalue = { "DDL", Division_Key.ToString(), "0", "", "0", "0", Company_Key.ToString() };
                DataSet ds = _dbAccess.Ds_Process("SP_GET_INV_MAST_WAREHOUSE", pname, pvalue);
                List<object> types = new List<object>();

                foreach (DataRow item in ds.Tables[0].Rows)
                {


                    //dropdown += "<option value=''>costCenterName</option>";
                    dropdown += $"<option value='{Convert.ToInt32(item["MAST_INV_WAREHOUSE_KEY"])}'>{item["WAREHOUSE_DESC"].ToString()}</option>";
                }

                dropdown += "</select>";
                return dropdown;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        //lOAD DESIGNATIONS
        public string GetDesignationsCostcenter_Division_Warehouse(int company_key, int costcenter_key, int division_key, int warehouse_key,int SetupKey)
        {

            string ii = ""; //"<tbody class='' style='width: 100%;'>";

            int DesignationId = 0;
            string LabelAmountMIN = "MinAmount";
            string LabelAmountMAX = "MaxAmount";
            string LabelPercentMIN = "Min%";
            string LabelPercentMax = "Max%";

            try
            {
                string[] pname = { "@COMPANY_KEY", "@COSTCENTERID", "@DIVISIONID", "@WAREHOUSEID", "@RECTYPE", "@INCREMENT_SETUP_KEY" };
                string[] pvalue = { company_key.ToString(), costcenter_key.ToString(), division_key.ToString(), warehouse_key.ToString(), "DESIGNATIONS", SetupKey.ToString() };
                DataSet ds = _dbAccess.Ds_Process("SP_FETCH_INCREMENT_SETUP", pname, pvalue);

                foreach (DataRow item1 in ds.Tables[0].Rows)
                {
                    if (DesignationId.ToString() != item1["DesignationId"].ToString())
                    {
                        
                        ii += "<tr data-DesignationId='" + item1["DesignationId"].ToString() + "'>";
                        ii += "<td>";
                        ii += "<div class='row'>";
                        ii += "<span class='mb-2 font-weight-bold'>" + item1["DesignationName"].ToString() + "</span><br>";
                        ii += "<div class='col-md-3'>" + LabelAmountMIN;
                        ii += "<input type='number' class='form-control txtvaluesMINAMOUNT' data-DesignationId='" + item1["DesignationId"].ToString() + "' value='" + item1["MIN_AMOUNT"].ToString() + "' style='width:90px'/></div>";
                        ii += "<div class='col-md-3'>" + LabelAmountMAX;
                        ii += "<input type='number' class='form-control txtvaluesMAXAMOUNT' data-DesignationId='" + item1["DesignationId"].ToString() + "' value='" + item1["MAX_AMOUNT"].ToString() + "' style='width:90px'/></div>";
                        ii += "<div class='col-md-3'>" + LabelPercentMIN;
                        ii += "<input type='number' class='form-control txtvaluesMINPERCENTAGE' data-DesignationId='" + item1["DesignationId"].ToString() + "' value='" + item1["MIN_PERCENTAGE"].ToString() + "' style='width:90px'/></div>";
                        ii += "<div class='col-md-3'>" + LabelPercentMax;
                        ii += "<input type='number' class='form-control txtvaluesMAXPERCENTAGE' data-DesignationId='" + item1["DesignationId"].ToString() + "' value='" + item1["MAX_PERCENTAGE"].ToString() + "' style='width:90px'/></div>";
                        ii += "</div>";
                        ii += "</td>";
                        ii += "</tr>"; // Close the row
                    }
                }

                // Close the table body
                ii += "</tbody>";
            }
            catch (Exception ex)
            {
                // Handle the exception
                throw;
            }

            // Return the constructed table body
            return ii;
        }

        //public string GetDesignationsCostcenter_Division_Warehouse(int company_key, int costcenter_key, int division_key, int warehouse_key)
        //{

        //    string ii = "<tbody>";
        //    int DesignationId = 0;
        //    string LabelAmountMIN = "MinAmount";
        //    string LabelAmountMAX = "MaxAmount";
        //    string LabelPercentMIN = "Min%";
        //    string LabelPercentMax = "Max%";
        //    try
        //    {

        //        string[] pname = { "@COMPANY_KEY", "@COSTCENTERID", "@DIVISIONID", "@WAREHOUSEID", "@RECTYPE" };
        //        string[] pvalue = {company_key.ToString(), costcenter_key.ToString(),division_key.ToString(),warehouse_key.ToString(), "DESIGNATIONS" };
        //        DataSet ds = _dbAccess.Ds_Process("SP_FETCH_INCREMENT_SETUP", pname, pvalue);






        //        foreach (DataRow item1 in ds.Tables[0].Rows)
        //        {
        //            if (DesignationId.ToString() != item1["DesignationId"].ToString())
        //            {
        //                ii += "<tr data-DesignationId='" + item1["DesignationId"].ToString() + ">";
        //                ii += "<td>" + item1["DesignationName"].ToString() + "<br>" +
        //                      LabelAmountMIN + ":" + "&nbsp;" +
        //                      "<input type='number' class='form-class txtvaluesMINAMOUNT' data-DesignationId='" + item1["DesignationId"].ToString() + "' value='" + item1["MIN_AMOUNT"].ToString() + "' style='width:90px'/>" + "&nbsp;" +
        //                      LabelAmountMAX + ":" + "&nbsp;" +
        //                      "<input type='number' class='form-class txtvaluesMAXAMOUNT' data-DesignationId='" + item1["DesignationId"].ToString() + "' value='" + item1["MAX_AMOUNT"].ToString() + "' style='width:90px'/>" + "&nbsp;" +
        //                      LabelPercentMIN + ":" + "&nbsp;" +
        //                      "<input type='number' class='form-class txtvaluesMINPERCENTAGE' data-DesignationId='" + item1["DesignationId"].ToString() + "' value='" + item1["MIN_PERCENTAGE"].ToString() + "' style='width:90px'/>" + "&nbsp;" +
        //                      LabelPercentMax + ":" + "&nbsp;" +
        //                      "<input type='number' class='form-class txtvaluesMAXPERCENTAGE'  data-DesignationId='" + item1["DesignationId"].ToString() + "' value='" + item1["MAX_PERCENTAGE"].ToString() + "' style='width:90px'/>" +
        //                      "</td>";
        //                ii += "</tr>";
        //            }
        //        }

        //        return ii;



        //        ii += "</tbody>";

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }


        //}



        public List<object> GetApplicableComponent()
        {
            try
            {
                string[] pname = { "@COMPANY_KEY", "@COSTCENTERID", "@DIVISIONID", "@WAREHOUSEID", "@RECTYPE" , "@INCREMENT_SETUP_KEY" };
                string[] pvalue = {"0","0","0","0" , "FETCH_APPLICABLE_COMP","0" };
                DataSet ds = _dbAccess.Ds_Process("SP_FETCH_INCREMENT_SETUP", pname, pvalue);
                List<object> types = new List<object>();

                foreach (DataRow item in ds.Tables[0].Rows)
                {

                    types.Add(
                        new { value = Convert.ToInt32(item["MAST_INCREMENT_COMPONENT_KEY"]), text = item["COMPONENT_NAME"].ToString() }
                        );


                }


                return types;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public string LoadEligibility()
        {
            string ii = "";
            int Incrementid = 0;
            string Value = "Value";
            try
            {
                string[] pname = { "@COMPANY_KEY", "@COSTCENTERID", "@DIVISIONID", "@WAREHOUSEID", "@RECTYPE", "@INCREMENT_SETUP_KEY" };
                string[] pvalue = { "0", "0", "0", "0", "LOAD_ELIG","0" };
                DataSet ds = _dbAccess.Ds_Process("SP_FETCH_INCREMENT_SETUP", pname, pvalue);






                foreach (DataRow item1 in ds.Tables[0].Rows)
                {
                    if (Incrementid.ToString() != item1["MAST_INCREMENT_ELIGIBILITY_KEY"].ToString())
                    {
                        ii += "<tr>";
                        ii += "<td>" + item1["ELIGIBILITY_NAME"].ToString()+ "<br>"+"&nbsp" + Value + "&nbsp" + "<input type='number' class=form-class style='width:90px'/>" + "</td>";

                        ii += "</tr>";
                    }
                }
                return ii;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public int SaveincrementSetup(List<IncrementSetupAPIModel> model)
        {
            int R = 0;
            try
            {
                IncrementSetupAPIModel model1 = new IncrementSetupAPIModel();


                if (model[0].SetupKey != 0 )
                {
                    Updatesetup(Convert.ToInt32(model[0].SetupKey));
                    foreach (IncrementSetupAPIModel employee in model)
                    {
                       // employee.SetupKey = R;

                        // Define parameter names and values
                        string[] pname = { "@INCREMENT_SETUP_KEY", "@DESIGNATION_ID", "@MAST_INCREMENT_COMPONENT_KEY", "@MAST_INCREMENT_ELIGIBILITY_KEY", "@REF_DATE", "@SETUP_APPLICABLE_ON_IR","@SETUP_APPLICABLE_ON_HR","@SETUP_APPLICABLE_ON_RB",
                "@APPLICABLE_ON_HR","@APPLICABLE_ON_RB","@APPLICABLE_ON_HR_CENTRAL","@COSTCENTER_KEY", "@DIVISION_KEY", "@WAREHOUSE_KEY", "@FINANCIALYEAR_STARTDATE", "@FINANCIALYEAR_ENDDATE", "@IMPLEMENTATION_STARTDATE", "@IMPLEMENTATION_ENDDATE",
                "@APPRAISAL_DIS_START_DATE", "@APPRAISAL_DIS_END_DATE", "@MIN_AMOUNT", "@MAX_AMOUNT", "@MIN_PERCENTAGE", "@MAX_PERCENTAGE", "@ELIGIBILITY_VALUE","@COMPANY_KEY","@RECTYPE"};

                        string[] pvalue = { employee.SetupKey.ToString(), employee.DesignationID.ToString(), employee.ApplicableComponent.ToString(), "0", employee.Refdate.ToString(),
                employee.SetupApplicableOn_IR.ToString(), employee.SetupApplicableOn_HR.ToString(), employee.SetupApplicableOn_RB.ToString(),employee.ApplicableTo_HR.ToString(), employee.ApplicableTo_RB.ToString(),employee.ApplicableTo_HR_CENTRAL.ToString(),employee.Costcenterkey.ToString(), employee.DivisionKey.ToString(), employee.WarehouseKey.ToString(),
                employee.FYStartDate.ToString(), employee.FYEndDate.ToString(), employee.ImplementationStartDate.ToString(), employee.ImplementationEndDate.ToString(),
                employee.AppraisalDISStartDate.ToString(), employee.AppraisalDISEndDate.ToString(), employee.Minamount?.ToString(), employee.Maxamount?.ToString(),
                employee.MinPercentage?.ToString(), employee.MaxPercentage?.ToString(), "0",employee.Companykey.ToString(),"UPDATE"};




                        // Call the stored procedure to save the data
                        R = _dbAccess.int_Process("SP_CRUD_INCREMENT_SETUP", pname, pvalue);

                        string[] pname1 = { "@INCREMENT_SETUP_KEY", "@DESIGNATION_ID", "@MAST_INCREMENT_COMPONENT_KEY", "@MAST_INCREMENT_ELIGIBILITY_KEY", "@REF_DATE", "@SETUP_APPLICABLE_ON_IR","@SETUP_APPLICABLE_ON_HR","@SETUP_APPLICABLE_ON_RB",
                "@APPLICABLE_ON_HR","@APPLICABLE_ON_RB","@APPLICABLE_ON_HR_CENTRAL","@COSTCENTER_KEY", "@DIVISION_KEY", "@WAREHOUSE_KEY", "@FINANCIALYEAR_STARTDATE", "@FINANCIALYEAR_ENDDATE", "@IMPLEMENTATION_STARTDATE", "@IMPLEMENTATION_ENDDATE",
                "@APPRAISAL_DIS_START_DATE", "@APPRAISAL_DIS_END_DATE", "@MIN_AMOUNT", "@MAX_AMOUNT", "@MIN_PERCENTAGE", "@MAX_PERCENTAGE", "@ELIGIBILITY_VALUE","@COMPANY_KEY","@RECTYPE"};

                        string[] pvalue1 = { employee.SetupKey.ToString(), employee.DesignationID.ToString(), employee.ApplicableComponent.ToString(), "0", employee.Refdate.ToString(),
                employee.SetupApplicableOn_IR.ToString(), employee.SetupApplicableOn_HR.ToString(), employee.SetupApplicableOn_RB.ToString(),employee.ApplicableTo_HR.ToString(), employee.ApplicableTo_RB.ToString(),employee.ApplicableTo_HR_CENTRAL.ToString(),employee.Costcenterkey.ToString(), employee.DivisionKey.ToString(), employee.WarehouseKey.ToString(),
                employee.FYStartDate.ToString(), employee.FYEndDate.ToString(), employee.ImplementationStartDate.ToString(), employee.ImplementationEndDate.ToString(),
                employee.AppraisalDISStartDate.ToString(), employee.AppraisalDISEndDate.ToString(), employee.Minamount?.ToString(), employee.Maxamount?.ToString(),
                employee.MinPercentage?.ToString(), employee.MaxPercentage?.ToString(), "0",employee.Companykey.ToString(),"INSERT_AFTERDELETE"};


                      int  R2 = _dbAccess.int_Process("SP_CRUD_INCREMENT_SETUP", pname1, pvalue1);
                      
                    }
                }
                else
                {


                    // Loop through each model in the list
                    foreach (IncrementSetupAPIModel employee in model)
                    {
                        employee.SetupKey = R;

                        // Define parameter names and values
                        string[] pname = { "@INCREMENT_SETUP_KEY", "@DESIGNATION_ID", "@MAST_INCREMENT_COMPONENT_KEY", "@MAST_INCREMENT_ELIGIBILITY_KEY", "@REF_DATE", "@SETUP_APPLICABLE_ON_IR","@SETUP_APPLICABLE_ON_HR","@SETUP_APPLICABLE_ON_RB",
                "@APPLICABLE_ON_HR","@APPLICABLE_ON_RB","@APPLICABLE_ON_HR_CENTRAL","@COSTCENTER_KEY", "@DIVISION_KEY", "@WAREHOUSE_KEY", "@FINANCIALYEAR_STARTDATE", "@FINANCIALYEAR_ENDDATE", "@IMPLEMENTATION_STARTDATE", "@IMPLEMENTATION_ENDDATE",
                "@APPRAISAL_DIS_START_DATE", "@APPRAISAL_DIS_END_DATE", "@MIN_AMOUNT", "@MAX_AMOUNT", "@MIN_PERCENTAGE", "@MAX_PERCENTAGE", "@ELIGIBILITY_VALUE","@COMPANY_KEY","@RECTYPE"};

                        string[] pvalue = { employee.SetupKey.ToString(), employee.DesignationID.ToString(), employee.ApplicableComponent.ToString(), "0", employee.Refdate.ToString(),
                employee.SetupApplicableOn_IR.ToString(), employee.SetupApplicableOn_HR.ToString(), employee.SetupApplicableOn_RB.ToString(),employee.ApplicableTo_HR.ToString(), employee.ApplicableTo_RB.ToString(),employee.ApplicableTo_HR_CENTRAL.ToString(),employee.Costcenterkey.ToString(), employee.DivisionKey.ToString(), employee.WarehouseKey.ToString(),
                employee.FYStartDate.ToString(), employee.FYEndDate.ToString(), employee.ImplementationStartDate.ToString(), employee.ImplementationEndDate.ToString(),
                employee.AppraisalDISStartDate.ToString(), employee.AppraisalDISEndDate.ToString(), employee.Minamount?.ToString(), employee.Maxamount?.ToString(),
                employee.MinPercentage?.ToString(), employee.MaxPercentage?.ToString(), "0",employee.Companykey.ToString(),"INSERT"};

                        // Call the stored procedure to save the data
                        R = _dbAccess.int_Process("SP_CRUD_INCREMENT_SETUP", pname, pvalue);
                        employee.SetupKey = R;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                throw;
            }

            // Return 1 to indicate success
            return 1;

        }
        public List<IncrementSetupAPIModel> getallincrementsetup()
        {
            try
            {
                DataSet dataSet = GetAll();
                List<IncrementSetupAPIModel> lst = new List<IncrementSetupAPIModel>();
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var info = new IncrementSetupAPIModel();
                    info.refNO = row["REF_NO"].ToString();
                    info.Refdate = row["REF_DATE"].ToString();

                    info.SetupKey = Convert.ToInt32(row["INCREMENT_SETUP_KEY"]);
                    
                    info.FYStartDate =Convert.ToDateTime( row["FINANCIALYEAR_STARTDATE"]).ToString("dd/MM/yyyy");
                    info.FYEndDate = Convert.ToDateTime(row["FINANCIALYEAR_ENDDATE"]).ToString("dd/MM/yyyy");
                    
                    info.AppraisalDISStartDate = Convert.ToDateTime(row["APPRAISAL_DIS_START_DATE"]).ToString("dd/MM/yyyy");
                    info.AppraisalDISEndDate = Convert.ToDateTime(row["APPRAISAL_DIS_END_DATE"]).ToString("dd/MM/yyyy");
                    
                    lst.Add(info);
                }
                return lst;
            }
            catch
            {
                throw;
            }
        }
        public DataSet GetAll()
        {
            try
            {
                string[] parameterNames = { "@COMPANY_KEY", "@COSTCENTERID", "@DIVISIONID", "@WAREHOUSEID", "@RECTYPE", "@INCREMENT_SETUP_KEY" };
                string[] parameterValues = {"0","0","0","0", "FETCHALL", "0" };
                return _dbAccess.Ds_Process("SP_FETCH_INCREMENT_SETUP", parameterNames, parameterValues);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataSet GetBYid(int INCREMENT_SETUP_KEY)
        {
            try
            {
                string[] parameterNames = { "@COMPANY_KEY", "@COSTCENTERID", "@DIVISIONID", "@WAREHOUSEID", "@RECTYPE", "@INCREMENT_SETUP_KEY" };
                string[] parameterValues = { "0", "0", "0", "0", "FETCH_BY_ID", INCREMENT_SETUP_KEY.ToString() };
                return _dbAccess.Ds_Process("SP_FETCH_INCREMENT_SETUP", parameterNames, parameterValues);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<IncrementSetupAPIModel> getallincrementsetupByid(int INCREMENT_SETUP_KEY)
        {
            try
            {
                DataSet dataSet = GetBYid(INCREMENT_SETUP_KEY);

                List<IncrementSetupAPIModel> lst = new List<IncrementSetupAPIModel>();
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var info = new IncrementSetupAPIModel();
                    info.refNO = row["REF_NO"].ToString();
                    info.SetupKey = Convert.ToInt32(row["INCREMENT_SETUP_KEY"]);
                    info.Refdate = row["REF_DATE"].ToString();
                    info.ApplicableComponent = row["MAST_INCREMENT_COMPONENT_KEY"].ToString();
                    info.SetupApplicableOn_HR = Convert.ToInt32(row["SETUP_APPLICABLE_ON_HR"]);
                    info.SetupApplicableOn_IR = Convert.ToInt32(row["SETUP_APPLICABLE_ON_IR"]);
                    info.SetupApplicableOn_RB = Convert.ToInt32(row["SETUP_APPLICABLE_ON_RB"]);
                    info.Companykey = Convert.ToInt32(row["COMPANY_KEY"]);
                    info.Costcenterkey = Convert.ToInt32(row["COSTCENTER_KEY"]);
                    info.DivisionKey = Convert.ToInt32(row["DIVISION_KEY"]);
                    info.WarehouseKey = Convert.ToInt32(row["WAREHOUSE_KEY"]);
                    info.DivisionKey = Convert.ToInt32(row["DIVISION_KEY"]);
                    info.COMPANY = row["COMPANY_NAME"].ToString();
                    info.COSTCENTER = row["COSTCENTERNAME"].ToString();
                    info.DIVISION = row["Division"].ToString();
                    info.WAREHOUSE = row["warehouse"].ToString();
                    info.Minamount= Convert.ToDecimal(row["MIN_AMOUNT"]);
                    info.Maxamount = Convert.ToDecimal(row["MAX_AMOUNT"]);
                    info.MinPercentage = Convert.ToDecimal(row["MIN_PERCENTAGE"]);
                    info.MaxPercentage = Convert.ToDecimal(row["MAX_PERCENTAGE"]);
                    info.DesignationID = Convert.ToInt32(row["DesignationId"]);
                    info.Designationname= row["DesignationName"].ToString();
                    info.FYStartDate = row["FINANCIALYEAR_STARTDATE"].ToString();
                    info.FYEndDate = row["FINANCIALYEAR_ENDDATE"].ToString();
                    info.ApplicableTo_HR = Convert.ToInt32(row["APPLICABLE_ON_HR"]);
                    info.ApplicableTo_RB = Convert.ToInt32(row["APPLICABLE_ON_RB"]);
                    info.ApplicableTo_HR_CENTRAL = Convert.ToInt32(row["APPLICABLE_ON_HR_CENTRAL"]);

                    info.AppraisalDISStartDate = row["APPRAISAL_DIS_START_DATE"].ToString();
                    info.AppraisalDISEndDate = row["APPRAISAL_DIS_END_DATE"].ToString();
                    info.ImplementationStartDate = row["IMPLEMENTATION_STARTDATE"].ToString();
                    info.ImplementationEndDate = row["IMPLEMENTATION_ENDDATE"].ToString();
                    lst.Add(info);
                }
                return lst;
            }
            catch
            {
                throw;

            }
           
        }
        public int Updatesetup(int id)
        {
            try
            {
                string[] parameterNames = { "@INCREMENT_SETUP_KEY" , "@rectype" };
                string[] parameterValues = { id.ToString() , "DElete" };
                return _dbAccess.int_Process("SP_DELETE_INCREMENT_SETUP", parameterNames, parameterValues);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int Deletesetup(int id)
        {
            try
            {
                string[] parameterNames = { "@INCREMENT_SETUP_KEY", "@rectype" };
                string[] parameterValues = { id.ToString(), "update_ac_flag_0" };
                return _dbAccess.int_Process("SP_DELETE_INCREMENT_SETUP", parameterNames, parameterValues);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
