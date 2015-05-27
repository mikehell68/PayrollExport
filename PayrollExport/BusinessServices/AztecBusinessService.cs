using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using LogThis;

namespace PayrollExport.BusinessServices
{
    internal static class AztecBusinessService
    {
        private const string _databaseMachine = "(local)";
        private const string _databaseName = "Aztec";

#if DEBUG
        private const string dbConnectionString = "Trusted_Connection=True;Initial Catalog={0};Data Source={1}";
#elif !DEBUG
        private const string dbConnectionString = "User ID=zonalsysadmin;Password=0049356GNHsxkzi26TYMF;Initial Catalog={0};Data Source={1}";
#endif

        private static XDocument _sqlScripts;

        static AztecBusinessService()
        {
            LoadSqlScripts();
        }

        private static string GetFromResources(string resourceName)
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            using (var stream = assem.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        internal static void LoadSqlScripts()
        {
            Log.LogThis("Loading scripts", eloglevel.info);
            string sqlScriptsXml;
            try
            {
                sqlScriptsXml = GetFromResources("PayrollExport.Scripts.SqlScripts.xml");
            }
            catch (Exception ex)
            {
                Log.LogThis(string.Format("Error reading scripts file: {0}", ex), eloglevel.error);
                throw;
            }

            var sqlScriptsStringReader = new StringReader(sqlScriptsXml);
            try
            {
                _sqlScripts = XDocument.Load(sqlScriptsStringReader);
            }
            catch (Exception ex)
            {
                Log.LogThis(string.Format("Error loading scripts: {0}", ex), eloglevel.error);
                throw;
            }
            finally
            {
                sqlScriptsStringReader.Close();
            }
            Log.LogThis(string.Format("{0} scripts loaded ", _sqlScripts.Root.Descendants("SqlScript").Count()), eloglevel.info);
        }

        internal static DataSet GetPayrollDataFromAztec(DateTime startDate, DateTime endDate)
        {
            Log.LogThis("Begin GetPayrollDataFromAztec()", eloglevel.info);

            try
            {
                using (var sqlConnection = new SqlConnection(String.Format(dbConnectionString, _databaseName, _databaseMachine)))
                {
                    sqlConnection.Open();
                    var sqlAdapter = new SqlDataAdapter();
                    var cmd = sqlConnection.CreateCommand();
                    var ds = new DataSet();

                    var sqlScript = _sqlScripts.Descendants("SqlScript").Where(
                            s => s.Attribute("Name").Value == "PayrollExportActual").FirstOrDefault().Value;

                    sqlScript = string.Format(sqlScript, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                    cmd.CommandText = sqlScript;
                    cmd.CommandTimeout = 0;
                    sqlAdapter.SelectCommand = cmd;
                    sqlAdapter.Fill(ds);
                    sqlConnection.Close();

                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return null;

                    Log.LogThis(string.Format("Payroll export query generated {0} rows", ds.Tables[0].Rows.Count), eloglevel.info);

                    return ds;
                }
            }
            finally
            {
                Log.LogThis("End GetPayrollDataFromAztec()", eloglevel.info);
            }
        }
    }
}
