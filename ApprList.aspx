<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApprList.aspx.cs" Inherits="ApprList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="ko" class="no-js">

<head runat="server">
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>모바일SIS</title>
    <link rel="stylesheet" type="text/css" href="css/normalize.css" />
    <link rel="stylesheet" type="text/css" href="css/demo.css" />
    <link rel="stylesheet" type="text/css" href="css/icons.css" />
    <link rel="stylesheet" type="text/css" href="css/component.css" />
    <link rel="stylesheet" type="text/css" href="css/style.css" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <script src="js/modernizr.custom.js"></script>
    <script src="js/classie.js"></script>
    <script src="js/mlpushmenu.js"></script>
    <script src="js/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#menu').load('Menu.aspx');
        });
    </script>
</head>

<body>
    <form runat="server" id="form1">
        <div class="container">
            <div id="mp-pusher" class="mp-pusher">
                <span id="menu"></span>

                <a href="#" id="trigger" class="header">
                    <asp:Label runat="server" ID="pageTitle" />
                    <span class="icon icon-arrow-right-3" />
                    <span class="icon-menu" />
                </a>

                <div class="contents">
                    <asp:Label runat="server" ID="ErrorMsg" />

                    <asp:Repeater runat="server" ID="ApprListRepeater">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" CssClass="board_list" NavigateUrl='<%# string.Format("ApprDetail.aspx?apprName={0}&type={1}&apprId={2}&apprDate={3}&apprDept={4}", Request.QueryString.Get("apprName"), Request.QueryString.Get("type"), Eval("id"), Eval("reqdate"), Eval("reqdeptcd")) %>'>
					            <div class="board_list_id">
                                    <%# Eval("id") + " |" %>
					            </div>
					            <div class="board_list_title ellipsis">                                    
						            <%# Eval("title") %>
					            </div>
				                <div class="board_list_name">
                                    <%# Eval("gukname") %><%# Eval("korename") %>
					            </div>
                                <div class="board_list_date ellipsis">
                                    <%# Eval("reg_date") %>
					            </div>
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:HyperLink runat="server" ID="empty" CssClass="board_list board_preview_empty ellipsis" Text="결재할 문서가 없습니다." Visible="false" />
                </div>

            </div>
        </div>
    </form>
</body>

</html>
