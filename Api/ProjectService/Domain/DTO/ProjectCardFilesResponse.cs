namespace Domain.DTO
{
    public class ProjectCardFilesResponse
    {
        public byte[] Logo { get; set; }
        public string LogoMimeType { get; set; }

        public byte[] Photo { get; set; }
        public string PhotoMimeType { get; set; }

        public byte[] Documentation { get; set; }
        public string DocumentationMimeType { get; set; }
    }
}
