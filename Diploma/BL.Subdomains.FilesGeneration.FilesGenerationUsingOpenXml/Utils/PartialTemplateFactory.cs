using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
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

        #region Names of files with partial templates
        internal const string FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE = @"PartialTemplate_FullNameSignatureDate.docx";
        internal const string POSITION_SIGNATURE_FULL_NAME_PARTIAL_TEMPLATE = @"PartialTemplate_PositionSignatureFullName.docx";
        internal const string DATE_IN_FORMAT_dd_MM_yyyy_PARTIAL_TEMPLATE = @"PartialTemplate_Date_In_Format_dd_MM_yyyy.docx";
        internal const string DATE_IN_FORMAT_dd_MMMM_yyyy_PARTIAL_TEMPLATE = @"PartialTemplate_Date_In_Format_dd_MMMM_yyyy.docx";
        #endregion

        #region Names of placeholders in files templates 
        private const string NAME_OF_PLACEHOLDER_FOR_FULL_NAME_IN_PARTIAL_TEMPLATE = "$FullName$";
        private const string NAME_OF_PLACEHOLDER_FOR_POSITION_IN_PARTIAL_TEMPLATE = "$Position$";
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


        internal async Task<List<OpenXmlElement>> GetFullNameSignatureDatePartialTemplateAsync(string fullName, DateTime? date = default)
        {
            // Load partial template from file
            using var memStream = await _templateLoader.LoadTemplateAsync(FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            // replacements for partial template
            var dictForReplace = GetReplacementsForDate(date);
            dictForReplace.Add(NAME_OF_PLACEHOLDER_FOR_FULL_NAME_IN_PARTIAL_TEMPLATE, fullName);

            wordDoc.ReplaceText(dictForReplace, false);

            var paragraphs = wordDoc.MainDocumentPart.Document.Body.ChildElements.Where(e => e is Paragraph).ToList();

            return paragraphs ?? throw new FileFormatException("Base template from file for partial template " +
                $"FullNameSignatureDate is invalid. \n" +
                $"File name of base template:{FULL_NAME_SIGNATURE_DATE_PARTIAL_TEMPLATE}");
        }

        internal async Task<List<OpenXmlElement>> GetPositionSignatureFullNamePartialTemplateAsync(string position, string fullName)
        {
            // Load partial template from file
            using var memStream = await _templateLoader.LoadTemplateAsync(POSITION_SIGNATURE_FULL_NAME_PARTIAL_TEMPLATE);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            // replacements for partial template
            var dictForReplace = new Dictionary<string, string> {
                { NAME_OF_PLACEHOLDER_FOR_POSITION_IN_PARTIAL_TEMPLATE,
                    position ?? new string(NON_BREAKING_SPACE, 10) },

                { NAME_OF_PLACEHOLDER_FOR_FULL_NAME_IN_PARTIAL_TEMPLATE,
                    fullName ?? new string(NON_BREAKING_SPACE, 10) }
            };

            wordDoc.ReplaceText(dictForReplace, false);

            var paragraphs = wordDoc.MainDocumentPart.Document.Body.ChildElements.Where(e => e is Paragraph).ToList();

            return paragraphs ?? throw new FileFormatException("Base template from file for partial template " +
                $"PositionSignatureFullName is invalid. \n" +
                $"File name of base template:{POSITION_SIGNATURE_FULL_NAME_PARTIAL_TEMPLATE}");
        }

        /// <summary>
        /// Create a partial template (set of paraggraphs) with one position and multiple
        /// fields for signarute each person from fullNames array
        /// </summary>
        /// <param name="position"></param>
        /// <param name="fullNames"></param>
        /// <returns></returns>
        internal async Task<List<OpenXmlElement>> GetPositionSignatureFullNamePartialTemplateAsync(string position, string[] fullNames)
        {
            // Load partial template from file
            using var memStream = await _templateLoader.LoadTemplateAsync(POSITION_SIGNATURE_FULL_NAME_PARTIAL_TEMPLATE);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            // replacements for partial template
            var dictForReplace = new Dictionary<string, string> {
                { NAME_OF_PLACEHOLDER_FOR_POSITION_IN_PARTIAL_TEMPLATE,
                    position ?? new string(NON_BREAKING_SPACE, 10) },

                { NAME_OF_PLACEHOLDER_FOR_FULL_NAME_IN_PARTIAL_TEMPLATE,
                    fullNames.First() }
            };

            wordDoc.ReplaceText(dictForReplace, false);

            var nodeForInserting = wordDoc.MainDocumentPart.Document.Body.FirstOrDefault(
                n => n.InnerText.Contains(position)) as Paragraph;

            // add fields for signature of rest persons
            foreach (var name in fullNames.Skip(1))
            {
                var lineForMemberSignatureWithoutPosition = GetSignatureFullNameTemplate(name, position.Length);

                nodeForInserting.InsertAfterSelf(lineForMemberSignatureWithoutPosition);
            }

            var paragraphs = wordDoc.MainDocumentPart.Document.Body.ChildElements.Where(e => e is Paragraph).ToList();

            return paragraphs ?? throw new FileFormatException("Base template from file for partial template " +
                $"PositionSignatureFullName is invalid. \n" +
                $"File name of base template:{POSITION_SIGNATURE_FULL_NAME_PARTIAL_TEMPLATE}");
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

        private Paragraph GetSignatureFullNameTemplate(string fullName, int numberOfWhitespacesForLeftPadding)
        {
            Paragraph resultParagraph = new Paragraph();

            resultParagraph.ParagraphProperties = new ParagraphProperties()
            {
                SpacingBetweenLines = new SpacingBetweenLines()
                {
                    Line = "360",
                    LineRule = LineSpacingRuleValues.Auto
                }
            };

            var positionRun = new Run();

            // 5 is a number of white spaces, that equals to 1 tab
            int numberOfTabSpaces = numberOfWhitespacesForLeftPadding / 5;

            positionRun.Append(
                Enumerable.Repeat(new TabChar(), numberOfTabSpaces).
                Select(c => c.CloneNode(true))
            );


            var signatureRun = new Run();
            signatureRun.Append(new TabChar());
            signatureRun.Append(new Text()
            {
                Text = "___________"
            });
            signatureRun.Append(new TabChar());
            signatureRun.Append(new TabChar());


            var fullNameRun = new Run();

            var fullNameRunProp = new RunProperties();
            fullNameRunProp.Append(new Underline() { Val = UnderlineValues.Single });
            
            fullNameRun.Append(fullNameRunProp);
            fullNameRun.Append(new Text()
            {
                Text = fullName
            });
            fullNameRun.Append(new TabChar());
            
            
            resultParagraph.Append(positionRun);
            resultParagraph.Append(signatureRun);
            resultParagraph.Append(fullNameRun);

            return resultParagraph;
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
