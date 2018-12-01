using FourHealth.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Services;


namespace Broker
{
    /// <summary>
    /// Descrição resumida de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        string ApiBaseUrl;// endereço da sua api
        SqlConnection myConnection = new SqlConnection(
              "Server=den1.mssql8.gear.host; DataBase=servidores; User ID=servidores; Password=Ns8eRukh!E7?");

        [WebMethod]
        public string Broker_IncluirServidor(string servidor)
        {
            String selectCmd = "insert servidores values('"+ servidor + "')";
            try
            {
                SqlDataAdapter myCommand =
                    new SqlDataAdapter(selectCmd, myConnection);

                DataSet ds = new DataSet();
                myCommand.Fill(ds, "servidor");
            }
            catch (Exception ex)
            {
                return "Falha ao inserir servidor. "+ ex.Message;
            }

            return "Servidor inserido com sucesso";
        }

        [WebMethod]
        public string Broker_ExcluirServidor(string servidor)
        {
            String selectCmd = "delete servidores where servidor = '" + servidor + "'";
            try
            {
                SqlDataAdapter myCommand =
                    new SqlDataAdapter(selectCmd, myConnection);

                DataSet ds = new DataSet();
                myCommand.Fill(ds, "servidor");
            }
            catch (Exception ex)
            {
                return "Falha ao excluir servidor. " + ex.Message;
            }

            return "Servidor excluído com sucesso";
        }

        [WebMethod]
        public DataSet Broker_ListarServidores()
        {
            String selectCmd = "select * from servidores";

            DataSet ds = new DataSet();

            try
            {
                SqlDataAdapter myCommand =
                    new SqlDataAdapter(selectCmd, myConnection);
                    myCommand.Fill(ds, "servidor");
            }
            catch (Exception ex)
            {
                throw new System.Exception("Falha ao listar servidores. " + ex.Message);
            }

            if (ds.Tables["servidor"].Rows.Count == 0)
            {
                throw new System.Exception("Nenhum servidor disponível");
            }

            return ds;
        }

        [WebMethod]
        public string Broker_BuscarServidor()
        {
            String selectCmd = "select top 1* from servidores order by newid()";

            SqlDataAdapter myCommand =
               new SqlDataAdapter(selectCmd, myConnection);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "servidor");

            if(ds.Tables["servidor"].Rows.Count == 0)
            {
                throw new System.Exception("Nenhum servidor disponível");
            }

            ApiBaseUrl = ds.Tables["servidor"].Rows[0]["servidor"].ToString();

            return ApiBaseUrl;
        }

       


        private void Serialize(Stream output, object input)
        {
            var ser = new DataContractSerializer(input.GetType());
            ser.WriteObject(output, input);
        }
    }
}

//public Beneficiario Proxy_ConsultarBeneficiarioPorId(int id)
//{
//    try
//    {

//        buscarServidor();
//        string MetodoPath = "/api/Beneficiario/" + id.ToString(); //caminho do método a ser chamado

//        var httpWebRequest = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + MetodoPath);
//        httpWebRequest.ContentType = "application/json";
//        httpWebRequest.Method = "GET";

//        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
//        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
//        {
//            var valor = streamReader.ReadToEnd();
//            if (valor.Substring(1, 1) == "[")
//            {
//                var retorno = JsonConvert.DeserializeObject<List<Beneficiario>>(valor);
//                return retorno.FirstOrDefault();
//            }
//            else
//            {
//                var retorno = JsonConvert.DeserializeObject<Beneficiario>(valor);
//                return retorno;
//            }

//        }
//    }
//    catch (Exception ex)
//    {
//        throw new System.Exception(ex.Message);
//    }
//}

//[WebMethod]
//public string Proxy_IncluirBeneficiario(Beneficiario beneficiario)
//{
//    try
//    {
//        buscarServidor();
//        string MetodoPath = "/api/Beneficiario/"; //caminho do método a ser chamado

//        var httpWebRequest = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + MetodoPath);
//        httpWebRequest.ContentType = "application/json";
//        httpWebRequest.Method = "POST";

//        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
//        {
//            streamWriter.Write(JsonConvert.SerializeObject(beneficiario));
//            streamWriter.Flush();
//            streamWriter.Close();
//        }


//        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
//        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
//        {
//            var valor = streamReader.ReadToEnd();

//            return valor.ToString();

//        }
//    }
//    catch (Exception ex)
//    {
//        throw new System.Exception(ex.Message);
//    }
//}