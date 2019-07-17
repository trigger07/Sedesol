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
    public class CaptureDAO
    {
        public List<CaptureDTO> GetCaptures()
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                try
                {
                    var query = from capture in entities.CAPTUREs
                                    //join month in entities.MONTHs on capture.Id_Month equals month.Id
                                    //join year in entities.YEARs on capture.Id_Year equals year.Id
                                    //join status in entities.STATUS on capture.Id_Status equals status.Id
                                select new CaptureDTO
                                {
                                    Id = capture.Id,
                                    Description = capture.Description,
                                    CreateDate = capture.CreateDate,
                                    IsActive = capture.IsActive,
                                    ReceiverCount = capture.ATTENDANCEs.Count(),
                                    AttendanceCount = capture.ATTENDANCEs.Sum(x => x.Quantity) ?? 0,
                                    Folio = capture.Folio,
                                    InspectionCode = capture.InspectionCode,
                                    Id_LevelApproval = capture.Id_LevelApproval,
                                    Id_User = capture.Id_User,
                                    Month = new MonthDTO
                                    {
                                        Id = capture.MONTH.Id,
                                        Description = capture.MONTH.Description,
                                        IsActive = capture.MONTH.IsActive
                                    },
                                    Year = new YearDTO
                                    {
                                        Id = capture.YEAR.Id,
                                        Description = capture.YEAR.Description,
                                        IsActive = capture.YEAR.IsActive
                                    },
                                    Status = new StatusDTO
                                    {
                                        Id = capture.STATUS.Id,
                                        Description = capture.STATUS.Description,
                                        IsActive = capture.STATUS.IsActive
                                    },
                                    SoupKitchen = new SoupKitchenDTO
                                    {
                                        Id = capture.SOUP_KITCHEN.Id,
                                        Name = capture.SOUP_KITCHEN.Name,
                                        Description = capture.SOUP_KITCHEN.Description,
                                        Capacity = capture.SOUP_KITCHEN.Capacity,
                                        Address = capture.SOUP_KITCHEN.Address,
                                        PhoneNumber = capture.SOUP_KITCHEN.PhoneNumber,
                                        ContactName = capture.SOUP_KITCHEN.ContactName,
                                        IsActive = capture.SOUP_KITCHEN.IsActive,
                                        AllowAnonym = capture.SOUP_KITCHEN.AllowAnonym,
                                        Folio = capture.SOUP_KITCHEN.Folio,
                                        State = new StateDTO
                                        {
                                            Id = capture.SOUP_KITCHEN.STATE.Id,
                                            Name = capture.SOUP_KITCHEN.STATE.Name
                                        }
                                    },
                                    UserTypeDto = new UserTypeDTO
                                    {
                                        Id = capture.USER_TYPE.Id,
                                        Description = capture.USER_TYPE.Description
                                    }
                                };
                    //if (StateId > 0)
                    //{
                    //    query = query.Where(e => e. == StateId);
                    //}
                    //if (SoupKitchenId > 0)
                    //{
                    //    query = query.Where(e => e.SoupKitchen.Id == SoupKitchenId);
                    //}
                    return query.ToList<CaptureDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public List<CaptureDTO> GetCaptures(int userTypeId, int userId)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                try
                {
                    var query = from capture in entities.CAPTUREs
                                select new CaptureDTO
                                {
                                    Id = capture.Id,
                                    Description = capture.Description,
                                    CreateDate = capture.CreateDate,
                                    IsActive = capture.IsActive,
                                    ReceiverCount = capture.ATTENDANCEs.Count(),
                                    AttendanceCount = capture.ATTENDANCEs.Sum(x => x.Quantity) ?? 0,
                                    Folio = capture.Folio,
                                    InspectionCode = capture.InspectionCode,
                                    Id_LevelApproval = capture.Id_LevelApproval,
                                    Id_User = capture.Id_User,
                                    Month = new MonthDTO
                                    {
                                        Id = capture.MONTH.Id,
                                        Description = capture.MONTH.Description,
                                        IsActive = capture.MONTH.IsActive
                                    },
                                    Year = new YearDTO
                                    {
                                        Id = capture.YEAR.Id,
                                        Description = capture.YEAR.Description,
                                        IsActive = capture.YEAR.IsActive
                                    },
                                    Status = new StatusDTO
                                    {
                                        Id = capture.STATUS.Id,
                                        Description = capture.STATUS.Description,
                                        IsActive = capture.STATUS.IsActive
                                    },
                                    SoupKitchen = new SoupKitchenDTO
                                    {
                                        Id = capture.SOUP_KITCHEN.Id,
                                        Name = capture.SOUP_KITCHEN.Name,
                                        Description = capture.SOUP_KITCHEN.Description,
                                        Capacity = capture.SOUP_KITCHEN.Capacity,
                                        Address = capture.SOUP_KITCHEN.Address,
                                        PhoneNumber = capture.SOUP_KITCHEN.PhoneNumber,
                                        ContactName = capture.SOUP_KITCHEN.ContactName,
                                        IsActive = capture.SOUP_KITCHEN.IsActive,
                                        AllowAnonym = capture.SOUP_KITCHEN.AllowAnonym,
                                        Folio = capture.SOUP_KITCHEN.Folio,
                                        State = new StateDTO
                                        {
                                            Id = capture.SOUP_KITCHEN.STATE.Id,
                                            Name = capture.SOUP_KITCHEN.STATE.Name
                                        }
                                    },
                                    UserTypeDto = new UserTypeDTO
                                    {
                                        Id = capture.USER_TYPE.Id,
                                        Description = capture.USER_TYPE.Description
                                    }
                                };
                    if (userTypeId == 1)
                    {
                        query = query.Where(e => e.Id_User == userId);
                    }
                    else
                    {
                        if(userTypeId != 2)
                        {
                            // obtener comedores asignados al usuario y filtrar capturas de ese comedor
                            var querySk = entities.USER_SOUP_KITCHEN.Where(p => p.Id_User == userId && p.IsActive == true);

                            query = query.Where(e => querySk.Any(f => f.Id_Soup_Kitchen == e.SoupKitchen.Id));
                        }
                    }
                    return query.ToList<CaptureDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public List<CaptureDTO> GetCapturesSearch(int userTypeId, int userId, int pState, int pSoupK, int pStatus)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                try
                {
                    var query = from capture in entities.CAPTUREs
                                select new CaptureDTO
                                {
                                    Id = capture.Id,
                                    Description = capture.Description,
                                    CreateDate = capture.CreateDate,
                                    IsActive = capture.IsActive,
                                    ReceiverCount = capture.ATTENDANCEs.Count(),
                                    AttendanceCount = capture.ATTENDANCEs.Sum(x => x.Quantity) ?? 0,
                                    Folio = capture.Folio,
                                    InspectionCode = capture.InspectionCode,
                                    Id_LevelApproval = capture.Id_LevelApproval,
                                    Id_User = capture.Id_User,
                                    Month = new MonthDTO
                                    {
                                        Id = capture.MONTH.Id,
                                        Description = capture.MONTH.Description,
                                        IsActive = capture.MONTH.IsActive
                                    },
                                    Year = new YearDTO
                                    {
                                        Id = capture.YEAR.Id,
                                        Description = capture.YEAR.Description,
                                        IsActive = capture.YEAR.IsActive
                                    },
                                    Status = new StatusDTO
                                    {
                                        Id = capture.STATUS.Id,
                                        Description = capture.STATUS.Description,
                                        IsActive = capture.STATUS.IsActive
                                    },
                                    SoupKitchen = new SoupKitchenDTO
                                    {
                                        Id = capture.SOUP_KITCHEN.Id,
                                        Name = capture.SOUP_KITCHEN.Name,
                                        Description = capture.SOUP_KITCHEN.Description,
                                        Capacity = capture.SOUP_KITCHEN.Capacity,
                                        Address = capture.SOUP_KITCHEN.Address,
                                        PhoneNumber = capture.SOUP_KITCHEN.PhoneNumber,
                                        ContactName = capture.SOUP_KITCHEN.ContactName,
                                        IsActive = capture.SOUP_KITCHEN.IsActive,
                                        AllowAnonym = capture.SOUP_KITCHEN.AllowAnonym,
                                        Folio = capture.SOUP_KITCHEN.Folio,
                                        State = new StateDTO
                                        {
                                            Id = capture.SOUP_KITCHEN.STATE.Id,
                                            Name = capture.SOUP_KITCHEN.STATE.Name
                                        }
                                    },
                                    UserTypeDto = new UserTypeDTO
                                    {
                                        Id = capture.USER_TYPE.Id,
                                        Description = capture.USER_TYPE.Description
                                    }
                                };
                    if (userTypeId == 1)
                    {
                        query = query.Where(e => e.Id_User == userId);
                    }
                    else
                    {
                        if (userTypeId != 2)
                        {
                            // obtener comedores asignados al usuario y filtrar capturas de ese comedor
                            var querySk = entities.USER_SOUP_KITCHEN.Where(p => p.Id_User == userId && p.IsActive == true);

                            query = query.Where(e => querySk.Any(f => f.Id_Soup_Kitchen == e.SoupKitchen.Id));
                        }
                    }
                    if (pState > 0)
                    {
                        query = query.Where(e => e.SoupKitchen.State.Id == pState);
                    }
                    if (pSoupK > 0)
                    {
                        query = query.Where(e => e.SoupKitchen.Id == pSoupK);
                    }
                    if (pStatus > 0)
                    {
                        query = query.Where(e => e.Status.Id == pStatus);
                    }

                    return query.ToList<CaptureDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        //public List<CaptureDTO> GetAllCaptures(int userTypeId, int userId)
        //{
        //    using (SEDESOLEntities entities = new SEDESOLEntities())
        //    {
        //        try
        //        {
        //            var query = from capture in entities.CAPTUREs
        //                        join userSk in entities.USER_SOUP_KITCHEN
        //                            on capture.Id_SoupKitchen equals userSk.Id_Soup_Kitchen
        //                        select new CaptureDTO
        //                        {
        //                            Id = capture.Id,
        //                            Description = capture.Description,
        //                            CreateDate = capture.CreateDate,
        //                            IsActive = capture.IsActive,
        //                            ReceiverCount = capture.ATTENDANCEs.Count(),
        //                            AttendanceCount = capture.ATTENDANCEs.Sum(x => x.Quantity) ?? 0,
        //                            Folio = capture.Folio,
        //                            InspectionCode = capture.InspectionCode,
        //                            Id_LevelApproval = capture.Id_LevelApproval,
        //                            Id_User = capture.Id_User,
        //                            Month = new MonthDTO
        //                            {
        //                                Id = capture.MONTH.Id,
        //                                Description = capture.MONTH.Description,
        //                                IsActive = capture.MONTH.IsActive
        //                            },
        //                            Year = new YearDTO
        //                            {
        //                                Id = capture.YEAR.Id,
        //                                Description = capture.YEAR.Description,
        //                                IsActive = capture.YEAR.IsActive
        //                            },
        //                            Status = new StatusDTO
        //                            {
        //                                Id = capture.STATUS.Id,
        //                                Description = capture.STATUS.Description,
        //                                IsActive = capture.STATUS.IsActive
        //                            },
        //                            SoupKitchen = new SoupKitchenDTO
        //                            {
        //                                Id = capture.SOUP_KITCHEN.Id,
        //                                Name = capture.SOUP_KITCHEN.Name,
        //                                Description = capture.SOUP_KITCHEN.Description,
        //                                Capacity = capture.SOUP_KITCHEN.Capacity,
        //                                Address = capture.SOUP_KITCHEN.Address,
        //                                PhoneNumber = capture.SOUP_KITCHEN.PhoneNumber,
        //                                ContactName = capture.SOUP_KITCHEN.ContactName,
        //                                IsActive = capture.SOUP_KITCHEN.IsActive,
        //                                AllowAnonym = capture.SOUP_KITCHEN.AllowAnonym,
        //                                Folio = capture.SOUP_KITCHEN.Folio,
        //                                State = new StateDTO
        //                                {
        //                                    Id = capture.SOUP_KITCHEN.STATE.Id,
        //                                    Name = capture.SOUP_KITCHEN.STATE.Name
        //                                }
        //                            }
        //                        };
        //            if (userTypeId == 1)
        //            {
        //                query = query.Where(e => e.Id_User == userId);
        //            }
        //            else
        //            {
        //                if (userTypeId != 2)
        //                {
        //                    query = query.Where(e => );
        //                }
        //            }
        //            return query.ToList<CaptureDTO>();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception(ex.Message);
        //        }
        //    }
        //}

        public CaptureDTO GetCaptureById(int id)
        {
            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                var query = db.CAPTUREs.Where(f => f.Id == id).Include(p => p.ATTENDANCEs).Select(p =>
                             new CaptureDTO
                             {
                                 Id = p.Id,
                                 Description = p.Description,
                                 CreateDate = p.CreateDate,
                                 IsActive = p.IsActive,
                                 Folio = p.Folio,
                                 InspectionCode = p.InspectionCode,
                                 Id_LevelApproval = p.Id_LevelApproval,
                                 Id_User = p.Id_User,
                                 Month = new MonthDTO
                                 {
                                     Id = p.MONTH.Id,
                                     Description = p.MONTH.Description,
                                     IsActive = p.MONTH.IsActive
                                 },
                                 Year = new YearDTO
                                 {
                                     Id = p.YEAR.Id,
                                     Description = p.YEAR.Description,
                                     IsActive = p.YEAR.IsActive
                                 },
                                 Status = new StatusDTO
                                 {
                                     Id = p.STATUS.Id,
                                     Description = p.STATUS.Description,
                                     IsActive = p.STATUS.IsActive
                                 },
                                 SoupKitchen = new SoupKitchenDTO
                                 {
                                     Id = p.SOUP_KITCHEN.Id,
                                     Name = p.SOUP_KITCHEN.Name,
                                     Description = p.SOUP_KITCHEN.Description,
                                     Capacity = p.SOUP_KITCHEN.Capacity,
                                     Address = p.SOUP_KITCHEN.Address,
                                     PhoneNumber = p.SOUP_KITCHEN.PhoneNumber,
                                     ContactName = p.SOUP_KITCHEN.ContactName,
                                     IsActive = p.SOUP_KITCHEN.IsActive,
                                     AllowAnonym = p.SOUP_KITCHEN.AllowAnonym,
                                     Folio = p.SOUP_KITCHEN.Folio,
                                     State = new StateDTO
                                     {
                                         Id = p.SOUP_KITCHEN.STATE.Id,
                                         Name = p.SOUP_KITCHEN.STATE.Name
                                     }
                                 },
                                 AttendanceList = p.ATTENDANCEs.Select(a =>
                                 new AttendanceDTO
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     LastName = a.LastName,
                                     LastName2 = a.LastName2,
                                     Curp = a.Curp,
                                     Birthdate = a.Birthdate,
                                     Quantity = a.Quantity,
                                     Id_Capture = a.Id_Capture,
                                     IsAnonym = a.IsAnonym,
                                     Gender = a.Gender,
                                     Id_Condition = a.Id_Condition,
                                     Id_State_Birth = a.Id_State_Birth,
                                     Address = a.Address,
                                     PhoneNumber = a.PhoneNumber
                                     //StateDto = new StateDTO
                                     //{
                                     //    Id = a.STATE.Id,
                                     //    Name = a.STATE.Name
                                     //},
                                     //ConditionDto = new ConditionDTO
                                     //{
                                     //    Id = a.CONDITION.Id,
                                     //    Description = a.CONDITION.Name
                                     //}
                                 }).ToList<AttendanceDTO>(),
                                 ImageList = p.CAPTURE_IMAGE.Select(a =>
                                 new CaptureImageDTO
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     ImageFile = a.ImageFile,
                                     ImageFileB64 = string.Empty,
                                     Id_Capture = a.Id_Capture
                                 }).ToList<CaptureImageDTO>(),
                                 UserTypeDto = new UserTypeDTO
                                 {
                                     Id = p.USER_TYPE.Id,
                                     Description = p.USER_TYPE.Description
                                 }
                             });
                return query.FirstOrDefault();
            }
        }

        public void Deactivate(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                CAPTURE b = entities.CAPTUREs.Find(id);
                b.IsActive = false;

                entities.SaveChanges();
            }
        }

        public void Activate(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                CAPTURE b = entities.CAPTUREs.Find(id);
                b.IsActive = true;

                entities.SaveChanges();
            }
        }

        public CaptureDTO Save(CaptureDTO capture)
        {
            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    CAPTURE cap = db.CAPTUREs.FirstOrDefault(v => v.Id_Year == capture.Id_Year && v.Id_Month == capture.Id_Month && v.Id_SoupKitchen == capture.Id_Soup_Kitchen && capture.IsActive == true);
                    if (cap != null)
                    {
                        capture.Message = "Se ha creado previamente una captura para el periodo seleccionado.";
                    }
                    else
                    {
                        INSPECTION_CODE code = db.INSPECTION_CODE.FirstOrDefault(v => v.Id_Year == capture.Id_Year && v.Id_Month == capture.Id_Month && v.InspectionCode == capture.InspectionCode);
                        if (code == null)
                        {
                            capture.Message = "El Código de inspección ingresado es incorrecto";
                        }
                        else
                        {
                            CAPTURE captureEntity = new CAPTURE
                            {
                                Description = capture.Description,
                                CreateDate = capture.CreateDate,
                                IsActive = capture.IsActive,
                                Id_Month = capture.Id_Month,
                                Id_Year = capture.Id_Year,
                                Id_Status = capture.Id_Status,
                                Id_SoupKitchen = (int)capture.Id_Soup_Kitchen,
                                Folio = capture.Folio,
                                InspectionCode = capture.InspectionCode,
                                Id_LevelApproval = capture.Id_LevelApproval,
                                Id_User = capture.Id_User
                            };

                            db.CAPTUREs.Add(captureEntity);
                            if (db.SaveChanges() > 0)
                            {
                                capture.Id = captureEntity.Id;
                                capture.Message = "SUCCESS";

                                //var query = from st in db.STATUS
                                //            where st.Id == capture.Id_Status
                                //            select new StatusDTO
                                //            {
                                //                Id = captureEntity.STATUS.Id,
                                //                Description = captureEntity.STATUS.Description,
                                //                IsActive = captureEntity.STATUS.IsActive
                                //            };
                                //capture.Status = query.FirstOrDefault();
                            }
                            else
                            {
                                return new CaptureDTO();
                            }
                        }
                        
                    }
                    return capture;
                }
            }
            catch(Exception ex)
            {
                return new CaptureDTO();
            }

        }

        public bool Update(CaptureDTO capture)
        {
            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                CAPTURE b = db.CAPTUREs.Find(capture.Id);
                b.Description = capture.Description;
                b.CreateDate = capture.CreateDate;
                b.IsActive = capture.IsActive;
                b.Id_Month = capture.Id_Month;
                b.Id_Year = capture.Id_Year;
                b.Id_Status = capture.Id_Status;
                b.Id_SoupKitchen = (int)capture.Id_Soup_Kitchen;
                b.Folio = capture.Folio;
                b.InspectionCode = capture.InspectionCode;
                b.Id_LevelApproval = capture.Id_LevelApproval;
                b.Id_User = capture.Id_User;
                db.SaveChanges();
            }
           
            return true;
        }

        public string EditStatus(int idStatus, int idCapture, int UserTypeId)
        {
            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                CAPTURE b = db.CAPTUREs.FirstOrDefault(v => v.Id == idCapture);
                if (b != null)
                {
                    b.Id_Status = idStatus;
                    b.Id_LevelApproval = UserTypeId;
                    db.SaveChanges();
                    return "SUCCESS";
                }
                else
                {
                    return "El envío de captura no fue completado.";
                }
            }
        }
    }
}
