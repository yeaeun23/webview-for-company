using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Xml;

public partial class Family : Page
{
    string gukCode = "";
    string gukName = "";
    string buCode = "";
    mgate.serviceSIS.WebServiceSISSoapClient sis = new mgate.serviceSIS.WebServiceSISSoapClient();

    protected void Page_Load(object sender, EventArgs e)
    {
        Util.CheckSession();

        if (!IsPostBack)
        {
            gukCode = Request["gukCode"];
            gukName = Request["gukName"];

            // 페이지 로드        
            pageTitle.Text = gukName;
            SetBuList();

            if (Request["buCode"] != "" && Request["buCode"] != null)
            {
                buCode = Request["buCode"];
                SetEmplList();
            }
        }
    }

    private void SetBuList()
    {
        XmlDocument xml = new XmlDocument();
        List<GukBu> items = new List<GukBu>();

        try
        {
            xml.LoadXml(sis.GetBuList(gukCode));
            XmlNodeList xnList = xml.SelectNodes("guklist/item");

            foreach (XmlNode xn in xnList)
            {
                items.Add(new GukBu()
                {
                    code = xn["dept_code"].InnerText.Trim(),
                    name = xn["team_name"].InnerText.Trim() + " " + xn["kwa_name"].InnerText.Trim() + " (" + xn["deptCount"].InnerText.Trim() + ")"
                });
            }

            buListRepeater.DataSource = items;
            buListRepeater.DataBind();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    private void SetEmplList()
    {
        XmlDocument xml = new XmlDocument();
        List<Empl> items = new List<Empl>();

        try
        {
            xml.LoadXml(sis.GetEmplList(buCode, "0"));
            XmlNodeList xnList = xml.SelectNodes("guklist/item");

            foreach (XmlNode xn in xnList)
            {
                items.Add(new Empl()
                {
                    emplimg = xn["emplimg"].InnerText.Trim(),
                    kore_name = "<b>이름</b> <span class='icon icon-arrow-right'></span>" + xn["kore_name"].InnerText.Trim(),
                    degr_name = "<b>직급</b> <span class='icon icon-arrow-right'></span>" + xn["degr_name"].InnerText.Trim(),
                    grad_name = (xn["grad_name"].InnerText.Trim() == "") ? "" : "<b>직책</b> <span class='icon icon-arrow-right'></span>" + xn["grad_name"].InnerText.Trim(),
                    team_name2 = (xn["team_name2"].InnerText.Trim() == "") ? "" : "<b>" + ((xn["workflag"].InnerText.Trim() == "0") ? "겸직" : "원직") + "</b> <span class='icon icon-arrow-right'></span>" + xn["team_name2"].InnerText.Trim(),
                    email = "<b>이메일</b> <span class='icon icon-arrow-right'></span>" + xn["email"].InnerText.Trim(),
                    hptel = xn["hptel"].InnerText.Trim(),
                    officetel = "<b>구내전화</b> <span class='icon icon-arrow-right'></span>" + xn["officetel"].InnerText.Trim(),
                    work = (xn["work"].InnerText.Trim() == "") ? "" : "<b>업무</b> <span class='icon icon-arrow-right'></span>" + xn["work"].InnerText.Trim(),
                    work2 = xn["work2"].InnerText.Trim()
                });
            }

            emplListRepeater.DataSource = items;
            emplListRepeater.DataBind();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    protected void searchBtn_Click(object sender, EventArgs e)
    {
        if (searchTxt.Value.Length < 2)
        {
            ErrorMsg.Text = "&nbsp;2자 이상 입력하세요.";
        }
        else
        {
            XmlDocument xml = new XmlDocument();
            List<Empl> items = new List<Empl>();

            try
            {
                xml.LoadXml(sis.GetSearchList(searchTxt.Value));
                XmlNodeList xnList = xml.SelectNodes("guklist/item");

                foreach (XmlNode xn in xnList)
                {
                    items.Add(new Empl()
                    {
                        emplimg = xn["emplimg"].InnerText.Trim(),
                        kore_name = "<b>이름</b> <span class='icon icon-arrow-right'></span>" + xn["kore_name"].InnerText.Trim(),
                        team_name = "<b>소속</b> <span class='icon icon-arrow-right'></span>" + xn["guk_name"].InnerText.Trim() + " " + xn["team_name"].InnerText.Trim() + " " + xn["kwa_name"].InnerText.Trim(),
                        degr_name = "<b>직급</b> <span class='icon icon-arrow-right'></span>" + xn["degr_name"].InnerText.Trim(),
                        grad_name = (xn["grad_name"].InnerText.Trim() == "") ? "" : "<b>직책</b> <span class='icon icon-arrow-right'></span>" + xn["grad_name"].InnerText.Trim(),
                        team_name2 = (xn["team_name2"].InnerText.Trim() == "") ? "" : "<b>" + ((xn["workflag"].InnerText.Trim() == "0") ? "겸직" : "원직") + "</b> <span class='icon icon-arrow-right'></span>" + xn["team_name2"].InnerText.Trim(),
                        email = "<b>이메일</b> <span class='icon icon-arrow-right'></span>" + xn["email"].InnerText.Trim(),
                        hptel = xn["hptel"].InnerText.Trim(),
                        officetel = "<b>구내전화</b> <span class='icon icon-arrow-right'></span>" + xn["officetel"].InnerText.Trim(),
                        work = (xn["work"].InnerText.Trim() == "") ? "" : "<b>업무</b> <span class='icon icon-arrow-right'></span>" + xn["work"].InnerText.Trim(),
                        work2 = xn["work2"].InnerText.Trim()
                    });
                }

                emplListRepeater.DataSource = items;
                emplListRepeater.DataBind();

                ErrorMsg.Text = "";
                empty.Visible = (emplListRepeater.Items.Count == 0) ? true : false;
            }
            catch (Exception ex)
            {
                ErrorMsg.Text = ex.ToString();
            }

            Util.SetPageTitle(pageTitle, "Family");
            buListRepeater.Visible = false;
        }
    }
}
