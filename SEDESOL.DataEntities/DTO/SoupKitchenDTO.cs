using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class SoupKitchenDTO
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Capacidad")]
        public Nullable<int> Capacity { get; set; }

        [Display(Name = "Dirección")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Contacto")]
        public string ContactName { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        [Display(Name = "Estado")]
        public int Id_State { get; set; }

        public List<AttendanceDTO> Attendance { get; set; }

        public StateDTO State { get; set; }

        public string Message { get; set; }

        public bool AllowAnonym { get; set; }
        public string Folio { get; set; }

        public RegionDTO RegionDto { get; set; }

        public int Id_Region { get; set; }
        //public virtual ICollection<USER_SOUP_KITCHEN> USER_SOUP_KITCHEN { get; set; }
    }
}
