using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataProvider.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDD4All.SpecIF.Generators.DomainModels.Templates
{
    public partial class StringPropertyTemplate
    {
        private PropertyClass _propertyClass;

        public StringPropertyTemplate(ISpecIfMetadataReader metadataReader, PropertyDataContext propertyDataContext)
        {
            MetadataReader = metadataReader;
            PropertyDataContext = propertyDataContext;
            _propertyClass = metadataReader.GetPropertyClassByKey(propertyDataContext.PropertyClassKey);
            //InitializeData();
        }

        //private void InitializeData()
        //{

        //    // ResourceTitleWithoutNamespace
        //    string title = _propertyClass.Title;

        //    string[] fullTitleParts = title.Split(new char[] { ':' });

        //    if (fullTitleParts.Length > 1)
        //    {
        //        for (int counter = 0; counter < fullTitleParts.Length - 1; counter++)
        //        {
        //            NamespaceParts.Add(fullTitleParts[counter].Trim().Replace(" ", "").Replace(".", "_").ToUpper());
        //        }
        //    }

        //    string lastPart = fullTitleParts[fullTitleParts.Length - 1];

        //    lastPart = lastPart.Trim();

        //    lastPart = lastPart.Replace(" ", "");

        //    char[] letters = lastPart.ToCharArray();
        //    // upper case the first char
        //    letters[0] = char.ToUpper(letters[0]);

        //    PropertyClassTitleWithoutNamespace = new string(letters);
        //}

        private ISpecIfMetadataReader MetadataReader { get; set; }

        public PropertyDataContext PropertyDataContext { get; set; }

        

        private string Format
        {
            get
            {
                string result = "Plain";
                if (_propertyClass.Format == "xhtml")
                {
                    result = "XHTML";
                }
                return result;
            }
        }
    }
}
