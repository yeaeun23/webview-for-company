using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home : Page
{
    DataTable dt;
    mgate.serviceSIS.WebServiceSISSoapClient sis = new mgate.serviceSIS.WebServiceSISSoapClient();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["start"] == "1")
        {
            Session["empno"] = Request.Headers["empno"];
            Session["sessionid"] = Request.Headers["sessionid"];

            Util.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (로그인) empno : " + Session["empno"].ToString() + " / sessionid : " + Session["sessionid"].ToString());
        }
        else
        {
            Session["empno"] = "2018010";
            Session["sessionid"] = "166A-D71D-ED32-A6C1-5DDB-175F";
        }

        Util.CheckSession();

        // 페이지 로드
        Util.SetPageTitle(pageTitle, "Home");
        SetBoardPreview();
    }

    private void SetBoardPreview()
    {
        SetBoardList(boardPreviewRepeater0, boardPreviewEmpty0, @"SELECT id, title, (UNIX_TIMESTAMP(now())-UNIX_TIMESTAMP(reg_date))/3600 as newtag, reg_date, user_file FROM dmbbs.covid WHERE use_flag = 0 and DATEDIFF(now(), reg_date) < 7 order by id desc limit 3");
        SetBoardList(boardPreviewRepeater1, boardPreviewEmpty1, @"SELECT id, title, (UNIX_TIMESTAMP(now())-UNIX_TIMESTAMP(reg_date))/3600 as newtag, reg_date, user_file FROM dmbbs.hoisa WHERE use_flag = 0 and DATEDIFF(now(), reg_date) < 7 order by id desc limit 3");
        SetBoardList(boardPreviewRepeater2, boardPreviewEmpty2, @"SELECT id, title, (UNIX_TIMESTAMP(now())-UNIX_TIMESTAMP(reg_date))/3600 as newtag, reg_date, user_file FROM dmbbs.sawon WHERE use_flag = 0 and DATEDIFF(now(), reg_date) < 7 order by id desc limit 3");
        SetBoardList(boardPreviewRepeater3, boardPreviewEmpty3, @"SELECT id, title, (UNIX_TIMESTAMP(now())-UNIX_TIMESTAMP(reg_date))/3600 as newtag, reg_date, user_file FROM dmbbs.saju WHERE use_flag = 0 and DATEDIFF(now(), reg_date) < 7 order by id desc limit 2");
        SetBoardList(boardPreviewRepeater4, boardPreviewEmpty4, @"SELECT id, title, (UNIX_TIMESTAMP(now())-UNIX_TIMESTAMP(reg_date))/3600 as newtag, reg_date, user_file FROM dmbbs.nojo WHERE use_flag = 0 and DATEDIFF(now(), reg_date) < 7 order by id desc limit 2");
        SetBoardList(boardPreviewRepeater5, boardPreviewEmpty5, @"SELECT id, title, (UNIX_TIMESTAMP(now())-UNIX_TIMESTAMP(regdate))/3600 as newtag, regdate, user_file FROM marketbbs.market WHERE delgb = 0 and DATEDIFF(now(), regdate) < 7 order by id desc limit 2");
    }

    private void SetBoardList(Repeater repeater, HyperLink empty, string cmd)
    {
        List<Board> items = new List<Board>();

        try
        {
            dt = Util.ExecuteQueryOdbc(new OdbcCommand(cmd), "SELECT");

            if (dt.Rows.Count == 0)
            {
                empty.Visible = true;
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    items.Add(new Board()
                    {
                        //title = dr.ItemArray[1].ToString().Trim().Replace("\\", ""),
                        // html 태그 제거
                        title = System.Text.RegularExpressions.Regex.Replace(dr.ItemArray[1].ToString().Trim().Replace("\\", ""), @"<(.|\n)*?>", string.Empty),
                        reg_date = Util.SubDate(dr.ItemArray[3].ToString().Trim()),
                        id = dr.ItemArray[0].ToString().Trim(),
                        userfile = (dr.ItemArray[4].ToString().Trim() == "") ? "" : " <span class='board_list_userfiletag icon icon-clip'></span>",
                        newtag = (Convert.ToDouble(dr.ItemArray[2].ToString().Trim()) > 24) ? "" : " <span class='board_list_newtag'>N</span>"
                    });
                }
            }

            repeater.DataSource = items;
            repeater.DataBind();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }
}
