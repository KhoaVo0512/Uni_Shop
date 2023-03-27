using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
using Uni_Shop.Models;
using NHibernate.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text.RegularExpressions;

namespace Uni_Shop.Controllers
{
    public class EmailController : Controller
    {
        TN230_V1Context db = new TN230_V1Context();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult validate_account()
        {
            return View();
        }
        public IActionResult AdvertiseEmailFormat()

        {
            List<TaiKhoan> tk = db.TaiKhoans.ToList();
            int count = tk.Count;
            int i = 0;
            string[] listEmail = new string[count];
            foreach(var item in tk)
            {
                if (listEmail.Contains(item.Email) == false)
                {
                    listEmail[i] = item.Email;
                    i++;
                }                  
            }
            Console.WriteLine(listEmail);
            var result = new EmailController().SendMail("UniShop-Advertise Email", "advertise_email", listEmail, null, null, null);
            
            //var result = new EmailController().SendMail("UniShop-Validate Account", "validate_account", new string[1] { "mylinhpthi@gmail.com" }, 1029);
            return View();
        }
        public async Task<IActionResult> SendMail(String title, String action, string[] name, int? id,  CheckOut checkout, string payment)
        {
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com");
                client.Authenticate("tptai100@gmail.com", "rttedmeuxsbmdihc");

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $""
                };
               
                string filename = "D:/HK1 - 2022 - 2023/XDUDTMDT/DA_TN230/template/"+action+".html";
                using (StreamReader SourceReader = System.IO.File.OpenText(filename))
                {
                    string str = SourceReader.ReadToEnd();
                    if (action == "order_information")
                    {
                        str = Regex.Replace(str, "NguoimuaHoten", checkout.nguoidungdetail.TenNguoiDung);
                        str = Regex.Replace(str, "NguoimuaSDT", checkout.nguoidungdetail.Sdt);
                        str = Regex.Replace(str, "NguoimuaDiachi", checkout.nguoidungdetail.DiaChi);
                        str = Regex.Replace(str, "DonhangMa", "658364B2");
                        str = Regex.Replace(str, "DonhangHTTT", payment);
                        str = Regex.Replace(str, "DonhangNgay", @DateTime.Now.ToString());
                    }

                    if (action == "validate_account")
                    {
                        str = Regex.Replace(str, "XXXXX", "https://localhost:44369/validate/" + id.ToString());
                    }
                   
                    bodyBuilder.HtmlBody = str;
                }
                var message = new MimeMessage
                {
             
                    Body = bodyBuilder.ToMessageBody()
                };
                message.From.Add(new MailboxAddress("UniShop", "tptai100@gmail.com"));
                for (int i = 0; i < name.Length; i++)
                {
                    if(name[i]!= null)
                    message.To.Add(new MailboxAddress("suppri", name[i]));
                }

                message.Subject = title;
                await client.SendAsync(message);
               client.Disconnect(true);
            }
            return RedirectToAction("Index");
        }
        
    }
}
