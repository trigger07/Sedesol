using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities;
using SEDESOL.DataModel;
using SEDESOL.DataEntities.DTO;
using System.Data.Entity;
using System.Data;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace SEDESOL.DataAccess
{
    public class ReportDAO
    {
        public DataSet GetAttendanceReport(int CaptureId)
        {
            DataSet ds = new DataSet("Reporte_Asistencia");

            var efConnectionString = ConfigurationManager.ConnectionStrings["SEDESOLEntities"].ConnectionString;
            var builder = new EntityConnectionStringBuilder(efConnectionString);
            var regularConnectionString = builder.ProviderConnectionString;

            using (SqlConnection conn = new SqlConnection(regularConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand("sp_GetAttendanceReport", conn);
                sqlComm.Parameters.AddWithValue("@CaptureId", CaptureId);

                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }

            return ds;
        }

        public DataSet GetCaptureReport(List<FilterDTO> filters)
        {
            DataSet ds = new DataSet("Reporte_Capturas");

            var efConnectionString = ConfigurationManager.ConnectionStrings["SEDESOLEntities"].ConnectionString;
            var builder = new EntityConnectionStringBuilder(efConnectionString);
            var regularConnectionString = builder.ProviderConnectionString;

            using (SqlConnection conn = new SqlConnection(regularConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand("sp_GetCaptureReportFilter", conn);

                if (filters.Find(f => f.Name == "Id_UserType") != null)
                {
                    int Id_UserType = Convert.ToInt32(filters.Find(f => f.Name == "Id_UserType").Value);
                    sqlComm.Parameters.AddWithValue("@UserTypeId", Id_UserType);
                }
                if (filters.Find(f => f.Name == "Id_User") != null)
                {
                    int Id_User = Convert.ToInt32(filters.Find(f => f.Name == "Id_User").Value);
                    sqlComm.Parameters.AddWithValue("@UserId", Id_User);
                }

                if (filters.Find(f => f.Name == "Id_State") != null)
                {
                    int Id_State = Convert.ToInt32(filters.Find(f => f.Name == "Id_State").Value);
                    sqlComm.Parameters.AddWithValue("@StateId", Id_State);
                }
                if (filters.Find(f => f.Name == "Id_SoupKitchen") != null)
                {
                    int Id_SoupKitchen = Convert.ToInt32(filters.Find(f => f.Name == "Id_SoupKitchen").Value);
                    sqlComm.Parameters.AddWithValue("@SoupKitchenId", Id_SoupKitchen);
                }

                if (filters.Find(f => f.Name == "Id_Status") != null)
                {
                    int Id_Status = Convert.ToInt32(filters.Find(f => f.Name == "Id_Status").Value);
                    sqlComm.Parameters.AddWithValue("@StatusId", Id_Status);
                }

                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }

            return ds;
        }
    }
}
