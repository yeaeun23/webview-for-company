using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class SMS : Page
{
    string empno = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Util.CheckSession();

        empno = Session["empno"].ToString();

        Util.SetPageTitle(pageTitle, "SMS");

        HtmlControl iframe = (HtmlControl)FindControl("iframe");
        iframe.Attributes["src"] = "https://mgate.seoul.co.kr/SISXML/SMS.aspx?empno=" + empno;
    }
}