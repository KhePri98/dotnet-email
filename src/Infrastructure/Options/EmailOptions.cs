namespace Alanyang.DotNetEmail.Infrastructure.Options;

public class EmailOptions
{
    public EmailOptions()
    {
        
    }

    public string ApplicationName { get; set; } = string.Empty;
    public string ApplicationEmailAddress { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public bool RequiresAuthentication { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}