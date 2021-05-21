using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using wpdNS = DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlPowerTools;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils
{
    internal static class OpenXMLExtensions
    {
        internal static WordprocessingDocument ReplaceText(this WordprocessingDocument wpd, string oldValue, string newValue, bool matchCase)
        {
            TextReplacer.SearchAndReplace(wpd, oldValue, newValue, matchCase);
            return wpd;
        }

        internal static WordprocessingDocument ReplaceText(this WordprocessingDocument wpd, Dictionary<string, string> pairsForReplace, bool matchCase)
        {
            foreach (var pair in pairsForReplace)
            {
                TextReplacer.SearchAndReplace(wpd, pair.Key, pair.Value, matchCase);
            }

            return wpd;
        }

        internal static WordprocessingDocument ReplaceTextInsideTables(this WordprocessingDocument wpd, string oldValue, string newValue)
        {
            var docBody = wpd.MainDocumentPart.Document.Body;
            var tables = docBody.Descendants<wpdNS.Table>();

            foreach (var table in tables)
            {
                var textElements = table.Descendants<wpdNS.Text>();

                foreach(var textEl in textElements)
                {
                    if(textEl.Text.Contains(oldValue))
                    {
                        textEl.Text = textEl.Text.Replace(oldValue, newValue, StringComparison.CurrentCultureIgnoreCase);
                    }
                }
            }
            

            return wpd;
        }

        internal static List<OpenXmlElement> CloneAllBodyChildElements(this WordprocessingDocument wpd, bool deep)
        {
            return wpd.MainDocumentPart.Document.Body.Select(n => n.CloneNode(deep)).ToList();
        }

        internal static T FindNodeWhichContainsText<T>(this wpdNS.Body body, string nodeInnerText) where T : OpenXmlElement
        {
            return body.Descendants<T>().FirstOrDefault(e => e.InnerText.Contains(nodeInnerText)) as T;
        }

        /// <summary>
        /// Find a node by inner text oldNodeInnerText, a then replace this node with newNodes
        /// </summary>
        /// <param name="body"></param>
        /// <param name="oldNodeInnerText"></param>
        /// <param name="newNodes"></param>
        /// <returns></returns>
        internal static wpdNS.Body ReplaceNode<T>(this wpdNS.Body docBody, string oldNodeInnerText, List<OpenXmlElement> newNodes) where T : OpenXmlElement
        {
            var oldNode = docBody.FindNodeWhichContainsText<T>(oldNodeInnerText) ?? 
                throw new ArgumentException("Can't find an old node for replacing. " +
                $"Value of argument oldNodeInnerText is \"{oldNodeInnerText}\"");

            docBody.ReplaceNode(oldNode, newNodes);

            return docBody;
        }

        internal static wpdNS.Body ReplaceNode<T>(this wpdNS.Body docBody, T oldNode, List<T> newNodes) where T : OpenXmlElement
        {
            var nodeForInserting = oldNode;

            foreach (var node in newNodes)
            {
                nodeForInserting = nodeForInserting.InsertAfterSelf(node.CloneNode(true)) as T;
            }

            oldNode.Remove();

            return docBody;
        }

    }
}
