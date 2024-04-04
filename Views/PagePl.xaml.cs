namespace MauiAppMVVM.Views;
using Microsoft.Maui.ApplicationModel.Communication;

public partial class PagePl : ContentPage
{
    public PagePl()
    {
        InitializeComponent();
    }

    private async void SelectContactButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var contact = await Contacts.Default.PickContactAsync();

            if (contact == null)
                return;

            string id = contact.Id;
            string namePrefix = contact.NamePrefix;
            string givenName = contact.GivenName;
            string middleName = contact.MiddleName;
            string familyName = contact.FamilyName;
            string nameSuffix = contact.NameSuffix;
            string displayName = contact.DisplayName;
            List<ContactPhone> phones = contact.Phones;
            List<ContactEmail> emails = contact.Emails;
        }
        catch (Exception ex)
        {
            DisplayAlert("Alerta", $"Error a Leer Contactos {ex.Message} ", "OK");
        }
    }

    public async IAsyncEnumerable<string> GetContactNames()
    {
        var contacts = await Contacts.Default.GetAllAsync();


        if (contacts == null)
            yield break;

        foreach (var contact in contacts)
            yield return contact.DisplayName;
    }

    public async Task SendEmail()
    {
        if (Email.Default.IsComposeSupported)
        {
            string subject = "Hello friends!";
            string body = "It was great to see you last weekend.";
            string[] recipients = new[] { "john@contoso.com", "jane@contoso.com" };

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = new List<string>(recipients)
            };

            await Email.Default.ComposeAsync(message);
        }
        else
        {

            Console.WriteLine("La composición de correos electrónicos no es compatible en este dispositivo.");
        }
    }
    public async Task SendSMS()
    {
        if (Sms.Default.IsComposeSupported)
        {
            string[] recipients = new[] { "000-000-0000" };
            string text = "Hola como estas.";

            var message = new SmsMessage(text, recipients);

            await Sms.Default.ComposeAsync(message);
        }
    }
}
    
    