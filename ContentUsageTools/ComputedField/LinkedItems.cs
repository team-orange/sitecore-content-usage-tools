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
            if (!item.Paths.IsContentItem && !item.Paths.IsMediaItem)
            {
                return null;
            }
            return ContentUsageToolsHelper.GetLinkedItemsID(item);
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}