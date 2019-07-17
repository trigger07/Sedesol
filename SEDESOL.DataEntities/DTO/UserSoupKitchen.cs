using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class UserSoupKitchen
    {
        public int Id { get; set; }
        public int Id_User { get; set; }
        public int Id_Soup_Kitchen { get; set; }
        public bool IsActive { get; set; }

        public string Message { get; set; }
    }
}
