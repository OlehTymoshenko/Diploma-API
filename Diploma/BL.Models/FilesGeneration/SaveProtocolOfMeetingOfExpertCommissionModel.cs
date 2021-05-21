using DL.Entities.Enums;
using System;


namespace BL.Models.FilesGeneration
{
    public class SaveProtocolOfMeetingOfExpertCommissionModel
    {
        public FileFormat Format { get; set; }

        public int ActCopyNumber { get; set; }

        public int FacultyNumber { get; set; }

        public DateTime ProtocolCreationDate { get; set; }

        public string HeadOfTheCommissionName { get; set; }

        public string SecretaryOfTheCommissionName { get; set; }

        public string[] MembersOfTheCommissionNames { get; set; }

        public string[] SpeakersOfTheCommissionName { get; set; }

        public string PublishingNameWithItsStatics { get; set; }

        public bool IsPublicationAStateSecret { get; set; }

        public bool DoesPubliscationContainServiceInformation { get; set; }

        public string DescriptionOfStateSecrectsOrServiceInformation { get; set; }

        public bool DoesCommissionAllowAIssuingOfThePublication { get; set; }

        public string ChiefOfSecurityDepartment { get; set; }

    }
}
