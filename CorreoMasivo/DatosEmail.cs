using EnviarMail;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

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

        public DatosEmail(int IDEmail, string destinatario, string correoOrigen, string asunto, string cuerpo, ListDictionary replacements, string smtp, bool BodyHTML)
        {
            EmailID = IDEmail;
            Destinatario = destinatario;
            CorreoOrigen = correoOrigen;
            Asunto = asunto;
            Cuerpo = cuerpo;
            ListaPalabras = replacements;
            Smtp = smtp;
            bodyHtml = BodyHTML;
        }
        public DatosEmail() {
        
        }
    }
}
