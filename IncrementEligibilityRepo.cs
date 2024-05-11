
using Microsoft.AspNetCore.Mvc.Rendering;
using StaffType.Api.DAL;
using StaffType.Api.Interface;
using StaffType.Api.Models;
using System.Data;
using TenantCompany.Models;

namespace StaffType.Api.Repositoris
{
    public class IncrementEligibilityRepo : IIncrementEligibility
    {


        private readonly IConfiguration _configuration;
        private readonly DbAccess _dbAccess;
        public IncrementEligibilityRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbAccess = new DbAccess(_configuration);
        }


        public IEnumerable<IncrementEligibilityAPIModel> GetIncrementEligibility()
        {


            try
            {
                string[] ParametersNames = { "@MAST_INCREMENT_ELIGIBILITY_KEY" };
                string[] ParametersValues = { "0" };

                DataSet dataSet = _dbAccess.Ds_Process("SP_GET_MAST_INCREMENT_ELIGIBILITY", ParametersNames, ParametersValues);
                List<IncrementEligibilityAPIModel> lst = new List<IncrementEligibilityAPIModel>();
                if (dataSet.Tables.Count > 0)
                {

                    DataTable dt = dataSet.Tables[0];

                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            var obj = new IncrementEligibilityAPIModel();
                            obj.MAST_INCREMENT_ELIGIBILITY_KEY = Convert.ToInt32(row["MAST_INCREMENT_ELIGIBILITY_KEY"]);
                            obj.ELIGIBILITY_NAME = Convert.ToString(row["ELIGIBILITY_NAME"]);
                            obj.ApplicationDataTable_Master_KEY = Convert.ToInt32(row["ApplicationDataTable_Master_KEY"]);
                            obj.ApplicationDataTable_Dtls_KEY = Convert.ToInt32(row["ApplicationDataTable_Dtls_KEY"]);
                            obj.Table_Name = Convert.ToString(row["Table_Name"]);
                            obj.ColumnName = Convert.ToString(row["ColumnName"]);

                            lst.Add(obj);

                        }
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public IEnumerable<IncrementEligibilityAPIModel> GetIncrementEligibility(int id)
        {


            try
            {
                string[] ParametersNames = { "@MAST_INCREMENT_ELIGIBILITY_KEY" };
                string[] ParametersValues = { id.ToString() };

                DataSet dataSet = _dbAccess.Ds_Process("SP_GET_MAST_INCREMENT_ELIGIBILITY", ParametersNames, ParametersValues);
                List<IncrementEligibilityAPIModel> lst = new List<IncrementEligibilityAPIModel>();
                if (dataSet.Tables.Count > 0)
                {

                    DataTable dt = dataSet.Tables[0];

                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            var obj = new IncrementEligibilityAPIModel();
                            obj.MAST_INCREMENT_ELIGIBILITY_KEY = Convert.ToInt32(row["MAST_INCREMENT_ELIGIBILITY_KEY"]);
                            obj.ELIGIBILITY_NAME = Convert.ToString(row["ELIGIBILITY_NAME"]);
                            obj.ApplicationDataTable_Master_KEY = Convert.ToInt32(row["ApplicationDataTable_Master_KEY"]);
                            obj.ApplicationDataTable_Dtls_KEY = Convert.ToInt32(row["ApplicationDataTable_Dtls_KEY"]);
                            obj.Table_Name = Convert.ToString(row["Table_Name"]);
                            obj.ColumnName = Convert.ToString(row["ColumnName"]);
                           // obj.TableId = Convert.ToInt32(row["TableId"]);

                            lst.Add(obj);

                        }
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }


        public List<SelectListItem> GetTableForm()
        {
            try
            {
                string[] pname = { "@TableId", "@Table_Name" };
                string[] pvalue = { "0","" };
                DataSet DS = _dbAccess.Ds_Process("SP_GET_ApplicationDataTable_Master", pname, pvalue);
                List<SelectListItem> types = new List<SelectListItem>();

                types.Add(new SelectListItem { Text = "--Select--", Value = "" });
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    types.Add(new SelectListItem { Text = dr["Table_Name"].ToString(), Value = dr["TableId"].ToString() });
                }
                return types;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string GetFieldForm(int ApplicationDataTable_Master_KEY)
        {
            //string ii = "";

            string dropdown = "<select>";
            dropdown += $"<option value='{0}'>{"--Select--"}</option>";
            try
            {
                string[] pname = { "@TableId", "@ColumnId" };
                string[] pvalue = { ApplicationDataTable_Master_KEY.ToString(), "0" };
                DataSet ds = _dbAccess.Ds_Process("SP_GET_ApplicationDataTable_Dtls", pname, pvalue);
                List<object> types = new List<object>();

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    string ColumnName = item["ColumnName"].ToString();
                    int ApplicationDataTable_Dtls_KEY = Convert.ToInt32(item["ColumnId"]);

                    //dropdown += "<option value=''>costCenterName</option>";
                    dropdown += $"<option value='{ApplicationDataTable_Dtls_KEY}'>{ColumnName}</option>";
                }

                dropdown += "</select>";
                return dropdown;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public int SaveIncrementEligibility(IncrementEligibilityAPIModel model,string REC_TYPE)
        {
            try
            {

                string[] pname = { "@REC_TYPE", "@ELIGIBILITY_NAME", "@ApplicationDataTable_Master_KEY", "@ApplicationDataTable_Dtls_KEY", "@MAST_INCREMENT_ELIGIBILITY_KEY" };
                string[] pvalue = { REC_TYPE, model.ELIGIBILITY_NAME, model.ApplicationDataTable_Master_KEY.ToString(), model.ApplicationDataTable_Dtls_KEY.ToString(), model.MAST_INCREMENT_ELIGIBILITY_KEY.ToString() };
                return _dbAccess.int_Process("SP_CRUD_MAST_INCREMENT_ELIGIBILITY", pname, pvalue);

            }
            catch (Exception ex)
            
            {
                throw;
            }

        }

        public int DeleteIncrementEligibility(IncrementEligibilityAPIModel model,string REC_TYPE)
        {

            try
            {
                string[] pname = { "@REC_TYPE", "@ELIGIBILITY_NAME", "@ApplicationDataTable_Master_KEY", "@ApplicationDataTable_Dtls_KEY", "@MAST_INCREMENT_ELIGIBILITY_KEY" };
                string[] pvalue = { REC_TYPE,"0","0","0",model.MAST_INCREMENT_ELIGIBILITY_KEY.ToString() };

                return _dbAccess.int_Process("SP_CRUD_MAST_INCREMENT_ELIGIBILITY", pname, pvalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }





    }
}



