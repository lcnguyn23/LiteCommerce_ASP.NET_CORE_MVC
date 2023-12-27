using Dapper;
using SV20T1080003.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080003.DataLayers.SQLServer
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Add(Product data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Products where ProductName = @ProductName)
                                select -1
                            else
                                begin
                                    insert into Products(ProductName,ProductDescription,SupplierID,CategoryID,Unit,Price,Photo,IsSelling)
                                    values(@ProductName,@ProductDescription,@SupplierID,@CategoryID,@Unit,@Price,@Photo,@IsSelling);
                                    select @@identity;
                                end";
                var parameters = new
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierId,
                    CategoryID = data.CategoryId,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling,
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public long AddAttribute(ProductAttribute data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from ProductAttributes where AttributeName = @AttributeName)
                                select -1
                            else
                                begin
                                    insert into ProductAttributes(ProductID,AttributeName,AttributeValue,DisplayOrder)
                                    values(@ProductID,@AttributeName,@AttributeValue,@DisplayOrder);
                                    select @@identity;
                                end";
                var parameters = new
                {
                    ProductID = data.ProductId,
                    AttributeName = data.AttributeName,
                    AttributeValue = data.AttributeValue,
                    DisplayOrder = data.DisplayOrder
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public long AddPhoto(ProductPhoto data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from ProductPhotos where Photo = @Photo)
                                select -1
                            else
                                begin
                                    insert into ProductPhotos(ProductID,Photo,Description,DisplayOrder,IsHidden)
                                    values(@ProductID,@Photo,@Description,@DisplayOrder,@IsHidden);
                                    select @@identity;
                                end";
                var parameters = new
                {
                    ProductID = data.ProductId,
                    Photo = data.Photo ?? "",
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (var connection = OpenConnection())
            {
                var sql = @"SELECT COUNT(*) 
                    FROM Categories AS c
                        JOIN Products AS p ON c.CategoryID = p.CategoryID
                        JOIN Suppliers AS s ON p.SupplierID = s.SupplierID
                        WHERE (@categoryID = 0 OR p.CategoryID = @categoryID)
                          AND (@supplierID = 0 OR p.SupplierID = @supplierID)
                          AND (@minPrice = 0 OR p.Price >= @minPrice)
                          AND (@maxPrice = 0 OR p.Price <= @maxPrice)
                          AND (@searchValue = N'' OR p.ProductName LIKE @searchValue)";

                var parameters = new
                {
                    searchValue = searchValue,
                    CategoryId = categoryID,
                    SupplierId = supplierID,
                    minPrice = minPrice,
                    maxPrice = maxPrice
                };

                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Products
                            where ProductID = @ProductId and not exists(select * from OrderDetails where ProductID = @ProductId)
                            delete from ProductPhotos where ProductID = @ProductId
                            delete from ProductAttributes where ProductID = @ProductId";
                var parameters = new { ProductId = productID };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeleteAttribute(long attributeID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductAttributes
                            where AttributeID = @AttributeId";
                var parameters = new { AttributeId = attributeID };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeletePhoto(long photoID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductPhotos
                            where PhotoID = @PhotoId";
                var parameters = new { PhotoId = photoID };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Product? Get(int productID)
        {
            Product? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "select * from Products where ProductID = @ProductId";
                var parameters = new { ProductId = productID };
                data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductAttribute? GetAttribute(long attributeID)
        {
            ProductAttribute? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "select * from ProductAttributes where AttributeID = @AttributeId";
                var parameters = new { AttributeId = attributeID };
                data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductPhoto? GetPhoto(long photoID)
        {
            ProductPhoto? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "select * from ProductPhotos where PhotoID = @PhotoId";
                var parameters = new { PhotoId = photoID };
                data = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from OrderDetails where ProductID = @ProductId)
                                select 1
                            else 
	                            select 0";
                var parameters = new { ProductId = productID };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> data;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (var connection = OpenConnection())
            {
                var sql = @"WITH cte AS
                    (
                        SELECT p.*, ROW_NUMBER() OVER (ORDER BY ProductName) AS RowNumber
                        FROM Categories AS c
                        JOIN Products AS p ON c.CategoryID = p.CategoryID
                        JOIN Suppliers AS s ON p.SupplierID = s.SupplierID
                        WHERE (@categoryID = 0 OR p.CategoryID = @categoryID)
                          AND (@supplierID = 0 OR p.SupplierID = @supplierID)
                          AND (@minPrice = 0 OR p.Price >= @minPrice)
                          AND (@maxPrice = 0 OR p.Price <= @maxPrice)
                          AND (@searchValue = N'' OR p.ProductName LIKE @searchValue) 
                    )
                    SELECT * FROM cte
                    WHERE  (@pageSize = 0) 
                           OR (RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)
                    ORDER BY RowNumber";

                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue,
                    CategoryId = categoryID,
                    SupplierId = supplierID,
                    minPrice = minPrice,
                    maxPrice = maxPrice
                };

                data = connection.Query<Product>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }

            if (data == null)
                data = new List<Product>();

            return data;
        }

        public IList<ProductAttribute> ListAttributes(int productID)
        {
            List<ProductAttribute> data;
            using (var connection = OpenConnection())
            {
                var sql = "select * from ProductAttributes where ProductID = @ProductId order by DisplayOrder asc";
                var parameters = new { ProductId = productID };
                data = connection.Query<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            if (data == null) data = new List<ProductAttribute>();
            return data;
        }

        public IList<ProductPhoto> ListPhotos(int productID)
        {
            List<ProductPhoto> data;
            using (var connection = OpenConnection())
            {
                var sql = "select * from ProductPhotos where ProductID = @ProductId order by DisplayOrder asc";
                var parameters = new { ProductId = productID };
                data = connection.Query<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            if (data == null) data = new List<ProductPhoto>();
            return data;
        }

        public bool Update(Product data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Products where ProductID <> @ProductId and ProductName = @ProductName)
                                begin
                                    update Products 
                                    set ProductName = @ProductName,
                                        ProductDescription = @ProductDescription,
                                        SupplierID = @SupplierId,
                                        CategoryID = @CategoryId,
                                        Unit = @Unit,
                                        Price = @Price,
                                        Photo = @Photo,
                                        IsSelling = @IsSelling
                                    where ProductID = @ProductId
                                end";
                var parameters = new
                {
                    ProductID = data.ProductId,
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierId,
                    CategoryID = data.CategoryId,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo ?? "",
                    IsSelling = data.IsSelling
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }
            return result;
        }

        public bool UpdateAttribute(ProductAttribute data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from ProductAttributes where AttributeID <> @AttributeId and AttributeName = @AttributeName and ProductID = @ProductId)
                                begin
                                    update ProductAttributes 
                                    set AttributeName = @AttributeName,
                                        AttributeValue = @AttributeValue,
                                        DisplayOrder = @DisplayOrder,
                                    where AttrubuteID = @AttributeId
                                end";
                var parameters = new
                {
                    AttributeName = data.AttributeName,
                    AttributeValue = data.AttributeValue,
                    DisplayOrder = data.DisplayOrder,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }
            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from ProductPhotos where PhotoID <> @PhotoId and Photo = @Photo and ProductID = @ProductId)
                                begin
                                    update ProductPhotos
                                    set Photo = @Photo,
                                        Description = @Description,
                                        DisplayOrder = @DisplayOrder,
                                        IsHidden = @IsHidden
                                    where PhotoID = @PhotoId
                                end";
                var parameters = new
                {
                    PhotoId = data.PhotoId,
                    ProductId = data.ProductId,
                    Photo = data.Photo ?? "",
                    Description = data.Description ?? "",
                    DisplayOrder = data.@DisplayOrder,
                    IsHidden = data.IsHidden
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }
            return result;
        }
    }
}
