
namespace Common.Configurations.Sections
{
    public class AppSettingsSection
    {
        public string Environment { get; set; }

        public string Secret { get; set; }

        public string GeneratedFilesPath { get; set; }

        public string ExceptionLogs { get; set; }

        public string ClientUrl { get; set; }
    }
}
