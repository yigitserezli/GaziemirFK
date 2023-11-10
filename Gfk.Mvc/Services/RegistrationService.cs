using System;
using System.Net;
using System.Net.Mail;
using Gfk.Mvc.Helpers;
using Gfk.Mvc.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gfk.Mvc.Services
{
	public class RegistrationService
	{
        public RegistrationService(AppDbContext dbContext,
               EMailSenderService emailService,
               HashHelper hashHelper)
        {
            DbContext = dbContext;
            EmailService = emailService;
            HashHelper = hashHelper;
        }

        public AppDbContext DbContext { get; }
        public EMailSenderService EmailService { get; }
        public HashHelper HashHelper { get; }

        public async Task<RegistrationResult> RegisterAsync(string name,string surname, string password, string passwordConfirm,bool kvkk, string email, string phone)
        {
            string activationCode = new Random().Next(100000, 999999).ToString();

            string tokenActivationCode = HashHelper.HashToString(activationCode);

            string cancelationCode = Guid.NewGuid().ToString("n")[..16];

            var userEntity = new UserEntity
            {
                Name = name,
                Surname = surname,
                Email = email,
                Phone = PhoneFormatter.FormatPhone(phone),
                Password = HashHelper.HashToString(password),
                PasswordConfirm = HashHelper.HashToString(passwordConfirm),
                ActivationCode = activationCode,
                TokenActivationCode = tokenActivationCode,
                CancelationCode = cancelationCode,
                KVKK = kvkk,
                Role = "user"
            };

            DbContext.Users.Add(userEntity);
            await DbContext.SaveChangesAsync();

            return new RegistrationResult
            {
                ActivationCode = activationCode,
                TokenActivationCode = tokenActivationCode
            };
        }

        public class RegistrationResult
        {
            public string ActivationCode { get; set; } = string.Empty;
            public string TokenActivationCode { get; set; } = string.Empty;
        }
    }
}
