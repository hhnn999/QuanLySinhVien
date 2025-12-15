namespace GRPC_NHibernate.Entities
{
    public class Student
    {
        public virtual int Id { get; set; }

        public virtual string Code { get; set; } = "";
        public virtual string FullName { get; set; } = "";

        public virtual DateTime? BirthDate { get; set; }
        public virtual string? Address { get; set; }

        public virtual ClassRoom? ClassRoom { get; set; }

        public Student()
        {
        }
    }
}
