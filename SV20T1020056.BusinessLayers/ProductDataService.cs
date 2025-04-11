using Azure;
using SV20T1020056.DataLayers;
using SV20T1020056.DataLayers.SQLServer;
using SV20T1020056.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020056.BusinessLayers
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;

        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }
        /// <summary>
        /// Products
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(string searchValue = "")
        {
            
            return productDB.List().ToList();
        }
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "",int categoryId =0, int supplierID =0, decimal minPrice =0, decimal maxPrice =0)
        {
            rowCount= productDB.Count(searchValue, categoryId, supplierID, minPrice, maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryId, supplierID, minPrice, maxPrice).ToList();
        }
        public static Product GetProduct(int productID)
        {
            return productDB.Get(productID);
        }
        public static bool InUsedProduct(int productID)
        {
            return productDB.InUsed(productID);
        }
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }
        public static bool DeleteProducts(int productID)
        {
            return productDB.Delete(productID);
        }
        public static bool UpdateProducts(Product data)
        {
            return productDB.Update(data);
        }
        ////Photo
        ///
        public static List<ProductPhoto> ListPhotos(int productId)
        {
            return productDB.ListPhotos(productId).ToList();
        }
        public static ProductPhoto? GetPhoto(int productID)
        {
            return productDB.GetPhoto(productID);
        }

        public static long AddPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }
        public static bool DeletePhoto(long photoID)
        {
            return productDB.DeletePhoto(photoID);
        }
        public static bool UpdatePhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }
        ///AttributeID
        public static List<ProductAttribute> ListAttributes(int productId)
        {
            return productDB.ListAttributes(productId);
        }
        public static ProductAttribute? GetAttribute(int productID)
        {
            return productDB.GetAttribute(productID);
        }

        public static long AddAtribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }
        public static bool DeleteAttribute(long atributeID)
        {
            return productDB.DeleteAttribute(atributeID);
        }
        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }
        /// 
        

    }
}
