using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Xml;
using WBV.Interfaces;


namespace WBV.DataAccess
{
    public class DataService : IData
    {

        public SqlConnection getPooledConnection(SqlConnection sConn)
        {
            System.Data.SqlClient.SqlConnection newcon =
            new System.Data.SqlClient.SqlConnection();
            sConn.ConnectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            sConn.Open();
            sConn.Close();
            return (sConn);
        }

        public bool closeConnection(SqlConnection sConn)
        {
            bool blnRetVal = false;
            try
            {
                if (sConn.State.Equals("Open"))
                {
                    sConn.Close();
                }
                sConn.Close();
                sConn.Dispose();
                blnRetVal = true;
            }
            catch (Exception exp)
            {

            }
            return (blnRetVal);
        }

        public XmlDocument execStoredProc(String strProcName, XmlDocument strParameters)
        {
            XmlDocument Result = new XmlDocument();
            SqlConnection sConn = new SqlConnection();
            sConn = getPooledConnection(sConn);
            try
            {
                sConn.Open();
                SqlCommand command = new SqlCommand(strProcName, sConn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p", SqlDbType.Xml).Value = strParameters.InnerXml;
                SqlParameter result = command.Parameters.Add
                           ("@r", SqlDbType.Xml);
                result.Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                Result.LoadXml(result.Value.ToString());
                sConn.Close();
            }
            catch (Exception exp)
            {
                Result.LoadXml(exp.ToString());
                sConn.Close();
            }
            return (Result);

        }

    }
}