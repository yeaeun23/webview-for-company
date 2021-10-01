using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

public partial class ApprDetail : Page
{
    DataTable dt;
    string apprName = "";
    string type = "";
    string apprId = "";
    string apprDate = "";
    string apprDept = "";
    string jajecd = "";
    string empno = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Util.CheckSession();

        apprName = Request["apprName"];
        apprId = Request["apprId"];
        empno = Session["empno"].ToString();

        // 페이지 로드
        if (apprName == "board")        // 게시판(회사알림)
        {
            Util.SetPageTitle(pageTitle, "Appr_" + apprName);

            SetApprBoardDetail();
        }
        else if (apprName == "hyupjo")  // 협조전
        {
            type = Request["type"];

            if (type == "0")
                Util.SetPageTitle(pageTitle, "Appr_" + apprName);
            else
                Util.SetPageTitle(pageTitle, "Appr_" + apprName + "_hold");
            
            SetApprHyupjoDetail();
        }
        else if (apprName == "goods")   // 물품청구
        {
            apprDate = Request["apprDate"];
            apprDept = Request["apprDept"];

            Util.SetPageTitle(pageTitle, "Appr_" + apprName);

            SetApprGoodsDetail();
        }
    }

    private void SetApprBoardDetail()
    {
        startdate.Visible = false;
        address.Visible = false;
        refer.Visible = false;
        apprHold.Enabled = false;

        try
        {
            dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"select * from dmbbs.hoisa where id = {0}", apprId)), "SELECT");

            title.Text = dt.Rows[0]["title"].ToString().Trim().Replace("\\", "");
            gukName.Text = dt.Rows[0]["name"].ToString().Trim();
            emplCode.Text = dt.Rows[0]["company_id"].ToString().Trim();
            id.Text = "번호 " + dt.Rows[0]["id"].ToString().Trim();
            koreName.Text = " | 작성자 " + Util.GetNameByEmpno(dt.Rows[0]["company_id"].ToString().Trim());
            date.Text = " | 작성일 " + dt.Rows[0]["reg_date"].ToString().Trim();
            body.Text = dt.Rows[0]["body"].ToString().Trim().Replace("\n", "<br>").Replace("\\", "");

            if (dt.Rows[0]["user_file"].ToString().Trim() != "")
            {
                userFile.Text = "<b>첨부파일</b> " + dt.Rows[0]["user_file"].ToString().Trim();
                userFile.NavigateUrl = "https://mgate.seoul.co.kr/bbs/" + dt.Rows[0]["user_file"].ToString().Trim().Split('/')[1] + "/seoulcokr-" + dt.Rows[0]["user_file"].ToString().Trim().Split('/')[2];
            }
            else
            {
                userFile.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    private void SetApprHyupjoDetail()
    {
        if (type == "3")
            apprHold.Enabled = false;

        try
        {
            dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"select * from eip.siseip where doc_id = {0}", apprId)), "SELECT");

            title.Text = dt.Rows[0]["title"].ToString().Trim().Replace("\\", "");
            gukName.Text = dt.Rows[0]["sender_name"].ToString().Trim();
            emplCode.Text = dt.Rows[0]["sender_id"].ToString().Trim();
            id.Text = "번호 " + dt.Rows[0]["doc_id"].ToString().Trim();
            koreName.Text = " | 작성자 " + dt.Rows[0]["name"].ToString().Trim();
            date.Text = " | 작성일 " + dt.Rows[0]["maketime"].ToString().Trim();
            body.Text = dt.Rows[0]["body"].ToString().Trim().Replace("\n", "<br>").Replace("\\", "");
            startdate.Text = "<b>시행일</b> " + dt.Rows[0]["startdate"].ToString().Trim();
            address.Text = "<b>수신</b> " + SubAddressList(dt.Rows[0]["address"].ToString().Trim());

            if (dt.Rows[0]["refer"].ToString().Trim() != "")
            {
                refer.Text = "<b>참조</b> " + SubAddressList(dt.Rows[0]["refer"].ToString().Trim());
            }
            else
            {
                refer.Visible = false;
            }

            if (dt.Rows[0]["attach"].ToString().Trim() != "")
            {
                userFile.Text = "<b>첨부</b> " + dt.Rows[0]["attach"].ToString().Trim();
                userFile.NavigateUrl = "https://mgate.seoul.co.kr/other/siseip/file/seoulcokr-" + dt.Rows[0]["attach"].ToString().Trim();
            }
            else
            {
                userFile.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    private void SetApprGoodsDetail()
    {
        startdate.Visible = false;
        address.Visible = false;
        refer.Visible = false;
        userFile.Visible = false;
        apprHold.Enabled = false;

        try
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"SELECT c.reqdate, c.reqno, c.reqdeptcd, 
(SELECT deptname FROM SeoulERP.dbo.t_cmc040 WHERE deptcd = c.reqdeptcd) deptname, 
d.kempname, b.jajename, b.jajecd, b.size, a.balqty, 
(SELECT unitname FROM SeoulERP.dbo.t_cmc050 WHERE unitcd = b.unitcd) unitname, 
a.inhopedate, a.rmk 
FROM SeoulERP.dbo.t_mmt010 a, SeoulERP.dbo.t_mmc010 b, SeoulERP.dbo.t_mmt012 c, ( 
/*-------실장-------*/ 
SELECT a.reqdate, a.reqno, a.reqdeptcd, a.jajecd, b.kempname  
FROM SeoulERP.dbo.t_mmt012 a, SeoulERP.dbo.t_hrm010 b, 
(SELECT 1 sanction FROM SeoulERP.dbo.t_hrm010 WHERE empno = '{0}' AND 
((posdeptcd = '0500100' AND ISNULL(jobloccd, '') IN ('0324', '0330', '0370')) OR (ISNULL(pludeptcd, '') = '0500100' AND ISNULL(plujobloccd, '') IN ('0324', '0330', '0370')))) c 
WHERE a.empno = b.empno AND	a.reqdate = '{1}' AND a.reqno = '{2}' AND a.reqdeptcd = '{3}' AND a.mmtsanction1 = 'Y' AND a.mmtsanction2 = 'Y' AND a.mmtsanction3 = 'Y' AND a.sanction = 'N' AND c.sanction = 1 UNION 
/*-------일반-------*/ 
SELECT a.reqdate, a.reqno, a.reqdeptcd, a.jajecd, b.kempname  
FROM SeoulERP.dbo.t_mmt012 a, SeoulERP.dbo.t_hrm010 b 
WHERE a.empno = b.empno AND a.reqdate = '{1}' AND a.reqno = '{2}' AND a.reqdeptcd = '{3}' AND 
CASE WHEN a.approval1 = '{0}' THEN 'Y' WHEN a.approval2 = '{0}' THEN 'Y' WHEN a.approval3 = '{0}' THEN 'Y' ELSE 'N' END = 'Y' AND
CASE WHEN a.approval1 = '{0}' THEN a.sanction1 WHEN a.approval2 = '{0}' THEN a.sanction2 WHEN a.approval3 = '{0}' THEN a.sanction3 ELSE 'Y' END = 'N' AND
CASE WHEN a.approval1 = '{0}' THEN a.sanction2 WHEN a.approval2 = '{0}' THEN a.sanction3 WHEN a.approval3 = '{0}' THEN a.sanction3 ELSE 'Y' END = 'N' AND
CASE WHEN a.approval2 = '{0}' THEN a.sanction1 WHEN a.approval3 = '{0}' THEN a.sanction2 WHEN a.approval1 = '{0}' THEN 'Y' ELSE 'N' END = 'Y' AND 
a.mmtsanction1 = 'N' AND a.mmtsanction2 = 'N' AND a.mmtsanction3 = 'N' AND a.sanction = 'N' UNION 
/*-------총무일반-------*/ 
SELECT a.reqdate, a.reqno, a.reqdeptcd, a.jajecd, b.kempname  
FROM SeoulERP.dbo.t_mmt012 a, SeoulERP.dbo.t_hrm010 b, 
(SELECT 1 sanction FROM SeoulERP.dbo.t_hrm010 WHERE empno = '{0}' AND 
((posdeptcd = '0504000' AND ISNULL(jobloccd, '') NOT IN ('0340', '0342', '0345', '0350', '0382')) OR (ISNULL(pludeptcd, '') = '0504000' AND ISNULL(plujobloccd, '') NOT IN ('0340', '0342', '0345', '0350', '0382')))) c 
WHERE a.empno = b.empno AND a.reqdate = '{1}' AND a.reqno = '{2}' AND a.reqdeptcd = '{3}' AND 
CASE a.sanctionstep WHEN 0 THEN 'Y' WHEN 1 THEN a.sanction1 WHEN 2 THEN a.sanction2 WHEN 3 THEN a.sanction3 ELSE 'N' END = 'Y' AND 
a.mmtsanction1 = 'N' AND a.mmtsanction2 = 'N' AND a.mmtsanction3 = 'N' AND a.sanction = 'N' AND c.sanction = 1 UNION 
/*-------총무차장-------*/ 
SELECT a.reqdate, a.reqno, a.reqdeptcd, a.jajecd, b.kempname  
FROM SeoulERP.dbo.t_mmt012 a, SeoulERP.dbo.t_hrm010 b, 
(SELECT	1 sanction FROM SeoulERP.dbo.t_hrm010 WHERE empno = '{0}' AND 
((posdeptcd = '0504000' AND ISNULL(jobloccd, '') IN ('0350')) OR (ISNULL(pludeptcd, '') = '0504000' AND ISNULL(plujobloccd, '') IN ('0350')))) d 
WHERE a.empno = b.empno AND a.reqdate = '{1}' AND a.reqno = '{2}' AND a.reqdeptcd = '{3}' AND a.mmtsanction1 = 'Y' AND a.mmtsanction2 = 'N' AND a.mmtsanction3 = 'N' AND a.sanction = 'N' AND d.sanction = 1 UNION 
/*-------총무부장-------*/ 
SELECT a.reqdate, a.reqno, a.reqdeptcd, a.jajecd, b.kempname  
FROM SeoulERP.dbo.t_mmt012 a, SeoulERP.dbo.t_hrm010 b, 
(SELECT	1 sanction FROM SeoulERP.dbo.t_hrm010 WHERE empno = '{0}' AND 
((posdeptcd = '0504000' AND ISNULL(jobloccd, '') IN ('0340', '0342', '0345', '0382')) OR (ISNULL(pludeptcd, '') = '0504000' AND ISNULL(plujobloccd, '') IN ('0340', '0342', '0345', '0382')))) c 
WHERE a.empno = b.empno AND a.reqdate = '{1}' AND a.reqno = '{2}' AND a.reqdeptcd = '{3}' AND a.mmtsanction1 = 'Y' AND a.mmtsanction2 = 'Y' AND a.mmtsanction3 = 'N' AND a.sanction = 'N' AND c.sanction = 1) d 
WHERE a.jajecd = b.jajecd AND a.reqdate = d.reqdate AND a.reqno = d.reqno AND a.reqdeptcd = d.reqdeptcd AND a.jajecd = d.jajecd AND a.reqdate = c.reqdate AND a.reqno = c.reqno AND a.reqdeptcd = c.reqdeptcd AND a.jajecd = c.jajecd AND a.reqdate = '{1}' AND a.reqno = '{2}' AND a.reqdeptcd = '{3}'", empno, apprDate, apprId, apprDept)), "SELECT", "petra-sql2");

            title.Text = "물품청구 총 " + dt.Rows.Count + "건";
            gukName.Text = dt.Rows[0]["deptname"].ToString().Trim();
            id.Text = "번호 " + dt.Rows[0]["reqno"].ToString().Trim();
            koreName.Text = " | 작성자 " + dt.Rows[0]["kempname"].ToString().Trim();
            date.Text = " | 작성일 " + Util.SetDateFormat(dt.Rows[0]["reqdate"].ToString().Trim());

            foreach (DataRow dr in dt.Rows)
            {
                jajecd += dr["jajecd"].ToString().Trim() + "|";

                body.Text += "<b>품명</b> <span class='icon icon-arrow-right'></span>" + dr["jajename"].ToString().Trim()+"<br>";
                body.Text += "<b>규격</b> <span class='icon icon-arrow-right'></span>" + dr["size"].ToString().Trim()+ "<br>";
                body.Text += "<b>수량</b> <span class='icon icon-arrow-right'></span>" + dr["balqty"].ToString().Trim() + " (" + dr["unitname"].ToString().Trim() + ")<br>";
                body.Text += "<b>비고</b> <span class='icon icon-arrow-right'></span>" + dr["rmk"].ToString().Trim() + "<br>";
                body.Text += "<b>입고희망일</b> <span class='icon icon-arrow-right'></span>" + Util.SubDate(Util.SetDateFormat(dr["inhopedate"].ToString().Trim())) + "<hr>";
            }

            body.Text = body.Text.Substring(0, body.Text.Length - 4); // 마지막 <hr> 제거
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }    

    protected void apprSign_Click(object sender, EventArgs e)
    {
        try
        {
            if (apprName == "board")
            {
                // UPDATE
                dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"update dmbbs.hoisa set use_flag = 0 where id = {0}", apprId)), "UPDATE");

                // SELECT
                dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"select title from dmbbs.hoisa where id = {0}", apprId)), "SELECT");

                string message = dt.Rows[0]["title"].ToString().Trim().Replace("\\", "");

                // PUSH ALARM
                message = Regex.Replace(message, "<[^>]*>", string.Empty);

                byte[] b = Encoding.Unicode.GetBytes("[회사알림] " + message);
                byte[] buffer = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("EUC-KR"), b);

                WebRequest request = WebRequest.Create("https://mgate.seoul.co.kr/mobilepush/push.php?TYPE=hoisa&MESSAGE=" + HttpUtility.UrlEncode(buffer));
            }
            else if (apprName == "hyupjo")
            {
                string ret = get_docpos_withdocpos();

                if (ret != "")
                {
                    // UPDATE 1
                    dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"update eip.docpos set is_approval = '1', client_id = '{0}' where doc_id = {1} and lnempl_code = '{0}'", empno, apprId)), "UPDATE");

                    // UPDATE 2
                    int pos = Convert.ToInt32(ret);
                    int npos = pos + 1;

                    if (lastpos_check())
                    {
                        dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"update eip.siseip set doc_status = 1, doc_pos = {0}, doc_npos = {1}, sendtime = '{2}' where doc_id = {3}", pos, npos, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), apprId)), "UPDATE");
                    }
                    else
                    {
                        dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"update eip.siseip set doc_status = 0, doc_pos = {0}, doc_npos = {1} where doc_id = {2}", pos, npos, apprId)), "UPDATE");
                    }
                }
            }
            else if (apprName == "goods")
            {
                for (int i = 0; i < jajecd.Split('|').Length - 1; i++)
                {
                    using (SqlConnection con = new SqlConnection(@"Server=petra-sql2.seoul.co.kr;uid=erpcustomer;pwd=roqkfqn@1904;database=SeoulERP;"))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_submitGoods", con))
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@gs_UserCd", SqlDbType.VarChar).Value = empno;
                            cmd.Parameters.Add("@ls_ReqDate", SqlDbType.VarChar).Value = apprDate;
                            cmd.Parameters.Add("@li_ReqNo", SqlDbType.VarChar).Value = apprId;
                            cmd.Parameters.Add("@ls_ReqDeptCd", SqlDbType.VarChar).Value = apprDept;
                            cmd.Parameters.Add("@ls_JajeCd", SqlDbType.VarChar).Value = jajecd.Split('|')[i];

                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }

            Response.Redirect("ApprList.aspx?apprName=" + apprName + "&type=" + type);
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    protected void apprHold_Click(object sender, EventArgs e)
    {
        try
        {
            if (apprName == "hyupjo")
            {
                if (get_docpos_withdocpos() != "")
                    dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"update eip.siseip set doc_status = 3 where doc_id = {0}", apprId)), "UPDATE");
            }

            Response.Redirect("ApprList.aspx?apprName=" + apprName + "&type=" + type);
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    private string SubAddressList(string list)
    {
        string[] address = list.Split('\n');
        string new_list = "";

        for (int i = 0; i < address.Length; i++)
        {
            new_list += address[i].Split('-')[0].Trim();

            if (i != address.Length - 1)
                new_list += ", ";
        }

        return new_list;
    }

    private string get_docpos_withdocpos()
    {
        dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"select doc_pos from eip.docpos where doc_id = '{0}' and lnempl_code = '{1}' and doc_pos > 0", apprId, empno)), "SELECT");

        return dt.Rows[0]["doc_pos"].ToString().Trim();
    }

    private bool lastpos_check()
    {
        // SELECT 1
        dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"select max(doc_pos) as maxpos from eip.docpos where doc_id = {0}", apprId)), "SELECT");

        string strMax = dt.Rows[0]["maxpos"].ToString().Trim();

        // SELECT 2
        dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"select lnempl_code from eip.docpos where doc_id = {0} and doc_pos = {1}", apprId, strMax)), "SELECT");

        string lnempl_code = dt.Rows[0]["lnempl_code"].ToString().Trim();

        // COMPARE
        if (lnempl_code == empno)
            return true;
        else
            return false;
    }
}
