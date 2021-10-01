using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Settings : Page
{
    DataTable dt;
    string empno = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Util.CheckSession();

        empno = Session["empno"].ToString();

        // 페이지 로드
        Util.SetPageTitle(pageTitle, "Settings");
        SetInputChecked();
    }

    private void SetInputChecked()
    {
        if (!IsPostBack)
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select top 1 * from MOBILEPUSH.dbo.TPUSHSETTING where empno = '{0}'", empno)), "SELECT", "itdb2");

            if (dt.Rows[0]["HOISA"].ToString().Trim() == "1")
                alarmHoisa.Checked = true;

            if (dt.Rows[0]["SAWON"].ToString().Trim() == "1")
                alarmSawon.Checked = true;

            if (dt.Rows[0]["SAJU"].ToString().Trim() == "1")
                alarmSaju.Checked = true;

            if (dt.Rows[0]["NOZO"].ToString().Trim() == "1")
                alarmNojo.Checked = true;

            if (dt.Rows[0]["MARKET"].ToString().Trim() == "1")
                alarmMarket.Checked = true;
        }
    }

    protected void alarm_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox checkbox = (CheckBox)sender;
        string column = "";

        switch (checkbox.ID)
        {
            case "alarmHoisa":
                column = "HOISA";
                break;
            case "alarmSawon":
                column = "SAWON";
                break;
            case "alarmSaju":
                column = "SAJU";
                break;
            case "alarmNojo":
                column = "NOZO";
                break;
            case "alarmMarket":
                column = "MARKET";
                break;
        }

        if (checkbox.Checked)
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update MOBILEPUSH.dbo.TPUSHSETTING set " + column + " = 1 where empno = '{0}'", empno)), "UPDATE", "itdb2");
        else
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update MOBILEPUSH.dbo.TPUSHSETTING set " + column + " = 0 where empno = '{0}'", empno)), "UPDATE", "itdb2");
    }
}
