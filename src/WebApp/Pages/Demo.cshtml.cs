using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Alanyang.DotNetEmail.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class DemoModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IEmailSendingQueue _emailSendingQueue;
    private readonly IEmailSender _emailSender;

    [BindProperty]
    public EmailModel EmailModel { get; set; } = default!;
    public string Message { get; set; } = string.Empty;

    public DemoModel(
        ILogger<IndexModel> logger,
        IEmailSendingQueue emailSendingQueue,
        IEmailSender emailSender)
    {
        _logger = logger;
        _emailSendingQueue = emailSendingQueue;
        _emailSender = emailSender;
    }

    public void OnGet()
    {

    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var cc = new List<string>();
        var bcc = new List<string>();
        var attachments = new List<Attachment>();
        if (EmailModel.Cc != null)
        {
            cc.Add(EmailModel.Cc);
        }
        if (EmailModel.Bcc != null)
        {
            bcc.Add(EmailModel.Bcc);
        }
        if (EmailModel.FormFile != null)
        {
            // add a single file
            var memoryStream = new MemoryStream();
            await EmailModel.FormFile.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var attachment = new Attachment(memoryStream, EmailModel.FormFile.FileName);
            attachments.Add(attachment);
        }

        await _emailSender.SendEmailAsync(
            recipient: EmailModel.Email,
            subject: EmailModel.Subject,
            body: EmailModel.Text,
            cc: cc,
            bcc: bcc,
            attachments: attachments
        );
        Message = "已寄出";
        return Page();
    }
    
    public async Task<IActionResult> OnPostAddToQueueAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        var cc = new List<string>();
        var bcc = new List<string>();
        var attachments = new List<Attachment>();
        if (EmailModel.Cc != null)
        {
            cc.Add(EmailModel.Cc);
        }
        if (EmailModel.Bcc != null)
        {
            bcc.Add(EmailModel.Bcc);
        }
        if (EmailModel.FormFile != null)
        {
            // add a single file
            var memoryStream = new MemoryStream();
            await EmailModel.FormFile.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var attachment = new Attachment(memoryStream, EmailModel.FormFile.FileName);
            attachments.Add(attachment);
        }

        await _emailSendingQueue.AddEmailAsync(
            recipient: EmailModel.Email,
            subject: EmailModel.Subject,
            body: EmailModel.Text,
            cc: cc,
            bcc: bcc,
            attachments: attachments
        );
        Message = "已加入queue";
        return Page();
    }
    
    public async Task<IActionResult> OnPostSendFromQueueAsync()
    {
        await _emailSendingQueue.SendEmailAsync(5);
        Message = "已從queue寄送5封信";
        return Page();
    }
}

public class EmailModel
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string? Cc { get; set; }
    public string? Bcc { get; set; }
    public string Subject { get; set; } = null!;
    public string Text { get; set; } = null!;
    public IFormFile? FormFile { get; set; }
}