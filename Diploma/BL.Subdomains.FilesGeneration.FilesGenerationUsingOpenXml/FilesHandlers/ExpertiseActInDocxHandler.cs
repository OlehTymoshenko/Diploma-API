using System;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using BL.Models.FilesGeneration;
using BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils;
using BL.Interfaces.Subdomains.FilesGeneration;
using DL.Entities.Enums;
using DocumentFormat.OpenXml;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.FilesHandlers
{
    public class ExpertiseActInDocxHandler : IExpertiseActHandler
    {
        public FileType Type => FileType.ExpertiseAct;

        public FileFormat Format => FileFormat.DOCX;

        public string TemplateName { get; init; } = @"Template_ExpertiseAct.docx";

        #region Placeholders names in a template
        internal const string PROVOST_NAME_PLACEHOLDER_IN_TEMPLATE = @"$ProvostName$";
        internal const string DATE_IN_FORMAT_dd_MMMM_yyyy_PLACEHOLDER_IN_TEMPLATE = @"$DateInFormat_ddMMMMyyyy$";
        internal const string FACULTY_NUMBER_PLACEHOLDER_IN_TEMPLATE = @"$FacultyNumber$";
        internal const string HEAD_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$HeadOfTheCommission$";
        internal const string MEMBERS_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$MembersOfTheCommission$";
        internal const string AUTHORS_PLACEHOLDER_IN_TEMPLATE = @"$Authors$";
        internal const string PUBLICATION_NAME_WITH_ITS_STATISTIC_PLACEHOLDER_IN_TEMPLATE = @"$PublicationNameWithItsStatistic$";
        internal const string DOES_COMMISSION_ALLOW_ISSUING_PLACEHOLDER_IN_TEMPLATE = @"$AllowIssuing$";
        internal const string HEAD_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE = @"$HeadOfTheCommissionSignatureFullName$";
        internal const string SECRETARY_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE = @"$SecretaryOfTheCommissionSignatureFullName$";
        internal const string MEMBERS_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE = @"$MembersOfTheCommissionSignatureFullName$";
        internal const string AUTHORS_OF_THE_PUBLICATION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE = @"$AuthorsSignatureFullName$";
        internal const string CHIEF_OF_THE_SECURITY_DEPARTMENT_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE = @"$ChiefOfSecurityDepartmentSignatureFullName$";
        internal const string DATE_IN_FORMAT_ddMMyyyy_PLACEHOLDER_IN_TEMPLATE = @"$DateInFormat_ddMMyyyy$";

        #endregion

        private readonly TemplateLoader _templateLoader;
        private readonly PartialTemplateFactory _partialTemplateFactory;

        public ExpertiseActInDocxHandler()
        {
            _templateLoader = new TemplateLoader();
            _partialTemplateFactory = new PartialTemplateFactory();
        }


        public async Task<FileModel> CreateFileAsync(SaveExpertiseActModel saveExpertiseActModel)
        {
            using var memStream = await _templateLoader.LoadTemplateAsync(TemplateName);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            //////////////////////////////////////////////////// EXTREMELY IMPORTANT INFORMATION
            /// Order of calling methods is IMPORTANT. All async methods should be called after
            /// all sync methods. 
            /// The reason for this strange behaviour is unknown
            //////////////////////////////////////////////////// 

            SetFacultyNumber(wordDoc, saveExpertiseActModel.FacultyNumber);

            SetHeadOfTheCommission(wordDoc, saveExpertiseActModel.HeadOfTheCommission);

            SetMembersOfTheCommission(wordDoc, saveExpertiseActModel.MembersOfTheCommission);

            SetAuthors(wordDoc, saveExpertiseActModel.AuthorsOfThePublication);

            SetPublicationNameWithItsStatistic(wordDoc, saveExpertiseActModel.PublishingNameWithItsStatics);

            SetProvostName(wordDoc, saveExpertiseActModel.ProvostName);

            await SetDateInFormat_ddMMMMyyyyAsync(wordDoc, saveExpertiseActModel.ActCreationDate);

            await SetFieldsForSignatureAsync(wordDoc.MainDocumentPart.Document.Body,
                saveExpertiseActModel.HeadOfTheCommission.FullName,
                saveExpertiseActModel.MembersOfTheCommission.Select(m => m.FullName).ToArray(),
                saveExpertiseActModel.AuthorsOfThePublication.Select(m => m.FullName).ToArray(),
                saveExpertiseActModel.SecretaryOfTheCommission,
                saveExpertiseActModel.ChiefOfSecurityDepartment);

            await SetDateInFormat_ddMMyyyyAsync(wordDoc.MainDocumentPart.Document.Body, saveExpertiseActModel.ActCreationDate);

            // save wordDoc and get bytes from it
            wordDoc.Close();

            return new FileModel()
            {
                Format = this.Format,
                Type = this.Type,
                FileAsBytes = memStream.ToArray()
            };

        }

        private void SetProvostName(WordprocessingDocument wordDoc, string provostName)
        {
            wordDoc.ReplaceTextInsideTables(PROVOST_NAME_PLACEHOLDER_IN_TEMPLATE, provostName.Trim(' ', ','));
        }

        private async Task SetDateInFormat_ddMMMMyyyyAsync(WordprocessingDocument wordDoc, DateTime? date)
        {
            var partialTemplateNodes = await _partialTemplateFactory.GetDatePartialTemplateAsync(DateFormats.ddMMMMyyyy, date);

            wordDoc.MainDocumentPart.Document.Body.ReplaceNode<Paragraph>(DATE_IN_FORMAT_dd_MMMM_yyyy_PLACEHOLDER_IN_TEMPLATE, partialTemplateNodes);

            var paragraphWithDate = wordDoc.MainDocumentPart.Document.Body.FindNodeWhichContainsText<Paragraph>(
                string.Join(string.Empty, partialTemplateNodes.Select(x => x.InnerText))
            );

            paragraphWithDate.ParagraphProperties = new ParagraphProperties() {
                Justification = new Justification() { Val = JustificationValues.Right} 
            };
        }

        private void SetFacultyNumber(WordprocessingDocument wordDoc, int facultyNumber)
        {
            wordDoc.ReplaceText(FACULTY_NUMBER_PLACEHOLDER_IN_TEMPLATE,
                            facultyNumber.ToString(),
                            false);
        }

        private void SetHeadOfTheCommission(WordprocessingDocument wordDoc, Scientist headOfTheCommission)
        {
            wordDoc.ReplaceText(HEAD_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE,
                            ScientistToStringDegreesFirst(headOfTheCommission),
                            false);
        }

        private void SetMembersOfTheCommission(WordprocessingDocument wordDoc, Scientist[] membersOfTheCommission)
        {
            string membersNameAndDegrees = "";

            foreach (var member in membersOfTheCommission)
            {
                membersNameAndDegrees += ScientistToStringNameFirst(member) + ", ";
            }

            membersNameAndDegrees = membersNameAndDegrees.Trim(' ', ',');

            wordDoc.ReplaceText(MEMBERS_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE,
                            membersNameAndDegrees,
                            false);
        }

        private void SetAuthors(WordprocessingDocument wordDoc, Scientist[] authors)
        {
            string authorsNameAndDegrees = "";

            foreach (var author in authors)
            {
                authorsNameAndDegrees += ScientistToStringNameFirst(author) + ", ";
            }

            authorsNameAndDegrees = authorsNameAndDegrees.Trim(' ', ',');

            wordDoc.ReplaceText(AUTHORS_PLACEHOLDER_IN_TEMPLATE,
                            authorsNameAndDegrees,
                            false);
        }

        private void SetPublicationNameWithItsStatistic(WordprocessingDocument wordDoc, string publicationNameWithItsStatstics)
        {
            wordDoc.ReplaceText(PUBLICATION_NAME_WITH_ITS_STATISTIC_PLACEHOLDER_IN_TEMPLATE,
                            publicationNameWithItsStatstics.Trim(' ', ','),
                            false);
        }

        private async Task SetFieldsForSignatureAsync(Body docBody, string headOfTheCommissionName,
            string[] membersOfTheCommissionName,
            string[] authorsOfThePublication,
            string secretaryOfTheCommissionName,
            string chiefOfSecurityDepartmentName)
        {
            // set field for signature head of the commission
            var headOfTheCommissionPartialTemplateNodes =
                await _partialTemplateFactory.GetPositionSignatureFullNamePartialTemplateAsync("Голова комісії",
                headOfTheCommissionName);

            docBody.ReplaceNode<OpenXmlElement>(HEAD_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE,
                headOfTheCommissionPartialTemplateNodes);

            // set field for signature members of the commission
            var membersOfTheCommissionPartialTemplateNodes = await _partialTemplateFactory.
                GetPositionSignatureFullNamePartialTemplateAsync("Члени комісії", membersOfTheCommissionName);

            docBody.ReplaceNode<OpenXmlElement>(MEMBERS_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE,
                membersOfTheCommissionPartialTemplateNodes);

            // set field for signature authors of the publication
            var authorsSurnameAndInitials = authorsOfThePublication.Select(a => GetFromFullNameSurnameAndInitials(a)).ToArray();

            // add length to position in order to align fields for signature and full name with other rows
            // 6 is a difference in the number of characters between this position name and the longest position name in this document
            var authorsOfThePublicationPartialTemplateNodes = await _partialTemplateFactory.
                GetPositionSignatureFullNamePartialTemplateAsync("Автори", authorsSurnameAndInitials);

            docBody.ReplaceNode<OpenXmlElement>(AUTHORS_OF_THE_PUBLICATION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE,
                authorsOfThePublicationPartialTemplateNodes);

            // set field for signature secretary of the commission
            var secretaryOfTheCommissionPartialTemplateNodes =
                await _partialTemplateFactory.GetPositionSignatureFullNamePartialTemplateAsync("Секретар комісії",
                secretaryOfTheCommissionName);

            docBody.ReplaceNode<OpenXmlElement>(SECRETARY_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE,
                secretaryOfTheCommissionPartialTemplateNodes);


            // set field for signature chief of security department
            var chiefOfSecurityDepartmentPartialTemplateNodes =
                await _partialTemplateFactory.GetPositionSignatureFullNamePartialTemplateAsync("Начальник режимно-секретного відділу",
                chiefOfSecurityDepartmentName);

            docBody.ReplaceNode<OpenXmlElement>(CHIEF_OF_THE_SECURITY_DEPARTMENT_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE,
                chiefOfSecurityDepartmentPartialTemplateNodes);
        }

        private async Task SetDateInFormat_ddMMyyyyAsync(Body docBody, DateTime? date)
        {
            var partialTemplateNodes = await _partialTemplateFactory.GetDatePartialTemplateAsync(DateFormats.ddMMyyyy, date);

            docBody.ReplaceNode<OpenXmlElement>(DATE_IN_FORMAT_ddMMyyyy_PLACEHOLDER_IN_TEMPLATE, partialTemplateNodes);
        }


        private string ScientistToStringDegreesFirst(Scientist scientist)
        {
            string resultString = "";

            foreach (var degree in scientist.Degrees)
            {
                resultString += degree + ", ";
            }

            resultString = resultString.Trim(' ', ',');

            resultString += $" {scientist.FullName}";

            return resultString;
        }

        private string ScientistToStringNameFirst(Scientist scientist)
        {
            string resultString = "";

            resultString += $"{scientist.FullName}, ";

            foreach (var degree in scientist.Degrees)
            {
                resultString += degree + ", ";
            }

            resultString = resultString.Trim(' ', ',');

            return resultString;
        }

        /// <summary>
        /// fullName should be in that format: Surname Name Father'sName
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private string GetFromFullNameSurnameAndInitials(string fullName)
        {
            string surnameWithInitials = "";

            var partsOfFullName = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (partsOfFullName.Length < 3)
                surnameWithInitials = fullName;
            else
            {
                surnameWithInitials = partsOfFullName.First() + " "; // surname 
                partsOfFullName.Skip(1) // skip surname
                    .Select(s => s.Substring(0, 1) + ".")
                    .ToList()
                    .ForEach(e => surnameWithInitials += e); // get initials
            }

            return surnameWithInitials;
        }
    }
}
