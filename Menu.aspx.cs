using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Xml;

public partial class Menu : Page
{
    mgate.serviceSIS.WebServiceSISSoapClient sis = new mgate.serviceSIS.WebServiceSISSoapClient();

    protected void Page_Load(object sender, EventArgs e)
    {
        session.Text = Util.GetNameByEmpno(Session["empno"].ToString()) + "(" + Session["empno"].ToString() + ")";

        SetGukList();
    }

    private void SetGukList()
    {
        XmlDocument xml = new XmlDocument();
        List<GukBu> items = new List<GukBu>();

        try
        {
            xml.LoadXml(sis.GetGukList());
            XmlNodeList xnList = xml.SelectNodes("guklist/item");

            foreach (XmlNode xn in xnList)
            {
                items.Add(new GukBu()
                {
                    code = xn["guk_code"].InnerText.Trim(),
                    name = xn["guk_name"].InnerText.Trim()
                });
            }

            gukListRepeater.DataSource = items;
            gukListRepeater.DataBind();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }
}