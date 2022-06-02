using NHibernate.Extensions.NpgSql;
using NHibernate.Mapping.ByCode.Conformist;

namespace DIPS.Fhir.Service.Entities
{
    public class BinaryMapping : ClassMapping<BinaryEntity>
    {
        public BinaryMapping()
        {
            Id(x => x.Id);
            Schema("public");
            Table("binary");
            Property(
                p => p.Resource,
                m =>
                {
                    m.Type<JsonType>();
                });
        }
    }
}
