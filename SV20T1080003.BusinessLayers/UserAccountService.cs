using SV20T1080003.DataLayers;
using SV20T1080003.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080003.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeUserAccountDB;
        //private static readonly IUserAccountDAL customerUserAccountDB;

        static UserAccountService()
        {
            string connectionString = "server=TANLOC;user id=sa;password=12345;database=LiteCommerceDB;TrustServerCertificate=true";
            employeeUserAccountDB = new DataLayers.SQLServer.EmployeeUserAccountDAL(connectionString);
            //customerUserAccountDB = new DataLayers.SQLServer.CustomerUserAccountDAL(connectionString);
        }

        public static UserAccount? Authorize(string userName, string password, TypeOfAccount typeOfAccount)
        {
            switch (typeOfAccount)
            {
                case TypeOfAccount.Employee:
                    return employeeUserAccountDB.Authorize(userName, password);
                //case TypeOfAccount.Customer:
                //    return customerUserAccountDB.Authorized(userName, password);
                default: 
                    return null;
            }
        }  
    }

    /// <summary>
    /// Các loại Account
    /// </summary>
    public enum TypeOfAccount
    {
        Employee,
        Customer,
        Shipper
    }
}
