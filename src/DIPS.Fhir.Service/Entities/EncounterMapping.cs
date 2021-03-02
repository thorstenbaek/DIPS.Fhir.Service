using NHibernate.Extensions.NpgSql;
using NHibernate.Mapping.ByCode.Conformist;

namespace DIPS.Fhir.Service.Entities
{
	public class EncounterMapping : ClassMapping<EncounterEntity>
	{
		public EncounterMapping()
		{
            Id(x => x.Id);
            Schema("public");
            Table("encounter");
            Property(
                p => p.Resource,
                m =>
                {
                    m.Type<JsonType>();
                });            
        }
	}
}
