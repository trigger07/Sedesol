using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;

namespace SEDESOL.BusinessLogic
{
    public class ProductDAL
    {
        public List<ProductDTO> GetProductAll()
        {
            ProductDAO b = new ProductDAO();
            return b.GetProductAll();
        }

        public ProductDTO GetProductById(int id)
        {
            ProductDAO b = new ProductDAO();
            return b.GetProductById(id);
        }

        public ProductDTO Save(ProductDTO prodDto)
        {
            ProductDAO dao = new ProductDAO();
            return dao.Save(prodDto);
        }
    }
}
