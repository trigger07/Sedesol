using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities;
using SEDESOL.DataModel;
using SEDESOL.DataEntities.DTO;
using System.Data.Entity;

namespace SEDESOL.DataAccess
{
    public class GencodeDayDAO
    {
        public List<GencodeDayDTO> GetGenerationCodeDayAll()
        {
            List<GencodeDayDTO> list = new List<GencodeDayDTO>();
            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    var query = from c in db.GEN_CODE_DAY
                                //where c.Id_User == userId
                                select new GencodeDayDTO
                                {
                                    Id = c.Id,
                                    Id_Month = c.Id_Month,
                                    Id_Year = c.Id_Year,
                                    Day = c.Day,
                                    MonthDto = new MonthDTO
                                    {
                                        Id = c.MONTH.Id,
                                        Description = c.MONTH.Description
                                    },
                                    YearDto = new YearDTO
                                    {
                                        Id = c.YEAR.Id,
                                        Description = c.YEAR.Description
                                    }

                                };
                    return query.ToList<GencodeDayDTO>();
                }
            }
            catch (Exception ex)
            {
                return new List<GencodeDayDTO>();
            }
        }

        public GencodeDayDTO Save(GencodeDayDTO dto)
        {
            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    GEN_CODE_DAY day = db.GEN_CODE_DAY.FirstOrDefault(v => v.Id == dto.Id);
                    if (day != null)
                    {
                        GEN_CODE_DAY day2 = db.GEN_CODE_DAY.FirstOrDefault(v => v.Id_Year == dto.Id_Year && v.Id_Month == dto.Id_Month && v.Id != dto.Id);
                        if (day2 != null)
                        {
                            dto.Message = "Se ha ingresado previamente una fecha para el mes y año seleccionados.";
                        }
                        else
                        {
                            day.Id_Month = dto.Id_Month;
                            day.Id_Year = dto.Id_Year;
                            day.Day = dto.Day;
                            if(db.SaveChanges() > 0)
                            {
                                dto.Id = day.Id;
                                dto.Message = "SUCCESS";
                            }
                        }
                    }
                    else
                    {
                        GEN_CODE_DAY day2 = db.GEN_CODE_DAY.FirstOrDefault(v => v.Id_Year == dto.Id_Year && v.Id_Month == dto.Id_Month);
                        if (day2 != null)
                        {
                            dto.Message = "Se ha ingresado previamente una fecha para el mes y año seleccionados.";
                        }
                        else
                        {
                            day = new GEN_CODE_DAY();
                            day.Id_Month = dto.Id_Month;
                            day.Id_Year = dto.Id_Year;
                            day.Day = dto.Day;
                            db.GEN_CODE_DAY.Add(day);

                            if (db.SaveChanges() > 0)
                            {
                                dto.Id = day.Id;
                                dto.Message = "SUCCESS";
                            }
                        }
                    }
                    return dto;
                }
            }
            catch (Exception ex)
            {
                return new GencodeDayDTO();
            }

        }

        public GencodeDayDTO GetGenerationCodeDayById(int Id)
        {
            List<GencodeDayDTO> list = new List<GencodeDayDTO>();
            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    var query = from c in db.GEN_CODE_DAY
                                    where c.Id == Id
                                select new GencodeDayDTO
                                {
                                    Id = c.Id,
                                    Id_Month = c.Id_Month,
                                    Id_Year = c.Id_Year,
                                    Day = c.Day,
                                    MonthDto = new MonthDTO
                                    {
                                        Id = c.MONTH.Id,
                                        Description = c.MONTH.Description
                                    },
                                    YearDto = new YearDTO
                                    {
                                        Id = c.YEAR.Id,
                                        Description = c.YEAR.Description
                                    }

                                };
                    return query.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return new GencodeDayDTO();
            }
        }
    }
}
