namespace Qualcosa.Services;

public class CloudMailService : IMailService
{
    private readonly string _mailTo = string.Empty;
    private readonly string _mailFrom = string.Empty;
    /*
        Dal momento in cui passo i dati tramite il file json posso modificare queste 2 stringhe come qua sopra!
        private string _mailTo = "admin@mycompany.com";
        private string _mailFrom = "noreply@mycompany.com";
    */
    public CloudMailService(IConfiguration configuration)
    {
        _mailTo = configuration["mailSettings:mailToAddress"];
        _mailFrom = configuration["mailSettings:mailFromAddress"];

        /* Questo sopra posso farlo perchè ho aggiunto questo comando json
         * 
         * "mailSettings": {
            "mailToAddress": "admin@company.com",
            "mailFromAddress": "noreply@company.com"
            }

            al file appsettings.json, il programma andrà a prendere i dati dal file!!!
         */
    }

    public void Send(string subject, string message)
    {
        //send mail - output to console window
        Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " +
            $"with {nameof(CloudMailService)}.");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
    }
}
