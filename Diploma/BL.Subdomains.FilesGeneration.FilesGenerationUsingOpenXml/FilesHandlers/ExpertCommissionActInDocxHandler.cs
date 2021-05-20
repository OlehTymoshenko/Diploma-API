using System;
using System.Threading.Tasks;
using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Models.FilesGeneration;
using BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils;
using DL.Entities.Enums;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.FilesHandlers
{
    public class ExpertCommissionActInDocxHandler : IExpertCommissionActHandler
    {
        public FileType Type => FileType.ExpertCommissionAct;

        public FileFormat Format => FileFormat.DOCX;

        public string TemplateName { get; init; } = @"Template_ExpertCommissionAct.docx";

        #region Placeholders in the template
        internal const string ACT_COPY_NUMBER_PLACEHOLDER_IN_TEMPLATE = @"$ActCopyNumber$";
        internal const string FACULTY_NUMBER_PLACEHOLDER_IN_TEMPLATE = @"$FacultyNumber$";
        internal const string DATE_IN_FORMAT_ddMMMMyyyy_PLACEHOLDER_IN_TEMPLATE = @"$DateInFormat_ddMMMMyyyy$";
        internal const string HEAD_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$HeadOfTheCommission$";
        internal const string SECRETARY_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$SecretaryOfTheCommission$";
        internal const string MEMBERS_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$MembersOfTheCommission$";
        internal const string SPEAKERS_PLACEHOLDER_IN_TEMPLATE = @"$Speakers$";
        internal const string PUBLISHING_NAME_WITH_ITS_STATISTIC_PLACEHOLDER_IN_TEMPLATE = @"$PublishingNameWithItsStatistic$";
        internal const string IS_THE_PUBLICATION_A_STATE_SECRET_PLACEHOLDER_IN_TEMPLATE = @"$IsPublicationStateSecret$";
        internal const string DOES_PUBLICATION_CONTAIN_SERVICE_INFO_PLACEHOLDER_IN_TEMPLATE = @"$DoesContainServiceInfo$";
        internal const string DESCRIPTION_OF_STATE_SECRETS_OR_SERVICE_INFO_PLACEHOLDER_IN_TEMPLATE = @"$DescriptionOfStateSecrectsOrServiceInformation$";
        internal const string DOES_COMMISSION_ALLOW_ISSUING_PLACEHOLDER_IN_TEMPLATE = @"$AllowIssuing$";
        internal const string HEAD_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE = @"$HeadOfTheCommissionSignatureFullName$";
        internal const string SECRETARY_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE = @"$SecretaryOfTheCommissionSignatureFullName$";
        internal const string MEMBERS_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE = @"$MembersOfTheCommissionSignatureFullName$";
        internal const string CHIEF_OF_THE_SECURITY_DEPARTMENT_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE = @"$ChiefOfSecurityDepartmentSignatureFullName$";
        internal const string DATE_IN_FORMAT_ddMMyyyy_PLACEHOLDER_IN_TEMPLATE = @"$DateInFormat_ddMMyyyy$";
        #endregion

        private readonly TemplateLoader _templateLoader;
        private readonly PartialTemplateFactory _partialTemplateFactory;


        public ExpertCommissionActInDocxHandler()
        {
            _templateLoader = new TemplateLoader();
            _partialTemplateFactory = new PartialTemplateFactory();
        }

        public async Task<FileModel> CreateFileAsync(SaveExpertCommissionActModel saveExpertCommissionActModel)
        {
            //////////////////////////////////////////////////// EXTREMELY IMPORTANT INFORMATION
            /// Order of calling methods is IMPORTANT. All async methods should be called after
            /// all sync methods. 
            /// The reason for this strange behaviour is unknown
            //////////////////////////////////////////////////// 

            using var memStream = await _templateLoader.LoadTemplateAsync(TemplateName);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            SetActCopyNumber(wordDoc, saveExpertCommissionActModel.ActCopyNumber);

            SetFacultyNumber(wordDoc, saveExpertCommissionActModel.FacultyNumber);

            SetHeadOfTheCommissionFullName(wordDoc, saveExpertCommissionActModel.HeadOfTheCommissionName);

            SetSecretaryOfTheCommissionFullName(wordDoc, saveExpertCommissionActModel.SecretaryOfTheCommissionName);

            SetMembersOfTheCommissionFullNames (wordDoc, saveExpertCommissionActModel.MembersOfTheCommissionNames);

            SetSpeakersFullNames(wordDoc, saveExpertCommissionActModel.SpeakersOfTheCommissionName);

            SetPublicationNameWithItsStatistic(wordDoc, saveExpertCommissionActModel.PublishingNameWithItsStatics);

            SetDecisionOfTheCommision(wordDoc, saveExpertCommissionActModel.IsPublicationAStateSecret, 
                saveExpertCommissionActModel.DoesPubliscationContainServiceInformation, 
                saveExpertCommissionActModel.DescriptionOfStateSecrectsOrServiceInformation,
                saveExpertCommissionActModel.DoesCommissionAllowAIssuingOfThePublication);

            await SetDateInFormat_ddMMMMyyyyAsync(wordDoc.MainDocumentPart.Document.Body,
                saveExpertCommissionActModel.ActCreationDate);

            await SetFieldsForSignatureAsync(wordDoc.MainDocumentPart.Document.Body,
                saveExpertCommissionActModel.HeadOfTheCommissionName,
                saveExpertCommissionActModel.SecretaryOfTheCommissionName,
                saveExpertCommissionActModel.MembersOfTheCommissionNames,
                saveExpertCommissionActModel.ChiefOfSecurityDepartment);

            await SetDateInFormat_ddMMyyyyAsync(wordDoc.MainDocumentPart.Document.Body,
                saveExpertCommissionActModel.ActCreationDate);

            // save wordDoc and get bytes from it
            wordDoc.Close();

            return new FileModel()
            {
                Format = this.Format,
                Type = this.Type,
                FileAsBytes = memStream.ToArray()
            };
        }



        private void SetActCopyNumber(WordprocessingDocument wordDoc, int actCopyNumber)
        {
            wordDoc.ReplaceText(ACT_COPY_NUMBER_PLACEHOLDER_IN_TEMPLATE,
                            actCopyNumber.ToString(),
                            false);
        }

        private void SetFacultyNumber(WordprocessingDocument wordDoc, int facultyNumber)
        {
            wordDoc.ReplaceText(FACULTY_NUMBER_PLACEHOLDER_IN_TEMPLATE,
                            facultyNumber.ToString(),
                            false);
        }

        private async Task SetDateInFormat_ddMMMMyyyyAsync(Body docBody, DateTime? date)
        {
            var partialTemplateNodes = await _partialTemplateFactory.GetDatePartialTemplateAsync(DateFormats.ddMMMMyyyy, date);

            docBody.ReplaceNode(DATE_IN_FORMAT_ddMMMMyyyy_PLACEHOLDER_IN_TEMPLATE, partialTemplateNodes);
        }

        private void SetHeadOfTheCommissionFullName(WordprocessingDocument wordDoc, string headOfTheCommissionFullName)
        {
            wordDoc.ReplaceText(HEAD_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE,
                            headOfTheCommissionFullName,
                            false);
        }

        private void SetSecretaryOfTheCommissionFullName(WordprocessingDocument wordDoc, string secretaryOfTheCommissionFullName)
        {
            wordDoc.ReplaceText(SECRETARY_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE,
                            secretaryOfTheCommissionFullName,
                            false);
        }

        private void SetMembersOfTheCommissionFullNames(WordprocessingDocument wordDoc, string[] membersOfTheCommissionFullNames)
        {
            string membersOfTheCommissionAsSingleString = string.Join(", ", membersOfTheCommissionFullNames)
                                                                .Trim(',', ' ');

            wordDoc.ReplaceText(MEMBERS_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE,
                            membersOfTheCommissionAsSingleString,
                            false);
        }

        private void SetSpeakersFullNames(WordprocessingDocument wordDoc, string[] speakers)
        {
            string speakersAsSingleString = string.Join(", ", speakers)
                                                  .Trim(',', ' ');

            wordDoc.ReplaceText(SPEAKERS_PLACEHOLDER_IN_TEMPLATE,
                            speakersAsSingleString,
                            false);
        }

        private void SetPublicationNameWithItsStatistic(WordprocessingDocument wordDoc, string publicationNameWithItsStatstics)
        {
            wordDoc.ReplaceText(PUBLISHING_NAME_WITH_ITS_STATISTIC_PLACEHOLDER_IN_TEMPLATE,
                            publicationNameWithItsStatstics.Trim(' ', ','),
                            false);
        }

        /// <summary>
        /// Set the fields: $IsPublicationStateSecret$, $DoesContainServiceInfo$, 
        /// $DescriptionOfStateSecrectsOrServiceInformation$, $AllowIssuing$
        /// </summary>
        private void SetDecisionOfTheCommision(WordprocessingDocument wordDoc,  bool isPublicationAStateSecret, bool doesContainServiceInfo, 
            string descriptionOfStateSecretsOrServiceInfo, bool doesCommissionAllowIssuingOfThePublication)
        {
            // IsPublicationStateSecret
            wordDoc.ReplaceText(IS_THE_PUBLICATION_A_STATE_SECRET_PLACEHOLDER_IN_TEMPLATE,
                            isPublicationAStateSecret ? "становлять" : "не становлять",
                            false);

            // DoesContainServiceInfo
            wordDoc.ReplaceText(DOES_PUBLICATION_CONTAIN_SERVICE_INFO_PLACEHOLDER_IN_TEMPLATE,
                            doesContainServiceInfo ? "містять" : "не містять",
                            false);

            // DescriptionOfStateSecrectsOrServiceInformation
            wordDoc.ReplaceText(DESCRIPTION_OF_STATE_SECRETS_OR_SERVICE_INFO_PLACEHOLDER_IN_TEMPLATE,
                            descriptionOfStateSecretsOrServiceInfo ?? " ",
                            false);

            // AllowIssuing
            wordDoc.ReplaceText(DOES_COMMISSION_ALLOW_ISSUING_PLACEHOLDER_IN_TEMPLATE,
                            doesCommissionAllowIssuingOfThePublication ? "дає" : "не дає",
                            false);
        }

        private async Task SetFieldsForSignatureAsync(Body docBody, string headOfTheCommissionName, 
            string secretaryOfTheCommissionName,
            string[] membersOfTheCommissionName,
            string chiefOfSecurityDepartmentName)
        {
            // set field for signature head of the commission
            var headOfTheCommissionPartialTemplateNodes = 
                await _partialTemplateFactory.GetPositionSignatureFullNamePartialTemplateAsync("Голова комісії", 
                headOfTheCommissionName);

            docBody.ReplaceNode(HEAD_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE, 
                headOfTheCommissionPartialTemplateNodes);


            // set field for signature secretary of the commission
            var secretaryOfTheCommissionPartialTemplateNodes =
                await _partialTemplateFactory.GetPositionSignatureFullNamePartialTemplateAsync("Секретар комісії",
                secretaryOfTheCommissionName);

            docBody.ReplaceNode(SECRETARY_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE, 
                secretaryOfTheCommissionPartialTemplateNodes);


            // set field for signature members of the commission
            var membersOfTheCommissionPartialTemplateNodes = await _partialTemplateFactory.
                GetPositionSignatureFullNamePartialTemplateAsync("Члени комісії", membersOfTheCommissionName);

            docBody.ReplaceNode(MEMBERS_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE,
                membersOfTheCommissionPartialTemplateNodes);


            // set field for signature chief of security department
            var chiefOfSecurityDepartmentPartialTemplateNodes =
                await _partialTemplateFactory.GetPositionSignatureFullNamePartialTemplateAsync("Начальник режимно-секретного відділу",
                chiefOfSecurityDepartmentName);

            docBody.ReplaceNode(CHIEF_OF_THE_SECURITY_DEPARTMENT_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE,
                chiefOfSecurityDepartmentPartialTemplateNodes);
        }

        private async Task SetDateInFormat_ddMMyyyyAsync(Body docBody, DateTime? date)
        {
            var partialTemplateNodes = await _partialTemplateFactory.GetDatePartialTemplateAsync(DateFormats.ddMMyyyy, date);

            docBody.ReplaceNode(DATE_IN_FORMAT_ddMMyyyy_PLACEHOLDER_IN_TEMPLATE, partialTemplateNodes);
        }

    }
}
