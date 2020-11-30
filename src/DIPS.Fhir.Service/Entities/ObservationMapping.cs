using NHibernate.Extensions.NpgSql;
using NHibernate.Mapping.ByCode.Conformist;

namespace DIPS.Fhir.Service.Entities
{
    public class ObservationMapping : ClassMapping<ObservationEntity>
    {
        public ObservationMapping()
        {
            Id(x => x.Id);
            Schema("public");
            Table("observation");
            Property(
                p => p.Resource,
                m =>
                {
                    m.Type<JsonType>();
                });
        }
    }
}
