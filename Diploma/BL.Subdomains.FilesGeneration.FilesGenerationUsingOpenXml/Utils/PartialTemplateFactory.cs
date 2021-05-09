using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils
{
    internal class PartialTemplateFactory
    {
        #region Common consts
        private const char NON_BREAKING_SPACE = '\u00A0';
        #endregion

        #region Names of files with templates
        internal const string TEMPLATE_IN_FILE_OF_FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE = @"PartialTemplate_FullNameSignatureDate.docx";
        internal const string TEMPLATE_IN_FILE_OF_DATE_PARTIAL_TEMPLATE = @"PartialTemplate_Date.docx";
        #endregion

        #region Names of placeholders in files templates 
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


        internal async Task<List<OpenXmlElement>> GetFullNameSignatureDatePartialTemplateAsync(string personFullName, DateTime? date = default)
        {
            // Load partial template from file
            using var memStream = await _templateLoader.LoadTemplateAsync(TEMPLATE_IN_FILE_OF_FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            // replacements for partial template
            var dictForReplace = GetReplacementsForDate(date);
            dictForReplace.Add(NAME_OF_PLACEHOLDER_FOR_FULL_NAME_IN_PARTIAL_TEMPLATE, personFullName);

            wordDoc.ReplaceText(dictForReplace, false);

            var paragraphs = wordDoc.MainDocumentPart.Document.Body.ChildElements.Where(e => e is Paragraph).ToList();

            return paragraphs ?? throw new FileFormatException("Base template from file for partial template " +
                $"FullNameSignatureDate is invalid. \n" +
                $"Name of base template from file:{TEMPLATE_IN_FILE_OF_FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE}");
        }

        internal async Task<List<OpenXmlElement>> GetDatePartialTemplateAsync(DateTime? date = default)
        {
            // Load partial template from file
            using var memStream = await _templateLoader.LoadTemplateAsync(TEMPLATE_IN_FILE_OF_DATE_PARTIAL_TEMPLATE);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            // replacements for partial template
            var dictForReplace = GetReplacementsForDate(date);

            wordDoc.ReplaceText(dictForReplace, false);

            var paragraphs = wordDoc.MainDocumentPart.Document.Body.ChildElements.Where(e => e is Paragraph).ToList();

            return paragraphs ?? throw new FileFormatException("Base template from file for partial template " +
                $"FullNameSignatureDate is invalid. \n" +
                $"Name of base template from file:{TEMPLATE_IN_FILE_OF_FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE}");
        }


        private Dictionary<string,string> GetReplacementsForDate(DateTime? date, int nonBreakingSpacesNumber = 3)
        {
            return new Dictionary<string, string> {
                { NAME_OF_PLACEHOLDER_FOR_DAY_IN_PARTIAL_TEMPLATE,
                    date == default ? new string(NON_BREAKING_SPACE, nonBreakingSpacesNumber) : date.Value.ToString("dd") },

                { NAME_OF_PLACEHOLDER_FOR_MONTH_IN_PARTIAL_TEMPLATE,
                    date == default ? new string(NON_BREAKING_SPACE, nonBreakingSpacesNumber) : date.Value.ToString("MM")},

                { NAME_OF_PLACEHOLDER_FOR_YEAR_LAST_TWO_IN_PARTIAL_TEMPLATE,
                    date == default ? new string(NON_BREAKING_SPACE, nonBreakingSpacesNumber) : date.Value.ToString("yy")}
            };
        }
    }
}
