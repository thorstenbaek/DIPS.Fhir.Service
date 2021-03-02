using NHibernate.Extensions.NpgSql;
using NHibernate.Mapping.ByCode.Conformist;

namespace DIPS.Fhir.Service.Entities
{
    public class PractitionerMapping : ClassMapping<PractitionerEntity>
    {
        public PractitionerMapping()
        {
            Id(x => x.Id);
            Schema("public");
            Table("practitioner");            
            Property(
                p => p.Resource,
                m =>
                {
                    m.Type<JsonType>();
                });
        }
    }
}
