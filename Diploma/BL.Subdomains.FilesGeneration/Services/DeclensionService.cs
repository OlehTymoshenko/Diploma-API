using System;
using BL.Interfaces.Subdomains.FilesGeneration.Services;
using BL.Models.FilesGeneration;
using Common.Configurations.Sections;
using Microsoft.Extensions.Options;
using Morpher.WebService.V3;

namespace BL.Subdomains.FilesGeneration.Services
{
    public class DeclensionService : IDeclensionService
    {
        private MorpherClient _morpherClient;

        private MorpherSection _morpherSection;

        public DeclensionService(IOptions<MorpherSection> morpherOptions)
        {
            _morpherSection = morpherOptions?.Value ?? throw new ArgumentNullException("IOptions<MorpherOptions> argument doesn't containt value");

            _morpherClient = new MorpherClient(_morpherSection.Token, _morpherSection.MorpherWebApiUrl);
        }

        public InflectedUkrText ParseUkr(string lemma)
        {
            InflectedUkrText result;
            
            try
            {
                var declensionResult = _morpherClient.Ukrainian.Parse(lemma);

                result = new InflectedUkrText()
                {
                    Nominative = declensionResult.Nominative,
                    Accusative = declensionResult.Accusative,
                    Dative = declensionResult.Dative,
                    Genitive = declensionResult.Genitive,
                    Instrumental = declensionResult.Instrumental,
                    Prepositional = declensionResult.Prepositional,
                    Vocative = declensionResult.Vocative
                };
            }
            catch(DailyLimitExceededException)
            {
                result = GetInflectedUrkTextWithTheSameValueInAllLemmasForms(lemma);   
            }

            return result;
        }


        private InflectedUkrText GetInflectedUrkTextWithTheSameValueInAllLemmasForms(string value)
        {
            return new InflectedUkrText()
            {
                Nominative = value,
                Accusative = value,
                Dative = value,
                Genitive = value,
                Instrumental = value,
                Prepositional = value,
                Vocative = value
            };
        }
    }
}
