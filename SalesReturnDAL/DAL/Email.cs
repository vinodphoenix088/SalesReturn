using System;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace SalesReturnDAL.DAL
{
    class Email
    {
        public static string sendEmail(string subject, string FromMailAddress, MailAddressCollection toEmail, MailAddressCollection ccEmail, string Body)
        {
            string SMTPEmailHost = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
            string SMTPusername = System.Configuration.ConfigurationManager.AppSettings["SmtpUserName"];
            string SMTPpass = System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];
            string AllowEmail = System.Configuration.ConfigurationManager.AppSettings["AllowEmail"];
            int SMTPPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["smtpAddressPort"]);
            if (AllowEmail == "true")
            {
                try
                {
                    SmtpClient smtp = new SmtpClient(SMTPEmailHost);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(SMTPusername, SMTPpass);
                    smtp.Port = SMTPPort;
                    smtp.EnableSsl = true;
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(FromMailAddress);
                    List<string> emailChk = new List<string>();
                    foreach (var mail in toEmail)
                    {
                        message.To.Add(mail);
                    }
                    if (ccEmail != null)
                    {
                        foreach (var mail in ccEmail)
                        {
                            if (mail != null)
                            {
                                string Idx = emailChk.FirstOrDefault(x => x.ToString().Equals(mail.ToString()));
                                if (Idx == null)
                                {
                                    string str = mail.ToString();
                                    emailChk.Add(str);
                                    message.CC.Add(mail);
                                }
                            }
                        }
                    }
                    message.IsBodyHtml = true;
                    message.Subject = subject;
                    message.Body = Body;
                   smtp.Send(message);
                    return "Success: Notification sent";
                }
                catch (Exception Ex)
                {
                    return "Failed: Email ";
                }
            }
            else
            {
                return "Email not allowed";
            }
        }
    }
}
