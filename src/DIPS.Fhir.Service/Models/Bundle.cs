using System;
using System.Collections.Generic;

namespace DIPS.Fhir.Service.Models
{
    public class Bundle
    {
        public string ResourceType => "Bundle";
        public string Type { get; }
        public int Total { get; }
        public IEnumerable<IResource> Entry { get; }        

        public Bundle(IEnumerable<IResource> entry, string type, int total)
        {
            Entry = entry ?? throw new ArgumentNullException(nameof(entry));
            Type = type;
            Total = total;
        }
    }
}
