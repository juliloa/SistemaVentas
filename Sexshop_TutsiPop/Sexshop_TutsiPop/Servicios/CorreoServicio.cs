using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using Sexshop_TutsiPop.Models;
using MailKit.Net.Smtp;

namespace Sexshop_TutsiPop.Servicios
{
    public static class CorreoServicio
    {
        private static string _Host = "smtp.gmail.com";
        private static int _Puerto = 587;

        private static string _NombreEnvia = "tustsipop";
        private static string _Correo = "tutsipopcorreo@gmail.com";
        private static string _Clave = "bsldheerulxzdstl";

        public static bool Enviar(Correo correo)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(_NombreEnvia, _Correo));
                email.To.Add(MailboxAddress.Parse(correo.Para));
                email.Subject = correo.Asunto;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = correo.Contenido
                };

                var smtp = new SmtpClient();
                smtp.Connect(_Host, _Puerto, SecureSocketOptions.StartTls);

                smtp.Authenticate(_Correo, _Clave);
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
