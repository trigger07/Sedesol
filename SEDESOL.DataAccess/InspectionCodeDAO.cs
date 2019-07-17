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
    public class InspectionCodeDAO
    {
        public List<InspectionCodeDTO> GetCodesByUserId(int userId)
        {
            List<InspectionCodeDTO> list = new List<InspectionCodeDTO>();
            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    var query = from c in db.INSPECTION_CODE
                                where c.Id_User == userId
                                select new InspectionCodeDTO
                                {
                                    Id = c.Id,
                                    Id_Month = c.Id_Month,
                                    Id_Year = c.Id_Year,
                                    Id_User = c.Id_User,
                                    InspectionCode = c.InspectionCode,
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
                    return query.ToList<InspectionCodeDTO>();
                }
            }
            catch (Exception ex)
            {
                return new List<InspectionCodeDTO>();
            }
        }

        public InspectionCodeDTO Save(InspectionCodeDTO dto)
        {
            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    INSPECTION_CODE insCode = db.INSPECTION_CODE.FirstOrDefault(v => v.InspectionCode == dto.InspectionCode && v.Id_User == dto.Id_User);
                    if (insCode != null)
                    {
                        dto.Message = "Se ha ingresado previamente el código generado.";
                    }
                    else
                    {
                        INSPECTION_CODE insCode2 = db.INSPECTION_CODE.FirstOrDefault(v => v.Id_Year == dto.Id_Year && v.Id_Month == dto.Id_Month && v.Id_User == dto.Id_User);
                        if (insCode2 != null)
                        {
                            dto.Message = "Se ha generado previamente un código para el año y mes seleccionados.";
                        }
                        else
                        {
                            GEN_CODE_DAY genCode = db.GEN_CODE_DAY.FirstOrDefault(x => x.Id_Month == dto.Id_Month && x.Id_Year == dto.Id_Year);
                            if (genCode == null)
                            {
                                dto.Message = "No se ha habilitado la generación de códigos para el mes y año seleccionados.";
                            }
                            else
                            {
                                DateTime dateToValidate = new DateTime(Convert.ToInt32(genCode.YEAR.Description), genCode.Id_Month, genCode.Day);

                                if (DateTime.Today < dateToValidate)
                                {
                                    dto.Message = "No se ha habilitado la generación de códigos para el mes y año seleccionados.";
                                }
                                else
                                {
                                    INSPECTION_CODE insEntity = new INSPECTION_CODE
                                    {
                                        Id_Month = dto.Id_Month,
                                        Id_Year = dto.Id_Year,
                                        Id_User = dto.Id_User,
                                        InspectionCode = dto.InspectionCode
                                    };

                                    db.INSPECTION_CODE.Add(insEntity);
                                    if (db.SaveChanges() > 0)
                                    {
                                        dto.Id = insEntity.Id;
                                        dto.Message = "SUCCESS";
                                    }
                                    else
                                    {
                                        dto.Message = "Se ha producido un error al guardar el código de inspección";
                                    }
                                }
                            }
                        }
                    }
                    return dto;
                }
            }
            catch (Exception ex)
            {
                return new InspectionCodeDTO();
            }

        }

        public string GenerateCode(int pUserId)
        {
            string code = string.Empty;
            bool ok = false;

            while (!ok)
            {
                int longitud = 10;
                Guid miGuid = Guid.NewGuid();
                code = Convert.ToBase64String(miGuid.ToByteArray());
                code = code.Replace("=", "").Replace("+", "");
                code = code.Substring(0, longitud);

                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    INSPECTION_CODE insCode = db.INSPECTION_CODE.FirstOrDefault(v => v.InspectionCode == code && v.Id_User == pUserId);
                    if (insCode != null)
                    {
                        ok = false;
                    }
                    else
                    {
                        ok = true;
                        break;
                    }
                }
            }
            

            return code;
        }
    }
}
