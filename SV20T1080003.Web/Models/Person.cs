namespace SV20T1080003.Web.Models
{
    public class Person // public --> Access Modifier: public, private sử dụng trong namespace, internal sử dụng trong project của nó (thư viện)
    {
        public int PersonId { get; set; } // Thuộc tính
        public string? Name { get; set; } // ? --> thuộc tính này có thể nhận Null
        public string? Address { get; set; }
        public string? Email { get; set; }

    }
}

