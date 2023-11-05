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
        public RegistrationService(AppDbContext dbContext, EMailSenderService emailService)
        {
            DbContext = dbContext;
            EmailService = emailService;
        }

        public AppDbContext DbContext { get; }
        public EMailSenderService EmailService { get; }

        public async Task<string> RegisterAsync(string name,string surname, string password, string passwordConfirm,bool kvkk, string email, string phone)
        {
            string activationCode = new Random().Next(100000, 999999).ToString();

            string cancelationCode = Guid.NewGuid().ToString("n")[..16];

            var userEntity = new UserEntity
            {
                Name = name,
                Surname = surname,
                Email = email,
                Phone = phone,
                Password = HashHelper.HashToString(password),
                PasswordConfirm = HashHelper.HashToString(passwordConfirm),
                ActivationCode = activationCode,
                CancelationCode = cancelationCode,
                KVKK = kvkk
            };

            DbContext.Users.Add(userEntity);
            await DbContext.SaveChangesAsync();

            return activationCode;
        }
    }
}
