using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Xml;

public partial class BoardList : Page
{
    DataTable dt;
    mgate.serviceSIS.WebServiceSISSoapClient sis = new mgate.serviceSIS.WebServiceSISSoapClient();
    string boardName = "";
    string boardPage = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Util.CheckSession();

        boardName = (Request["boardName"] == "" || Request["boardName"] == null) ? "hoisa" : Request["boardName"];
        boardPage = (Request["boardPage"] == "" || Request["boardPage"] == null) ? "0" : Request["boardPage"];

        // 페이지 로드       
        Util.SetPageTitle(pageTitle, "Board_" + boardName);

        if (boardName == "temp")
            SetBoardList_temp();
        else
            SetBoardList();
    }

    private void SetBoardList()
    {
        XmlDocument xml = new XmlDocument();
        List<Board> items = new List<Board>();

        try
        {
            if (!IsPostBack || searchTxt.Text == "")
                xml.LoadXml(sis.GetBBSTitle(boardName, boardPage));
            else
                xml.LoadXml(sis.GetBBSSearchTitle(boardName, searchType.Value, searchTxt.Text, boardPage));

            // 이전/다음 버튼 set  
            boardPrevPage.PostBackUrl = "BoardList.aspx?boardName=" + boardName + "&boardPage=" + xml.GetElementsByTagName("bbsinfo")[0].Attributes["prevpage"].Value;
            boardNextPage.PostBackUrl = "BoardList.aspx?boardName=" + boardName + "&boardPage=" + xml.GetElementsByTagName("bbsinfo")[0].Attributes["nextpage"].Value;

            boardPrevPage.Enabled = (boardPage == "0") ? false : true;
            boardNextPage.Enabled = (Convert.ToInt64(xml.GetElementsByTagName("bbsinfo")[0].Attributes["nextpage"].Value) >= Convert.ToInt64(xml.GetElementsByTagName("bbsinfo")[0].Attributes["totalrecodecount"].Value)) ? false : true;

            // 게시물 리스트 set
            XmlNodeList xnList = xml.SelectNodes("bbsinfo/item");
            foreach (XmlNode xn in xnList)
            {
                items.Add(new Board()
                {
                    title = xn["title"].InnerText.Trim().Replace("\\", ""),
                    gukname = xn["gukname"].InnerText.Trim(),
                    korename = (xn["gukname"].InnerText.Trim().Contains(xn["korename"].InnerText.Trim())) ? "" : "(" + xn["korename"].InnerText.Trim() + ")",
                    reg_date = Util.SubDate(xn["reg_date"].InnerText.Trim()),
                    id = xn["id"].InnerText.Trim(),
                    userfile = (xn["userfile"].InnerText.Trim() == "") ? "" : " <span class='board_list_userfiletag icon icon-clip'></span>",
                    newtag = (xn["newtag"].InnerText.Trim() == "0") ? "" : " <span class='board_list_newtag'>N</span>"
                });
            }

            boardListRepeater.DataSource = items;
            boardListRepeater.DataBind();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    private void SetBoardList_temp()
    {
        List<Board> items = new List<Board>();

        try
        {
            if (!IsPostBack || searchTxt.Text == "")
                dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"SELECT title, name, company_id, reg_date, id, user_file, (UNIX_TIMESTAMP(now())-UNIX_TIMESTAMP(reg_date))/3600 as newtag FROM dmbbs.covid WHERE use_flag = 0 order by id desc")), "SELECT");
            else
                dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"SELECT title, name, company_id, reg_date, id, user_file, (UNIX_TIMESTAMP(now())-UNIX_TIMESTAMP(reg_date))/3600 as newtag FROM dmbbs.covid WHERE use_flag = 0 and {0} LIKE '%{1}%' order by id desc", searchType.Value, searchTxt.Text)), "SELECT");

            // 이전/다음 버튼 set  
            boardPrevPage.Visible = false;
            boardNextPage.Visible = false;

            // 게시물 리스트 set
            foreach (DataRow dr in dt.Rows)
            {
                items.Add(new Board()
                {
                    title = dr["title"].ToString().Trim().Replace("\\", ""),
                    gukname = dr["name"].ToString().Trim(),
                    korename = (dr["name"].ToString().Trim().Contains(Util.GetNameByEmpno(dr["company_id"].ToString().Trim()))) ? "" : "(" + Util.GetNameByEmpno(dr["company_id"].ToString().Trim()) + ")",
                    reg_date = Util.SubDate(dr["reg_date"].ToString().Trim()),
                    id = dr["id"].ToString().Trim(),
                    userfile = (dr["user_file"].ToString().Trim() == "") ? "" : " <span class='board_list_userfiletag icon icon-clip'></span>",
                    newtag = (Convert.ToDouble(dr["newtag"].ToString().Trim()) > 24) ? "" : " <span class='board_list_newtag'>N</span>"
                });
            }

            boardListRepeater.DataSource = items;
            boardListRepeater.DataBind();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    // 새로고침 버튼 클릭
    protected void searchBtn_Click(object sender, EventArgs e)
    {
        boardPage = "0";

        if (boardName == "temp")
            SetBoardList_temp();
        else
            SetBoardList();
    }
}
