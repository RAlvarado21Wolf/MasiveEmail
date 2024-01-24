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

        //No alterar, esta correcto
        internal void Update(int ID, byte Envio, string Mensaje, int Procesado)
        {
            
            try
            {
                string connect = "server=DESKTOP-7KDBKTG\\SQLEXPRESS; database=master; integrated security=true";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    string actualizacionEmail = @"UPDATE EMEmail SET FechaProcesado = @FechaProcesado, Enviado = @Enviado, MensajeResultado = @MensajeResultado WHERE EMEmailID = @EMEmailID;";
                    cmd.CommandText = actualizacionEmail;
                    cmd.Parameters.Add("@EMEmailID", SqlDbType.Int).Value = ID;
                    cmd.Parameters.Add("@FechaProcesado", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@Enviado", SqlDbType.Bit).Value = Envio;
                    cmd.Parameters.Add("@MensajeResultado", SqlDbType.VarChar).Value = Mensaje;
                    int respuesta = (int)cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex){

                Console.WriteLine("Error: " + ex.Message);
            
            }
        }


        //Hacer un query para buscar a los registros que no tengan el 1 en el enviado con SQLDataReader para el ID
        //Traer los datos de GetDatosEmailID
        //Lista de los que no han sido enviados o que no tengan
        //Traer solo los registros de ID de los usuarios que no tengan un correo enviado
        //Retornar los IDs de la tabla EMEmail
        public string Listado()
        {
            var Mail = "";
            try
            {
                string connect = "server=DESKTOP-7KDBKTG\\SQLEXPRESS; database=master; integrated security=true";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    string consultaEmail = @"SELECT EMEmailID FROM EMEmail WHERE Enviado IS NULL or Enviado = 0 and Procesar = 1;";
                    cmd.CommandText = consultaEmail;
                    SqlDataReader ID = cmd.ExecuteReader();
                    Console.WriteLine("IDs de Correos Faltantes");
                    while (ID.Read())
                    {
                        int Ident = 0;
                        Ident = +1;
                        Mail = "EMEmailID " + Ident + ": " + ID[0];
                        Console.WriteLine(Mail);

                    }
                    
                    Mail = "Correos Actualizados";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("No existen registros para mostrar: " + ex.Message);
            }
            return Mail;
        }

        public string EnviosPendientes() {

            var resultado = "";
            try
            {

                string connect = "server=DESKTOP-7KDBKTG\\SQLEXPRESS; database=master; integrated security=true";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    string consultaEmail = @"SELECT EMEmailID FROM EMEmail WHERE Enviado IS NULL or Enviado = 0 and Procesar = 1;";
                    cmd.CommandText = consultaEmail;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var id = (int)reader["EMEmailID"];
                                Console.WriteLine("Correo enviado: " + id);
                                UtilesEmail.sendMail(UtilesEmail.getDatosDB(id));
                            }

                            resultado = "Correos faltantes enviados";
                        }
                        else
                        {
                            resultado = "Ya se han enviado todos los correos";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("No existen registros para mostrar: " + ex.Message);

            }

            return resultado;
        }

        public string Envios()
        {
            var Mail = "";
            try
            {

                string connect = "server=DESKTOP-7KDBKTG\\SQLEXPRESS; database=master; integrated security=true";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    string consultaEmail = @"SELECT EMEmailID FROM EMEmail WHERE Enviado IS NULL or Enviado = 0 and Procesar = 1;";
                    cmd.CommandText = consultaEmail;
                    SqlDataReader ID = cmd.ExecuteReader();
                    if (ID.HasRows)
                    {
                        var id = (int)ID[0];
                        while (ID.Read())
                        {
                            Console.WriteLine("Correo enviado" + ID[0]);
                            UtilesEmail.sendMail(UtilesEmail.getDatosDB(id));
                        }
                        Mail = "Correos faltantes enviados";
                    }
                    else
                    {
                        Mail = "Ya se han enviado todos los correos";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("No existen registros para mostrar: " + ex.Message);
            }
            return Mail;
        }

    }
}
