using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SV20T1020056.DataLayers.SQLServer
{
    public abstract class _BaseDAL
    {
        protected string _connectionString = "";
        /// <summary>
        /// 
        /// </summary>
        public _BaseDAL(string connectionString)
        {
            _connectionString = connectionString;

        }
        /// <summary>
        /// Tạo và mở kết nối Sql
        /// </summary>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;

        }
    }
}
