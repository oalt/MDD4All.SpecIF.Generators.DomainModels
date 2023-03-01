using MDD4All.SpecIF.DataProvider.Contracts;
using MDD4All.SpecIF.DataProvider.File;

namespace MDD4All.SpecIF.Generators.DomainModels.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ISpecIfMetadataReader metadataReader = new SpecIfFileMetadataReader("./Assets/");

            DomainModelClassGenerator domainModelClassGenerator = new DomainModelClassGenerator(metadataReader);
            domainModelClassGenerator.GenerateCode();
        }
    }
}