using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Web.UI;

public partial class ApprList : Page
{
    DataTable dt;
    string apprName = "";
    string type = "";
    string empno = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Util.CheckSession();

        apprName = Request["apprName"];
        empno = Session["empno"].ToString();

        // 페이지 로드
        if (apprName == "board")        // 게시판(회사알림)
        {
            Util.SetPageTitle(pageTitle, "Appr_" + apprName);

            SetApprBoardList();
        }
        else if (apprName == "hyupjo")  // 협조전
        {     
            type = Request["type"];

            if (type == "0")
                Util.SetPageTitle(pageTitle, "Appr_" + apprName);
            else                
                Util.SetPageTitle(pageTitle, "Appr_" + apprName + "_hold");

            SetApprHyupjoList();
        }
        else if (apprName == "goods")   // 물품청구
        {
            Util.SetPageTitle(pageTitle, "Appr_" + apprName);

            SetApprGoodsList();
        }
    }

    private void SetApprBoardList()
    {
        List<Board> items = new List<Board>();

        try
        {
            dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"select * from dmbbs.hoisa where use_flag = 5 and app = '{0}' order by id desc", empno)), "SELECT");

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
                        title = dr["title"].ToString().Trim().Replace("\\", ""),
                        gukname = dr["name"].ToString().Trim(),
                        korename = (Util.GetNameByEmpno(dt.Rows[0]["company_id"].ToString().Trim()) == dr["name"].ToString().Trim()) ? "" : "(" + Util.GetNameByEmpno(dt.Rows[0]["company_id"].ToString().Trim()) + ")",
                        reg_date = Util.SubDate(dr["reg_date"].ToString().Trim()),
                        id = dr["id"].ToString().Trim()
                    });
                }
            }

            ApprListRepeater.DataSource = items;
            ApprListRepeater.DataBind();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    private void SetApprHyupjoList()
    {
        List<Board> items = new List<Board>();

        try
        {
            dt = Util.ExecuteQueryOdbc(new OdbcCommand(string.Format(@"select * from eip.siseip as a left join eip.docpos as b on a.doc_id = b.doc_id where a.doc_status = '{0}' and a.doc_npos = b.doc_pos and b.lnempl_code = '{1}' order by a.doc_id desc", type, empno)), "SELECT");

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
                        title = dr["title"].ToString().Trim().Replace("\\", ""),
                        gukname = dr["sender_name"].ToString().Trim(),
                        reg_date = Util.SubDate(dr["maketime"].ToString().Trim()),
                        id = dr["doc_id"].ToString().Trim()
                    });
                }
            }

            ApprListRepeater.DataSource = items;
            ApprListRepeater.DataBind();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }

    private void SetApprGoodsList()
    {
        List<Board> items = new List<Board>();

        try
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"/*-------일반-------*/ 
SELECT a.reqdate, a.reqno, a.reqdeptcd, b.deptname, c.kempname, COUNT(a.jajecd) cnt,
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.empno)), '') '청구사원', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval1)), '') '결재자1', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval2)), '') '결재자2', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval3)), '') '결재자3', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval1)), '') '총무담당', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval2)), '') '총무차장', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval3)), '') '총무부장', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval)), '') '실장', 
MIN(a.sanction1) sanction1, 
CASE WHEN MAX(a.sanctionstep) > 1 THEN MIN(a.sanction2) ELSE '-' END sanction2, 
CASE WHEN MAX(a.sanctionstep) > 2 THEN MIN(a.sanction3) ELSE '-' END sanction3, 
MIN(a.mmtsanction1) mmtsanction1, 
MIN(a.mmtsanction2) mmtsanction2, 
MIN(a.mmtsanction3) mmtsanction3, 
MIN(a.sanction) sanction 
FROM SeoulERP.dbo.t_mmt012 a, SeoulERP.dbo.t_cmc040 b, SeoulERP.dbo.t_hrm010 c 
WHERE a.reqdeptcd = b.deptcd AND a.empno = c.empno AND a.reqdate >= '{1}' AND 
CASE WHEN a.approval1 = '{0}' THEN 'Y' WHEN a.approval2 = '{0}' THEN 'Y' WHEN a.approval3 = '{0}' THEN 'Y' ELSE 'N' END = 'Y' AND 
CASE WHEN a.approval1 = '{0}' THEN a.sanction1 WHEN a.approval2 = '{0}' THEN a.sanction2 WHEN a.approval3 = '{0}' THEN a.sanction3 ELSE 'Y' END = 'N' AND 
CASE WHEN a.approval1 = '{0}' THEN a.sanction2 WHEN a.approval2 = '{0}' THEN a.sanction3 WHEN a.approval3 = '{0}' THEN a.sanction3 ELSE 'Y' END = 'N' AND 
CASE WHEN a.approval2 = '{0}' THEN a.sanction1 WHEN a.approval3 = '{0}' THEN a.sanction2 WHEN a.approval1 = '{0}' THEN 'Y' ELSE 'N' END = 'Y' AND 
a.mmtsanction1 = 'N' AND a.mmtsanction2 = 'N' AND a.mmtsanction3 = 'N' AND a.sanction = 'N' 
GROUP BY a.reqdate, a.reqno, a.reqdeptcd, b.deptname, c.kempname UNION 
/*-------총무일반-------*/ 
SELECT a.reqdate, a.reqno, a.reqdeptcd, b.deptname, c.kempname, COUNT(a.jajecd) cnt, 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.empno)), '') '청구사원', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval1)), '') '결재자1', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval2)), '') '결재자2', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval3)), '') '결재자3', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval1)), '') '총무담당', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval2)), '') '총무차장', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval3)), '') '총무부장', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval)), '') '실장', 
MIN(a.sanction1) sanction1, 
CASE WHEN MAX(a.sanctionstep) > 1 THEN MIN(a.sanction2) ELSE '-' END sanction2, 
CASE WHEN MAX(a.sanctionstep) > 2 THEN MIN(a.sanction3) ELSE '-' END sanction3, 
MIN(a.mmtsanction1) mmtsanction1, 
MIN(a.mmtsanction2) mmtsanction2, 
MIN(a.mmtsanction3) mmtsanction3, 
MIN(a.sanction) sanction 
FROM SeoulERP.dbo.t_mmt012 a, SeoulERP.dbo.t_cmc040 b, SeoulERP.dbo.t_hrm010 c, 
(SELECT 1 sanction FROM SeoulERP.dbo.t_hrm010 WHERE empno = '{0}' AND ((posdeptcd = '0504000' AND ISNULL(jobloccd, '') NOT IN ('0340', '0342', '0345', '0350', '0382')) OR (ISNULL(pludeptcd, '') = '0504000' AND ISNULL(plujobloccd, '') NOT IN ('0340','0342', '0345', '0350', '0382')))) d 
WHERE a.reqdeptcd = b.deptcd AND a.empno = c.empno AND a.reqdate >= '{1}' AND 
CASE a.sanctionstep WHEN 0 THEN 'Y' WHEN 1 THEN a.sanction1 WHEN 2 THEN a.sanction2 WHEN 3 THEN a.sanction3 ELSE 'N' END = 'Y' AND 
a.mmtsanction1 = 'N' AND a.mmtsanction2 = 'N' AND a.mmtsanction3 = 'N' AND a.sanction = 'N' AND d.sanction = 1 
GROUP BY a.reqdate, a.reqno, a.reqdeptcd, b.deptname, c.kempname UNION 
/*-------총무차장-------*/ 
SELECT a.reqdate, a.reqno, a.reqdeptcd, b.deptname, c.kempname, COUNT(a.jajecd) cnt, 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.empno)), '') '청구사원', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval1)), '') '결재자1', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval2)), '') '결재자2', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval3)), '') '결재자3', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval1)), '') '총무담당', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval2)), '') '총무차장', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval3)), '') '총무부장', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval)), '') '실장', 
MIN(a.sanction1) sanction1, 
CASE WHEN MAX(a.sanctionstep) > 1 THEN MIN(a.sanction2) ELSE '-' END sanction2, 
CASE WHEN MAX(a.sanctionstep) > 2 THEN MIN(a.sanction3) ELSE '-' END sanction3, 
MIN(a.mmtsanction1) mmtsanction1, 
MIN(a.mmtsanction2) mmtsanction2, 
MIN(a.mmtsanction3) mmtsanction3, 
MIN(a.sanction) sanction 
FROM SeoulERP.dbo.t_mmt012 a, SeoulERP.dbo.t_cmc040 b, SeoulERP.dbo.t_hrm010 c, 
(SELECT 1 sanction FROM SeoulERP.dbo.t_hrm010 WHERE	empno = '{0}' AND ((posdeptcd = '0504000' AND ISNULL(jobloccd, '') IN ('0350')) OR (ISNULL(pludeptcd, '') = '0504000' AND ISNULL(plujobloccd, '') IN ('0350')))) d 
WHERE a.reqdeptcd = b.deptcd AND a.empno = c.empno AND a.reqdate >= '{1}' AND a.mmtsanction1 = 'Y' AND a.mmtsanction2 = 'N' AND a.mmtsanction3 = 'N' AND a.sanction = 'N' AND d.sanction = 1 
GROUP BY a.reqdate, a.reqno, a.reqdeptcd, b.deptname, c.kempname UNION 
/*-------총무부장-------*/ 
SELECT a.reqdate, a.reqno, a.reqdeptcd, b.deptname, c.kempname, COUNT(a.jajecd) cnt, 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.empno)), '') '청구사원', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval1)), '') '결재자1', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval2)), '') '결재자2', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval3)), '') '결재자3', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval1)), '') '총무담당', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval2)), '') '총무차장', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.mmtapproval3)), '') '총무부장', 
ISNULL((SELECT kempname FROM SeoulERP.dbo.t_hrm010 WHERE empno = MIN(a.approval)), '') '실장', 
MIN(a.sanction1) sanction1, 
CASE WHEN MAX(a.sanctionstep) > 1 THEN MIN(a.sanction2) ELSE '-' END sanction2, 
CASE WHEN MAX(a.sanctionstep) > 2 THEN MIN(a.sanction3) ELSE '-' END sanction3, 
MIN(a.mmtsanction1) mmtsanction1, 
MIN(a.mmtsanction2) mmtsanction2, 
MIN(a.mmtsanction3) mmtsanction3, 
MIN(a.sanction) sanction 
FROM SeoulERP.dbo.t_mmt012 a, SeoulERP.dbo.t_cmc040 b, SeoulERP.dbo.t_hrm010 c, 
(SELECT 1 sanction FROM SeoulERP.dbo.t_hrm010 WHERE empno = '{0}' AND ((posdeptcd = '0504000' AND ISNULL(jobloccd, '') IN ('0340', '0342',  '0345', '0382')) OR (ISNULL(pludeptcd, '') = '0504000' AND ISNULL(plujobloccd, '') IN ('0340', '0342', '0345', '0382')))) d 
WHERE a.reqdeptcd = b.deptcd AND a.empno = c.empno AND a.reqdate >= '{1}' AND a.mmtsanction1 = 'Y' AND a.mmtsanction2 = 'Y' AND a.mmtsanction3 = 'N' AND a.sanction = 'N' AND d.sanction = 1 
GROUP BY a.reqdate, a.reqno, a.reqdeptcd, b.deptname, c.kempname 
ORDER BY a.reqdate desc, a.reqno desc", empno, DateTime.Now.AddMonths(-1).ToString("yyyyMMdd"))), "SELECT", "petra-sql2");
            
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
                        title = "물품청구 총 " + dr["cnt"].ToString().Trim() + "건",
                        gukname = dr["deptname"].ToString().Trim(),
                        korename = "(" + dr["kempname"].ToString().Trim() + ")",
                        reg_date = Util.SubDate(Util.SetDateFormat(dr["reqdate"].ToString().Trim())),
                        id = dr["reqno"].ToString().Trim(),
                        reqdate= dr["reqdate"].ToString().Trim(),
                        reqdeptcd= dr["reqdeptcd"].ToString().Trim()
                    });
                }
            }

            ApprListRepeater.DataSource = items;
            ApprListRepeater.DataBind();
        }
        catch (Exception ex)
        {
            ErrorMsg.Text = ex.ToString();
        }
    }
}
