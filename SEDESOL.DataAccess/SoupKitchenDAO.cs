using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities;
using SEDESOL.DataModel;
using SEDESOL.DataEntities.DTO;

namespace SEDESOL.DataAccess
{
    public class SoupKitchenDAO
    {
        public List<SoupKitchenDTO> GetSoupKitchenByUser(int userTypeId, int userId)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from soupK in entities.SOUP_KITCHEN
                                //join user in entities.USER_SOUP_KITCHEN
                                //on soupK.Id equals user.Id_Soup_Kitchen
                                //where user.Id_User == userId && user.IsActive == true
                            //where soupK.IsActive == true
                            select new SoupKitchenDTO
                            {
                                Id = soupK.Id,
                                Name = soupK.Name,
                                Description = soupK.Name + " | " + soupK.STATE.Name,
                                Capacity = soupK.Capacity,
                                Address = soupK.Address,
                                PhoneNumber = soupK.PhoneNumber,
                                ContactName = soupK.ContactName,
                                IsActive = soupK.IsActive,
                                Id_State = soupK.Id_State,
                                AllowAnonym = soupK.AllowAnonym,
                                Folio = soupK.Folio,
                                State = new StateDTO
                                {
                                    Id = soupK.STATE.Id,
                                    Name = soupK.STATE.Name
                                },
                                RegionDto = new RegionDTO
                                {
                                    Id = soupK.REGION.Id,
                                    Name = soupK.REGION.Name
                                },
                                Id_Region = (int)soupK.Id_Region

                            };
                if (userTypeId != 2)
                {
                    // obtener comedores asignados al usuario
                    var querySk = entities.USER_SOUP_KITCHEN.Where(p => p.Id_User == userId && p.IsActive == true);

                    query = query.Where(e => querySk.Any(f => f.SOUP_KITCHEN.Id == e.Id));
                }

                return query.ToList<SoupKitchenDTO>();
            }
        }

        public List<SoupKitchenDTO> GetSoupKitchenToAssignUser(int userId)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from soupK in entities.SOUP_KITCHEN
                                //join user in entities.USER_SOUP_KITCHEN
                                //on soupK.Id equals user.Id_Soup_Kitchen
                                //where user.Id_User == userId && user.IsActive == true
                                //where soupK.IsActive == true
                            select new SoupKitchenDTO
                            {
                                Id = soupK.Id,
                                Name = soupK.Name,
                                Description = soupK.Name + " | " + soupK.STATE.Name,
                                Capacity = soupK.Capacity,
                                Address = soupK.Address,
                                PhoneNumber = soupK.PhoneNumber,
                                ContactName = soupK.ContactName,
                                IsActive = soupK.IsActive,
                                Id_State = soupK.Id_State,
                                AllowAnonym = soupK.AllowAnonym,
                                Folio = soupK.Folio,
                                State = new StateDTO
                                {
                                    Id = soupK.STATE.Id,
                                    Name = soupK.STATE.Name
                                },
                                RegionDto = new RegionDTO
                                {
                                    Id = soupK.REGION.Id,
                                    Name = soupK.REGION.Name
                                },
                                Id_Region = (int)soupK.Id_Region
                            };
                // obtener comedores asignados al usuario
                var querySk = entities.USER_SOUP_KITCHEN.Where(p => p.Id_User == userId && p.IsActive == true);

                query = query.Where(e => !querySk.Any(f => f.SOUP_KITCHEN.Id == e.Id));

                return query.ToList<SoupKitchenDTO>();
            }
        }

        public List<SoupKitchenDTO> GetSKAssignedUser(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from soupK in entities.SOUP_KITCHEN
                            join userSk in entities.USER_SOUP_KITCHEN on soupK.Id equals userSk.Id_Soup_Kitchen
                            where userSk.Id_User == id
                            select new SoupKitchenDTO
                            {
                                Id = soupK.Id,
                                Name = soupK.Name,
                                Description = soupK.Name + " | " + soupK.STATE.Name,
                                Capacity = soupK.Capacity,
                                Address = soupK.Address,
                                PhoneNumber = soupK.PhoneNumber,
                                ContactName = soupK.ContactName,
                                IsActive = soupK.IsActive,
                                Id_State = soupK.Id_State,
                                AllowAnonym = soupK.AllowAnonym,
                                Folio = soupK.Folio,
                                State = new StateDTO
                                {
                                    Id = soupK.STATE.Id,
                                    Name = soupK.STATE.Name
                                },
                                RegionDto = new RegionDTO
                                {
                                    Id = soupK.REGION.Id,
                                    Name = soupK.REGION.Name
                                },
                                Id_Region = (int)soupK.Id_Region
                            };
                return query.ToList<SoupKitchenDTO>();
            }
        }

        public List<SoupKitchenDTO> GetSoupKitchenAll()
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from soupK in entities.SOUP_KITCHEN

                            select new SoupKitchenDTO
                            {
                                Id = soupK.Id,
                                Name = soupK.Name,
                                Description = soupK.Name + " | " + soupK.STATE.Name,
                                Capacity = soupK.Capacity,
                                Address = soupK.Address,
                                PhoneNumber = soupK.PhoneNumber,
                                ContactName = soupK.ContactName,
                                IsActive = soupK.IsActive,
                                Id_State = soupK.Id_State,
                                AllowAnonym = soupK.AllowAnonym,
                                Folio = soupK.Folio,
                                State = new StateDTO
                                {
                                    Id = soupK.STATE.Id,
                                    Name = soupK.STATE.Name,
                                    IsActive = soupK.STATE.IsActive
                                },
                                RegionDto = new RegionDTO
                                {
                                    Id = soupK.REGION.Id,
                                    Name = soupK.REGION.Name
                                },
                                Id_Region =(int)soupK.Id_Region
                            };
                return query.ToList<SoupKitchenDTO>();
            }
        }

        public SoupKitchenDTO Save(SoupKitchenDTO att)
        {
            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                SOUP_KITCHEN sk = db.SOUP_KITCHEN.FirstOrDefault(v => v.Id == att.Id);
                if (sk != null)
                {
                    SOUP_KITCHEN sk2 = db.SOUP_KITCHEN.FirstOrDefault(v => v.Name == att.Name && v.Id_State == att.Id_State && v.Id != att.Id);
                    if (sk2 != null)
                    {

                        att.Message = "El comedor ha sido ingresado previamente";
                    }
                    else
                    {
                        sk.Name = att.Name;
                        sk.Description = att.Description;
                        sk.Capacity = att.Capacity;
                        sk.Address = att.Address;
                        sk.PhoneNumber = att.PhoneNumber;
                        sk.ContactName = att.ContactName;
                        //sk.IsActive = att.IsActive;
                        sk.Id_State = att.Id_State;
                        sk.AllowAnonym = att.AllowAnonym;
                        sk.Folio = att.Folio;
                        sk.Id_Region = att.Id_Region;

                        if (db.SaveChanges() > 0)
                        {
                            att.Id = sk.Id;
                            att.Message = "SUCCESS";
                        }
                    }
                }
                else
                {
                    SOUP_KITCHEN sk2 = db.SOUP_KITCHEN.FirstOrDefault(v => v.Name == att.Name && v.Id_State == att.Id_State);
                    if (sk2 != null)
                    {

                        att.Message = "El comedor ha sido ingresado previamente";
                    }
                    else
                    {
                        sk = new SOUP_KITCHEN();
                        sk.Name = att.Name;
                        sk.Description = att.Description;
                        sk.Capacity = att.Capacity;
                        sk.Address = att.Address;
                        sk.PhoneNumber = att.PhoneNumber;
                        sk.ContactName = att.ContactName;
                        sk.IsActive = true;
                        sk.Id_State = att.Id_State;
                        sk.AllowAnonym = att.AllowAnonym;
                        sk.Folio = att.Folio;
                        sk.Id_Region = att.Id_Region;

                        db.SOUP_KITCHEN.Add(sk);

                        if (db.SaveChanges() > 0)
                        {
                            att.Id = sk.Id;
                            att.Message = "SUCCESS";
                        }
                    }

                }
                return att;
            }
        }

        public SoupKitchenDTO GetSoupKitchenById(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from soupK in entities.SOUP_KITCHEN
                            where soupK.Id == id
                            select new SoupKitchenDTO
                            {
                                Id = soupK.Id,
                                Name = soupK.Name,
                                Description = soupK.Description,
                                Capacity = soupK.Capacity,
                                Address = soupK.Address,
                                PhoneNumber = soupK.PhoneNumber,
                                ContactName = soupK.ContactName,
                                IsActive = soupK.IsActive,
                                Id_State = soupK.Id_State,
                                AllowAnonym = soupK.AllowAnonym,
                                Folio = soupK.Folio,
                                State = new StateDTO
                                {
                                    Id = soupK.STATE.Id,
                                    Name = soupK.STATE.Name,
                                    IsActive = soupK.STATE.IsActive
                                },
                                RegionDto = new RegionDTO
                                {
                                    Id = soupK.REGION.Id,
                                    Name = soupK.REGION.Name
                                },
                                Id_Region = (int)soupK.Id_Region
                            };
                return query.FirstOrDefault();
            }
        }

        public void Deactivate(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                SOUP_KITCHEN sk = entities.SOUP_KITCHEN.Find(id);
                sk.IsActive = false;

                entities.SaveChanges();
            }
        }

        public void Activate(int id)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                SOUP_KITCHEN sk = entities.SOUP_KITCHEN.Find(id);
                sk.IsActive = true;

                entities.SaveChanges();
            }
        }

        public List<SkUserTypeDTOcs> GetUserTypeBySKId(int skId)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from soupK in entities.SOUP_KITCHEN_USER_TYPE
                            where soupK.Id_SoupKitchen == skId
                            select new SkUserTypeDTOcs
                            {
                                Id = soupK.Id,
                                Id_UserType = soupK.Id_UserType,
                                Id_SoupKitchen = soupK.Id_SoupKitchen,
                                UserTypeDto = new UserTypeDTO
                                {
                                    Id = soupK.USER_TYPE.Id,
                                    Description = soupK.USER_TYPE.Description,
                                    ApprovalOrder = soupK.USER_TYPE.ApprovalOrder
                                },
                                SoupKitchenDto = new SoupKitchenDTO
                                {
                                    Id = soupK.SOUP_KITCHEN.Id,
                                    Name = soupK.SOUP_KITCHEN.Name,
                                    Description = soupK.SOUP_KITCHEN.Description,
                                    Capacity = soupK.SOUP_KITCHEN.Capacity,
                                    Address = soupK.SOUP_KITCHEN.Address,
                                    PhoneNumber = soupK.SOUP_KITCHEN.PhoneNumber,
                                    ContactName = soupK.SOUP_KITCHEN.ContactName,
                                    IsActive = soupK.SOUP_KITCHEN.IsActive,
                                    Id_State = soupK.SOUP_KITCHEN.Id_State,
                                    AllowAnonym = soupK.SOUP_KITCHEN.AllowAnonym,
                                    Folio = soupK.SOUP_KITCHEN.Folio,
                                    State = new StateDTO
                                    {
                                        Id = soupK.SOUP_KITCHEN.STATE.Id,
                                        Name = soupK.SOUP_KITCHEN.STATE.Name,
                                        IsActive = soupK.SOUP_KITCHEN.STATE.IsActive
                                    },
                                    RegionDto = new RegionDTO
                                    {
                                        Id = soupK.SOUP_KITCHEN.REGION.Id,
                                        Name = soupK.SOUP_KITCHEN.REGION.Name
                                    },
                                    Id_Region = (int)soupK.SOUP_KITCHEN.Id_Region
                                }
                            };
                return query.ToList<SkUserTypeDTOcs>();
            }
        }

        public string Delete(int id)
        {
            string result = "";

            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    SOUP_KITCHEN_USER_TYPE att = db.SOUP_KITCHEN_USER_TYPE.FirstOrDefault(v => v.Id == id);
                    if (att != null)
                    {
                        db.SOUP_KITCHEN_USER_TYPE.Remove(att);
                        db.SaveChanges();
                        result = "SUCCESS";
                    }
                    else
                    {
                        result = "No se pudo eliminar el nivel de aprobación.";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "No se pudo eliminar el nivel de aprobación";
            }
            return result;
        }
    }
}
