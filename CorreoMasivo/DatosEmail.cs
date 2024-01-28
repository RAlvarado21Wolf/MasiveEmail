using System;
using System.Collections.Specialized;

namespace CorreoMasivo
{
    public class DatosEmail
    {
        public int EmailID {
            get;
            set;
        }
        public string Destinatario {
            get;
            set;
        }
        public string CorreoOrigen {
            get;
            set;
        }
        public string Asunto {
            get;
            set;
        }
        public string Cuerpo {
            get;
            set;
        }
        public ListDictionary ListaPalabras {
            get;
            set;
        }
        public string Smtp {
            get;
            set;
        }
        public bool bodyHtml {
            get;
            set;
        }
        public DateTime FechaProcesado {
            get;
            set;
        }
        public ListDictionary ListaAdjuntos { 
            get; 
            set; 
        }
        public ListDictionary ListaLinked {
            get;
            set;
        }
        public string Adjunto{
            get;
            set;
        }
        

        public DatosEmail(int IDEmail, string destinatario, string correoOrigen, string asunto, string cuerpo, ListDictionary replacements, string smtp, bool BodyHTML, ListDictionary attachments, ListDictionary linked, string archivo)
        {
            EmailID = IDEmail;
            Destinatario = destinatario;
            CorreoOrigen = correoOrigen;
            Asunto = asunto;
            Cuerpo = cuerpo;
            ListaPalabras = replacements;
            Smtp = smtp;
            bodyHtml = BodyHTML;
            ListaAdjuntos = attachments;
            ListaLinked = linked;
            Adjunto = archivo;
        }
        public DatosEmail() {
        
        }
    }
}
