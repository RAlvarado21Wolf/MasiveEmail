using EnviarMail;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace CorreoMasivo
{
    class Program
    {

        static void Main(string[] args)
        {
            // obtencion de parametros para email
            DatosEmail datosEmail = UtilesEmail.getDatosDB(3);

            // envio de email
            UtilesEmail.sendMail(datosEmail);

            // DatosEmail Funciones = new DatosEmail();

            //Funciones.Listado();

        }
        
    }
}