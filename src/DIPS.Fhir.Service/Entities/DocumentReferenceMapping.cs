using NHibernate.Extensions.NpgSql;
using NHibernate.Mapping.ByCode.Conformist;

namespace DIPS.Fhir.Service.Entities
{
    public class DocumentReferenceMapping : ClassMapping<DocumentReferenceEntity>
    {
        public DocumentReferenceMapping()
        {
            Id(x => x.Id);
            Schema("public");
            Table("documentreference");
            Property(
                p => p.Resource,
                m =>
                {
                    m.Type<JsonType>();
                });
        }
    }
}
