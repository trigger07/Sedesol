using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;

namespace SEDESOL.BusinessLogic
{
    public class ParamDAL
    {
        public List<YearDTO> GetYears()
        {
            ParamDAO b = new ParamDAO();
            return b.GetYears();
        }

        public List<MonthDTO> GetMonths()
        {
            ParamDAO b = new ParamDAO();
            return b.GetMonths();
        }

        public List<StateDTO> GetStates()
        {
            ParamDAO b = new ParamDAO();
            return b.GetStates();
        }

        public List<StateDTO> GetStatesFilter(int userTypeId, int userId)
        {
            ParamDAO b = new ParamDAO();
            return b.GetStatesFilter(userTypeId, userId);
        }

        public List<StatusDTO> GetStatus()
        {
            ParamDAO b = new ParamDAO();
            return b.GetStatus();
        }

        public void UpdateState(StateDTO dto)
        {
            var b = new ParamDAO();
            if (dto.Id > 0)
            {
                b.UpdateState(dto, true);
            }
            else
            {
                b.UpdateState(dto, false);
            }
        }

        public void UpdateStatus(StatusDTO dto)
        {
            var b = new ParamDAO();
            if (dto.Id > 0)
            {
                b.UpdateStatus(dto, true);
            }
            else
            {
                b.UpdateStatus(dto, false);
            }
        }

        public void UpdateYear(YearDTO dto)
        {
            var b = new ParamDAO();
            if (dto.Id > 0)
            {
                b.UpdateYear(dto, true);
            }
            else
            {
                b.UpdateYear(dto, false);
            }
        }

        public void UpdateMonth(MonthDTO dto)
        {
            var b = new ParamDAO();
            if (dto.Id > 0)
            {
                b.UpdateMonth(dto, true);
            }
            else
            {
                b.UpdateMonth(dto, false);
            }
        }

        public void ActivateStatus(int id)
        {
            ParamDAO dao = new ParamDAO();
            dao.ActivateStatus(id);
        }

        public void DeactivateStatus(int id)
        {
            ParamDAO dao = new ParamDAO();
            dao.DeactivateStatus(id);
        }

        public void ActivateState(int id)
        {
            ParamDAO dao = new ParamDAO();
            dao.ActivateState(id);
        }

        public void DeactivateState(int id)
        {
            ParamDAO dao = new ParamDAO();
            dao.DeactivateState(id);
        }

        public void ActivateYear(int id)
        {
            ParamDAO dao = new ParamDAO();
            dao.ActivateYear(id);
        }

        public void DeactivateYear(int id)
        {
            ParamDAO dao = new ParamDAO();
            dao.DeactivateYear(id);
        }

        public void ActivateMonth(int id)
        {
            ParamDAO dao = new ParamDAO();
            dao.ActivateMonth(id);
        }

        public void DeactivateMonth(int id)
        {
            ParamDAO dao = new ParamDAO();
            dao.DeactivateMonth(id);
        }

        public List<StateDTO> GetActiveStates()
        {
            ParamDAO b = new ParamDAO();
            return b.GetActiveStates();
        }

        public List<ConditionDTO> GetActiveCondition()
        {
            ParamDAO b = new ParamDAO();
            return b.GetActiveCondition();
        }
    }
}
