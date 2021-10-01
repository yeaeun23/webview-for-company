using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Work : Page
{
    string type = "";
    string empno = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Util.CheckSession();

        type = (Request["type"] == "" || Request["type"] == null) ? "workReg" : Request["type"];
        empno = Session["empno"].ToString();

        Util.SetPageTitle(pageTitle, "Work_" + type);

        HtmlControl iframe = (HtmlControl)FindControl("iframe");
        switch (type)
        {
            case "workReg":
            default:
                iframe.Attributes["src"] = "https://mgate.seoul.co.kr/work/mwork.aspx?empno=" + empno;
                break;
            case "workAppr":
                iframe.Attributes["src"] = "https://mgate.seoul.co.kr/work/mapproval.aspx?empno=" + empno;
                break;
            case "accReg":
                iframe.Attributes["src"] = "https://mgate.seoul.co.kr/work/maccident.aspx?empno=" + empno;
                break;
            case "accAppr":
                iframe.Attributes["src"] = "https://mgate.seoul.co.kr/work/mapproval_Acc.aspx?empno=" + empno;
                break;
        }
    }
}