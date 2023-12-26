using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SV20T1080003.DataLayers.SQLServer
{
    public class _BaseDAL
    {
        protected string _connectionString;
        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="connectionString"></param>
        public _BaseDAL(string connectionString)
        {
            this._connectionString = connectionString;
        }
        protected SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _connectionString;
            conn.Open();
            return conn;
        }
    }
}
