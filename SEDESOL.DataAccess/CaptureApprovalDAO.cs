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
    public class CaptureApprovalDAO
    {
        public string Save(CaptureApprovalDTO dto, int approveStatus, int level)
        {
            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        string msj = string.Empty;

                        CAPTURE_APPROVAL app = new CAPTURE_APPROVAL();
                        app.Id_Capture = dto.Id_Capture;
                        app.Id_User = dto.Id_User;
                        app.Id_Status = approveStatus;
                        app.CreateDate = DateTime.Now;

                        db.CAPTURE_APPROVAL.Add(app);

                        if (db.SaveChanges() > 0)
                        {
                            dto.Id = app.Id;
                            msj = "SUCCESS";
                        }
                        else
                        {
                            return "ERROR";
                        }

                        CAPTURE b = db.CAPTUREs.FirstOrDefault(v => v.Id == dto.Id_Capture);
                        if (b != null)
                        {
                            b.Id_Status = dto.Id_Status;
                            b.Id_LevelApproval = level;
                            db.SaveChanges();
                            msj = "SUCCESS";
                        }
                        else
                        {
                            transaction.Rollback();
                            return "ERROR";
                        }

                        transaction.Commit();
                        return msj;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        return "ERROR";
                    }
                }
            }
        }


        public List<CaptureApprovalDTO> GetCaptureApprovalByCaptureId(int captureId)
        {
            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                var query = from cap in db.CAPTURE_APPROVAL.Where(f => f.Id_Capture == captureId)
                            select new CaptureApprovalDTO
                            {
                                Id = cap.Id,
                                Id_User = cap.Id_User,
                                Id_Capture = cap.Id_Capture,
                                Id_Status = cap.Id_Status,
                                CreateDate = cap.CreateDate,
                                UserDto = new UserDTO
                                {
                                    Id = cap.USER.Id,
                                    Name = cap.USER.Name,
                                    LastName = cap.USER.LastName,
                                    Username = cap.USER.Username,
                                    UserType = new UserTypeDTO
                                    {
                                        Description = cap.USER.USER_TYPE.Description
                                    }
                                },
                                StatusDto = new StatusDTO
                                {
                                    Id = cap.STATUS.Id,
                                    Description = cap.STATUS.Description
                                }
                            };

                return query.ToList<CaptureApprovalDTO>();
            }
        }
    }
}
