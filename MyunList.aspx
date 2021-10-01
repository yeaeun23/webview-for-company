<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyunList.aspx.cs" Inherits="MyunList" %>

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

                    <div class="board_preview_header">
                        <asp:TextBox runat="server" ID="dateTB" CssClass="cb" TextMode="Date" AutoPostBack="true" />
                        <asp:DropDownList runat="server" ID="panCB" CssClass="cb" AutoPostBack="true" />
                        <asp:Button runat="server" ID="refreshBtn" CssClass="cb" Text="새로고침" OnClick="refreshBtn_Click" ForeColor="Black" />
                    </div>

                    <asp:Repeater runat="server" ID="myunListRepeater">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" CssClass="board_list" NavigateUrl='<%# Eval("thumb").ToString().Replace("thumb", "original") %>'>
                                <img class="family_img" src='<%# Eval("thumb") %>' />
					            <div class="family_info">                                    
						            <div style="margin-bottom: 10px;"><%# Eval("Page") %></div>
						            <div><%# Eval("InputTime") %></div>
						            <div><%# Eval("OutputTime") %></div>
					            </div>
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:Repeater>

                </div>
            </div>
        </div>
    </form>
</body>

</html>
