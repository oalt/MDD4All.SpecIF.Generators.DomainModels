using MDD4All.SpecIF.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDD4All.SpecIF.Generators.DomainModels
{
    public class PropertyDataContext
    {
        public string Title { get; set; }

        public Key PropertyClassKey { get; set; }

        public string PropertyClassTitleWithoutNamespace { get; set; }

        public List<string> NamespaceParts { get; set; } = new List<string>();

        public string NamespaceBase { get; set; } = "MDD4All.SpecIF.DomainModels.v1_1";

        public string DotNetNamespacePostfix
        {
            get; set;
            //get
            //{
            //    string result = "";

            //    if (NamespaceParts.Any())
            //    {
            //        result = ".";
            //    }

            //    for (int counter = 0; counter < NamespaceParts.Count; counter++)
            //    {
            //        result += NamespaceParts[counter];

            //        if (counter < NamespaceParts.Count - 1)
            //        {
            //            result += ".";
            //        }
            //    }


            //    return result;
            //}
        }
    }
}
