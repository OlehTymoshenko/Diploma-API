using System;
using System.Threading.Tasks;
using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Models.FilesGeneration;
using BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils;
using DL.Entities.Enums;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.FilesHandlers
{
    public class ExpertCommissionActInDocxHandler : IExpertCommissionActHandler
    {
        public FileType Type => FileType.ExpertCommissionAct;

        public FileFormat Format => FileFormat.DOCX;

        public string TemplateName { get; init; } = @"Template_ExpertCommissionAct.docx";

        #region Placeholders names in a template
        internal const string ACT_COPY_NUMBER_PLACEHOLDER_IN_TEMPLATE = @"$ActCopyNumber$";
        internal const string FACULTY_NUMBER_PLACEHOLDER_IN_TEMPLATE = @"$FacultyNumber$";
        internal const string DATE_PLACEHOLDER_IN_TEMPLATE = @"$DateInFormat_ddMMMMyyyy$";
        internal const string HEAD_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$HeadOfTheCommission$";
        internal const string SECRETARY_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$SecretaryOfTheCommission$";
        internal const string MEMBERS_OF_THE_COMMISSION_PLACEHOLDER_IN_TEMPLATE = @"$MembersOfTheCommission$";
        internal const string SPEAKERS_PLACEHOLDER_IN_TEMPLATE = @"$Speakers$";
        internal const string PUBLISHING_NAME_WITH_ITS_STATISTIC_LOCATIVE_CASE_PLACEHOLDER_IN_TEMPLATE = @"$PublishingNameWithItsStatisticLocativeCase$";
        internal const string IS_THE_PUBLICATION_A_STATE_SECRET_PLACEHOLDER_IN_TEMPLATE = @"$IsPublicationStateSecret$";
        internal const string DOES_PUBLICATION_CONTAIN_SERVICE_INFO_PLACEHOLDER_IN_TEMPLATE = @"$DoesContainServiceInfo$";
        internal const string DESCRIPTION_OF_STATE_SECRETS_OR_SERVICE_INFO_PLACEHOLDER_IN_TEMPLATE = @"DescriptionOfStateSecrectsOrServiceInformation";
        // TODO: Остановился на моменте "комісія дає/не дає дозвіл"

        #endregion

        private readonly TemplateLoader _templateLoader;
        private readonly PartialTemplateFactory _partialTemplateFactory;


        public ExpertCommissionActInDocxHandler()
        {
            _templateLoader = new TemplateLoader();
            _partialTemplateFactory = new PartialTemplateFactory();
        }

        public Task<FileModel> CreateFileAsync(SaveExpertCommissionActModel saveExpertCommissionActModel)
        {
            throw new NotImplementedException();
        }
    }
}
