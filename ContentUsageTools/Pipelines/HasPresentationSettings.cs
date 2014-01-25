using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Pipelines;

namespace ContentUsageTools.Pipelines
{
    /// <summary>
    /// Check to see if the current item has presentation settings
    /// </summary>
    public class HasPresentationSettings
    {
        public virtual void Process(DetermineIfPagePipelineArgs args)
        {
            if (args.Item != null)
            {
                args.IsPage = args.Item.Visualization.Layout != null;
            }
        }

    }
}