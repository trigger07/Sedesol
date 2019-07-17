using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class UserDTO
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "DNI")]
        public string Dni { get; set; }

        [Display(Name = "Estado")]
        public bool IsActive { get; set; }

        [Display(Name = "Tipo")]
        public int Id_User_Type { get; set; }

        [Display(Name = "Comedor:")]
        [Required(ErrorMessage = "El Comedor es Requerido")]
        public List<SoupKitchenDTO> SoupKitchenList { get; set; }

        [Display(Name = "Tipo:")]
        [Required(ErrorMessage = "El Tipo de Usuario es Requerido")]
        public UserTypeDTO UserType { get; set; }

        public string Message { get; set; }
    }
}
