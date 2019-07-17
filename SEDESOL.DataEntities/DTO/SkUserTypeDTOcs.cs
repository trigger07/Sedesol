using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class SkUserTypeDTOcs
    {
        public int Id { get; set; }
        public int Id_SoupKitchen { get; set; }
        public int Id_UserType { get; set; }
        public bool IsActive { get; set; }

        public SoupKitchenDTO SoupKitchenDto { get; set; }
        public UserTypeDTO UserTypeDto { get; set; }
        public string Message { get; set; }

    }
}
