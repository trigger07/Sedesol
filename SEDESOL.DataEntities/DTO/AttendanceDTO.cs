using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class AttendanceDTO
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }

        [Display(Name = "Apellido Paterno")]
        [Required(ErrorMessage = "El campo Apellido Paterno es requerido")]
        public string LastName { get; set; }

        [Display(Name = "Curp")]
        [Required(ErrorMessage = "El campo Curp es requerido")]
        public string Curp { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "El campo es requerido")]
        public Nullable<System.DateTime> Birthdate { get; set; }

        public Nullable<System.DateTime> CreateDate { get; set; }

        [Display(Name = "Raciones")]
        public Nullable<int> Quantity { get; set; }

        public Nullable<int> Id_Capture { get; set; }
        
        public Nullable<int> Id_Receiver { get; set; }

        [Display(Name = "Activa")]
        public bool IsActive { get; set; }

        public CaptureDTO Capture { get; set; }
        public ReceiverDTO Receiver { get; set; }

        public string Message { get; set; }

        public bool IsAnonym { get; set; }

        [Display(Name = "Sexo")]
        [Required(ErrorMessage = "El campo Sexo es requerido")]
        public string Gender { get; set; }

        [Display(Name = "Condición")]
        [Required(ErrorMessage = "El campo Condición es requerido")]
        public Nullable<int> Id_Condition { get; set; }

        [Display(Name = "Lugar Nacimiento")]
        [Required(ErrorMessage = "El campo Lugar Nacimiento es requerido")]
        public Nullable<int> Id_State_Birth { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Domicilio")]
        [Required(ErrorMessage = "El campo Condición es requerido")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo Teléfono es requerido")]
        public string PhoneNumber { get; set; }

        public ConditionDTO ConditionDto { get; set; }

        public StateDTO StateDto { get; set; }

        [Display(Name = "Apellido Materno")]
        [Required(ErrorMessage = "El campo Apellido Materno es requerido")]
        public string LastName2 { get; set; }

        public bool HasCurp { get; set; }
    }
}
