using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class ApprGongmun : Page
{
    string empno = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Util.CheckSession();

        empno = Session["empno"].ToString();

        Util.SetPageTitle(pageTitle, "ApprGongmun");

        HtmlControl iframe = (HtmlControl)FindControl("iframe");
        iframe.Attributes["src"] = "https://mgate.seoul.co.kr/gongmun/IOS/Gongmun.aspx?empno=" + empno;
    }
}