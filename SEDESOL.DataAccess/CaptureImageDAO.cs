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
    public class CaptureImageDAO
    {
        public int Save(CaptureImageDTO dto)
        {
            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    byte[] imgBytes;
                    if (dto.FromCam)
                    {
                        imgBytes = Convert.FromBase64String(dto.ImageFileB64);
                    }
                    else
                    {
                        imgBytes = dto.ImageFile;
                    }
                    CAPTURE_IMAGE captureEntity = new CAPTURE_IMAGE
                    {
                        Name = dto.Name,
                        ImageFile = imgBytes,
                        ImagePath = dto.ImagePath,
                        Id_Capture = dto.Id_Capture
                    };

                    db.CAPTURE_IMAGE.Add(captureEntity);
                    if (db.SaveChanges() > 0)
                    {
                        dto.Id = captureEntity.Id;

                        return dto.Id;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public string Delete(int id)
        {
            string result = "";

            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    CAPTURE_IMAGE att = db.CAPTURE_IMAGE.FirstOrDefault(v => v.Id == id);
                    if (att != null)
                    {
                        db.CAPTURE_IMAGE.Remove(att);
                        db.SaveChanges();
                        
                        result = "SUCCESS";
                    }
                    else
                    {
                        result = "No se pudo eliminar el archivo.";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "No se pudo eliminar el archivo";
            }
            return result;
        }

        public List<CaptureImageDTO> GetImageByCaptureId(int CaptureId)
        {
            List<CaptureImageDTO> list = new List<CaptureImageDTO>();
            try
            {
                using (SEDESOLEntities db = new SEDESOLEntities())
                {
                    var query = from img in db.CAPTURE_IMAGE
                                where img.Id_Capture == CaptureId
                                select new CaptureImageDTO
                                {
                                    Id = img.Id,
                                    Id_Capture = img.Id_Capture,
                                    ImageFile = img.ImageFile,
                                    ImageFileB64 = string.Empty,
                                    Name = string.Empty
                                };
                    return query.ToList<CaptureImageDTO>();
                }
            }
            catch(Exception ex)
            {
                return new List<CaptureImageDTO>();
            }
        }
    }
}
