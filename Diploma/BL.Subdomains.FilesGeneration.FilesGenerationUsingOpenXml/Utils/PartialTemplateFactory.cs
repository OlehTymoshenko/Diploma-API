using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.Globalization;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils
{
    internal class PartialTemplateFactory
    {
        #region Common consts
        private const char NON_BREAKING_SPACE = '\u00A0';
        #endregion

        #region Names of files with templates
        internal const string FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE = @"PartialTemplate_FullNameSignatureDate.docx";
        internal const string DATE_IN_FORMAT_dd_MM_yyyy_PARTIAL_TEMPLATE = @"PartialTemplate_Date_In_Format_dd_MM_yyyy.docx";
        internal const string DATE_IN_FORMAT_dd_MMMM_yyyy_PARTIAL_TEMPLATE = @"PartialTemplate_Date_In_Format_dd_MMMM_yyyy.docx";
        #endregion

        #region Names of placeholders in files templates 
        private const string NAME_OF_PLACEHOLDER_FOR_FULL_NAME_IN_PARTIAL_TEMPLATE = "$FullName$";
        private const string NAME_OF_PLACEHOLDER_FOR_DAY_IN_PARTIAL_TEMPLATE = "$dd$";
        private const string NAME_OF_PLACEHOLDER_FOR_MONTH_AS_2_DIGITS_IN_PARTIAL_TEMPLATE = "$mm$";
        private const string NAME_OF_PLACEHOLDER_FOR_MONTH_AS_WORD_IN_PARTIAL_TEMPLATE = "$mmmm$";
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
            using var memStream = await _templateLoader.LoadTemplateAsync(FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            // replacements for partial template
            var dictForReplace = GetReplacementsForDate(date);
            dictForReplace.Add(NAME_OF_PLACEHOLDER_FOR_FULL_NAME_IN_PARTIAL_TEMPLATE, personFullName);

            wordDoc.ReplaceText(dictForReplace, false);

            var paragraphs = wordDoc.MainDocumentPart.Document.Body.ChildElements.Where(e => e is Paragraph).ToList();

            return paragraphs ?? throw new FileFormatException("Base template from file for partial template " +
                $"FullNameSignatureDate is invalid. \n" +
                $"File name of base template:{FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE}");
        }

        internal async Task<List<OpenXmlElement>> GetDatePartialTemplateAsync(DateFormats dateFormat, DateTime? date = default)
        {
            var nameOfFileWithPartialTemplate = dateFormat switch
            {
                DateFormats.ddMMyyyy => DATE_IN_FORMAT_dd_MM_yyyy_PARTIAL_TEMPLATE,
                DateFormats.ddMMMMyyyy => DATE_IN_FORMAT_dd_MMMM_yyyy_PARTIAL_TEMPLATE,
                _ => DATE_IN_FORMAT_dd_MM_yyyy_PARTIAL_TEMPLATE
            };

            // Load partial template from file
            using var memStream = await _templateLoader.LoadTemplateAsync(
                nameOfFileWithPartialTemplate);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            // replacements for partial template
            var dictForReplace = GetReplacementsForDate(date);

            wordDoc.ReplaceText(dictForReplace, false);

            var paragraphs = wordDoc.MainDocumentPart.Document.Body.ChildElements.Where(e => e is Paragraph).ToList();

            return paragraphs ?? throw new FileFormatException("Base template from file for partial template " +
                $"{dateFormat} is invalid. \n" +
                $"File name of base template:{nameOfFileWithPartialTemplate}");
        }


        private Dictionary<string,string> GetReplacementsForDate(DateTime? date, int minCountOfNonBreakingSpacesNumber = 3, string cultureName = "uk-UA")
        {
            var genetiveMonths = CultureInfo.GetCultureInfo(cultureName).DateTimeFormat.MonthGenitiveNames;

            return new Dictionary<string, string> {
                { NAME_OF_PLACEHOLDER_FOR_DAY_IN_PARTIAL_TEMPLATE,
                    date == default ? new string(NON_BREAKING_SPACE, minCountOfNonBreakingSpacesNumber) : 
                    date.Value.ToString("dd") },

                { NAME_OF_PLACEHOLDER_FOR_MONTH_AS_2_DIGITS_IN_PARTIAL_TEMPLATE,
                    date == default ? new string(NON_BREAKING_SPACE, minCountOfNonBreakingSpacesNumber) : 
                    date.Value.ToString("MM") },

                // 5 whitespaces is min size of field for date as word
                { NAME_OF_PLACEHOLDER_FOR_MONTH_AS_WORD_IN_PARTIAL_TEMPLATE,
                    date == default ? new string(NON_BREAKING_SPACE, minCountOfNonBreakingSpacesNumber + 5) : 
                    genetiveMonths[date.Value.Month - 1] },

                { NAME_OF_PLACEHOLDER_FOR_YEAR_LAST_TWO_IN_PARTIAL_TEMPLATE,
                    date == default ? new string(NON_BREAKING_SPACE, minCountOfNonBreakingSpacesNumber) : 
                    date.Value.ToString("yy") }
            };
        }
    }
}
