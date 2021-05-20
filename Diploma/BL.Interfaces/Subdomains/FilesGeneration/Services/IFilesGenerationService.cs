﻿using BL.Models.FilesGeneration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BL.Interfaces.Subdomains.FilesGeneration
{
    public interface IFilesGenerationService
    {
        Task<CreatedFileModel> CreateNotesOfAuthorsAsync(SaveNoteOfAuthorsModel saveNoteOfAuthorsModel, IEnumerable<Claim> userClaims);

        Task<CreatedFileModel> CreateExpertiseActAsync(SaveExpertiseActModel saveExpertiseActModel, IEnumerable<Claim> userClaims);

        Task<CreatedFileModel> CreateProtocolOfMeetingOfExpertCommissionAsync(SaveProtocolOfMeetingOfExpertCommissionModel saveProtocolOfMeetingOfExpertCommissionModel,
            IEnumerable<Claim> userClaims);

    }
}
