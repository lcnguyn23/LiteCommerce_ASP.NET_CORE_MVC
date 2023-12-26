using Dapper;
using SV20T1080003.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;

namespace SV20T1080003.DataLayers.SQLServer
{
    /// <summary>
    /// 
    /// </summary>
    public class EmployeeUserAccountDAL : _BaseDAL, IUserAccountDAL
    {
        public EmployeeUserAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount Authorize(string userName, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select EmployeeID as UserId, Email as UserName, FullName, Email, Photo 
                            from Employees 
                            where Email = @userName and Password = @password";
                var parameters = new { 
                    userName = userName,
                    password = password
                };
                data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        } 

        public bool ChangePassword(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
