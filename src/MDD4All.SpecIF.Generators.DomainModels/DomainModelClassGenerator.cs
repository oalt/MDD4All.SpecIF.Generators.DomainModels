using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataProvider.Contracts;
using MDD4All.SpecIF.Generators.DomainModels.Templates;
using System.Collections.Generic;
using System.IO;

namespace MDD4All.SpecIF.Generators.DomainModels
{
    public class DomainModelClassGenerator
    {

        private string OutputDirectory { get; set; } = @"../../../../GeneratedDomainClasses/";

        private ISpecIfMetadataReader MetadataReader { get; set; }

        private Dictionary<Key, PropertyDataContext> PropertiesToGenerate { get; set; } = new Dictionary<Key, PropertyDataContext>();

        public DomainModelClassGenerator(ISpecIfMetadataReader metadataReader)
        {
            MetadataReader = metadataReader;
        }

        public void GenerateCode()
        {
            List<ResourceClass> resourceClasses = MetadataReader.GetAllResourceClasses();

            string resourcesDirectoryBasePath = Path.GetDirectoryName(OutputDirectory)+ "/Resources/";
            string propertiesDirectoryBasePath = Path.GetDirectoryName(OutputDirectory) + "/Properties/";
            if (!Directory.Exists(resourcesDirectoryBasePath))
            {
                Directory.CreateDirectory(resourcesDirectoryBasePath);
            }

            foreach (ResourceClass resourceClass in resourceClasses)
            {
                ResourceTemplate resourceTemplate = new ResourceTemplate(MetadataReader, new Key(resourceClass.ID, resourceClass.Revision));
                string code = resourceTemplate.TransformText();
                string resourcesDirectoryPath = resourcesDirectoryBasePath;

                if (resourceTemplate.NamespaceParts.Count > 0)
                {
                    

                    for (int counter = 0; counter < resourceTemplate.NamespaceParts.Count; counter++)
                    {
                        resourcesDirectoryPath += resourceTemplate.NamespaceParts[counter] + "/";
                        if(!Directory.Exists(resourcesDirectoryPath))
                        {
                            Directory.CreateDirectory(resourcesDirectoryPath);
                        }
                    }
                }

                System.IO.File.WriteAllText(resourcesDirectoryPath + resourceTemplate.ResourceTitleWithoutNamespace + ".cs", code);

                foreach(PropertyDataContext propertyDataContext in resourceTemplate.PropertyDataContexts)
                {
                    if(!PropertiesToGenerate.ContainsKey(propertyDataContext.PropertyClassKey))
                    {
                        PropertiesToGenerate.Add(propertyDataContext.PropertyClassKey, propertyDataContext);
                    }
                }
            }

            // generate properties
            foreach(KeyValuePair<Key, PropertyDataContext> keyValuePair in PropertiesToGenerate)
            {
                PropertyDataContext propertyDataContext = keyValuePair.Value;

                PropertyClass propertyClass = MetadataReader.GetPropertyClassByKey(keyValuePair.Key);

                DataType dataType = MetadataReader.GetDataTypeByKey(propertyClass.DataType);

                string propertyDirectoryPath = propertiesDirectoryBasePath;

                if (propertyDataContext.NamespaceParts.Count > 0)
                {
                    for (int counter = 0; counter < propertyDataContext.NamespaceParts.Count; counter++)
                    {
                        propertyDirectoryPath += propertyDataContext.NamespaceParts[counter] + "/";
                        if (!Directory.Exists(propertyDirectoryPath))
                        {
                            Directory.CreateDirectory(propertyDirectoryPath);
                        }
                    }
                }

                if (dataType.Type == "xs:string" && (dataType.Enumeration == null || dataType.Enumeration.Count == 0))
                {
                    StringPropertyTemplate stringPropertyTemplate = new StringPropertyTemplate(MetadataReader, keyValuePair.Value);

                    string code = stringPropertyTemplate.TransformText();

                    System.IO.File.WriteAllText(propertyDirectoryPath + propertyDataContext.PropertyClassTitleWithoutNamespace + ".cs", code);

                }
                else if(dataType.Type == "xs:string" && (dataType.Enumeration != null))
                {
                    if(propertyClass.Multiple == true)
                    {
                        MultipleEnumerationPropertyTemplate multipleEnumerationPropertyTemplate = new MultipleEnumerationPropertyTemplate();
                        multipleEnumerationPropertyTemplate.PropertyDataContext = propertyDataContext;

                        string code = multipleEnumerationPropertyTemplate.TransformText();

                        System.IO.File.WriteAllText(propertyDirectoryPath + propertyDataContext.PropertyClassTitleWithoutNamespace + ".cs", code);
                    }
                    else
                    {
                        SingleEnumerationPropertyTemplate singleEnumerationPropertyTemplate = new SingleEnumerationPropertyTemplate();
                        singleEnumerationPropertyTemplate.PropertyDataContext = propertyDataContext;

                        string code = singleEnumerationPropertyTemplate.TransformText();

                        System.IO.File.WriteAllText(propertyDirectoryPath + propertyDataContext.PropertyClassTitleWithoutNamespace + ".cs", code);
                    }
                }
            }

        }
    }
}
