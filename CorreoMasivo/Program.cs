using System;

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

        }
    }
}