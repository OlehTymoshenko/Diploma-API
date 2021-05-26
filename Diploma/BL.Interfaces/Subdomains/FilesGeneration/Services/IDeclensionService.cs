using BL.Models.FilesGeneration;

namespace BL.Interfaces.Subdomains.FilesGeneration.Services
{
    public interface IDeclensionService
    {
        InflectedUkrText ParseUkr(string lemma);
    }
}
