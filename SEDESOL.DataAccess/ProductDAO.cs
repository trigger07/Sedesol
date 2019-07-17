using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities;
using SEDESOL.DataModel;
using SEDESOL.DataEntities.DTO;
using System.Data.Entity.Validation;

namespace SEDESOL.DataAccess
{
    public class ProductDAO
    {
        public List<ProductDTO> GetProductAll()
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from prod in entities.PRODUCTs

                            select new ProductDTO
                            {
                                Id = prod.Id,
                                Description = prod.Description,
                                Measure = prod.Measure,
                                UnitMeasure = prod.UnitMeasure,
                                Momio = prod.Momio,
                                Price = prod.Price,
                                IsActive = prod.IsActive
                            };
                return query.ToList<ProductDTO>();
            }
        }

        public ProductDTO GetProductById(int IdProduct)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from prod in entities.PRODUCTs
                            //join regProd in entities.REGION_PRODUCT on prod.Id equals regProd.Id_Product
                            //join region in entities.REGIONs on regProd.Id_Region equals region.Id
                            where prod.Id == IdProduct
                            select new ProductDTO
                            {
                                Id = prod.Id,
                                Description = prod.Description,
                                Measure = prod.Measure,
                                UnitMeasure = prod.UnitMeasure,
                                Momio = prod.Momio,
                                Price = prod.Price,
                                IsActive = prod.IsActive,
                                ListRegion = prod.REGION_PRODUCT.Where(x => x.IsActive == true).Select(a => new RegionDTO
                                {
                                    Id = a.REGION.Id,
                                    Name = a.REGION.Name,
                                    Description = a.REGION.Description,
                                    IsActive= a.REGION.IsActive
                                }).ToList<RegionDTO>()
                            };

                var productQuery = query.FirstOrDefault();

                var queryRegion = from region in entities.REGIONs
                            where region.IsActive == true
                            select new RegionDTO
                            {
                                Id = region.Id,
                                Name = region.Name,
                                Description = region.Description,
                                IsActive = region.IsActive
                            };
                var listRegion = queryRegion.ToList<RegionDTO>();

                foreach (var item in listRegion)
                {
                    if(productQuery.ListRegion.Any(x => x.Id == item.Id))
                    {
                        item.IsAdded = true;
                    }
                    else
                    {
                        item.IsAdded = false;
                    }
                }

                productQuery.ListRegion = listRegion;
                return productQuery;

            }
        }

        public ProductDTO Save(ProductDTO prodDto)
        {
            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //guardar producto
                        PRODUCT product = db.PRODUCTs.FirstOrDefault(v => v.Id == prodDto.Id);
                        if (product != null)
                        {
                            product.Description = prodDto.Description;
                            product.Measure = prodDto.Measure;
                            product.UnitMeasure = prodDto.UnitMeasure;
                            product.Momio = prodDto.Momio;
                            product.Price = 0;
                            product.IsActive = prodDto.IsActive;
                            product.EditUser = prodDto.EditUser;
                            product.EditDate = prodDto.EditDate;

                            if (db.SaveChanges() > 0)
                            {
                                prodDto.Id = product.Id;
                                prodDto.Message = "SUCCESS";
                            }
                            else
                            {
                                transaction.Rollback();
                                prodDto.Message = "Ha ocurriddo un error al guardar el producto.";
                                return prodDto;
                            }
                        }
                        else
                        {
                            product = new PRODUCT();
                            product.Description = prodDto.Description;
                            product.Measure = prodDto.Measure;
                            product.UnitMeasure = prodDto.UnitMeasure;
                            product.Momio = prodDto.Momio;
                            product.Price = 0;
                            product.IsActive = prodDto.IsActive;
                            product.CreateUser = prodDto.CreateUser;
                            product.CreateDate = prodDto.CreateDate;

                            db.PRODUCTs.Add(product);

                            if (db.SaveChanges() > 0)
                            {
                                prodDto.Id = product.Id;
                                prodDto.Message = "SUCCESS";
                            }
                            else
                            {
                                prodDto.Message = "Ha ocurriddo un error al guardar el producto.";
                                transaction.Rollback();
                                return prodDto;
                            }
                        }

                        //guardar regiones del producto
                        foreach (var item in prodDto.ListRegion)
                        {
                            //obtener si previamente ha sido guardada
                            REGION_PRODUCT regionProd = db.REGION_PRODUCT.FirstOrDefault(v => v.Id_Product == prodDto.Id && v.Id_Region == item.Id);
                            if(regionProd != null)
                            {
                                regionProd.Id_Region = item.Id;
                                regionProd.Id_Product = prodDto.Id;
                                regionProd.IsActive = item.IsActive;

                                db.SaveChanges();
                                item.Id = regionProd.Id;
                                prodDto.Message = "SUCCESS";
                                //if (db.SaveChanges() > 0)
                                //{
                                //    item.Id = regionProd.Id;
                                //    prodDto.Message = "SUCCESS";
                                //}
                                //else
                                //{
                                //    prodDto.Message = "Ha ocurriddo un error al asociar la region al producto.";
                                //    transaction.Rollback();
                                //    return prodDto;
                                //}
                            }
                            else
                            {
                                regionProd = new REGION_PRODUCT();
                                regionProd.Id_Region = item.Id;
                                regionProd.Id_Product = prodDto.Id;
                                regionProd.IsActive = item.IsActive;

                                db.REGION_PRODUCT.Add(regionProd);

                                db.SaveChanges();
                                item.Id = regionProd.Id;
                                prodDto.Message = "SUCCESS";

                                //if (db.SaveChanges() > 0)
                                //{
                                //    item.Id = regionProd.Id;
                                //    prodDto.Message = "SUCCESS";
                                //}
                                //else
                                //{
                                //    prodDto.Message = "Ha ocurriddo un error al asociar la region al producto.";
                                //    transaction.Rollback();
                                //    return prodDto;
                                //}
                            }
                        }
                        transaction.Commit();
                        return prodDto;
                    }
                    //catch (DbEntityValidationException e)
                    //{
                    //    foreach (var eve in e.EntityValidationErrors)
                    //    {
                    //        //registrar errores en log
                    //    }
                    //    throw;
                    //}
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        prodDto.Message = "Ha ocurrido un error al guardar el producto.";
                        return prodDto;
                    }
                }
            }
        }
    }
}
