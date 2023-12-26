namespace SV20T1080003.Web.Models
{
    public class Student
    {
        public string StudentId {  get; set; }
        public string StudentName { get; set;}
        
    }

    public class StudentDAL
    {
        public List<Student> List()
        {
            List<Student> students = new List<Student>();

            students.Add(new Student
            {
                StudentId = "20T1080003",
                StudentName = "Nguyễn Lộc"
            });

            students.Add(new Student
            {
                StudentId = "20T1080033",
                StudentName = "Nguyễn Văn"
            });

            return students;
        }
    }
}
