using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;


namespace SEDESOL.BusinessLogic
{
    public class SoapKitchenDAL
    {
        public List<SoupKitchenDTO> GetSoapKitchenByUser(int userTypeId, int userId)
        {
            SoupKitchenDAO b = new SoupKitchenDAO();
            return b.GetSoupKitchenByUser(userTypeId, userId);
        }

        public List<SoupKitchenDTO> GetSoupKitchenAll()
        {
            SoupKitchenDAO b = new SoupKitchenDAO();
            return b.GetSoupKitchenAll();
        }

        public List<SoupKitchenDTO> GetSoupKitchenToAssignUser(int id)
        {
            SoupKitchenDAO b = new SoupKitchenDAO();
            return b.GetSoupKitchenToAssignUser(id);
        }

        public List<SoupKitchenDTO> GetSKAssignedUser(int id)
        {
            SoupKitchenDAO b = new SoupKitchenDAO();
            return b.GetSKAssignedUser(id);
        }

        public SoupKitchenDTO Save(SoupKitchenDTO dto)
        {
            SoupKitchenDAO dao = new SoupKitchenDAO();
            return dao.Save(dto);
        }

        public SoupKitchenDTO GetSoupKitchenById(int id)
        {
            SoupKitchenDAO b = new SoupKitchenDAO();
            return b.GetSoupKitchenById(id);
        }

        public void Deactivate(int id)
        {
            SoupKitchenDAO dao = new SoupKitchenDAO();
            dao.Deactivate(id);
        }

        public void Activate(int id)
        {
            SoupKitchenDAO dao = new SoupKitchenDAO();
            dao.Activate(id);
        }

        public List<SkUserTypeDTOcs> GetUserTypeBySKId(int skId)
        {
            SoupKitchenDAO b = new SoupKitchenDAO();
            return b.GetUserTypeBySKId(skId);
        }

        public string DeleteUserTypeSk(int id)
        {
            SoupKitchenDAO dao = new SoupKitchenDAO();
            return dao.Delete(id);
        }
    }
}
