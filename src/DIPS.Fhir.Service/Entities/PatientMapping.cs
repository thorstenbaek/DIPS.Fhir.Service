using NHibernate.Extensions.NpgSql;
using NHibernate.Mapping.ByCode.Conformist;

namespace DIPS.Fhir.Service.Entities
{
    public class PatientMapping : ClassMapping<PatientEntity>
    {
        public PatientMapping()
        {
            Id(x => x.Id);
            Schema("public");
            Table("patient");            
            Property(
                p => p.Resource,
                m =>
                {
                    m.Type<JsonType>();
                });
        }
    }
}
