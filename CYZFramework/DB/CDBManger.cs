using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oracle.DataAccess.Client;
using System.Configuration;
using System.Data;
using CYZFramework.Log;
//using System.Data.OracleClient;

namespace CYZFramework.DB
{

    public class CDBManger
    {   //oracle
        static string connstring = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["dbInstance"]].ConnectionString;//  "User Id=gzdlgis;Password=bcc;Data Source=192.168.1.100/DGGISBCC";
        OracleConnection conn = null;

        public string UserName { get; set; }
        public string Password { get; set; }
       public bool open()
        {
            bool reval = false;
            try
            {
                close();
                conn = new OracleConnection(connstring);
                conn.Open();
                reval = true;
   
                
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                throw ex;
            }
            finally
            {
            }            
            return reval;
        }
        public bool close()
        {
            bool reval = false;
            try
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
            return reval;
        }

        public bool ExecuteNonQueryNoClose(string sqlstr)
        {
            bool reval = false;
            try
            {

                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                cmd.ExecuteNonQuery();
                reval = true;

            }
            catch (Exception ex)
            {
                CYZLog.writeLog(sqlstr);
                CYZLog.writeLog(ex.ToString());
            }
            finally
            {

            }
            return reval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public OracleDataReader ExecuteReader(string sqlstr)
        {
            OracleDataReader reval = null;
            try
            {
                if (open())
                {
                    var cmd = new OracleCommand(sqlstr, conn);
                    reval = cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(sqlstr);
                CYZLog.writeLog(ex.ToString());
            }

            return reval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string sqlstr)
        {
            bool reval = false;
            try
            {
                using (OracleConnection connexc = new OracleConnection(connstring))
                {
                    connexc.Open();
                    OracleCommand cmd = new OracleCommand(sqlstr, connexc);
                    cmd.ExecuteNonQuery();
                    reval = true;
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(sqlstr);
                CYZLog.writeLog(ex.ToString());
                throw ex;
            }finally
            {
                close();
            }

            return reval;
        }

        /// <summary>
        /// 刷新数据到modlog
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public bool ExecuteNonQuerySxsj(string sqlstr)
        {
            bool reval = false;
            try
            {
                   string[] connectiongStr = connstring.Split(';');
                   var ipStr = connectiongStr[2].Split('=')[1];
                   var connStr = string.Format("User Id={0};Password={1};Data Source={2}", UserName, Password, ipStr);

                   using (OracleConnection connexc = new OracleConnection(connStr))
                {
                    connexc.Open();
                    OracleCommand cmd = new OracleCommand(sqlstr, connexc);
                    cmd.ExecuteNonQuery();
                    close();
                    reval = true;
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(sqlstr);
                CYZLog.writeLog(ex.ToString());
                throw ex;
            }

            return reval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sqlstr)
        {
            DataTable reval = null;
            try
            {
                using (OracleConnection connexc = new OracleConnection(connstring))
                {
                    connexc.Open();
                    OracleCommand cmd = new OracleCommand(sqlstr, connexc);
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    reval = new DataTable();
                    da.Fill(reval);
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(sqlstr);
                CYZLog.writeLog(ex.ToString());
                reval = null;
            }

            return reval;
        }
    }

}
