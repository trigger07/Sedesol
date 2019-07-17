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
    public class UserDAO
    {

        public List<UserDTO> GetUserAll()
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from user in entities.USERs

                            select new UserDTO
                            {
                                Id = user.Id,
                                Username = user.Username,
                                Password = user.Password,
                                Name = user.Name,
                                LastName = user.LastName,
                                PhoneNumber = user.PhoneNumber,
                                Email = user.Email,
                                Dni = user.Dni,
                                IsActive = user.IsActive,
                                Id_User_Type = user.Id_User_Type,
                                UserType = new UserTypeDTO
                                {
                                    Id = user.USER_TYPE.Id,
                                    Description = user.USER_TYPE.Description,
                                    IsActive = user.USER_TYPE.IsActive
                                }
                            };
                return query.ToList<UserDTO>();
            }
        }

        public UserDTO GetUserById(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from user in entities.USERs
                            where user.Id == id
                            select new UserDTO
                            {
                                Id = user.Id,
                                Username = user.Username,
                                Password = user.Password,
                                Name = user.Name,
                                LastName = user.LastName,
                                PhoneNumber = user.PhoneNumber,
                                Email = user.Email,
                                Dni = user.Dni,
                                IsActive = user.IsActive,
                                Id_User_Type = user.Id_User_Type,
                                UserType = new UserTypeDTO
                                {
                                    Id = user.USER_TYPE.Id,
                                    Description = user.USER_TYPE.Description,
                                    IsActive = user.USER_TYPE.IsActive
                                }
                            };
                UserDTO userDto = query.FirstOrDefault();
                if (userDto == null)
                {
                    return new UserDTO();
                }

                var querySoupK = from usr in entities.SOUP_KITCHEN
                                 join usrSoup in entities.USER_SOUP_KITCHEN
                                 on usr.Id equals usrSoup.Id_Soup_Kitchen
                                 where usrSoup.Id_User == userDto.Id
                                 select new SoupKitchenDTO
                                 {
                                     Id = usr.Id,
                                     Name = usr.Name,
                                     Description = usr.Description,
                                     Capacity = usr.Capacity,
                                     Address = usr.PhoneNumber,
                                     ContactName = usr.ContactName,
                                     PhoneNumber = usr.PhoneNumber,
                                     State = new StateDTO
                                     {
                                         Id = usr.STATE.Id,
                                         Name = usr.STATE.Name
                                     }
                                 };
                userDto.SoupKitchenList = querySoupK.ToList();

                return userDto;
            }
        }

        public UserDTO Save(UserDTO dto)
        {
            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                USER sk = db.USERs.FirstOrDefault(v => v.Id == dto.Id);
                if (sk != null)
                {
                    sk.Name = dto.Name;
                    sk.Username = dto.Username;
                    if (dto.Password != string.Empty)
                    {
                        sk.Password = dto.Password;
                    }
                    sk.LastName = dto.LastName;
                    sk.PhoneNumber = dto.PhoneNumber;
                    sk.Email = dto.Email;
                    sk.Dni = dto.Dni;
                    sk.Id_User_Type = dto.Id_User_Type;

                    if (db.SaveChanges() > 0)
                    {
                        dto.Id = sk.Id;
                        dto.Message = "SUCCESS";
                    }
                }
                else
                {
                    sk = new USER();
                    sk.Name = dto.Name;
                    sk.Username = dto.Username;
                    sk.Password = dto.Password;
                    sk.LastName = dto.LastName;
                    sk.PhoneNumber = dto.PhoneNumber;
                    sk.Email = dto.Email;
                    sk.Dni = dto.Dni;
                    sk.Id_User_Type = dto.Id_User_Type;
                    sk.IsActive = true;
                    db.USERs.Add(sk);

                    if (db.SaveChanges() > 0)
                    {
                        dto.Id = sk.Id;
                        dto.Message = "SUCCESS";
                    }

                }
                return dto;
            }
        }

        public void Deactivate(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                USER sk = entities.USERs.Find(id);
                sk.IsActive = false;

                entities.SaveChanges();
            }
        }

        public void Activate(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                USER sk = entities.USERs.Find(id);
                sk.IsActive = true;

                entities.SaveChanges();
            }
        }

        public List<UserTypeDTO> GetUserTypeAll()
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from user in entities.USER_TYPE

                            select new UserTypeDTO
                            {
                                Id = user.Id,
                                Description = user.Description,
                                IsActive = user.IsActive
                            };
                return query.ToList<UserTypeDTO>();
            }
        }

        public List<UserTypeDTO> GetUserTypeApproval()
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from user in entities.USER_TYPE
                            where user.IsActive == true && user.ApprovalOrder > 0
                            select new UserTypeDTO
                            {
                                Id = user.Id,
                                Description = user.Description,
                                IsActive = user.IsActive
                            };
                return query.ToList<UserTypeDTO>();
            }
        }

        public SkUserTypeDTOcs SaveUserTypeSK(SkUserTypeDTOcs dto)
        {
            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    SOUP_KITCHEN_USER_TYPE ut = db.SOUP_KITCHEN_USER_TYPE.FirstOrDefault(v => v.Id_UserType == dto.Id_UserType && v.Id_SoupKitchen == dto.Id_SoupKitchen);
                    if (ut != null)
                    {
                        dto.Message = "Se ha ingresado previamente.";
                    }
                    else
                    {
                        ut = new SOUP_KITCHEN_USER_TYPE();
                        ut.Id_SoupKitchen = dto.Id_SoupKitchen;
                        ut.Id_UserType = dto.Id_UserType;
                        ut.IsActive = true;
                        db.SOUP_KITCHEN_USER_TYPE.Add(ut);

                        if (db.SaveChanges() > 0)
                        {
                            dto.Id = ut.Id;
                            dto.Message = "SUCCESS";
                        }
                    }
                    return dto;
                }
            }
            catch (Exception ex)
            {
                return new SkUserTypeDTOcs();
            }

        }

        public UserSoupKitchen SaveUserSoupKitchen(UserSoupKitchen dto)
        {
            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    USER_SOUP_KITCHEN ut = db.USER_SOUP_KITCHEN.FirstOrDefault(v => v.Id_User == dto.Id_User && v.Id_Soup_Kitchen == dto.Id_Soup_Kitchen);
                    if (ut != null)
                    {
                        dto.Message = "Se ha ingresado previamente.";
                    }
                    else
                    {
                        ut = new USER_SOUP_KITCHEN();
                        ut.Id_Soup_Kitchen = dto.Id_Soup_Kitchen;
                        ut.Id_User = dto.Id_User;
                        ut.IsActive = true;
                        db.USER_SOUP_KITCHEN.Add(ut);

                        if (db.SaveChanges() > 0)
                        {
                            dto.Id = ut.Id;
                            dto.Message = "SUCCESS";
                        }
                    }
                    return dto;
                }
            }
            catch (Exception ex)
            {
                return new UserSoupKitchen();
            }

        }

        public string DeleteUserSoupKitchen(int idSk, int idUser)
        {
            string result = "";

            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    USER_SOUP_KITCHEN att = db.USER_SOUP_KITCHEN.FirstOrDefault(v => v.Id_Soup_Kitchen == idSk && v.Id_User == idUser);
                    if (att != null)
                    {
                        db.USER_SOUP_KITCHEN.Remove(att);
                        db.SaveChanges();

                        result = "SUCCESS";
                    }
                    else
                    {
                        result = "No se pudo desasociar el comedor.";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "No se pudo desasociar el comedor";
            }
            return result;
        }
    }
}
