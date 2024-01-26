using Microsoft.SqlServer.Server;
using System.Data.Sql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Net.Http;
using System.ComponentModel;
using System.Deployment.Internal;

namespace CorreoMasivo
{
    public class UtilesEmail
    {
        static bool mailSend = false;

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e, int pdatos) {

            DatosEmail datos = new DatosEmail();
            String token = (String)e.UserState;

            if (e.Cancelled) {
                Console.WriteLine("[{0}] Send canceled.", token);
                Update(pdatos, 0, token, 0);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                Update(pdatos, 0, e.Error.ToString(), 0);
            }
            else {
                
                Update(pdatos, 1, "Actualizado", 1);
                // datos.Listado();

            }
            mailSend = true;

        }

        public static DatosEmail getDatosDB(int EmailID) {
            DatosEmail datos = new DatosEmail();
            try
            {
                string connect = "server=DESKTOP-7KDBKTG\\SQLEXPRESS; database=master; integrated security=true";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    Int32 IDTemplate;
                    // Conexion Aperturada
                    connection.Open();
                    //Parametros de llamada y envio de Email
                    SqlCommand cmd = connection.CreateCommand();
                    string consultaVerificacion = @"SELECT E.ToAddress, E.FromAddress, E.Subject, E.IsBodyHtml, E.Body, E.EMEmailTemplateID, S.SmtpServerName FROM  EMEmail E JOIN EMSmtpServer S ON E.EMSmtpServerID = S.EMSmtpServerID WHERE E.EMEmailID = @EMEmailID;";                   
                    cmd.CommandText = consultaVerificacion;
                    // cmd.Parameters.Add("@EMEmailID", SqlDbType.Int).Value = EmailID;
                    cmd.Parameters.AddWithValue("@EMEmailID",EmailID);
                    SqlDataReader DE = cmd.ExecuteReader();
                    if (DE.Read()) {

                        datos.EmailID = EmailID; 
                        datos.Destinatario = DE.IsDBNull(0) ? "" : DE.GetString(0);
                        datos.CorreoOrigen = DE.IsDBNull(1) ? "" : DE.GetString(1);
                        datos.Asunto = DE.IsDBNull(2) ? "" : DE.GetString(2);
                        datos.bodyHtml = DE.IsDBNull(3) ? false : DE.GetBoolean(3);
                        datos.Cuerpo = DE.IsDBNull(4) ? "" :  DE.GetString(4);
                        IDTemplate = DE.IsDBNull(5) ? 0 : DE.GetInt32(5);
                        datos.Smtp = DE.IsDBNull(6) ? "" : DE.GetString(6);
                        
                        DE.Close();

                        string consultaReplacement =   @"SELECT ReplacementKey, ReplacementValue FROM EMEmailTemplateReplacement WHERE EMEmailTemplateID = @EMEmailTemplateID2 AND EMEmailID = @EMEmailID2;";
                        cmd.CommandText = consultaReplacement;
                        cmd.Parameters.AddWithValue("@EMEmailTemplateID2", IDTemplate);
                        cmd.Parameters.AddWithValue("@EMEmailID2", EmailID);
                        SqlDataReader DR = cmd.ExecuteReader();
                        datos.ListaPalabras = new ListDictionary();
                        while (DR.Read()){

                            Console.WriteLine(DR[0] + ", " + DR[1]);
                            var key = DR.GetString(0);
                            var value = DR.GetString(1);
                            datos.ListaPalabras.Add(key, value);
                                
                        }

                        DR.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return datos;

        }

        internal static void Update(int ID, byte Envio, string Mensaje, int Procesado)
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
                    cmd.Parameters.AddWithValue("@EMEmailID", ID);
                    cmd.Parameters.AddWithValue("@FechaProcesado", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Enviado", Envio);
                    cmd.Parameters.AddWithValue("@MensajeResultado", Mensaje);

                    int respuesta = (int)cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);

            }
        }

        public static List<int> Listado()
        {
            List<int> Mail = new List<int>();
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
                    while (ID.Read())
                    {
                        Mail.Add(Convert.ToInt32(ID[0].ToString()));
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("No existen registros para mostrar: " + ex.Message);
            }
            return Mail;
        }

        public string EnviosPendientes()
        {

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

        public static string sendMail(DatosEmail param)
        {
            string msge = "";
            try
            {
                
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(param.CorreoOrigen, param.Asunto);
            mail.To.Add(param.Destinatario);

            if (param.ListaPalabras != null)
            {
                foreach (DictionaryEntry replacement in param.ListaPalabras)
                {
                    param.Cuerpo = param.Cuerpo.Replace(replacement.Key.ToString(), replacement.Value.ToString());
                }

            }
                mail.Subject = param.Asunto;
                mail.Body = param.Cuerpo;
                mail.IsBodyHtml = true;
                SmtpClient client = new SmtpClient(param.Smtp, 587);
                client.Credentials = new NetworkCredential(param.CorreoOrigen, "miofqxyvyamwxudi");
                client.EnableSsl = true;
                client.SendCompleted += (sender, e) => SendCompletedCallback(sender, e, param.EmailID);
                client.SendAsync(mail, "Mensaje");
                

            }
            catch (Exception ex)
            {
                msge = ex.Message + ". Por favor verifica tu conexión a internet y que tus datos sean correctos e intenta nuevamente.";
            }
            
            Console.ReadLine();
            return msge;
            
        }

    }

}





