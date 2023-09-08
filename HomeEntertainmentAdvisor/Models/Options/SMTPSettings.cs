namespace HomeEntertainmentAdvisor.Models.Options
{
    public class SMTPSettings
    {
        public SMTPSettings()
        {
            Mail="";
            DisplayName="";
            Password="";
            Host="";
        }

        public SMTPSettings(string mail, string displayName, string password, string host, int port)
        {
            Mail=mail;
            DisplayName=displayName;
            Password=password;
            Host=host;
            Port=port;
        }

        public string Mail { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }
}
