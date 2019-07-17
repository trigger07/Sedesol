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
    public class AttendanceDAO
    {
        public AttendanceDTO Save(AttendanceDTO att)
        {
            using(SEDESOLEntities db = new SEDESOLEntities())
            {

                ATTENDANCE attendance = db.ATTENDANCEs.FirstOrDefault(v => v.Id == att.Id);
                if (attendance != null)
                {

                    ATTENDANCE attendance2 = db.ATTENDANCEs.FirstOrDefault(v => v.Curp.Trim() == att.Curp.Trim() && v.Id_Capture == att.Id_Capture && v.Id != att.Id
                                                && v.IsAnonym == false);
                    if (attendance2 != null && att.HasCurp == true)
                    {

                        att.Message = "Se ha ingresado previamente una asistencia.";
                    }
                    else
                    {
                        ATTENDANCE attendance3 = db.ATTENDANCEs.FirstOrDefault(v => v.Name.Trim().ToUpper() == att.Name.Trim().ToUpper() && v.LastName.Trim().ToUpper() == att.LastName.Trim().ToUpper()
                                && v.LastName2.Trim().ToUpper() == att.LastName2.Trim().ToUpper() && v.Id_Capture == att.Id_Capture && v.Id != att.Id);
                        if (attendance3 != null && att.IsAnonym == false)
                        {

                            att.Message = "Se ha ingresado previamente una asistencia.";
                        }
                        else
                        {
                            attendance.Id_Capture = att.Id_Capture;
                            attendance.Birthdate = att.Birthdate;
                            attendance.CreateDate = att.CreateDate;
                            attendance.Curp = att.Curp.Trim();
                            attendance.IsActive = att.IsActive;
                            attendance.LastName = att.LastName.Trim();
                            attendance.LastName2 = att.LastName2.Trim();
                            attendance.Quantity = att.Quantity;
                            attendance.Name = att.Name.Trim();
                            attendance.IsAnonym = att.IsAnonym;
                            attendance.Gender = att.Gender;
                            attendance.Id_Condition = att.Id_Condition;
                            attendance.Id_State_Birth = att.Id_State_Birth;
                            attendance.Address = att.Address;
                            attendance.PhoneNumber = att.PhoneNumber;
                            if (db.SaveChanges() > 0)
                            {
                                att.Id = attendance.Id;
                                att.Message = "SUCCESS";
                            }
                        }
                        
                    } 
                }
                else
                {
                    ATTENDANCE attendance2 = db.ATTENDANCEs.FirstOrDefault(v => v.Curp.Trim() == att.Curp.Trim() && v.Id_Capture == att.Id_Capture && v.IsAnonym == false);
                    if (attendance2 != null && att.HasCurp == true)
                    {

                        att.Message = "Se ha ingresado previamente una asistencia.";
                    }
                    else
                    {
                        ATTENDANCE attendance3 = db.ATTENDANCEs.FirstOrDefault(v => v.Name.Trim().ToUpper() == att.Name.Trim().ToUpper() && v.LastName.Trim().ToUpper() == att.LastName.Trim().ToUpper()
                                && v.LastName2.Trim().ToUpper() == att.LastName2.Trim().ToUpper() && v.Id_Capture == att.Id_Capture && v.Id != att.Id);
                        if (attendance3 != null && att.IsAnonym == false)
                        {

                            att.Message = "Se ha ingresado previamente una asistencia.";
                        }
                        else
                        {
                            attendance = new ATTENDANCE();
                            attendance.Id_Capture = att.Id_Capture;
                            attendance.Birthdate = att.Birthdate;
                            attendance.CreateDate = att.CreateDate;
                            attendance.Curp = att.Curp.Trim();
                            attendance.IsActive = att.IsActive;
                            attendance.LastName = att.LastName.Trim();
                            attendance.LastName2 = att.LastName2.Trim();
                            attendance.Quantity = att.Quantity;
                            attendance.Name = att.Name.Trim();
                            attendance.IsAnonym = att.IsAnonym;
                            attendance.Gender = att.Gender;
                            attendance.Id_Condition = att.Id_Condition;
                            attendance.Id_State_Birth = att.Id_State_Birth;
                            attendance.Address = att.Address;
                            attendance.PhoneNumber = att.PhoneNumber;

                            db.ATTENDANCEs.Add(attendance);

                            if (db.SaveChanges() > 0)
                            {
                                att.Id = attendance.Id;
                                att.Message = "SUCCESS";
                            }
                        }
                        
                    }
                    
                }
                return att;
            }
        }

        public AttendanceDTO GetAttendanceById(int id)
        {
            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                var query = db.ATTENDANCEs.Where(f => f.Id == id).Select(p =>
                             new AttendanceDTO
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 LastName = p.LastName,
                                 LastName2 = p.LastName2,
                                 Curp = p.Curp,
                                 Birthdate = p.Birthdate,
                                 CreateDate = p.CreateDate,
                                 Quantity = p.Quantity,
                                 Id_Capture = p.Id_Capture,
                                 Id_Receiver = p.Id_Receiver,
                                 IsActive = p.IsActive,
                                 IsAnonym = p.IsAnonym,
                                 Gender = p.Gender,
                                 Id_Condition = p.Id_Condition,
                                 Id_State_Birth = p.Id_State_Birth,
                                 Address = p.Address,
                                 PhoneNumber = p.PhoneNumber
                             });
                return query.FirstOrDefault();
            }
        }

        public string Delete(int id)
        {
            string result = "";

            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    ATTENDANCE att = db.ATTENDANCEs.FirstOrDefault(v => v.Id == id);
                    if (att != null)
                    {
                        db.ATTENDANCEs.Remove(att);
                        db.SaveChanges();
                        result = "SUCCESS";
                    }
                    else
                    {
                        result = "No se pudo eliminar la asistencia.";
                    }
                }
            }
            catch(Exception ex)
            {
                result = "No se pudo eliminar la asistencia";
            }
            return result;
        }

        public List<AttendanceDTO> GetDataForAc(string term)
        {
            try
            {
                List<AttendanceDTO> listAtt = new List<AttendanceDTO>();

                var date = DateTime.Now.AddDays(-90);
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    var query = db.ATTENDANCEs.Where(f => f.CreateDate > date && (f.Name.Contains(term) || f.LastName.Contains(term) || f.Curp.Contains(term))).Select(p =>
                                 new AttendanceDTO
                                 {
                                     Name = p.Name,
                                     LastName = p.LastName,
                                     LastName2 = p.LastName2,
                                     Curp = p.Curp,
                                     Birthdate = p.Birthdate,
                                     Gender = p.Gender,
                                     Id_Condition = p.Id_Condition,
                                     Id_State_Birth = p.Id_State_Birth,
                                     Address = p.Address,
                                     PhoneNumber = p.PhoneNumber
                                    
                                 }).Distinct().ToList();

                    return query.ToList<AttendanceDTO>();

                    //List<ATTENDANCE> query = db.ATTENDANCEs.Where(f => f.CreateDate > date && (f.Name.Contains(term) || f.LastName.Contains(term) || f.Curp.Contains(term))).GroupBy(p => new
                    //{
                    //    p.Name,
                    //    p.LastName,
                    //    p.Curp,
                    //    p.Birthdate
                    //}).ToList().Select(g => g.FirstOrDefault()).ToList();

                    //foreach(var item in query)
                    //{
                    //    AttendanceDTO dto = new AttendanceDTO();
                    //    dto.Name = item.Name;
                    //    dto.LastName = item.LastName;
                    //    dto.Curp = item.Curp;
                    //    dto.Birthdate = item.Birthdate;
                    //    listAtt.Add(dto);
                    //}

                    //return listAtt;

                    //var query = db.ATTENDANCEs.Where(f => f.CreateDate > date && (f.Name.Contains(term) || f.LastName.Contains(term) || f.Curp.Contains(term))).Select(p =>
                    //        new AttendanceDTO
                    //        {
                    //            Id = p.Id,
                    //            Name = p.Name,
                    //            LastName = p.LastName,
                    //            Curp = p.Curp,
                    //            Birthdate = p.Birthdate
                    //        });

                    //var query = from att in db.ATTENDANCEs
                    //            where att.CreateDate > date
                    //            select new AttendanceDTO
                    //            {
                    //                Id = att.Id,
                    //                Name = att.Name,
                    //                LastName = att.LastName,
                    //                Curp = att.Curp,
                    //                Birthdate = att.Birthdate
                    //            };
                    //listAtt = query.ToList<AttendanceDTO>();

                    //return listAtt;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
