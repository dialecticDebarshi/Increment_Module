using Microsoft.AspNetCore.Mvc.Rendering;
using StaffType.Api.DAL;
using StaffType.Api.Interface;
using StaffType.Api.Models;
using System.Data;
using System.Security.Cryptography;
namespace StaffType.Api.Repositoris
{
    public class MastIncrementComponentUIRepo : IMastIncrementComponent
    {

        private readonly IConfiguration _configuration;
        private readonly DbAccess _dbAccess;

        public MastIncrementComponentUIRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbAccess = new DbAccess(_configuration);

        }


        public IEnumerable<MastIncrementComponentUIModel> GetIncrementComponent()

        {
            try
            {
                string[] parameterNames = { "@MAST_INCREMENT_COMPONENT_KEY" };
                string[] parameterValues = { "0" };
                DataSet dataSet = _dbAccess.Ds_Process("SP_GET_MAST_INCREMENT_COMPONENT", parameterNames, parameterValues);
                List<MastIncrementComponentUIModel> lst = new List<MastIncrementComponentUIModel>();

                if (dataSet.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        var mdl = new MastIncrementComponentUIModel();
                        mdl.MAST_INCREMENT_COMPONENT_KEY = Convert.ToInt32(row["MAST_INCREMENT_COMPONENT_KEY"]);
                        mdl.SERIAL_NO = Convert.ToInt32(row["SERIAL_NO"]);
                
                        mdl.COMPONENT_NAME = Convert.ToString(row["COMPONENT_NAME"]);

                        lst.Add(mdl);
                    }
                }
                return lst;

            }

            catch
            {
                throw;
            }
        }

        public IEnumerable<MastIncrementComponentUIModel> GetIncrementComponent(int id)

        {
            try
            {
                string[] parameterNames = { "@MAST_INCREMENT_COMPONENT_KEY" };
                string[] parameterValues = { id.ToString() };
                DataSet dataSet = _dbAccess.Ds_Process("SP_GET_MAST_INCREMENT_COMPONENT", parameterNames, parameterValues);
                List<MastIncrementComponentUIModel> lst = new List<MastIncrementComponentUIModel>();

                if (dataSet.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        var mdl = new MastIncrementComponentUIModel();
                        mdl.MAST_INCREMENT_COMPONENT_KEY = Convert.ToInt32(row["MAST_INCREMENT_COMPONENT_KEY"]);
               
                        mdl.COMPONENT_NAME = Convert.ToString(row["COMPONENT_NAME"]);

                        lst.Add(mdl);
                    }
                }
                return lst;

            }

            catch
            {
                throw;
            }
        }

        public int SaveIncrementComponent(MastIncrementComponentUIModel model)
        {

            try
            {
                string[] pname = { "@REC_TYPE", "@MAST_INCREMENT_COMPONENT_KEY", "@COMPONENT_NAME" };


                string[] pvalue =
                    {
                   model.REC_TYPE="INSERT",
                   model.MAST_INCREMENT_COMPONENT_KEY.ToString(),               
                   model.COMPONENT_NAME.ToString()
                  

                };
                return _dbAccess.int_Process("SP_CRUD_MAST_INCREMENT_COMPONENT", pname, pvalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int DeleteIncrementComponent(MastIncrementComponentUIModel model)
        {

            try
            {
                string[] pname = { "@REC_TYPE", "@MAST_INCREMENT_COMPONENT_KEY", "@COMPONENT_NAME" };
                string[] pvalue = { "DELETE", model.MAST_INCREMENT_COMPONENT_KEY.ToString(), ""};

                return _dbAccess.int_Process("SP_CRUD_MAST_INCREMENT_COMPONENT", pname, pvalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
