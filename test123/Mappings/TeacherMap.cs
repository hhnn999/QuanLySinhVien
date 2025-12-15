using FluentNHibernate.Mapping;
using GRPC_NHibernate.Entities;

namespace GRPC_NHibernate.Mappings
{
    public class TeacherMap : ClassMap<Teacher>
    {
        public TeacherMap()
        {
            Table("Teacher");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Code).Not.Nullable().Length(20);
            Map(x => x.FullName).Not.Nullable().Length(100);
            Map(x => x.BirthDate).Nullable();

            HasMany(x => x.ClassRooms)
                .KeyColumn("TeacherId")
                .Inverse()
                .Cascade.None();
        }
    }
}
