namespace SV20T1080003.Web.Models
{
    public class PersonDAL
    {
        public List<Person> List()
        {
            List<Person> list = new List<Person>(); // ten bien: camelCase firstName, nameOfPerson
                                                    // ten lop: PascalCase
       
            list.Add(new Person() // instance/object
            {
                PersonId = 1,
                Name = "Nguyễn Lộc",
                Address = "666 Điện Biên Phủ",
                Email = "locnguyen@email.com"
            });

            list.Add(new Person()
            {
                PersonId = 2,
                Name = "Nguyễn Văn",
                Address = "666 Chi Lăng",
                Email = "vannguyen@email.com"
            });

            list.Add(new Person()
            {
                PersonId = 3,
                Name = "Nguyễn Nam",
                Address = "666 Thích Tịnh Khiết",
                Email = "namnguyen@email.com"
            });

            return list;
        }
    }
}
