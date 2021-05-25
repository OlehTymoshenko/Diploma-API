using System;
using System.Linq;
using System.Threading.Tasks;
using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Interfaces.Subdomains.FilesGeneration.Services;
using BL.Models.FilesGeneration;
using BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils;
using DL.Entities.Enums;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.FilesHandlers
{
    public class ProtocolOfMeetingOfExpertCommissionInDocxHandler : IProtocolOfMeetingOfExpertCommissionHandler
    {
        public FileType Type => FileType.ProtocolOfMeetingOfExpertCommission;

        public FileFormat Format => FileFormat.DOCX;

        public string TemplateName { get; init; } = @"Template_ProtocolOfMeetingOfExpertCommission.docx";

        #region Placeholders in the template
        internal const string ACT_COPY_NUMBER_PLACEHOLDER_IN_TEMPLATE = @"$ActCopyNumber$";
        internal const string FACULTY_NUMBER_PLACEHOLDER_IN_TEMPLATE = @"$FacultyNumber$";
        internal const string DATE_IN_FORMAT_ddMMMMyyyy_PLACEHOLDER_IN_TEMPLATE = @"$DateInFormat_ddMMMMyyyy$";
        internal const string HEAD_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$HeadOfTheCommission$";
        internal const string SECRETARY_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$SecretaryOfTheCommission$";
        internal const string MEMBERS_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$MembersOfTheCommission$";
        internal const string SPEAKERS_PLACEHOLDER_IN_TEMPLATE = @"$Speakers$";
        internal const string PUBLISHING_NAME_WITH_ITS_STATISTIC_IN_GENITIVE_CASE_PLACEHOLDER_IN_TEMPLATE = @"$PublishingNameWithItsStatisticGenitiveCase$";
        internal const string PUBLISHING_NAME_WITH_ITS_STATISTIC_PREPOSITION_PLACEHOLDER_IN_TEMPLATE = @"$PublishingNameWithItsStatisticPreposition$";
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
        private IDeclensionService _declensionService;


        public ProtocolOfMeetingOfExpertCommissionInDocxHandler(IDeclensionService declensionService)
        {
            _declensionService = declensionService;
            _templateLoader = new TemplateLoader();
            _partialTemplateFactory = new PartialTemplateFactory();
        }

        public async Task<FileModel> CreateFileAsync(SaveProtocolOfMeetingOfExpertCommissionModel saveProtocolOfMeetingOfExpertCommissionModel)
        {
            //////////////////////////////////////////////////// EXTREMELY IMPORTANT INFORMATION
            /// Order of calling methods is IMPORTANT. All async methods should be called after
            /// all sync methods. 
            /// The reason for this strange behaviour is unknown
            //////////////////////////////////////////////////// 

            using var memStream = await _templateLoader.LoadTemplateAsync(TemplateName);
            using var wordDoc = WordprocessingDocument.Open(memStream, true);

            SetActCopyNumber(wordDoc, saveProtocolOfMeetingOfExpertCommissionModel.ActCopyNumber);

            SetFacultyNumber(wordDoc, saveProtocolOfMeetingOfExpertCommissionModel.FacultyNumber);

            SetHeadOfTheCommissionFullName(wordDoc, saveProtocolOfMeetingOfExpertCommissionModel.HeadOfTheCommissionName);

            SetSecretaryOfTheCommissionFullName(wordDoc, saveProtocolOfMeetingOfExpertCommissionModel.SecretaryOfTheCommissionName);

            SetMembersOfTheCommissionFullNames (wordDoc, saveProtocolOfMeetingOfExpertCommissionModel.MembersOfTheCommissionNames);

            SetSpeakersFullNames(wordDoc, saveProtocolOfMeetingOfExpertCommissionModel.SpeakersOfTheCommissionName);

            SetPublicationNameWithItsStatistic(wordDoc, saveProtocolOfMeetingOfExpertCommissionModel.PublishingNameWithItsStatics);

            SetDecisionOfTheCommision(wordDoc, saveProtocolOfMeetingOfExpertCommissionModel.IsPublicationAStateSecret, 
                saveProtocolOfMeetingOfExpertCommissionModel.DoesPubliscationContainServiceInformation, 
                saveProtocolOfMeetingOfExpertCommissionModel.DescriptionOfStateSecrectsOrServiceInformation,
                saveProtocolOfMeetingOfExpertCommissionModel.DoesCommissionAllowAIssuingOfThePublication);

            await SetDateInFormat_ddMMMMyyyyAsync(wordDoc.MainDocumentPart.Document.Body,
                saveProtocolOfMeetingOfExpertCommissionModel.ProtocolCreationDate);

            await SetFieldsForSignatureAsync(wordDoc.MainDocumentPart.Document.Body,
                saveProtocolOfMeetingOfExpertCommissionModel.HeadOfTheCommissionName,
                saveProtocolOfMeetingOfExpertCommissionModel.SecretaryOfTheCommissionName,
                saveProtocolOfMeetingOfExpertCommissionModel.MembersOfTheCommissionNames,
                saveProtocolOfMeetingOfExpertCommissionModel.ChiefOfSecurityDepartment);

            await SetDateInFormat_ddMMyyyyAsync(wordDoc.MainDocumentPart.Document.Body,
                saveProtocolOfMeetingOfExpertCommissionModel.ProtocolCreationDate);

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

            docBody.ReplaceNode<OpenXmlElement>(DATE_IN_FORMAT_ddMMMMyyyy_PLACEHOLDER_IN_TEMPLATE, partialTemplateNodes);
        }

        private void SetHeadOfTheCommissionFullName(WordprocessingDocument wordDoc, string headOfTheCommissionFullName)
        {
            wordDoc.ReplaceText(HEAD_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE,
                            GetFromFullNameSurnameAndInitials(headOfTheCommissionFullName),
                            false);
        }

        private void SetSecretaryOfTheCommissionFullName(WordprocessingDocument wordDoc, string secretaryOfTheCommissionFullName)
        {
            wordDoc.ReplaceText(SECRETARY_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE,
                            GetFromFullNameSurnameAndInitials(secretaryOfTheCommissionFullName),
                            false);
        }

        private void SetMembersOfTheCommissionFullNames(WordprocessingDocument wordDoc, string[] membersOfTheCommissionFullNames)
        {
            string membersOfTheCommissionAsSingleString = 
                string.Join(", ", membersOfTheCommissionFullNames.Select(n => GetFromFullNameSurnameAndInitials(n)))
                .Trim(',', ' ');

            wordDoc.ReplaceText(MEMBERS_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE,
                            membersOfTheCommissionAsSingleString,
                            false);
        }

        private void SetSpeakersFullNames(WordprocessingDocument wordDoc, string[] speakers)
        {
            string speakersAsSingleString =
                string.Join(", ", speakers.Select(n => GetFromFullNameSurnameAndInitials(n)))
                .Trim(',', ' ');

            wordDoc.ReplaceText(SPEAKERS_PLACEHOLDER_IN_TEMPLATE,
                            speakersAsSingleString,
                            false);
        }

        private void SetPublicationNameWithItsStatistic(WordprocessingDocument wordDoc, string publicationNameWithItsStatstics)
        {
            // example: "навчальний посібник"
            int indexOfStartNameOfScientificWork = publicationNameWithItsStatstics.IndexOfAny(new char[] { '«', '"' });
            string typeOfScientificWork = publicationNameWithItsStatstics.Substring(0, indexOfStartNameOfScientificWork);

            var ukrInflectedTypeOfScientificWork = _declensionService.ParseUkr(typeOfScientificWork);

            var resultScientificPublicationNameInGenetiveCase = ukrInflectedTypeOfScientificWork.Genitive +
                publicationNameWithItsStatstics.Substring(indexOfStartNameOfScientificWork).Trim(' ', ',');

            var resultScientificPublicationNamePreposition = ukrInflectedTypeOfScientificWork.Prepositional +
                publicationNameWithItsStatstics.Substring(indexOfStartNameOfScientificWork).Trim(' ', ',');

            wordDoc.ReplaceText(PUBLISHING_NAME_WITH_ITS_STATISTIC_IN_GENITIVE_CASE_PLACEHOLDER_IN_TEMPLATE,
                            resultScientificPublicationNameInGenetiveCase.Trim(' ', ','),
                            false);

            wordDoc.ReplaceText(PUBLISHING_NAME_WITH_ITS_STATISTIC_PREPOSITION_PLACEHOLDER_IN_TEMPLATE,
                            resultScientificPublicationNamePreposition.Trim(' ', ','),
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
                GetFromFullNameSurnameAndInitials(headOfTheCommissionName));

            docBody.ReplaceNode<OpenXmlElement>(HEAD_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE, 
                headOfTheCommissionPartialTemplateNodes);


            // set field for signature secretary of the commission
            var secretaryOfTheCommissionPartialTemplateNodes =
                await _partialTemplateFactory.GetPositionSignatureFullNamePartialTemplateAsync("Секретар комісії",
                GetFromFullNameSurnameAndInitials(secretaryOfTheCommissionName));

            docBody.ReplaceNode<OpenXmlElement>(SECRETARY_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE, 
                secretaryOfTheCommissionPartialTemplateNodes);


            // set field for signature members of the commission
            var membersOfTheCommissionPartialTemplateNodes = await _partialTemplateFactory.
                GetPositionSignatureFullNamePartialTemplateAsync("Члени комісії", 
                membersOfTheCommissionName.Select(n => GetFromFullNameSurnameAndInitials(n)).ToArray());

            docBody.ReplaceNode<OpenXmlElement>(MEMBERS_OF_THE_COMMISSION_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE,
                membersOfTheCommissionPartialTemplateNodes);


            // set field for signature chief of security department
            var chiefOfSecurityDepartmentPartialTemplateNodes =
                await _partialTemplateFactory.GetPositionSignatureFullNamePartialTemplateAsync("Начальник режимно-секретного відділу",
                GetFromFullNameSurnameAndInitials(chiefOfSecurityDepartmentName));

            docBody.ReplaceNode<OpenXmlElement>(CHIEF_OF_THE_SECURITY_DEPARTMENT_SIGNATURE_FULLNAME_PLACEHOLDER_IN_TEMPLATE,
                chiefOfSecurityDepartmentPartialTemplateNodes);
        }

        private async Task SetDateInFormat_ddMMyyyyAsync(Body docBody, DateTime? date)
        {
            var partialTemplateNodes = await _partialTemplateFactory.GetDatePartialTemplateAsync(DateFormats.ddMMyyyy, date);

            docBody.ReplaceNode<OpenXmlElement>(DATE_IN_FORMAT_ddMMyyyy_PLACEHOLDER_IN_TEMPLATE, partialTemplateNodes);
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
