using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gfk.Mvc.Helpers;
using Gfk.Mvc.Models;
using Gfk.Mvc.Models.Entities;
using Gfk.Mvc.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using static Gfk.Mvc.Services.RegistrationService;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gfk.Mvc.Controllers
{
    public class AuthController : Controller
    {
        public AuthController(AppDbContext appDbContext,
            HashHelper hashHelper,
            EMailSenderService eMailSenderService,
            RegistrationService registrationService)
        {
            _dbContext = appDbContext;
            _hashHelper = hashHelper;
            _mailSender = eMailSenderService;
            _registration = registrationService;
        }

        public AppDbContext _dbContext { get; }
        public HashHelper _hashHelper { get; }
        public EMailSenderService _mailSender { get; }
        public RegistrationService _registration { get; }


        [HttpGet]
        public IActionResult LandingConfirmSection()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Form valid değil keke";
                return View();
            }

            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == loginViewModel.Email);

            if (user is null)
            {
                ViewBag.ErrorMessage = "Bu mail ile kayıtlı bir kullanıcı bulunmamaktadır.";
                return View();
            }

            if (HashHelper.HashToString(loginViewModel.Password) != user.Password)
            {
                // yanlışsa kullanıcı bulunamadı hatası ver
                ViewBag.ErrorMessage = "Yanlış parola, lütfen tekrar deneyiniz.";
                return View();
            }

            if (user.ActivationCode != "0")
            {
                ViewBag.ErrorMessage = "Hesabınızın aktif edilmediği görüntülenmektedir. Tarafınıza iletilen e-posta üzerinden öncelikle hesabınızı aktif etmeniz gerekmektedir.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties
            {
                IsPersistent = loginViewModel.RememberMe
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            Response.Cookies.Append("UserName", user.Name, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(1), // Cookie'nin süresi, isteğinize bağlı olarak ayarlanabilir
                HttpOnly = true // JavaScript ile erişimi engellemek için
            });

            Response.Cookies.Append("EmailAddress", user.Email, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(1),
                HttpOnly = true
            });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("UserName"); //Bunu silmediğim için çıkış yapsamda, cookilerde username kalıyordu. Bu işlemle Cache' den de oluşturduğum cookie' yi silmiş oldum.

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Lütfen form bilgilerini kontrol ediniz.";
                return View();
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user != null && user.Email == model.Email)
            {
                ViewBag.ErrorMessage = "Zaten böyle bir kullanıcı var.";
                ModelState.Clear();
                return View();
            }

            if (HashHelper.HashToString(model.Password) != HashHelper.HashToString(model.PasswordConfirm))
            {
                ViewBag.ErrorMessage = "İki farklı şifre girdiniz, bir daha deneyiniz.";
                ModelState.Clear();
                return View(model);
            }

            var registrationResult = await _registration.RegisterAsync(model.Name, model.Surname, model.Password, model.PasswordConfirm, model.KVKK, model.Email, model.Phone);

            var tokenActivationCode = registrationResult.TokenActivationCode;
            var activationCode = registrationResult.ActivationCode;

            var confirmationUrl = Url.Action(nameof(Confirm),
                "Auth",
                new { id = tokenActivationCode },
                Request.Scheme,
                Request.Host.ToString());

            var cancelingUrl = Url.Action(nameof(ContactPermissionCanceling),
                "Auth",
                new { id = tokenActivationCode },
                Request.Scheme,
                Request.Host.ToString());

            await _mailSender.SendEmailAsync(model.Email, "Hesap Onayı", confirmationUrl, cancelingUrl, activationCode, tokenActivationCode);

            ViewBag.SuccessMessage = "Kayıt işlemi başarılı. Hesabınızı onayladıktan sonra giriş yapabilirsiniz.";
            ModelState.Clear();

            return RedirectToAction(nameof(LandingConfirmSection));
        }

        [HttpGet]
        public async Task<IActionResult> Confirm([FromRoute] string tokenActivationCode)
        {
            var user = _dbContext.Users.SingleOrDefaultAsync(x => x.TokenActivationCode == tokenActivationCode);

            if (user is null)
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı.";
                return View();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Confirm([FromRoute] string id, [FromForm] ActivationAccountViewModel model)
        {
            var tokenActivationCode = id;


            if (tokenActivationCode is null)
            {
                tokenActivationCode = model.User?.TokenActivationCode;

            }
            if (string.IsNullOrWhiteSpace(tokenActivationCode))
            {
                ViewBag.ErrorMessage = "Hatalı kod";
                return View();
            }

            var tokenActivationCodeFromModel = model.tokenVerificationCode;

            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.ActivationCode == model.verificationCode);
            if (user is null)
            {
                ViewBag.ErrorMessage = "Hatalı kod";
                return View();
            }

            if(model.verificationCode != user.ActivationCode)
            {
                ViewBag.ErrorMessage = "Kodlar doğru bir şekilde eşleşmedi. Destek ile irtibate geçiniz.";
                return View();
            }

            int tempNo = 0;
            user.ActivationCode = tempNo.ToString();

            if (user.ActivationCode == "0")
            {

                await _dbContext.SaveChangesAsync();

                ViewBag.SuccessMessage = "Kullanıcı aktifleştirildi.";

                return RedirectToAction("WelcomeSession", "Home");
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ContactPermissionCanceling()
        {
            var emailAddress = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            var user = !string.IsNullOrEmpty(emailAddress)
                ? await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == emailAddress)
                : null;

            var model = user ?? new UserEntity();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ContactPermissionCanceling([FromForm] UserEntity userEntity)
        {
            if (User.Identity.IsAuthenticated)
            {
                var emailAddress = userEntity.Email;

                if (string.IsNullOrWhiteSpace(emailAddress))
                {
                    ViewBag.ErrorMessage = "Lütfen bir elektronik posta adresi girin.";
                    ModelState.Clear();
                    return View();
                }

                var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == emailAddress);
                if (user is null)
                {
                    ViewBag.ErrorMessage = "Böyle bir maile kayıtlı kullanıcı bulunamadı.";
                    ModelState.Clear();
                    return View();
                }

                if (user.Email != HttpContext.Request.Cookies["EmailAddress"] && HttpContext.Request.Cookies["EmailAddress"] == null)
                {
                    ViewBag.ErrorMessage = "Lütfen kendi e-posta adresinizi girin";
                    ModelState.Clear();
                    return View();
                }

                if (user.KVKK == false)
                {
                    ViewBag.ErrorMessage = "Zaten iletişim izinlerini iptal ettiniz. Hala sorun yaşıyor iseniz info@gaziemirfk.com adresi ile iletişime geçebilirsiniz.";
                    ModelState.Clear();
                    return View();
                }

                user.CancelationCode = "0";
                user.KVKK = false;

                await _dbContext.SaveChangesAsync();
                ViewBag.SuccessMessage = "İletişim izinleri başarılı şekilde iptal edilmiştir. Bundan sonra kulübümüz tarafından sizlere tanıtım ve duyuru elektronik postası iletilmeyecektir.";
                return View();
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

    }
}

