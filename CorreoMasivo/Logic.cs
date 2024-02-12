using System.IO;
using System.Net.Mail;
using System.Web;
using System;
using CorreoMasivo;
using System.Collections;
using System.Web.UI.WebControls;

namespace EnviarMail
{
    class logic
    {
        /*public static MailMessage CreateMailMessage(DatosEmail datosEmail)
        {
            /*try
            {
                MailDefinition md = new MailDefinition();
                md.IsBodyHtml = true;
                md.From = datosEmail.CorreoOrigen;

                Control control = HttpContext.Current != null ? null : new Control();

                if (datosEmail.Destinatario != null && datosEmail.ListaPalabras != null && control != null)
                {
                    MailMessage mailMessage = md.CreateMailMessage(datosEmail.Destinatario, datosEmail.ListaPalabras, control);

                    if (datosEmail.ListaLinked != null)
                    {
                        foreach (DictionaryEntry linkedResource in datosEmail.ListaLinked)
                        {
                            string llave = linkedResource.Key.ToString();
                            string ruta = linkedResource.Value.ToString();

                            string mappedPath = HttpContext.Current.Server.MapPath(ruta);

                            if (!string.IsNullOrEmpty(mappedPath))
                            {
                                string MimeType = MimeMapping.GetMimeMapping(mappedPath);

                                LinkedResource lr = new LinkedResource(mappedPath, MimeType);
                                lr.ContentId = Guid.NewGuid().ToString();

                                AlternateView av = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, "text/html");
                                av.LinkedResources.Add(lr);
                                mailMessage.AlternateViews.Add(av);

                                // Reemplazar palabras clave en el cuerpo del correo
                                mailMessage.Body = mailMessage.Body.Replace($"<%{llave}%>", $"cid:{lr.ContentId}");
                            }
                            else
                            {
                                // Manejar el caso en el que mappedPath sea null o vacío
                            }
                        }
                    }

                    mailMessage.Subject = datosEmail.Asunto;
                    mailMessage.Body = datosEmail.Cuerpo;

                    return mailMessage;
                }
                else
                {
                    Console.WriteLine("Al menos uno de los valores requeridos es nulo.");
                    return new MailMessage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new MailMessage();
            }
        }*/

    }
}
