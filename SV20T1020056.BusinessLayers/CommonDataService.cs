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
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Province> pronvinceDB;
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        private static readonly ICommonDAL<Category> categoryDB;

        static CommonDataService()
        {
            string connectionString = Configuration.ConnectionString;
            pronvinceDB = new ProvinceDAL(connectionString);
            customerDB = new CustomerDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
            shipperDB = new ShipperDAL(connectionString);
            employeeDB = new EmployeeDAL(connectionString);
            categoryDB = new CategoryDAL(connectionString);
        }
        /// <summary>
        /// Danh sach tinh thanh
        /// </summary>
        /// <returns></returns>
        public static List<Province> ListOfProvinces()
        {
            return pronvinceDB.List().ToList();
        }


        /// Customer
        /// Tìm kiếm danh sách khách hàng
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount= customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        /// Lấy thông tin của 1 khách hàng
        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }
        /// Bổ sung khách hàng mới
        public static int AddCustomer(Customer customer)
        {
            return customerDB.Add(customer);
        }
        /// Cập nhật khách hàng
        public static bool UpdateCustomer(Customer customer)
        {
            return customerDB.Update(customer);
        }
        /// Xóa khách hàng có mã là id
        public static bool DeleteCustomer(int id)
        {
            if (customerDB.IsUsed(id))
            {
                return false;
            }
            return customerDB.Delete(id);
        }
        /// Kiểm tra xem khách hàng có mã id hiện có dữ liệu liên quan hay không
        public static bool IsUsedCustomer(int id)
        {
            return customerDB.IsUsed(id);
        }

        /// Supplier
        /// Tìm kiếm và lấy danh sách nhà cung cấp
        public static List<Supplier> ListOfSupplier(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount= supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }
        /// Lấy thông tin của 1 nhà cung cấp theo mã nhà cung cấp
        public static Supplier? GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }
        /// Bổ sung nhà cung cấp
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }
        /// Cập nhật nhà cung cấp
        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }
        /// Xóa nhà cung cấp có mã là id

        public static bool DeleteSupplier(int id)
        {
            if (supplierDB.IsUsed(id))
            {
                return false;
            }
            return supplierDB.Delete(id);
        }
        /// Kiểm tra xem nhà cung cấp có mã id hiện có dữ liệu liên quan hay không
        public static bool IsUsedSupplier(int id)
        {
            return supplierDB.IsUsed(id);
        }

        ///Shipper
        /// Tìm kiếm và lấy danh sách người giao hàng
        public static List<Shipper> ListOfShipper(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount= shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }
        /// Lấy thông tin của 1 nhà cung cấp theo mã người giao hàng
        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }
        /// Bổ sung người giao hàng
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }
        /// Cập nhật người giao hàng
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }
        /// Xóa người giao hàng
        public static bool DeleteShipper(int id)
        {
            if (shipperDB.IsUsed(id))
            {
                return false;
            }
            return shipperDB.Delete(id);
        }
        /// Kiểm tra xem người giao hàng có mã id hiện có dữ liệu liên quan hay không
        public static bool IsUsedShipper(int id)
        {
            return shipperDB.IsUsed(id);
        }


        ///Employee
        /// Tìm kiếm và lấy danh sách nhân viên
        public static List<Employee> ListOfEmployee(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount= employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }
        /// Lấy thông tin của 1 nhân viên theo mã nhân viên
        public static Employee? GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }
        /// Bổ sung nhân viên
        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }
        /// Cập nhật nhân viên
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }
        /// Xóa nhân viên
        public static bool DeleteEmployee(int id)
        {
            if (employeeDB.IsUsed(id))
            {
                return false;
            }
            return employeeDB.Delete(id);
        }
        /// Kiểm tra xem nhân viên có mã id hiện có dữ liệu liên quan hay không
        public static bool IsUsedEmployee(int id)
        {
            return employeeDB.IsUsed(id);
        }

        ///Category
        /// Tìm kiếm và lấy danh sách loại hàng
        public static List<Category> ListOfCategory(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount= categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }
        /// Lấy thông tin của 1 loại hàng theo mã loại hàng
        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }
        /// bổ sung thông tin loại hàng
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }
        /// Cập nhật thông tin loại hàng
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }
        /// Xóa thông loại hàng
        public static bool DeleteCategory(int id)
        {
            if (categoryDB.IsUsed(id))
            {
                return false;
            }
            return categoryDB.Delete(id);
        }
        /// Kiểm tra xem loại hàng có mã id hiện có dữ liệu liên quan hay không
        public static bool IsUsedCategory(int id)
        {
            return categoryDB.IsUsed(id);
        }

        public static List<Category> ListOfCategory()
        {
            return categoryDB.List().ToList();
        }

        public static List<Supplier> ListOfSupplier()
        {
            return supplierDB.List().ToList();
        }
    }
}
