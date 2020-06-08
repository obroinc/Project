using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ClubWestRFC.Models;
using ClubWestRFC.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace ClubWestRFC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //Adding additional properties to the regestry page

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Second Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Please fill in Phone number to allow club to communicate via Whats app groups")]
            [RegularExpression(@"^([\+][0-9]{1,3}([ \.\-])?)?([\(]{1}[0-9]{3}[\)])?([0-9A-Z \.\-]{1,32})((x|ext|extension)?[0-9]{1,4}?)$",
                ErrorMessage="Not a correct phone number" )]
            public string PhoneNumber { get; set; }

            [Display(Name = "If a player please fill in details")]
            public string FirstNameFamily1 { get; set; }

            public string LastNameFamily1 { get; set; }

            [Display(Name = "If a player please enter DOB")]
            [DataType(DataType.Date)]
           [DisplayFormat(DataFormatString = "{0:dd-mm-yy}", ApplyFormatInEditMode = true)]
            public DateTime? DOBFamily1 { get; set; }

            //[Display(Name = "What team would they wish to play on?")]
            //public string TeamFamily1 { get; set; }


            //Additional Family members 2 of 2

            [Display(Name = "If a player please fill in details")]
            public string FirstNameFamily2 { get; set; }

            public string LastNameFamily2 { get; set; }

            [Display(Name = "If a player please enter DOB")]
            [DataType(DataType.Date)]
          //  [DisplayFormat(DataFormatString = "{0:dd-mm-yy}", ApplyFormatInEditMode = true)]
            public DateTime? DOBFamily2 { get; set; }

            //[Display(Name = "What team would they wish to play on?")]
            //public string TeamFamily2 { get; set; }



        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }




        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //adding the roles
            string role = Request.Form["rdUserRole"].ToString();



            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PhoneNumber = Input.PhoneNumber,                   
                    FirstNameFamily1 = Input.FirstNameFamily1,
                    LastNameFamily1 = Input.LastNameFamily1,
                    FirstNameFamily2 = Input.FirstNameFamily2,
                    LastNameFamily2 = Input.LastNameFamily2,
                    DOBFamily1 = Input.DOBFamily1,
                    DOBFamily2 = Input.DOBFamily2


                };
                var result = await _userManager.CreateAsync(user, Input.Password);

                //Creating roles
                if (!await _roleManager.RoleExistsAsync(SD.ShopAdminRole))
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.ShopAdminRole)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.CoachRole)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.MemberRole)).GetAwaiter().GetResult();
                }



                if (result.Succeeded)
                {

                    //Assigning roles
                    if (role == SD.ShopAdminRole)
                    {
                        await _userManager.AddToRoleAsync(user, SD.ShopAdminRole);
                    }
                    else
                    {
                        if (role == SD.CoachRole)
                        {
                            await _userManager.AddToRoleAsync(user, SD.CoachRole);
                        }
                        else
                        {
                            if (role == SD.AdminRole)
                            {
                                await _userManager.AddToRoleAsync(user, SD.AdminRole);
                            }
                            else
                            {
                                await _userManager.AddToRoleAsync(user, SD.MemberRole);
                            }
                        }
                    }


                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                // If we got this far, something failed, redisplay form
                return Page();
            }
        }
    }

