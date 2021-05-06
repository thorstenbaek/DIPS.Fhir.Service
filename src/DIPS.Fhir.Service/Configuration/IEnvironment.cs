using System;

namespace DIPS.Fhir.Service.Configuration
{
    public interface IEnvironment
    {
        public string GetEnvironmentVariable(string name);
    }

    public class EnvironmentImpl : IEnvironment
    {
        public string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}