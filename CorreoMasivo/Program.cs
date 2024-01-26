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
            DatosEmail datosEmail = new DatosEmail();
            foreach (int Mail in UtilesEmail.Listado())
            {

                Console.WriteLine(Mail);
                DatosEmail datos = UtilesEmail.getDatosDB(Mail);
                UtilesEmail.sendMail(datos);

            }

        }
    }
}