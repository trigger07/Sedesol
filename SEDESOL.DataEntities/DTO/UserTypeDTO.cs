using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class UserTypeDTO
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Estado")]
        public bool IsActive { get; set; }

        public int ApprovalOrder { get; set; }
    }
}
