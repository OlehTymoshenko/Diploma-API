using System;
using DL.Entities.Enums;

namespace BL.Models.FilesGeneration
{
    public class SaveExpertiseActModel
    {
        public FileFormat Format { get; set; }

        public string ProvostName { get; set; }

        public DateTime ActCreationDate { get; set; }

        public int FacultyNumber { get; set; }

        public Scientist HeadOfTheCommission { get; set; }

        public Scientist[] MembersOfTheCommission { get; set; }

        public Scientist[] AuthorsOfThePublication { get; set; }

        public string PublishingNameWithItsStatics { get; set; }

        public string SecretaryOfTheCommission { get; set; }

        public string ChiefOfSecurityDepartment { get; set; }

    }
}
