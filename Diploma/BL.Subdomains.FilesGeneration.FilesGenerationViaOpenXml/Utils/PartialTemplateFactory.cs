using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlPowerTools;
using BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml;
using System.IO;
using DocumentFormat.OpenXml;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils
{
    internal class PartialTemplateFactory
    {
        #region Common consts
        private const char NON_BREAKING_SPACE = '\u00A0';
        #endregion

        #region Consts for full name signature date partial template
        internal const string TEMPLATE_IN_FILE_OF_FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE = @"Paragph_FullNameSignatureDate.docx";
        private const string NAME_OF_PLACEHOLDER_FOR_FULL_NAME_IN_PARTIAL_TEMPLATE = "$FullName$";
        private const string NAME_OF_PLACEHOLDER_FOR_DAY_IN_PARTIAL_TEMPLATE = "$dd$";
        private const string NAME_OF_PLACEHOLDER_FOR_MONTH_IN_PARTIAL_TEMPLATE = "$mm$";
        private const string NAME_OF_PLACEHOLDER_FOR_YEAR_LAST_TWO_IN_PARTIAL_TEMPLATE = "$yy$";
        #endregion

        private readonly TemplateLoader _templateLoader;


        public PartialTemplateFactory()
        {
            _templateLoader = new TemplateLoader();
        }


        internal async Task<List<OpenXmlElement>> GetFullNameSignatureDatePartialTemplateAsync(string chiefFullName, DateTime? date = default)
        {
            // Load base template from file
            using var memStream = await _templateLoader.LoadTemplateAsync(TEMPLATE_IN_FILE_OF_FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            // replacements for base template
            var dictForReplace = new Dictionary<string, string> {
                { NAME_OF_PLACEHOLDER_FOR_FULL_NAME_IN_PARTIAL_TEMPLATE, chiefFullName ?? "" },
                { NAME_OF_PLACEHOLDER_FOR_DAY_IN_PARTIAL_TEMPLATE, 
                    date == default ? new string(NON_BREAKING_SPACE, 3) : date.Value.ToString("dd") },
                { NAME_OF_PLACEHOLDER_FOR_MONTH_IN_PARTIAL_TEMPLATE, 
                    date == default ? new string(NON_BREAKING_SPACE, 3) : date.Value.ToString("MM")},
                { NAME_OF_PLACEHOLDER_FOR_YEAR_LAST_TWO_IN_PARTIAL_TEMPLATE,
                    date == default ? new string(NON_BREAKING_SPACE, 3) : date.Value.ToString("yy")}
            };

            wordDoc.ReplaceText(dictForReplace, false);

            wordDoc.Save();

            var paragraphs = wordDoc.MainDocumentPart.Document.Body.ChildElements.Where(e => e is Paragraph).ToList();

            return paragraphs ?? throw new FileFormatException("Base template from file for partial template " +
                $"FullNameSignatureDate is invalid. \n" +
                $"Name of base template from file:{TEMPLATE_IN_FILE_OF_FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE}");
        }



    }
}
