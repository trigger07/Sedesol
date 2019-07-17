using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities;
using SEDESOL.DataModel;
using SEDESOL.DataEntities.DTO;

namespace SEDESOL.DataAccess
{
    public class ParamDAO
    {

        public List<YearDTO> GetYears()
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from year in entities.YEARs
                            //join month in entities.MONTHs on capture.Id_Month equals month.Id
                            //join year in entities.YEARs on capture.Id_Year equals year.Id
                            //join status in entities.STATUS on capture.Id_Status equals status.Id
                            select new YearDTO
                            {
                                Id = year.Id,
                                Description = year.Description,
                                IsActive = year.IsActive
                            };
                return query.ToList<YearDTO>();
            }
        }

        public List<MonthDTO> GetMonths()
        {
            try
            {
                using (SEDESOLEntities entities = new SEDESOLEntities())
                {
                    var query = from month in entities.MONTHs
                                //join month in entities.MONTHs on capture.Id_Month equals month.Id
                                //join year in entities.YEARs on capture.Id_Year equals year.Id
                                //join status in entities.STATUS on capture.Id_Status equals status.Id
                                select new MonthDTO
                                {
                                    Id = month.Id,
                                    Description = month.Description,
                                    IsActive = month.IsActive
                                };
                    return query.ToList<MonthDTO>();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public List<StateDTO> GetStates()
        {
            try
            {
                using (SEDESOLEntities entities = new SEDESOLEntities())
                {
                    var query = from state in entities.STATEs
                                    //join month in entities.MONTHs on capture.Id_Month equals month.Id
                                    //join year in entities.YEARs on capture.Id_Year equals year.Id
                                    //join status in entities.STATUS on capture.Id_Status equals status.Id
                                select new StateDTO
                                {
                                    Id = state.Id,
                                    Name = state.Name,
                                    Abbreviation = state.Abbreviation,
                                    IsActive = state.IsActive
                                };
                    return query.ToList<StateDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<StateDTO> GetStatesFilter(int userTypeId, int userId)
        {
            try
            {
                using (SEDESOLEntities entities = new SEDESOLEntities())
                {
                    var query = from state in entities.STATEs
                                select new StateDTO
                                {
                                    Id = state.Id,
                                    Name = state.Name,
                                    Abbreviation = state.Abbreviation,
                                    IsActive = state.IsActive
                                };
                    if (userTypeId != 2)
                    {
                        // obtener estados de comedores asignados al usuario y filtrar estados
                        var querySk = entities.USER_SOUP_KITCHEN.Where(p => p.Id_User == userId && p.IsActive == true).Distinct();
                        
                        query = query.Where(e => querySk.Any(f => f.SOUP_KITCHEN.Id_State == e.Id));
                    }
                    return query.ToList<StateDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<StatusDTO> GetStatus()
        {
            try
            {
                using (SEDESOLEntities entities = new SEDESOLEntities())
                {
                    var query = from status in entities.STATUS
                                    //join month in entities.MONTHs on capture.Id_Month equals month.Id
                                    //join year in entities.YEARs on capture.Id_Year equals year.Id
                                    //join status in entities.STATUS on capture.Id_Status equals status.Id
                                select new StatusDTO
                                {
                                    Id = status.Id,
                                    Description = status.Description,
                                    IsActive = status.IsActive
                                };
                    return query.ToList<StatusDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void UpdateState(StateDTO state, bool editar)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                if (editar)
                {
                    STATE existente = entities.STATEs.FirstOrDefault(v => v.Id == state.Id);
                    if (existente != null)
                    {
                        existente.Name = state.Name;
                        entities.SaveChanges();
                    }
                }
                else
                {
                    STATE nueva = new STATE();
                    nueva.Name = state.Name;
                    nueva.IsActive = true;
                    nueva.Abbreviation = string.Empty;
                    entities.STATEs.Add(nueva);
                    entities.SaveChanges();
                }
            }
        }

        public void UpdateStatus(StatusDTO status, bool editar)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                if (editar)
                {
                    STATUS existente = entities.STATUS.FirstOrDefault(v => v.Id == status.Id);
                    if (existente != null)
                    {
                        existente.Description = status.Description;
                        entities.SaveChanges();
                    }
                }
                else
                {
                    STATUS nueva = new STATUS();
                    nueva.Description = status.Description;
                    nueva.IsActive = true;
                    entities.STATUS.Add(nueva);
                    entities.SaveChanges();
                }
            }
        }

        public void UpdateYear(YearDTO year, bool editar)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                if (editar)
                {
                    YEAR existente = entities.YEARs.FirstOrDefault(v => v.Id == year.Id);
                    if (existente != null)
                    {
                        existente.Description = year.Description;
                        entities.SaveChanges();
                    }
                }
                else
                {
                    YEAR nueva = new YEAR();
                    nueva.Description = year.Description;
                    nueva.IsActive = true;
                    entities.YEARs.Add(nueva);
                    entities.SaveChanges();
                }
            }
        }

        public void UpdateMonth(MonthDTO month, bool editar)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                if (editar)
                {
                    MONTH existente = entities.MONTHs.FirstOrDefault(v => v.Id == month.Id);
                    if (existente != null)
                    {
                        existente.Description = month.Description;
                        entities.SaveChanges();
                    }
                }
                else
                {
                    MONTH nueva = new MONTH();
                    nueva.Description = month.Description;
                    nueva.IsActive = true;
                    entities.MONTHs.Add(nueva);
                    entities.SaveChanges();
                }
            }
        }

        public void ActivateState(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                STATE state = entities.STATEs.Find(id);
                state.IsActive = true;
                entities.SaveChanges();
            }
        }

        public void DeactivateState(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                STATE state = entities.STATEs.Find(id);
                state.IsActive = false;
                entities.SaveChanges();
            }
        }

        public void ActivateStatus(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                STATUS status = entities.STATUS.Find(id);
                status.IsActive = true;
                entities.SaveChanges();
            }
        }

        public void DeactivateStatus(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                STATUS status = entities.STATUS.Find(id);
                status.IsActive = false;
                entities.SaveChanges();
            }
        }

        public void ActivateYear(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                YEAR year = entities.YEARs.Find(id);
                year.IsActive = true;
                entities.SaveChanges();
            }
        }

        public void DeactivateYear(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                YEAR year = entities.YEARs.Find(id);
                year.IsActive = false;
                entities.SaveChanges();
            }
        }

        public void ActivateMonth(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                MONTH month = entities.MONTHs.Find(id);
                month.IsActive = true;
                entities.SaveChanges();
            }
        }

        public void DeactivateMonth(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                MONTH month = entities.MONTHs.Find(id);
                month.IsActive = false;
                entities.SaveChanges();
            }
        }

        public List<ConditionDTO> GetActiveCondition()
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from con in entities.CONDITIONs
                            where con.IsActive == true
                            select new ConditionDTO
                            {
                                Id = con.Id,
                                Name = con.Name,
                                Description = con.Description,
                                IsActive = con.IsActive
                            };
                return query.ToList<ConditionDTO>();
            }
        }

        public List<StateDTO> GetActiveStates()
        {
            try
            {
                using (SEDESOLEntities entities = new SEDESOLEntities())
                {
                    var query = from state in entities.STATEs
                                where state.IsActive == true
                                select new StateDTO
                                {
                                    Id = state.Id,
                                    Name = state.Name,
                                    Abbreviation = state.Abbreviation,
                                    IsActive = state.IsActive
                                };
                    return query.ToList<StateDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
