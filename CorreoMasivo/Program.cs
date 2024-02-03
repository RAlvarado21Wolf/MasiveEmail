using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CorreoMasivo
{
    class Program
    {

        static void Main(string[] args)
        { 
            DatosEmail datosEmail = new DatosEmail();
            foreach (int Mail in UtilesEmail.GetListadoCorreosPendientes())
            {

                Console.WriteLine(Mail);
                DatosEmail datos = UtilesEmail.getDatosDB(Mail);
                UtilesEmail.sendMail(datos);

            }
            
            /*DatosEmail datosEmail = new DatosEmail();
            foreach (int Mail in UtilesEmail.GetListadoCorreosPendientes())
            {
                Console.WriteLine(Mail);
                DatosEmail datos = UtilesEmail.getDatosDB(Mail);

                MailDefinition mailDefinition = new MailDefinition();
                mailDefinition.From = datos.CorreoOrigen;
                mailDefinition.Subject = datos.Asunto;
                mailDefinition.BodyFileName = "~/EmailTemplates/HolaUsuario.txt";

                // Crear el cuerpo del correo electrónico con datos dinámicos
                ListDictionary replacements = new ListDictionary();
                replacements.Add(datos.ListaPalabras.Keys.ToString(), datos.ListaPalabras.Values.ToString());

                string body = mailDefinition.CreateMailMessage(datos.Destinatario, replacements, new Control()).Body;



            }*/
        }
    }
}