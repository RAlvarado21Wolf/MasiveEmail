﻿using Microsoft.SqlServer.Server;
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

        /* public static DatosEmail getDatos()
        {
            string from = "testeoprueba96@gmail.com";
            string displayName = "Testeo";
            string bodyFilePath = "C:\\Users\\RICARDO\\Downloads\\CorreoMasivo (1)\\CorreoMasivo\\CorreoMasivo\\EmailTemplates\\HolaUsuario.txt";
            string addressFilePath = "C:\\Users\\RICARDO\\Downloads\\CorreoMasivo (1)\\CorreoMasivo\\CorreoMasivo\\documentos\\Listado de Destinatarios.txt";
            string descripcion = File.ReadAllText(bodyFilePath);
            string address = File.ReadAllText(addressFilePath);
            string smtp = "smtp.gmail.com";
            bool htmlBody = true;
            ListDictionary ListaPalabras = new ListDictionary{
                { "<%Usuario%>", "Usuario Prueba - 1" },
                { "<%Celular%>", "12345678 - Prueba"},
                { "<%Empresa%>", "Empresa Prueba" },
                { "<%FechaLimiteRenovacion%>", "01/02/03"},
                { "<%CantidadCuotas%>", "123"},
                { "<%TextoDevolucion%>", "Reporte de Correo"}
            };
            DatosEmail datos = new DatosEmail(1, address, from, displayName, descripcion, ListaPalabras, smtp, htmlBody);
            return datos;
        }*/

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e, int pdatos) {

            DatosEmail datos = new DatosEmail();
            String token = (String)e.UserState;

            if (e.Cancelled) {
                Console.WriteLine("[{0}] Send canceled.", token);
                datos.Update(pdatos, 0, token, 0);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                datos.Update(pdatos, 0, e.Error.ToString(), 0);
            }
            else {
                
                datos.Update(pdatos, 1, "Actualizado", 1);
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
                        // cmd.Parameters.Add("@EMEmailTemplateID2", SqlDbType.Int).Value = 1;
                        // cmd.Parameters.Add("@EMEmailID2", SqlDbType.Int).Value = EmailID;
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

        /* public static DatosEmail getDatosDBPendientes(int EmailID)
        {
            DatosEmail datos = new DatosEmail();
            try
            {
                string connect = "server=DESKTOP-7KDBKTG\\SQLEXPRESS; database=master; integrated security=true";
                using (SqlConnection connection = new SqlConnection(connect))
                {
                    Int32 IDTemplate;
                    // Conexion Aperturada
                    connection.Open();
                    // Parametros de llamada y envio de Email
                    SqlCommand cmd = connection.CreateCommand();
                    string consultaVerificacion = @"SELECT E.ToAddress, E.FromAddress, E.Subject, E.IsBodyHtml, E.Body, E.EMEmailTemplateID, S.SmtpServerName FROM EMEmail E JOIN EMSmtpServer S ON E.EMSmtpServerID = S.EMSmtpServerID WHERE E.EMEmailID = @EMEmailID AND (E.Enviado IS NULL OR E.Enviado = 0);";
                    cmd.CommandText = consultaVerificacion;
                    cmd.Parameters.AddWithValue("@EMEmailID", EmailID);
                    SqlDataReader DE = cmd.ExecuteReader();
                    if (DE.Read())
                    {

                        datos.EmailID = EmailID;
                        datos.Destinatario = DE.IsDBNull(0) ? "" : DE.GetString(0);
                        datos.CorreoOrigen = DE.IsDBNull(1) ? "" : DE.GetString(1);
                        datos.Asunto = DE.IsDBNull(2) ? "" : DE.GetString(2);
                        datos.bodyHtml = DE.IsDBNull(3) ? false : DE.GetBoolean(3);
                        datos.Cuerpo = DE.IsDBNull(4) ? "" : DE.GetString(4);
                        IDTemplate = DE.IsDBNull(5) ? 0 : DE.GetInt32(5);
                        datos.Smtp = DE.IsDBNull(6) ? "" : DE.GetString(6);

                        DE.Close();

                        string consultaReplacement = @"SELECT ReplacementKey, ReplacementValue FROM EMEmailTemplateReplacement WHERE EMEmailTemplateID = @EMEmailTemplateID2 AND EMEmailID = @EMEmailID2;";
                        cmd.CommandText = consultaReplacement;
                        cmd.Parameters.AddWithValue("@EMEmailTemplateID2", IDTemplate);
                        cmd.Parameters.AddWithValue("@EMEmailID2", EmailID);
                        SqlDataReader DR = cmd.ExecuteReader();
                        datos.ListaPalabras = new ListDictionary();
                        while (DR.Read())
                        {

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
        }*/

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
                // client.Timeout = 100000;
                // client.DeliveryMethod = SmtpDeliveryMethod.Network;
                // client.UseDefaultCredentials = false;
                client.SendCompleted += (sender, e) => SendCompletedCallback(sender, e, param.EmailID);
                Console.ReadLine();
                client.SendAsync(mail, "Mensaje");
                
            }
            catch (Exception ex)
            {
                msge = ex.Message + ". Por favor verifica tu conexión a internet y que tus datos sean correctos e intenta nuevamente.";
            }

            return msge;
        }

    }

}





