namespace Qualcosa.Services;

public class CloudMailService : IMailService
{
    private readonly string _mailTo = string.Empty;
    private readonly string _mailFrom = string.Empty;
    /*
        Per poter usare questa altra soluzione ho creato un file appsettings, chiamato "appSettings.Development.json" e sono andato
        ad inserirci dei dati. Per usarlo in debug devo fare click destro su "Qualcosa" poi proprietà, andare in Debug
        cliccare su "Aprire interfaccia utente..." e cambiare la variabile d'ambiente da "Developement" a "Production"!!!
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
