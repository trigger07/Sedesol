using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class CaptureDTO
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Descripción:")]
        public string Description { get; set; }

        [Display(Name = "Fecha:")]
        public System.DateTime CreateDate { get; set; }

        [Display(Name = "Activa:")]
        public bool IsActive { get; set; }

        [Display(Name = "Mes:")]
        public int Id_Month { get; set; }

        [Display(Name = "Año:")]
        public int Id_Year { get; set; }

        [Display(Name = "Estado:")]
        public int Id_Status { get; set; }


        [Display(Name = "Mes:")]
        [Required(ErrorMessage = "El campo Mes es requerido")]
        public MonthDTO Month { get; set; }

        [Display(Name = "Año:")]
        [Required(ErrorMessage = "El campo Año es requerido")]
        public YearDTO Year { get; set; }

        [Display(Name = "Estado:")]
        [Required(ErrorMessage = "El campo Estado es requerido")]
        public StatusDTO Status { get; set; }

        [Display(Name = "Comedor:")]
        public Nullable<int> Id_Soup_Kitchen { get; set; }


        [Display(Name = "Comedor:")]
        [Required(ErrorMessage = "El Comedor es Requerido")]
        public SoupKitchenDTO SoupKitchen { get; set; }

        //[Display(Name = "Mes:")]
        //[Required(ErrorMessage = "El campo Mes es requerido")]
        //public List<MonthDTO> Months { get; set; }

        //[Display(Name = "Año:")]
        //[Required(ErrorMessage = "El campo Año es requerido")]
        //public List<YearDTO> Years { get; set; }

        //[Display(Name = "Estado:")]
        //[Required(ErrorMessage = "El campo Estado es requerido")]
        //public List<StatusDTO> StatusList { get; set; }

        public List<AttendanceDTO> AttendanceList { get; set; }

        public List<CaptureImageDTO> ImageList { get; set; }
        public int? AttendanceCount { get; set; }

        public int? ReceiverCount { get; set; }

        public string Folio { get; set; }
        public string InspectionCode { get; set; }

        public string Message { get; set; }

        public int Id_LevelApproval { get; set; }

        public int Id_User { get; set; }

        public UserTypeDTO UserTypeDto { get; set; }
    }
}
