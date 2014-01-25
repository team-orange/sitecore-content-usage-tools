using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace ContentUsageTools.Pipelines
{
    public class DetermineIfPagePipelineArgs : PipelineArgs
    {
        public Item Item { get; private set; }
        public bool IsPage { get; set; }

        public DetermineIfPagePipelineArgs(Item item)
        {
            Item = item;
        }
    }
}