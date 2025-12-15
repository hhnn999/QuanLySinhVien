namespace GRPC_NHibernate.Entities
{
    public class Teacher
    {
        public virtual int Id { get; set; }

        public virtual string Code { get; set; } = "";
        public virtual string FullName { get; set; } = "";

        public virtual DateTime? BirthDate { get; set; }

        public virtual IList<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();

        public Teacher()
        {
        }
    }
}
