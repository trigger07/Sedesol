using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;
using System.Data;
using SEDESOL.DataEntities.IntegrationObjects;

namespace SEDESOL.BusinessLogic
{
    public class RationDAL
    {
        public List<RationDTO> GetRationSearch(int pYear, int pMonth)
        {
            RationDAO b = new RationDAO();
            return b.GetSearch(pYear, pMonth);
        }

        public string SaveRations(DataTable dt)
        {
            RationDAO dao = new RationDAO();
            return dao.SaveDataRation(dt);
        }

        public CouponModel GetCouponData(int pIdRation)
        {
            RationDAO dao = new RationDAO();
            return dao.GetCouponData(pIdRation);
        }

        public List<CouponModel> GetCouponDataByState(int pIdState, int pIdYear, int pIdMonth)
        {
            RationDAO dao = new RationDAO();
            List<RationDTO> listRation = dao.GetRationsByState(pIdState, pIdYear, pIdMonth);
            List<CouponModel> listCoupon = new List<CouponModel>();

            foreach (var item in listRation)
            {
                listCoupon.Add(GetCouponData(item.Id));
            }

            return listCoupon;
        }

        public string GetExcel(int pIdState, int pIdMonth, int pIdYear)
        {
            RationDAO reporting = new RationDAO();
            //return reporting.GetAttendanceReport(CaptureId);
            return DTOSerializer.Serialize(reporting.GetExcel(pIdState, pIdMonth, pIdYear)).InnerXml;

        }
    }
}
