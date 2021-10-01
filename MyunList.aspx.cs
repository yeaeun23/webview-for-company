using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Web.UI;
using System.IO;
using System.Data.SqlClient;
using System.Data;

public partial class MyunList : Page
{
    DataTable dt;
    string paperDate = "";
    string pan = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Util.CheckSession();

        if (!IsPostBack)
        {
            Util.SetPageTitle(pageTitle, "Myun");

            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select top 1 yyyy, mm, dd from PaperProduction.dbo.v_MediaDatePan where PublicationID = 21 and yyyy >= LEFT(CONVERT(VARCHAR, dateadd(M, -1, getdate()), 112), 4) order by yyyy desc, mm desc, dd desc")), "SELECT", "sql");
            paperDate = dt.Rows[0]["yyyy"].ToString() + dt.Rows[0]["mm"].ToString() + dt.Rows[0]["dd"].ToString();  // 최근 강판일자
            //paperDate = DateTime.Now.AddDays(1).ToString("yyyyMMdd");   // 내일자

            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select distinct pan from PaperProduction.dbo.v_MediaDatePan where PublicationID = 21 and yyyy = {0} and mm = {1} and dd = {2} order by pan desc", paperDate.Substring(0, 4), paperDate.Substring(4, 2), paperDate.Substring(6))), "SELECT", "sql");
            pan = dt.Rows[0]["pan"].ToString(); // 최근 강판
        }
        else
        {
            paperDate = dateTB.Text.Replace("-", "");
            pan = panCB.Text.Replace("판", "");
        }

        // 페이지 로드        
        dateTB.Text = Util.SetDateFormat(paperDate);
        SetPanList(RequestJson(@"https://mgate.seoul.co.kr/NS5/Service.aspx?FN=pan&PARA1=21&PARA2=" + paperDate));
        SetMyunList(RequestJson(@"https://mgate.seoul.co.kr/NS5/Service.aspx?FN=ImageView&PARA1=21&PARA2=" + paperDate + "&PAN=" + pan));
    }

    private void SetPanList(string result)
    {
        panCB.Items.Clear();

        if (result == "")
            return;

        try
        {
            JObject obj = JObject.Parse(result);
            JArray array = JArray.Parse(obj["data"].ToString());
            bool flag = false;

            foreach (JObject jObj in array)
            {
                panCB.Items.Add(jObj["Edition"].ToString().Trim());

                if (jObj["Edition"].ToString().Trim().Replace("판", "") == pan)
                    flag = true;
            }

            if (!flag)
                pan = "05";
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.Message;
        }

        panCB.SelectedValue = pan + "판";
    }

    private void SetMyunList(string result)
    {
        if (result == "")
            return;

        List<Myun> items = new List<Myun>();

        try
        {
            JObject obj = JObject.Parse(result);
            JArray array = JArray.Parse(obj["data"].ToString());

            foreach (JObject jObj in array)
            {
                items.Add(new Myun()
                {
                    Page = "<b>" + pan + "판 " + jObj["Page"].ToString().Trim() + "면 - " + jObj["PageName"].ToString().Trim() + "</b>",
                    InputTime = "<b>수신</b> <span class='icon icon-arrow-right'></span>" + jObj["InputTime"].ToString().Trim(),
                    OutputTime = "<b>출력</b> <span class='icon icon-arrow-right'></span>" + jObj["OutputTime"].ToString().Trim(),
                    thumb = "https://mgate.seoul.co.kr/ctp/Images/" + paperDate.Substring(0, 6) + "/" + paperDate + "/thumb/" + jObj["thumb"].ToString().Trim()
                });
            }

            myunListRepeater.DataSource = items;
            myunListRepeater.DataBind();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.Message;
        }
    }

    private string RequestJson(string url)
    {
        string result = "";

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            result = reader.ReadToEnd();
            stream.Close();
            response.Close();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.Message;
        }

        return result;
    }

    // 새로고침 버튼 클릭
    protected void refreshBtn_Click(object sender, EventArgs e)
    {
        SetMyunList(RequestJson(@"https://mgate.seoul.co.kr/NS5/Service.aspx?FN=ImageView&PARA1=21&PARA2=" + paperDate + "&PAN=" + pan));
    }
}
