using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;

namespace SEDESOL.BusinessLogic
{
    public class UserDAL
    {
        public List<UserDTO> GetUserAll()
        {
            UserDAO dao = new UserDAO();
            return dao.GetUserAll();
        }

        public UserDTO GetUserById(int id)
        {
            UserDAO dao = new UserDAO();
            return dao.GetUserById(id);
        }

        public void Deactivate(int id)
        {
            UserDAO dao = new UserDAO();
            dao.Deactivate(id);
        }

        public void Activate(int id)
        {
            UserDAO dao = new UserDAO();
            dao.Activate(id);
        }

        public UserDTO Save(UserDTO dto)
        {
            UserDAO dao = new UserDAO();
            return dao.Save(dto);
        }

        public List<UserTypeDTO> GetUserTypeAll()
        {
            UserDAO dao = new UserDAO();
            return dao.GetUserTypeAll();
        }

        public List<UserTypeDTO> GetUserTypeApproval()
        {
            UserDAO dao = new UserDAO();
            return dao.GetUserTypeApproval();
        }

        public SkUserTypeDTOcs SaveUserTypeSK(SkUserTypeDTOcs dto)
        {
            UserDAO dao = new UserDAO();
            return dao.SaveUserTypeSK(dto);
        }

        public UserSoupKitchen SaveUserSoupKitchen(UserSoupKitchen dto)
        {
            UserDAO dao = new UserDAO();
            return dao.SaveUserSoupKitchen(dto);
        }

        public string DeleteUserSoupKitchen(int idSk, int idUser)
        {
            UserDAO dao = new UserDAO();
            return dao.DeleteUserSoupKitchen(idSk, idUser);
        }
    }
}
