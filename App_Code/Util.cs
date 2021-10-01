using System;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI.WebControls;
using Resources;
using System.Data.SqlClient;
using System.Web;

public class Util
{
    public static void CheckSession()
    {
        DataTable dt = ExecuteQuery(new SqlCommand(string.Format(@"select empno, sessionID from AUTH.dbo.emplist where empno = '{0}' and sessionID = '{1}'", HttpContext.Current.Session["empno"].ToString(), HttpContext.Current.Session["sessionid"].ToString())), "SELECT", "itdb2");

        if (dt.Rows.Count == 0) // sessionid 불일치
            HttpContext.Current.Response.Redirect("error.html");
    }

    public static string GetNameByEmpno(string empno)
    {
        DataTable dt = ExecuteQueryOdbc(new OdbcCommand(string.Format(@"select kore_name from jis_user.dh_empl where empl_code = {0}", empno)), "SELECT");

        return dt.Rows[0].ItemArray[0].ToString().Trim();
    }

    public static string SetDateFormat(string date)
    {
        return date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6);
    }

    public static string SubDate(string date)
    {
        string dateNow = DateTime.Now.ToString("d");    // yyyy-MM-dd

        if (date.Substring(0, 10) == dateNow && date.Length > 10)   // 년월일 같으면, 시간 출력 
            return date.Substring(11, date.Length - 3 - 11);
        else if (date.Substring(0, 4) == dateNow.Substring(0, 4))   // 년 같으면, 월일 출력
            return date.Substring(5, 5);
        else                                                        // 모두 다르면, 년월일 출력
            return date.Substring(0, 10);
    }

    public static void SetPageTitle(Label label, string boardName)
    {
        label.Text = Resource.ResourceManager.GetString(boardName);
    }

    public static DataTable ExecuteQuery(SqlCommand cmd, string action, string server)
    {
        string conStr = "Server=" + server + ".seoul.co.kr;uid=erpcustomer;pwd=roqkfqn@1904;";

        using (SqlConnection con = new SqlConnection(conStr))
        {
            cmd.Connection = con;

            try
            {
                switch (action)
                {
                    case "SELECT":
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            sda.SelectCommand = cmd;

                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                return dt;
                            }
                        }
                    case "UPDATE":
                    case "INSERT":
                    case "DELETE":
                        con.Open();
                        cmd.ExecuteNonQuery();
                        break;
                }
            }
            catch
            {
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return null;
        }
    }

    public static DataTable ExecuteQueryOdbc(OdbcCommand cmd, string action)
    {
        string conStr = "DRIVER={MySQL ODBC 5.1 Driver};Server=petra-sql2.seoul.co.kr;Database=dmbbs;UID=ituser;PASSWORD=roqkfqn@1904";

        using (OdbcConnection con = new OdbcConnection(conStr))
        {
            cmd.Connection = con;

            try
            {
                switch (action)
                {
                    case "SELECT":
                        using (OdbcDataAdapter sda = new OdbcDataAdapter())
                        {
                            sda.SelectCommand = cmd;

                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                return dt;
                            }
                        }
                    case "UPDATE":
                    case "INSERT":
                    case "DELETE":
                        con.Open();
                        cmd.ExecuteNonQuery();
                        break;
                }
            }
            catch
            {
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return null;
        }
    }

    public static void SaveLog(string message)
    {
        string curdir = ConfigurationManager.AppSettings["logpath"];

        if (!Directory.Exists(curdir))
            Directory.CreateDirectory(curdir);

        string strFileName = curdir + "\\mobsis_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        FileStream fo = null;
        StreamWriter sw = null;

        try
        {
            fo = new FileStream(strFileName, FileMode.Append);
            sw = new StreamWriter(fo);

            sw.WriteLine(message);
            sw.Close();
            fo.Close();
        }
        catch (Exception)
        {
            if (sw != null)
                sw.Close();

            if (fo != null)
                fo.Close();
        }
    }
}