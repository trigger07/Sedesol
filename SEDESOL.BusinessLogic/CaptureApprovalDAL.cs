using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;

namespace SEDESOL.BusinessLogic
{
    public class CaptureApprovalDAL
    {
        public List<CaptureApprovalDTO> GetApprovalByCaptureId(int captureId)
        {
            CaptureApprovalDAO dao = new CaptureApprovalDAO();
            return dao.GetCaptureApprovalByCaptureId(captureId);
        }

        public string SaveApproval(CaptureApprovalDTO dto)
        {
            string msg = string.Empty;
            int approvalStatus = 0;
            int level = 0;

            SoupKitchenDAO skDao = new SoupKitchenDAO();
            CaptureDAO capDao = new CaptureDAO();
            List<SkUserTypeDTOcs> listLevel = new List<SkUserTypeDTOcs>();

            //get current level of captura approval
            var capture = capDao.GetCaptureById(dto.Id_Capture);
            //get list of SK levels
            listLevel = skDao.GetUserTypeBySKId((int)capture.SoupKitchen.Id);
            //get top level approval
            var topApproval = listLevel.OrderByDescending(i => i.UserTypeDto.ApprovalOrder).Take(1);
            

            //validations
            
            if (dto.UserDto.Id_User_Type != 2)
            {
                if (listLevel.Count() == 0 || listLevel == null)
                {
                    msg = "El comedor no tiene niveles de aprobación asignados.";
                    return msg;
                }

                //1. level of user in session is a part of levels of sk
                listLevel.Where(i => i.Id_UserType == dto.UserDto.Id_User_Type).ToList();
                if (listLevel.Count() == 0 || listLevel == null)
                {
                    msg = "Su usuario no tiene el nivel de aprobación permitido para este comedor.";
                    return msg;
                }
                //validate if userType is equals to current level
                else if (dto.UserDto.Id_User_Type != capture.Id_LevelApproval)
                {
                    msg = "La captura se encuentra asignada para aprobación del nivel: " + capture.UserTypeDto.Description;
                    return msg;
                }
            }
            
            if (dto.UserDto.Id_User_Type == 2)
            {
                //2. admin set status in approve by default, if not verify user level and appply status
                if (dto.Id_Status == 4)
                {
                    approvalStatus = 4;
                    dto.Id_Status = 4;
                    level = 2;
                }
                else
                {
                    approvalStatus = 5;
                    dto.Id_Status = 6;
                    //return to kitchen top level
                    level = topApproval.FirstOrDefault().Id_UserType;
                }
            }
            else
            {
                //3. if level of user is top of sk set in approved, if not set in process of approval
                if (dto.Id_Status == 4)
                {
                    if (dto.UserDto.Id_User_Type == topApproval.FirstOrDefault().Id_UserType)
                    {
                        approvalStatus = 4;
                        dto.Id_Status = 4;
                        level = dto.UserDto.Id_User_Type;
                    }
                    else
                    {
                        approvalStatus = 4;
                        dto.Id_Status = 6;
                        level = dto.UserDto.Id_User_Type + 1;
                    }
                }
                else
                {
                    approvalStatus = 5;
                    dto.Id_Status = 6;
                    if ((dto.UserDto.Id_User_Type - 1) > 2)
                    {
                        level = dto.UserDto.Id_User_Type - 1;
                    }
                    else
                    {
                        level = dto.UserDto.Id_User_Type;
                    }
                   
                }

            }

            CaptureApprovalDAO dao = new CaptureApprovalDAO();
            //take dto.Id_Status
            return dao.Save(dto, approvalStatus, level);
        }
    }
}
