using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;


namespace SEDESOL.BusinessLogic
{
    public class ConfigurationDAL
    {
        public List<ConfigurationDTO> GetConfiguration(string value)
        {
            ConfigurationDAO dao = new ConfigurationDAO();
            return dao.GetConfiguration(value);
        }
    }
}
