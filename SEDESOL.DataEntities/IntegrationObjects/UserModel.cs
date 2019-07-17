using SEDESOL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.IntegrationObjects
{
    public class UserModel
    {
        public UserDTO userDTO { get; set; }

        public List<SoupKitchenDTO> ListSKToAssign { get; set; }

        public List<SoupKitchenDTO> ListSKAssigned { get; set; }

        public List<StateDTO> StateDTO { get; set; }

        public List<UserTypeDTO> UserTypeDTO { get; set; }
    }
}
