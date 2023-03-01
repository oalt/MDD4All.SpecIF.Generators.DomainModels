using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataProvider.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace MDD4All.SpecIF.Generators.DomainModels.Templates
{
    public partial class ResourceTemplate
    {

        private ISpecIfMetadataReader MetadataReader { get; set; }

        public ResourceTemplate(ISpecIfMetadataReader metadataReader, Key resourceClassKey)
        {
            MetadataReader = metadataReader;
            _resourceClass = metadataReader.GetResourceClassByKey(resourceClassKey);
            InitializeData();
        }

        private void InitializeData()
        {
            // ResourceTitleWithoutNamespace
            string title = _resourceClass.Title;

            string[] fullTitleParts = title.Split(new char[] { ':' });

            if(fullTitleParts.Length > 1) 
            { 
                for(int counter = 0; counter < fullTitleParts.Length - 1; counter++) 
                {
                    NamespaceParts.Add(fullTitleParts[counter].Trim().Replace(" ", "").Replace(".", "_").ToUpper());
                }
            }

            string lastPart = fullTitleParts[fullTitleParts.Length - 1];

            lastPart = lastPart.Trim();

            lastPart = lastPart.Replace(" ", "");

            char[] letters = lastPart.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);

            ResourceTitleWithoutNamespace = new string(letters) + "Resource";

            // propertyDataContexts
            foreach(Key propertyClassKey in _resourceClass.PropertyClasses)
            {
                PropertyClass propertyClass = MetadataReader.GetPropertyClassByKey(propertyClassKey);

                if (propertyClass != null)
                {
                    string[] propertyClassTitleParts = propertyClass.Title.Split(new char[] { ':' });
                    
                    string propertyTitle = propertyClassTitleParts[propertyClassTitleParts.Length - 1];

                    propertyTitle = propertyTitle.Trim();
                    propertyTitle = propertyTitle.Replace(" ", "");

                    char[] propertyTitleLetters = propertyTitle.ToCharArray();
                    propertyTitleLetters[0] = char.ToUpper(propertyTitleLetters[0]);

                    string propertyNamespacePostfix = "";

                    List<string> propertyNamespaceParts = new List<string>();
                    for (int counter = 0; counter < propertyClassTitleParts.Length - 1; counter++)
                    {
                        string part = propertyClassTitleParts[counter].Trim().Replace(" ", "").ToUpper();
                        propertyNamespaceParts.Add(part);

                        propertyNamespacePostfix += "." + part;
                    }

                    PropertyDataContext propertyDataContext = new PropertyDataContext
                    {
                        Title = new string(propertyTitleLetters),
                        PropertyClassKey = new Key(propertyClass.ID, propertyClass.Revision),
                        NamespaceBase= NamespaceBase,
                        DotNetNamespacePostfix = propertyNamespacePostfix,
                        NamespaceParts = propertyNamespaceParts,
                        PropertyClassTitleWithoutNamespace = new string(propertyTitleLetters)
                    };

                    PropertyDataContexts.Add(propertyDataContext);

                    

                    if(!DotnetPropertyUsingPostfixes.Contains(propertyNamespacePostfix))
                    {
                        DotnetPropertyUsingPostfixes.Add(propertyNamespacePostfix);
                    }
                }
            }
        }

        private ResourceClass _resourceClass { get; set; }

        public string NamespaceBase { get; set; } = "MDD4All.SpecIF.DomainModels.v1_1";

        public string ResourceTitleWithoutNamespace { get; set; }

        public List<string> NamespaceParts { get; set; } = new List<string>();

        public string DotNetNamespacePostfix
        {
            get
            {
                string result = "";

                if(NamespaceParts.Any())
                {
                    result = ".";
                }

                for (int counter = 0; counter < NamespaceParts.Count; counter++)
                {
                    result += NamespaceParts[counter];

                    if(counter < NamespaceParts.Count - 1)
                    {
                        result += ".";
                    }
                }


                return result;
            }
        }

        public List<PropertyDataContext> PropertyDataContexts { get; set; } = new List<PropertyDataContext>();

        public List<string> DotnetPropertyUsingPostfixes { get; set; } = new List<string>();
    }
}
