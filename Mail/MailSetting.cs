namespace PRN222_TaskManagement.Mail
{
    public class MailSetting
    {
        public string Mail {  get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }

        public int Port { get; set; }
        public string AppPassword { get; set; }

        public bool EnableSSL { get; set; }
    }
}
