using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class ConfigurationDTO
    {
        public int Id { get; set; }
        public string KeyName { get; set; }
        public string KeyDetail { get; set; }
        public string KeyValue { get; set; }
    }
}
