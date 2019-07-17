using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities;
using SEDESOL.DataModel;
using SEDESOL.DataEntities.DTO;
using System.Data.Entity;
using System.Data;
using SEDESOL.DataEntities.IntegrationObjects;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace SEDESOL.DataAccess
{
    public class RationDAO
    {
        public List<RationDTO> GetSearch(int pYear, int pMonth)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                try
                {
                    var query = from ration in entities.RATIONs
                                join coup in entities.COUPONs on ration.Id equals coup.Id_Ration
                                select new RationDTO
                                {
                                    Id = ration.Id,
                                    Description = ration.Description,
                                    CreateDate = ration.CreateDate,
                                    IsActive = ration.IsActive,
                                    RationQuantity = ration.RationQuantity,
                                    HasCoupon = ration.HasCoupon,
                                    MONTHDTO = new MonthDTO
                                    {
                                        Id = ration.MONTH.Id,
                                        Description = ration.MONTH.Description,
                                        IsActive = ration.MONTH.IsActive
                                    },
                                    YEARDTO = new YearDTO
                                    {
                                        Id = ration.YEAR.Id,
                                        Description = ration.YEAR.Description,
                                        IsActive = ration.YEAR.IsActive
                                    },
                                    SOUP_KITCHENDTO = new SoupKitchenDTO
                                    {
                                        Id = ration.SOUP_KITCHEN.Id,
                                        Name = ration.SOUP_KITCHEN.Name,
                                        Description = ration.SOUP_KITCHEN.Description,
                                        Capacity = ration.SOUP_KITCHEN.Capacity,
                                        Address = ration.SOUP_KITCHEN.Address,
                                        PhoneNumber = ration.SOUP_KITCHEN.PhoneNumber,
                                        ContactName = ration.SOUP_KITCHEN.ContactName,
                                        IsActive = ration.SOUP_KITCHEN.IsActive,
                                        AllowAnonym = ration.SOUP_KITCHEN.AllowAnonym,
                                        Folio = ration.SOUP_KITCHEN.Folio,
                                        State = new StateDTO
                                        {
                                            Id = ration.SOUP_KITCHEN.STATE.Id,
                                            Name = ration.SOUP_KITCHEN.STATE.Name
                                        }
                                    },
                                    Sequential = coup.Sequential
                                };
                    query = query.Where(e => e.YEARDTO.Id == pYear && e.MONTHDTO.Id == pMonth);

                    
                    return query.ToList<RationDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public string SaveDataRation(DataTable dt)
        {
            string result = "SUCCESS";

            using (SEDESOLEntities db = new SEDESOLEntities())
            {
                foreach (DataRow item in dt.Rows)
                {
                    try
                    {
                        string folio = item["Folio_Comedor"].ToString();
                        SOUP_KITCHEN sk = db.SOUP_KITCHEN.FirstOrDefault(x => x.Folio == folio);
                        if (sk != null)
                        {
                            int year = Convert.ToInt32(item["Year"].ToString());
                            int month = Convert.ToInt32(item["Month"].ToString());

                            // save ration table
                            RATION rat = new RATION();
                            rat.Description = "carga de excel";
                            rat.IsActive = true;
                            rat.Id_SoupKitchen = sk.Id;
                            rat.Id_Month = Convert.ToInt32(item["Month"].ToString());
                            rat.Id_Year = Convert.ToInt32(item["Year"].ToString());
                            rat.RationQuantity = Convert.ToDecimal(item["Raciones"].ToString());
                            rat.CreateUser = "fileUpload";
                            rat.CreateDate = DateTime.Now;
                            rat.HasCoupon = false;

                            db.RATIONs.Add(rat);
                            db.SaveChanges();

                            var coupList = db.COUPONs.Where(v => v.Id_Year == year && v.Id_Month == month && v.Id_SoupKitchen == sk.Id).ToList();
                            var yearDto = db.YEARs.FirstOrDefault(v => v.Id == year);

                            int count;
                            if (coupList != null)
                                count = coupList.Count() + 1;
                            else
                                count = 1;

                            string monthS = string.Empty;
                            //string yearS = string.Empty;

                            if (month.ToString().Count() == 1)
                                monthS = "0" + month.ToString();
                            else
                                monthS = month.ToString();

                            string sequential = sk.Folio + "-" + monthS + yearDto.Description.ToString().Substring(2, 2) + "-" + count.ToString();

                            COUPON cou = new COUPON();
                            cou.Description = "carga de excel";
                            cou.IsActive = true;
                            cou.Id_SoupKitchen = sk.Id;
                            cou.Id_Month = Convert.ToInt32(item["Month"].ToString());
                            cou.Id_Year = Convert.ToInt32(item["Year"].ToString());
                            cou.Id_Ration = rat.Id;
                            cou.Sequential = sequential;
                            cou.RationQuantity = Convert.ToDecimal(item["Raciones"].ToString());
                            cou.CreateUser = "fileUpload";
                            cou.CreateDate = DateTime.Now;

                            db.COUPONs.Add(cou);
                            db.SaveChanges();

                            #region Commented update option
                            //RATION rat = db.RATIONs.FirstOrDefault(v => v.Id_Year == year && v.Id_Month == month && v.Id_SoupKitchen == sk.Id);
                            //if (rat != null)
                            //{
                            //    rat.Description = "carga de excel";
                            //    rat.IsActive = true;
                            //    rat.Id_SoupKitchen = sk.Id;
                            //    rat.Id_Month = Convert.ToInt32(item["Month"].ToString());
                            //    rat.Id_Year = Convert.ToInt32(item["Year"].ToString());
                            //    rat.RationQuantity = Convert.ToDecimal(item["Raciones"].ToString());
                            //    rat.CreateUser = "fileUpload";
                            //    rat.CreateDate = DateTime.Now;
                            //    rat.HasCoupon = false;

                            //    db.SaveChanges();
                            //}
                            //else
                            //{
                            //    rat = new RATION();
                            //    rat.Description = "carga de excel";
                            //    rat.IsActive = true;
                            //    rat.Id_SoupKitchen = sk.Id;
                            //    rat.Id_Month = Convert.ToInt32(item["Month"].ToString());
                            //    rat.Id_Year = Convert.ToInt32(item["Year"].ToString());
                            //    rat.RationQuantity = Convert.ToDecimal(item["Raciones"].ToString());
                            //    rat.CreateUser = "fileUpload";
                            //    rat.CreateDate = DateTime.Now;
                            //    rat.HasCoupon = false;

                            //    db.RATIONs.Add(rat);

                            //    db.SaveChanges();
                            //}

                            //COUPON cou = db.COUPONs.FirstOrDefault(v => v.Id_Year == year && v.Id_Month == month && v.Id_SoupKitchen == sk.Id);
                            //COUPON cou = db.COUPONs.FirstOrDefault(v => v.Id_Ration == rat.Id);
                            //if (cou != null)
                            //{
                            //    cou.Description = "carga de excel";
                            //    cou.IsActive = true;
                            //    cou.Id_SoupKitchen = sk.Id;
                            //    cou.Id_Month = Convert.ToInt32(item["Month"].ToString());
                            //    cou.Id_Year = Convert.ToInt32(item["Year"].ToString());
                            //    cou.Id_Ration = rat.Id;
                            //    cou.RationQuantity = Convert.ToDecimal(item["Raciones"].ToString());
                            //    cou.CreateUser = "fileUpload";
                            //    cou.CreateDate = DateTime.Now;
                            //    //cou.HasCoupon = false;

                            //    db.SaveChanges();
                            //}
                            //else
                            //{
                            //    cou = new COUPON();
                            //    cou.Description = "carga de excel";
                            //    cou.IsActive = true;
                            //    cou.Id_SoupKitchen = sk.Id;
                            //    cou.Id_Month = Convert.ToInt32(item["Month"].ToString());
                            //    cou.Id_Year = Convert.ToInt32(item["Year"].ToString());
                            //    cou.Id_Ration = rat.Id;
                            //    cou.RationQuantity = Convert.ToDecimal(item["Raciones"].ToString());
                            //    cou.CreateUser = "fileUpload";
                            //    cou.CreateDate = DateTime.Now;

                            //    db.COUPONs.Add(cou);

                            //    db.SaveChanges();
                            //}
                            #endregion
                        }
                        
                    }
                    catch(Exception ex)
                    {
                       return ex.Message;
                    }
                }
            }
            return result;

        }

        public string SaveCoupon()
        {
            return "";
        }

        public List<RationDTO> GetRationsByState(int pIdState, int pIdYear, int pIdMonth)
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                try
                {
                    var query = from ration in entities.RATIONs
                                where ration.Id_Year == pIdYear && ration.Id_Month == pIdMonth && ration.SOUP_KITCHEN.Id_State == pIdState
                                select new RationDTO
                                {
                                    Id = ration.Id,
                                    Description = ration.Description,
                                    CreateDate = ration.CreateDate,
                                    IsActive = ration.IsActive,
                                    RationQuantity = ration.RationQuantity,
                                    HasCoupon = ration.HasCoupon
                                };
                    
                    return query.ToList<RationDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public CouponModel GetCouponData(int pIdRation)
        {
            CouponModel coupon = new CouponModel();
            List<ProductDTO> listProd = new List<ProductDTO>();

            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var queryRation = from ration in entities.RATIONs
                                  join coup in entities.COUPONs on ration.Id equals coup.Id_Ration
                                  where ration.Id == pIdRation
                                  select new CouponModel
                                  {
                                      //Id = ration.Id,
                                      //Id_Month = ration.Id_Month,
                                      //Id_Year = ration.Id_Year,
                                      //Id_SoupKitchen = ration.Id_SoupKitchen
                                      Id_Coupon = ration.Id.ToString(),
                                      Id_Region = (int)ration.SOUP_KITCHEN.Id_Region,
                                      SoupKitchen = ration.SOUP_KITCHEN.Name,
                                      Region = ration.SOUP_KITCHEN.REGION.Name,
                                      Month = ration.MONTH.Description,
                                      Year = ration.YEAR.Description,
                                      RationQuantity = ration.RationQuantity,
                                      Folio = ration.SOUP_KITCHEN.Folio,
                                      Address = ration.SOUP_KITCHEN.Address,
                                      Description = ration.SOUP_KITCHEN.Description,
                                      Contact = ration.SOUP_KITCHEN.ContactName,
                                      Coupon_Folio = coup.Sequential
                                  };
                coupon = queryRation.FirstOrDefault();

                var queryProduct = from cou in entities.COUPONs
                                   join prod in entities.COUPON_DETAIL on cou.Id equals prod.Id_Coupon
                                   where cou.Id_Ration == pIdRation
                                   select new ProductDTO
                                   {
                                       Id = prod.PRODUCT.Id,
                                       Description = prod.PRODUCT.Description,
                                       Measure = prod.PRODUCT.Measure,
                                       UnitMeasure = prod.PRODUCT.UnitMeasure,
                                       Momio = prod.PRODUCT.Momio,
                                       QuantityCoupon = (decimal)prod.AskedQuantity
                                   };

                //var queryProduct = from prod in entities.REGION_PRODUCT
                //                   where prod.Id_Region == coupon.Id_Region
                //                   select new ProductDTO
                //                   {
                //                       Id = prod.PRODUCT.Id,
                //                       Description = prod.PRODUCT.Description,
                //                       Measure = prod.PRODUCT.Measure,
                //                       UnitMeasure = prod.PRODUCT.UnitMeasure,
                //                       Momio = prod.PRODUCT.Momio
                //                   };

                listProd = queryProduct.ToList<ProductDTO>();

                foreach (var item in listProd)
                {
                    if (item.QuantityCoupon < 1)
                        item.QuantityCoupon = 1;
                    item.QuantityCoupon = Math.Round(item.QuantityCoupon);
                }

                coupon.ListProduct = listProd;

                return coupon;
            }
        }

        public DataSet GetExcel(int pIdState, int pIdMonth, int pIdYear)
        {
            DataSet ds = new DataSet("Excel_Consolidado");

            var efConnectionString = ConfigurationManager.ConnectionStrings["SEDESOLEntities"].ConnectionString;
            var builder = new EntityConnectionStringBuilder(efConnectionString);
            var regularConnectionString = builder.ProviderConnectionString;

            using (SqlConnection conn = new SqlConnection(regularConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand("sp_GetDataForExcel", conn);
                sqlComm.Parameters.AddWithValue("@StateId", pIdState);
                sqlComm.Parameters.AddWithValue("@MonthId", pIdMonth);
                sqlComm.Parameters.AddWithValue("@YearId", pIdYear);

                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }

            return ds;
        }
    }
}
