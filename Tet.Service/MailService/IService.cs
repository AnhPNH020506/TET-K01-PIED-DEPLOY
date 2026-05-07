namespace Tet.Service.MailService;

public interface IService
{
    public Task SendMail(MailContent mailContent);
}
public class MailContent
{
    public required string To { get; set; }// ddiaj chir gui den
    public required string Subject { get; set; }//chu de(tieu de mail)
    public required string Body { get; set; }//Noi dung(Ho tro HTML) cua mail
}