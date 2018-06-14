using System;
using System.Net;
using System.Net.Mail;

namespace NbPilot.ConsoleApp.Demos.SendEmails
{
    public interface ISimpleEmailConfigService
    {
        SimpleEmailConfig GetSimpleEmailConfig();
        void SaveSimpleEmailConfig(SimpleEmailConfig config);
    }

    public class SimpleEmailConfigService : ISimpleEmailConfigService
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
            var simpleEmailConfigService = new SimpleEmailConfigService();
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
            //var smtpAddress = "smtp.mxhichina.com";
            //var portNumber = 25;
            //var emailFrom = "nbservice@zqnb.com.cn";
            //var emailFromPassword = "Zqnb12345";

            //var emailTo = "46074987@qq.com";
            //bool enableSSL = true;

            ////string password = "abcdefg";
            ////string emailTo = "someone@domain.com";
            //string subject = "Hello";
            //string body = "<h2>Hello, I'm just writing this to say Hi!</h2>";


            //using (MailMessage mail = new MailMessage())
            //{
            //    mail.From = new MailAddress(emailFrom);
            //    mail.To.Add(emailTo);
            //    mail.Subject = subject;
            //    mail.Body = body;
            //    mail.IsBodyHtml = true;
            //    // Can set to false, if you are sending pure text.

            //    //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
            //    //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

            //    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
            //    {
            //        smtp.Credentials = new NetworkCredential(emailFrom, emailFromPassword);
            //        smtp.EnableSsl = enableSSL;
            //        smtp.Send(mail);
            //    }
            //}
        }
    }
}
