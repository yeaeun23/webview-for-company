<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Family.aspx.cs" Inherits="Family" %>

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

            // 부서 클릭 시
            var getUrlParameter = function getUrlParameter(sParam) {
                var sURLVariables = window.location.search.substring(1).split('&');
                var sParameterName;

                for (var i = 0; i < sURLVariables.length; i++) {
                    sParameterName = sURLVariables[i].split('=');

                    if (sParameterName[0] === sParam)
                        return (sParameterName[1] === undefined) ? true : decodeURIComponent(sParameterName[1]);
                }
            };

            if (getUrlParameter('buCode') != undefined) {
                $('.empl_list').insertAfter('.' + getUrlParameter('buCode')); // 사원 리스트 위치
                $('.contents').scrollTop($('.' + getUrlParameter('buCode')).offset().top - $('.header').height()); // 스크롤 위치
            }
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
                            <input runat="server" id="searchTxt" class="cb" type="text" placeholder="이름/이메일/전화" style="width: 185px;" />
                            <asp:Button runat="server" ID="searchBtn" CssClass="cb" Text="검색" OnClick="searchBtn_Click" ForeColor="Black" />
                        </div>
                    </div>

                    <asp:Repeater runat="server" ID="buListRepeater">
                        <ItemTemplate>
                            <div class="board_preview">
                                <asp:HyperLink runat="server" CssClass='<%# "board_preview_header " + Eval("code") %>' NavigateUrl='<%# string.Format("Family.aspx?gukCode={0}&gukName={1}&buCode={2}", Request.QueryString.Get("gukCode"), Request.QueryString.Get("gukName"), Eval("code")) %>'>    
						            <span><%# Eval("name") %></span>
                                    <span class="icon icon-arrow-right-3" />
                                </asp:HyperLink>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <div class="empl_list">
                        <asp:Repeater runat="server" ID="emplListRepeater">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" CssClass="board_list" NavigateUrl='<%# string.Format("tel:{0}", Eval("hptel")) %>'>
                                    <img class="family_img" src='<%# string.Format("data:image/png;base64, {0}", Eval("emplimg")) %>' />
					                <div class="family_info">             
						                <div><%# Eval("kore_name") %></div>
                                        <div><%# Eval("team_name") %></div>
						                <div><%# Eval("degr_name") %></div>
						                <div><%# Eval("grad_name") %></div>
						                <div><%# Eval("team_name2") %></div>
						                <div><%# Eval("email") %></div>
						                <div><b>휴대전화</b> <span class='icon icon-arrow-right'></span><%# Eval("hptel") %></div>
						                <div><%# Eval("officetel") %></div>
						                <div><%# Eval("work") %></div>
						                <div class="point_color"><%# Eval("work2") %></div>
					                </div>
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <asp:HyperLink runat="server" ID="empty" CssClass="board_list board_preview_empty ellipsis" Text="검색 결과가 없습니다." Visible="false" />
                </div>

            </div>
        </div>
    </form>
</body>

</html>
