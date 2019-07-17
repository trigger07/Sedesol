using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class StateDTO
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Código")]
        public string Abbreviation { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }


        public List<SoupKitchenDTO> SoupKitchenList { get; set; }
    }
}
