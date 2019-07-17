using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;

namespace SEDESOL.BusinessLogic
{
    public class CaptureDAL
    {
        public List<CaptureDTO> GetCaptures()
        {
            CaptureDAO b = new CaptureDAO();
            return b.GetCaptures();
        }

        public List<CaptureDTO> GetCaptures(int userTypeId, int userId)
        {
            CaptureDAO b = new CaptureDAO();
            return b.GetCaptures(userTypeId, userId);
        }

        public List<CaptureDTO> GetCapturesSearch(int userTypeId, int userId, int pState, int pSoupK, int pStatus)
        {
            CaptureDAO b = new CaptureDAO();
            return b.GetCapturesSearch(userTypeId, userId, pState, pSoupK, pStatus);
        }


        public CaptureDTO GetCaptureById(int id)
        {
            CaptureDAO c = new CaptureDAO();
            return c.GetCaptureById(id);
        }

        public void Deactivate(int id)
        {
            CaptureDAO c = new CaptureDAO();
            c.Deactivate(id);
        }

        public void Activate(int id)
        {
            CaptureDAO c = new CaptureDAO();
            c.Activate(id);
        }

        public bool Update(CaptureDTO dto)
        {
            CaptureDAO b = new CaptureDAO();
            return b.Update(dto);
        }

        public CaptureDTO Save(CaptureDTO dto)
        {
            CaptureDAO b = new CaptureDAO();
            dto = b.Save(dto);
            string message = dto.Message;

            CaptureDTO capt = new CaptureDTO();
            capt = b.GetCaptureById(dto.Id);
            if (capt == null)
                capt = new CaptureDTO();
            capt.Message = message;

            return capt;
        }

        public string EditStatus(int idStatus, int idCapture, int idUserType)
        {
            CaptureDAO capDao = new CaptureDAO();
            List<SkUserTypeDTOcs> listLevel = new List<SkUserTypeDTOcs>();
            SoupKitchenDAO skDao = new SoupKitchenDAO();
            int IdUserType = 0;

            var capture = capDao.GetCaptureById(idCapture);

            //get list of SK levels
            listLevel = skDao.GetUserTypeBySKId((int)capture.SoupKitchen.Id);
            if (listLevel.Count == 0 || listLevel == null)
            {
                IdUserType = 2;
            }
            else
            {
                if (idUserType == 1)
                {
                    listLevel = listLevel.Where(f => f.UserTypeDto.ApprovalOrder > 0).ToList();
                    var topApproval = listLevel.OrderBy(i => i.UserTypeDto.ApprovalOrder).Take(1);
                    IdUserType = topApproval.FirstOrDefault().UserTypeDto.Id;
                }
                else if(idUserType == 2)
                {
                    IdUserType = 2;
                }
                else
                {
                    IdUserType = idUserType;
                }
            }
           

            return capDao.EditStatus(idStatus, idCapture, IdUserType);
        }
    }
}
