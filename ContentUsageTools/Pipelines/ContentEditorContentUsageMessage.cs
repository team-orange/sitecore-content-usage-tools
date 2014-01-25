using System.Linq;
using ContentUsageTools.Helpers;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Pipelines.GetContentEditorWarnings;

namespace ContentUsageTools.Pipelines
{
    /// <summary>
    /// Shows a message in the content editor to display on which pages the item is being used as a datasource.
    /// </summary>
    public class ContentEditorContentUsageMessage
    {
        public void Process(GetContentEditorWarningsArgs args)
        {
            if (args.Item == null
                || ContentUsageToolsHelper.IsPage(args.Item) // don't show this message for pages themselves
                || args.Item.Fields.All(f => f.Name.StartsWith("_"))) 
            {
                return;
            }

            if (ContentUsageToolsHelper.IsUnused(args.Item))
            {
                // Display a message suggesting to remove this item
                GetContentEditorWarningsArgs.ContentEditorWarning contentEditorWarning = args.Add();
                contentEditorWarning.Title = Translate.Text("This item is not being used on any page.");
                contentEditorWarning.AddOption(Translate.Text("Delete this item"), "item:delete");
            }
            else
            {
                // Display what pages the item is being used on as a datasource, so you can easily navigate to them
                GetContentEditorWarningsArgs.ContentEditorWarning contentEditorWarning = args.Add();
                contentEditorWarning.Title = Translate.Text("This item is being used on the following pages:");
                foreach (Item linkedPage in ContentUsageToolsHelper.GetLinkedItemsInContentAndMediaLibrary(args.Item))
                {
                    string text = string.Format("Open {0} <i>({1})</i>", linkedPage.DisplayName, linkedPage.Paths.ContentPath);
                    string command = string.Format("item:load(id={0})", linkedPage.ID);
                    contentEditorWarning.AddOption(Translate.Text(text), command);
                }
            }
        }
    }
}