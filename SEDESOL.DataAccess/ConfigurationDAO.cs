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
    public class ConfigurationDAO
    {
        public List<ConfigurationDTO> GetConfiguration(string value)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                try
                {
                    var query = from config in entities.CONFIGURATIONs
                                where config.KeyName == value
                                select new ConfigurationDTO
                                {
                                    Id = config.Id,
                                    KeyName = config.KeyName,
                                    KeyDetail = config.KeyDetail,
                                    KeyValue = config.KeyValue
                                };

                    return query.ToList<ConfigurationDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
