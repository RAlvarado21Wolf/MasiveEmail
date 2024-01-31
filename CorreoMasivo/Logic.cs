using System.IO;
using System.Net.Mail;
using System.Web;
using System;

namespace EnviarMail
{
    class logic
    {
        /*
        if (param.ListaLinked != null && param.ListaLinked.Count > 0)
                {
                    foreach (DictionaryEntry linkedEntry in param.ListaLinked)
                    {
                        string llave = linkedEntry.Key?.ToString();
        string ruta = linkedEntry.Value?.ToString();

                        try
                        {
                            if (!string.IsNullOrEmpty(llave) && !string.IsNullOrEmpty(ruta) && File.Exists(ruta))
                            {
                                string fullPath = Path.GetFullPath(ruta);

                                if (!string.IsNullOrEmpty(fullPath))
                                {
                                    string mimeType = MimeMapping.GetMimeMapping(fullPath);
        LinkedResource lr = new LinkedResource(fullPath, mimeType);
        lr.ContentId = Guid.NewGuid().ToString();
        string body = File.ReadAllText(fullPath);
        AlternateView av = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");

        mail.Headers.Add(llave, ruta);
                                    param.Cuerpo = param.Cuerpo.Replace(llave, ruta);

                                    av.LinkedResources.Add(lr);
                                    mail.AlternateViews.Add(av);
                                }
                                else
                                {
                                    throw new Exception("Error: La ruta del archivo es nula o vacía.");
}
                            }
                            else
{
    throw new Exception("Error: La llave o la ruta son nulas o vacías, o el archivo no existe.");
}
                        }
                        catch (FileNotFoundException ex)
                        {
    Console.WriteLine($"Error: {ex.Message}");
}
                        catch (Exception ex)
                        {
    Console.WriteLine($"Error al procesar el archivo adjunto {llave}: {ex.Message}");
}
                    }
                }
                else
{
    Console.WriteLine("Datos Nulos");
}
*/

        //Espacio de Adjuntos
        /*if (param.ListaAdjuntos != null && param.ListaAdjuntos.Count > 0)
        {
            foreach (DictionaryEntry adjuntoEntry in param.ListaAdjuntos)
            {
                string nombreAdjunto = adjuntoEntry.Key.ToString();
                string rutaArchivo = adjuntoEntry.Value.ToString();

                if (File.Exists(rutaArchivo))
                {
                    Attachment attachment = new Attachment(rutaArchivo);
                    attachment.Name = nombreAdjunto; 
                    mail.Attachments.Add(attachment);
                }
                else
                {
                    Console.WriteLine($"El archivo adjunto {rutaArchivo} no existe.");
                }
            }
        }
        else
        {
            Console.WriteLine("Revision necesaria");
        }*/
    }
}
