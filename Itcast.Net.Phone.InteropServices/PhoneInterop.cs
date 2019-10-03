using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Media;
using System.Net.Mail;
using System.Net;

namespace Itcast.Net.Phone.InteropServices
{
    [ComVisible(true)]
    [Guid("F68194B7-D951-441F-BF33-7D351B6A3071")]
    public class PhoneInterop
    {
        public void SendMail(string stmpAddr, string stmpUserName, string stmpPassword, string from, string to,
            string subject, string body)
        {
            MailMessage mailMsg = new MailMessage();//两个类，别混了 引入System.Web这个Assembly
            mailMsg.From = new MailAddress(from,"传智播客.Net培训WP7模拟器");//源邮件地址 
            mailMsg.To.Add(new MailAddress(to));//目的邮件地址。可以有多个收件人
            mailMsg.Subject = subject;//发送邮件的标题 
            mailMsg.Body = body;//发送邮件的内容 
            SmtpClient client = new SmtpClient(stmpAddr);
            client.Credentials = new NetworkCredential(stmpUserName, stmpPassword);
            client.Send(mailMsg);
        }
    }
}
