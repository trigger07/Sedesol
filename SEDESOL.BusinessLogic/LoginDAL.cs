using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;
using System.Data;
using SEDESOL.DataEntities.IntegrationObjects;

namespace SEDESOL.BusinessLogic
{
    public class LoginDAL
    {
        public UserDTO ValidateUser(UserValidationRequest req)
        {
            LoginDAO dao = new LoginDAO();
            return dao.GetUser(req);
        }
    }
}
