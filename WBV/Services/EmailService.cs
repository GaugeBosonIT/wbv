using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WBV.Services
{
    
    public class Mail
    {
        public string username = ConfigurationManager.AppSettings["username"].ToString();
        public string password = ConfigurationManager.AppSettings["password"].ToString();
        public string SmtpClient = ConfigurationManager.AppSettings["SmtpClient"].ToString();
        public string FromAddress = ConfigurationManager.AppSettings["FromAddress"].ToString();
        public string subject;
        public string body;
        public string toAddress;



        public Mail(string _subject, string _body, string _toAddress)
        {
            subject = _subject;
            body = _body;
            toAddress = _toAddress;
        }

        public void SendMail()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(SmtpClient);
            mail.From = new MailAddress(FromAddress);
            mail.To.Add(toAddress);
            mail.Subject = subject;
            mail.Body = body;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
         
        }


    }
}