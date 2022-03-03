using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HtmlEmailInjection.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;

    [BindProperty] public SignUpForm SignUpForm { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        this.logger = logger;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] EmailSender emailSender,
        [FromServices] HtmlEncoder htmlEncoder
    )
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var firstName = htmlEncoder.Encode(SignUpForm.FirstName);
        var lastName = htmlEncoder.Encode(SignUpForm.LastName);
        var emailAddress = SignUpForm.EmailAddress;

        var subject = $"{firstName}, confirm your newsletter subscription";
        var htmlBody =
            $"Hi {firstName} {lastName}, <br />" +
            "Thank you for signing up for our newsletter. Please click the link below to confirm your subscription. <br />" +
            "<a href=\"https://localhost/confirm?token=???\">Confirm your subscription</a>";

        await emailSender.SendEmailAsync(subject, htmlBody, emailAddress);

        return new ContentResult {Content = "Thank you for signing up!"};
    }
}

public class SignUpForm
{
    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    [Required, EmailAddress] public string EmailAddress { get; set; }
}