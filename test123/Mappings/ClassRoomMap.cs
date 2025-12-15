using FluentNHibernate.Mapping;
using GRPC_NHibernate.Entities;

namespace GRPC_NHibernate.Mappings
{
    public class ClassRoomMap : ClassMap<ClassRoom>
    {
        public ClassRoomMap()
        {
            Table("ClassRoom");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Code).Not.Nullable().Length(20);
            Map(x => x.Name).Not.Nullable().Length(100);
            Map(x => x.Subject).Nullable().Length(100);

            References(x => x.Teacher)
                .Column("TeacherId")
                .Nullable()
                .Cascade.None();

            HasMany(x => x.Students)
                .KeyColumn("ClassId")
                .Inverse()
                .Cascade.None();
        }
    }
}
