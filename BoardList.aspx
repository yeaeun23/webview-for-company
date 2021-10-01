<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BoardList.aspx.cs" Inherits="BoardList" %>

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

                    <div class="board_preview">
                        <div class="board_preview_header">
                            <select runat="server" id="searchType" class="cb">
                                <option value="title">제목</option>
                                <option value="body">내용</option>
                                <option value="name">작성자</option>
                            </select>
                            <asp:TextBox runat="server" ID="searchTxt" CssClass="cb" Width="185px" />
                            <asp:Button runat="server" ID="searchBtn" CssClass="cb" Text="새로고침" OnClick="searchBtn_Click" ForeColor="Black" />
                        </div>
                    </div>

                    <asp:Repeater runat="server" ID="boardListRepeater">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" CssClass="board_list" NavigateUrl='<%# string.Format("BoardDetail.aspx?boardName={0}&boardId={1}", Request.QueryString.Get("boardName"), Eval("id")) %>'>
					            <div class="board_list_id">
                                    <%# Eval("id") + " |" %>
					            </div>
					            <div class="board_list_title ellipsis">                                    
						            <%# Eval("title") %>
                                    <%# Eval("userfile") %>
                                    <%# Eval("newtag") %>
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

                    <div>
                        <asp:Button runat="server" ID="boardPrevPage" CssClass="board_prev_page" Text="이전" />
                        <asp:Button runat="server" ID="boardNextPage" CssClass="board_next_page" Text="다음" />
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>

</html>
