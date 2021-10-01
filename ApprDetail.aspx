<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApprDetail.aspx.cs" Inherits="ApprDetail" %>

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

                    <div class="board_detail_section">
                        <div>
                            <asp:Label runat="server" ID="title" CssClass="board_detail_title" />
                        </div>
                        <div>
                            <asp:Label runat="server" ID="gukName" />
                            <asp:Label runat="server" ID="emplCode" Visible="false" />
                        </div>
                        <div>
                            <asp:Label runat="server" ID="id" CssClass="board_detail_id" />
                            <asp:Label runat="server" ID="koreName" CssClass="board_detail_hits" />
                            <asp:Label runat="server" ID="date" CssClass="board_detail_date" />
                        </div>
                    </div>
                    <asp:Label runat="server" ID="body" CssClass="board_detail_section board_detail_body" />

                    <asp:HyperLink runat="server" ID="startdate" CssClass="board_detail_section" />
                    <asp:HyperLink runat="server" ID="address" CssClass="board_detail_section" />
                    <asp:HyperLink runat="server" ID="refer" CssClass="board_detail_section" />
                    <asp:HyperLink runat="server" ID="userFile" CssClass="board_detail_section ellipsis" />

                    <div>
                        <asp:Button runat="server" ID="apprSign" CssClass="board_prev_page" OnClick="apprSign_Click" Text="승인" ForeColor="Red" />
                        <asp:Button runat="server" ID="apprHold" CssClass="board_next_page" OnClick="apprHold_Click" Text="보류" ForeColor="Black" />
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>

</html>
