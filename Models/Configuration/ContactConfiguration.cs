namespace VoluntariosConectadosRD.Models.Configuration
{
    public class ContactConfiguration
    {
        public const string SectionName = "ContactInformation";
        
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public SocialMediaLinks SocialMedia { get; set; } = new();
    }

    public class SocialMediaLinks
    {
        public string Facebook { get; set; } = string.Empty;
        public string Twitter { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;
    }
}