using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class ReceiverDTO
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "Curp")]
        public string Curp { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public Nullable<System.DateTime> Birthday { get; set; }

        public System.DateTime CreateDate { get; set; }

        public List<AttendanceDTO> Attendances { get; set; }
    }
}
