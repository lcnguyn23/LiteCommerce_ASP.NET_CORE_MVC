using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV20T1080003.DomainModels;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SV20T1080003.DataLayers.SQLServer
{
    public class ShipperDAL : _BaseDAL, ICommonDAL<Shipper>
    {
        public ShipperDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Shipper data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Shippers where Phone = @Phone)
                                select -1
                            else
                                begin
                                    insert into Shippers(ShipperName,Phone)
                                    values(@ShipperName,@Phone);
                                    select @@identity;
                                end";
                var parameters = new
                {
                    ShipperName = data.ShipperName ?? "",
                    Phone = data.Phone ?? "",
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*) from Shippers 
                            where (@searchValue = N'') or (ShipperName like @searchValue)";
                var parameters = new { searchValue = searchValue };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = "delete from Shippers where ShipperID = @shipperId and not exists(select * from Orders where ShipperID = @shipperId)";
                var parameters = new { shipperId = id };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Shipper? Get(int id)
        {
            Shipper? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "select * from Shippers where ShipperID = @shipperId";
                var parameters = new { shipperId = id };
                data = connection.QueryFirstOrDefault<Shipper>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where ShipperId = @shipperId)
                                select 1
                            else 
                                select 0";
                var parameters = new { shipperId = id };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public IList<Shipper> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Shipper> data;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                            (
	                            select  *, row_number() over (order by ShipperName) as RowNumber
	                            from    Shippers
	                            where   (@searchValue = N'') or (ShipperName like @searchValue)
                            )
	                        select * from cte
	                        where   (@pageSize=0)
		                        or  (RowNumber between (@page -1) * @pageSize + 1 and @page * @pageSize)
	                        order by RowNumber";
                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue
                };
                data = connection.Query<Shipper>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            if (data == null)
                data = new List<Shipper>();
            return data;
        }

        public bool Update(Shipper data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Shippers where ShipperID <> @shipperId and Phone = @phone)
                                begin
                                    update Shippers
                                    set ShipperName = @shipperName,
                                        Phone = @phone
                                    where ShipperID = @shipperId
                                end";
                var parameters = new
                {
                    shipperID = data.ShipperID,
                    shipperName = data.ShipperName ?? "",
                    Phone = data.Phone ?? "",
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }
            return result;
        }
    }
}
