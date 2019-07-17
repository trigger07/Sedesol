using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities;
using SEDESOL.DataModel;
using SEDESOL.DataEntities.DTO;
using System.Data.Entity;
using SEDESOL.DataEntities.IntegrationObjects;

namespace SEDESOL.DataAccess
{
    public class LoginDAO
    {
        public UserDTO GetUser(UserValidationRequest req)
        {
            UserDTO uDto = new UserDTO();
            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                var query = from user in db.USERs
                            where user.Password == req.Password && user.Username == req.Username
                            select new UserDTO
                            {
                                Id = user.Id,
                                Email = user.Email,
                                Dni = user.Dni,
                                Username = user.Username,
                                Password = user.Password,
                                Name = user.Name,
                                LastName = user.LastName,
                                PhoneNumber = user.PhoneNumber,
                                IsActive = user.IsActive,
                                Id_User_Type = user.Id_User_Type,
                                UserType = new UserTypeDTO
                                {
                                    Id = user.USER_TYPE.Id,
                                    Description = user.USER_TYPE.Description,
                                    IsActive = user.USER_TYPE.IsActive
                                }
                            };
                return query.FirstOrDefault();
            }
        }
    }
}
