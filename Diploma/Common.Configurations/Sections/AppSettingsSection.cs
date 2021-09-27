
namespace Common.Configurations.Sections
{
    public class AppSettingsSection
    {
        public string Environment { get; set; }

        public string Secret { get; set; }

        public string PasswordSalt { get; set; }
    }
}
