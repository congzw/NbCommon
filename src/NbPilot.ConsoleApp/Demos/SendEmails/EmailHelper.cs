using System;
using System.Net;
using System.Net.Mail;
using NbPilot.Common;

namespace NbPilot.ConsoleApp.Demos.SendEmails
{
    public class EmailDemo
    {
        public static void Run()
        {
            Console.WriteLine("BEGIN");
            Send();
            Console.WriteLine("DONE");
        }
        
        private static void Send()
        {
            var simpleEmailConfigService = new SimpleEmailConfigRepository();
            var simpleEmailConfig = simpleEmailConfigService.GetSimpleEmailConfig();

            var simpleEmailHelper = new SimpleEmailHelper();
            var simpleEmail = new SimpleEmail()
            {
                EmailTo = "46074987@qq.com",
                Body = "<h2>Hello, I'm just writing this to say Hi!</h2>",
                Subject = "Hello",
                IsBodyHtml = true
            };
            string message;
            var success = simpleEmailHelper.TrySendEmail(simpleEmailConfig, simpleEmail, out message);
            Console.WriteLine(success);
            Console.WriteLine(message);
        }
    }

    #region Common

    public class MessageResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    #region SimpleEmail Helpers


    public interface ISimpleEmailHelper
    {
        bool TrySendEmail(SimpleEmailConfig config, SimpleEmail simpleEmail, out string message);
    }

    public class SimpleEmailConfig
    {
        //Yahoo!	smtp.mail.yahoo.com	587	Yes
        //GMail	smtp.gmail.com	587	Yes
        //Hotmail	smtp.live.com	587	Yes

        public string SmtpClientAddress { get; set; }
        public int SmtpClientPort { get; set; }
        public bool EnableSsl { get; set; }
        public string EmailFrom { get; set; }
        public string EmailPassword { get; set; }
    }

    public class SimpleEmail
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
    }

    public class SimpleEmailHelper : ISimpleEmailHelper
    {
        public bool TrySendEmail(SimpleEmailConfig config, SimpleEmail simpleEmail, out string message)
        {
            message = "";
            if (config == null || simpleEmail == null)
            {
                return false;
            }

            var success = false;
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(config.EmailFrom);
                    mail.To.Add(simpleEmail.EmailTo);
                    mail.Subject = simpleEmail.Subject;
                    mail.Body = simpleEmail.Body;
                    mail.IsBodyHtml = simpleEmail.IsBodyHtml; //Set to false, if send pure text.

                    //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                    //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

                    using (var smtp = new SmtpClient(config.SmtpClientAddress, config.SmtpClientPort))
                    {
                        smtp.Credentials = new NetworkCredential(config.EmailFrom, config.EmailPassword);
                        smtp.EnableSsl = config.EnableSsl;
                        smtp.Send(mail);
                        message = "邮件发送成功";
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                success = false;
            }
            return success;
        }
    }

    #endregion

    #endregion

    #region Domain
    
    #region PasswordResetByEmail Service

    public interface IPasswordResetByEmailService
    {
        MessageResult SendEmail(SimpleEmail email);
        MessageResult Validate(ResetToken token);
        MessageResult Reset(ResetPasswordDto resetPasswordDto);
    }

    public class ResetPasswordDto
    {
        public ResetToken Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public MessageResult ValidateSelf()
        {
            //todo
            return null;
        }
    }

    public class ResetToken
    {
        public string Key { get; set; }

        public MessageResult ValidateSelf()
        {
            //todo
            return null;
        }
    }

    public class PasswordResetByEmailService : IPasswordResetByEmailService
    {
        private readonly IResetPasswordByEmailRepository _resetPasswordByEmailRepository;
        private readonly ISimpleEmailConfigRepository _simpleEmailConfigRepository;
        private readonly ISimpleEmailHelper _simpleEmailHelper;
        private readonly INbAuthenticationManager _authenticationManager;

        public PasswordResetByEmailService(IResetPasswordByEmailRepository resetPasswordByEmailRepository,
            ISimpleEmailConfigRepository simpleEmailConfigRepository,
            ISimpleEmailHelper simpleEmailHelper,
            INbAuthenticationManager authenticationManager)
        {
            _resetPasswordByEmailRepository = resetPasswordByEmailRepository;
            _simpleEmailConfigRepository = simpleEmailConfigRepository;
            _simpleEmailHelper = simpleEmailHelper;
            _authenticationManager = authenticationManager;
        }

        public MessageResult SendEmail(SimpleEmail email)
        {
            //get email config
            //send email
            //save ResetPasswordByEmail
            //done!
            throw new NotImplementedException();
        }

        public MessageResult Validate(ResetToken token)
        {
            throw new NotImplementedException();
        }

        public MessageResult Reset(ResetPasswordDto resetPasswordDto)
        {
            //validate
            //get ResetPasswordByEmail
            //reset password
            //process ResetPasswordByEmail (remove or change record status)
            //event bus & done!
            throw new NotImplementedException();
        }
    }

    #endregion

    #region ResetPasswordByEmail & Reposigory

    public interface IResetPasswordByEmailRepository
    {
        ResetPasswordByEmail GetByTokenKey(string tokenKey);
        void Save(ResetPasswordByEmail resetPasswordByEmail);
        MessageResult Process(ResetPasswordByEmail entity);
    }

    public class ResetPasswordByEmail
    {
        public virtual string TokenKey { get; set; }
        public virtual string Email { get; set; }
        public virtual bool Processed { get; set; }
        public virtual DateTime CreateTime { get; set; }
    }

    #endregion

    #region SimpleEmailConfig Repository

    public interface ISimpleEmailConfigRepository
    {
        SimpleEmailConfig GetSimpleEmailConfig();
        void SaveSimpleEmailConfig(SimpleEmailConfig config);
    }

    public class SimpleEmailConfigRepository : ISimpleEmailConfigRepository
    {
        public SimpleEmailConfig GetSimpleEmailConfig()
        {
            //todo read from config 
            //todo encrypt/decrypt EmailPassword
            var config = new SimpleEmailConfig();
            config.SmtpClientAddress = "smtp.mxhichina.com";
            config.SmtpClientPort = 25;
            config.EnableSsl = true;
            config.EmailFrom = "nbservice@zqnb.com.cn";
            config.EmailPassword = "{Password}"; //Company[Zq..]+1~5 
            return config;
        }

        public void SaveSimpleEmailConfig(SimpleEmailConfig config)
        {
            //todo save from config 
            //todo encrypt/decrypt EmailPassword
            throw new NotImplementedException();
        }
    }

    #endregion

    #region outer refs

    public interface INbAuthenticationManager
    {
        MessageResult ResetUserPassword(string identity, string newPassword);
    }

    public interface IAccountService
    {
        object GetAccount(string email);
    }

    #endregion

    #endregion

    #region App

    public class AccountController
    {
        private readonly IPasswordResetByEmailService _passwordResetByEmailService;
        private readonly IAccountService _accountService;
        private readonly ISimpleEmailConfigRepository _simpleEmailConfigRepository;
        private readonly ISimpleEmailHelper _simpleEmailHelper;

        public AccountController(IPasswordResetByEmailService passwordResetByEmailService, IAccountService accountService, ISimpleEmailConfigRepository simpleEmailConfigRepository, ISimpleEmailHelper simpleEmailHelper)
        {
            _passwordResetByEmailService = passwordResetByEmailService;
            _accountService = accountService;
            _simpleEmailConfigRepository = simpleEmailConfigRepository;
            _simpleEmailHelper = simpleEmailHelper;
        }

        //[HttpPost]
        public void SendResetEmail(string email)
        {
            //validate if should send email format...
            var account = _accountService.GetAccount(email);
            if (account == null)
            {
                return;
            }
            var simpleEmail = new SimpleEmail();
            //init email values
            var messageResult = _passwordResetByEmailService.SendEmail(simpleEmail);
            //return View();
        }
        //[HttpGet]
        public void ResetPassword(ResetToken token)
        {
            var vr = ValidateResetToken(token);
            if (!vr.Success)
            {
                throw new NbException(vr.Message);
            }
            //return View();
        }

        //[HttpPost]
        public void ResetPassword(ResetPasswordDto dto)
        {
            var vr = dto.ValidateSelf();
            if (!vr.Success)
            {
                throw new NbException(vr.Message);
            }
            vr = ValidateResetToken(dto.Token);
            if (!vr.Success)
            {
                throw new NbException(vr.Message);
            }

            vr = _passwordResetByEmailService.Reset(dto);
            if (!vr.Success)
            {
                throw new NbException(vr.Message);
            }
            //return Json(vr);
        }

        private MessageResult ValidateResetToken(ResetToken token)
        {
            var vr = token.ValidateSelf();
            if (!vr.Success)
            {
                return vr;
            }
            vr = _passwordResetByEmailService.Validate(token);
            if (!vr.Success)
            {
                return vr;
            }
            return new MessageResult() { Success = true, Message = "ResetToken Validate OK" };
        }
    }

    #endregion
}
