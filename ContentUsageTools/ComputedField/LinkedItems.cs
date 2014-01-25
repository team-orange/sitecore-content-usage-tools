using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContentUsageTools.Helpers;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace ContentUsageTools.ComputedField
{
    public class LinkedItems : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);
            if (item == null )
            {
                return null;
            }
            return ContentUsageToolsHelper.IsInContentOrMediaLibrary(item) ? ContentUsageToolsHelper.GetLinkedItemsID(item) : null;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}