using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Xml;

public partial class BoardDetail : Page
{
    DataTable dt;
    mgate.serviceSIS.WebServiceSISSoapClient sis = new mgate.serviceSIS.WebServiceSISSoapClient();
    string boardName = "";
    string boardId = "";
    string empno = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Util.CheckSession();

        if (Request["boardName"] == "" || Request["boardName"] == null)
        {
            Response.Redirect("BoardList.aspx");
        }
        else if (Request["boardId"] == "" || Request["boardId"] == null)
        {
            Response.Redirect("BoardList.aspx?boardName=" + Request["boardName"]);
        }
        else
        {
            boardName = Request["boardName"];
            boardId = Request["boardId"];
        }

        empno = Session["empno"].ToString();

        // 페이지 로드
        Util.SetPageTitle(pageTitle, "Board_" + boardName);

        if (boardName == "temp")
            SetBoardDetail_temp();
        else
            SetBoardDetail();
    }

    private void SetBoardDetail()
    {
        XmlDocument xml = new XmlDocument();

        try
        {
            xml.LoadXml(sis.GetBBSDataCount(boardName, boardId, empno));
            XmlNodeList xnList = xml.SelectNodes("item");

            foreach (XmlNode xn in xnList)
            {
                title.Text = xn["title"].InnerText.Trim().Replace("\\", "");

                if (xn["userfile"].InnerText.Trim() == "")
                    userFileTag.Visible = false;

                if (xn["newtag"].InnerText.Trim() == "0")
                    newTag.Visible = false;

                gukName.Text = xn["gukname"].InnerText.Trim();
                koreName.Text = xn["gukname"].InnerText.Trim().Contains(xn["korename"].InnerText.Trim()) ? "" : "(" + xn["korename"].InnerText.Trim() + ")";
                emplCode.Text = xn["empl_code"].InnerText.Trim();
                id.Text = "번호 " + xn["id"].InnerText.Trim();
                hits.Text = " | 조회 " + xn["hits"].InnerText.Trim();
                date.Text = " | 작성일 " + xn["reg_date"].InnerText.Trim();

                if (boardName == "market")
                    body.Text = xn["body"].InnerText.Trim().Replace("<br>", "").Replace("ahref", "a href");
                else
                    body.Text = xn["body"].InnerText.Trim().Replace("\\", "");

                if (xn["userfile"].InnerText.Trim() != "")
                {
                    userFile.Text = "<b>첨부</b> " + xn["userfile"].InnerText.Trim();
                    userFile.NavigateUrl = "https://mgate.seoul.co.kr/bbs/" + xn["userfile"].InnerText.Trim().Split('/')[1] + "/seoulcokr-" + xn["userfile"].InnerText.Trim().Split('/')[2];
                }
                else
                {
                    userFile.Visible = false;
                }

                if (xn["nextid"].InnerText.Trim() != "")
                {
                    prevId.Text = "<b>이전</b> <span class='icon icon-arrow-left-3'></span>" + GetTitle(xn["nextid"].InnerText.Trim());
                    prevId.NavigateUrl = "BoardDetail.aspx?boardName=" + boardName + "&boardId=" + xn["nextid"].InnerText.Trim();
                }
                else
                {
                    prevId.Visible = false;
                }

                if (xn["previd"].InnerText.Trim() != "")
                {
                    nextId.Text = "<b>다음</b> <span class='icon icon-arrow-right-2'></span>" + GetTitle(xn["previd"].InnerText.Trim());
                    nextId.NavigateUrl = "BoardDetail.aspx?boardName=" + boardName + "&boardId=" + xn["previd"].InnerText.Trim();
                }
                else
                {
                    nextId.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    private void SetBoardDetail_temp()
    {
        try
        {
            dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"SELECT title, user_file, (UNIX_TIMESTAMP(now())-UNIX_TIMESTAMP(reg_date))/3600 as newtag, name, company_id, id, count, reg_date, body, (SELECT id FROM dmbbs.covid WHERE id = (SELECT MIN(id) FROM dmbbs.covid WHERE id > {0} and use_flag = 0)) nextid, (SELECT id FROM dmbbs.covid WHERE id = (SELECT MAX(id) FROM dmbbs.covid WHERE id < {0} and use_flag = 0)) previd FROM dmbbs.covid WHERE id = {0}", boardId)), "SELECT");

            foreach (DataRow dr in dt.Rows)
            {
                title.Text = dr["title"].ToString().Trim().Replace("\\", "");

                if (dr["user_file"].ToString().Trim() == "")
                    userFileTag.Visible = false;

                if (Convert.ToDouble(dr["newtag"].ToString().Trim()) > 24)
                    newTag.Visible = false;

                gukName.Text = dr["name"].ToString().Trim();
                koreName.Text = dr["name"].ToString().Trim().Contains(Util.GetNameByEmpno(dr["company_id"].ToString().Trim())) ? "" : "(" + Util.GetNameByEmpno(dr["company_id"].ToString().Trim()) + ")";
                emplCode.Text = dr["company_id"].ToString().Trim();
                id.Text = "번호 " + dr["id"].ToString().Trim();
                hits.Text = " | 조회 " + dr["count"].ToString().Trim();
                date.Text = " | 작성일 " + dr["reg_date"].ToString().Trim();
                body.Text = dr["body"].ToString().Trim().Replace("\\", "").Replace("\r\n", "<br>");

                if (dr["user_file"].ToString().Trim() != "")
                {
                    userFile.Text = "<b>첨부</b> " + dr["user_file"].ToString().Trim();
                    userFile.NavigateUrl = "https://mgate.seoul.co.kr/bbs/" + dr["user_file"].ToString().Trim().Split('/')[1] + "/seoulcokr-" + dr["user_file"].ToString().Trim().Split('/')[2];
                }
                else
                {
                    userFile.Visible = false;
                }

                if (dr["nextid"].ToString().Trim() != "")
                {
                    prevId.Text = "<b>이전</b> <span class='icon icon-arrow-left-3'></span>" + GetTitle_temp(dr["nextid"].ToString().Trim());
                    prevId.NavigateUrl = "BoardDetail.aspx?boardName=" + boardName + "&boardId=" + dr["nextid"].ToString().Trim();
                }
                else
                {
                    prevId.Visible = false;
                }

                if (dr["previd"].ToString().Trim() != "")
                {
                    nextId.Text = "<b>다음</b> <span class='icon icon-arrow-right-2'></span>" + GetTitle_temp(dr["previd"].ToString().Trim());
                    nextId.NavigateUrl = "BoardDetail.aspx?boardName=" + boardName + "&boardId=" + dr["previd"].ToString().Trim();
                }
                else
                {
                    nextId.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    private string GetTitle(string id)
    {
        XmlDocument xml = new XmlDocument();
        string title = "";

        try
        {
            xml.LoadXml(sis.GetBBSDataCount(boardName, id, empno));
            XmlNodeList xnList = xml.SelectNodes("item");

            foreach (XmlNode xn in xnList)
            {
                title += xn["title"].InnerText.Trim().Replace("\\", "");
                title += (xn["userfile"].InnerText.Trim() == "") ? "" : " <span class='board_list_userfiletag icon icon-clip'></span>";
                title += (xn["newtag"].InnerText.Trim() == "0") ? "" : " <span class='board_list_newtag'>N</span>";
            }
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }

        return title;
    }

    private string GetTitle_temp(string id)
    {
        string title = "";

        try
        {
            dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"SELECT title, user_file, (UNIX_TIMESTAMP(now())-UNIX_TIMESTAMP(reg_date))/3600 as newtag FROM dmbbs.covid WHERE id = {0}", id)), "SELECT");

            foreach (DataRow dr in dt.Rows)
            {
                title += dr["title"].ToString().Trim().Replace("\\", "");
                title += (dr["user_file"].ToString().Trim() == "") ? "" : " <span class='board_list_userfiletag icon icon-clip'></span>";
                title += (Convert.ToDouble(dr["newtag"].ToString().Trim()) > 24) ? "" : " <span class='board_list_newtag'>N</span>";
            }
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }

        return title;
    }
}
