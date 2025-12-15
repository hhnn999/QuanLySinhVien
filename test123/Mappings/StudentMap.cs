using FluentNHibernate.Mapping;
using GRPC_NHibernate.Entities;

namespace GRPC_NHibernate.Mappings
{
    public class StudentMap : ClassMap<Student>
    {
        public StudentMap()
        {
            Table("Student");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Code).Not.Nullable().Length(20);
            Map(x => x.FullName).Not.Nullable().Length(100);
            Map(x => x.BirthDate).Column("BirthDate").CustomSqlType("date").Nullable();
            Map(x => x.Address).Nullable().Length(255);


            References(x => x.ClassRoom)
                .Column("ClassId")
                .Nullable()
                .Cascade.None();
        }
    }
}
