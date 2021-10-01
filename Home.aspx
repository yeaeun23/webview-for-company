<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

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
                    <span class="icon-menu" />
                </a>

                <div class="contents">
                    <asp:Label runat="server" ID="ErrorMsg" />

                    <div class="board_preview">
                        <asp:HyperLink runat="server" CssClass="board_preview_header" NavigateUrl="BoardList.aspx?boardName=temp">
                            <%= GetGlobalResourceObject("Resource", "Board_temp") %>
                            <span class="icon icon-arrow-right-3" />
                        </asp:HyperLink>

                        <asp:Repeater runat="server" ID="boardPreviewRepeater0">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" CssClass="board_list" NavigateUrl='<%# string.Format("BoardDetail.aspx?boardName=temp&boardId={0}", Eval("id")) %>'>
                                    <div style="display: none;">
                                        <%# Eval("id") %>
                                    </div>
                                    <div class="board_preview_date">
                                        <%# Eval("reg_date") %>
                                    </div>
                                    <div class="ellipsis">
                                        <%# Eval("title") %>
                                        <%# Eval("userfile") %>
                                        <%# Eval("newtag") %>
                                    </div>                                    
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:HyperLink runat="server" ID="boardPreviewEmpty0" CssClass="board_list board_preview_empty ellipsis" Text="<%$ Resources: Resource, board_preview_empty %>" Visible="false" />
                    </div>


                    <div class="board_preview">
                        <asp:HyperLink runat="server" CssClass="board_preview_header" NavigateUrl="BoardList.aspx?boardName=hoisa">
                            <%= GetGlobalResourceObject("Resource", "Board_hoisa") %>
                            <span class="icon icon-arrow-right-3" />
                        </asp:HyperLink>

                        <asp:Repeater runat="server" ID="boardPreviewRepeater1">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" CssClass="board_list" NavigateUrl='<%# string.Format("BoardDetail.aspx?boardName=hoisa&boardId={0}", Eval("id")) %>'>
                                    <div style="display: none;">
                                        <%# Eval("id") %>
                                    </div>
                                    <div class="board_preview_date">
                                        <%# Eval("reg_date") %>
                                    </div>
                                    <div class="ellipsis">
                                        <%# Eval("title") %>
                                        <%# Eval("userfile") %>
                                        <%# Eval("newtag") %>
                                    </div>                                    
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:HyperLink runat="server" ID="boardPreviewEmpty1" CssClass="board_list board_preview_empty ellipsis" Text="<%$ Resources: Resource, board_preview_empty %>" Visible="false" />
                    </div>


                    <div class="board_preview">
                        <asp:HyperLink runat="server" CssClass="board_preview_header" NavigateUrl="BoardList.aspx?boardName=sawon">
                            <%= GetGlobalResourceObject("Resource", "Board_sawon") %>
                            <span class="icon icon-arrow-right-3" />
                        </asp:HyperLink>

                        <asp:Repeater runat="server" ID="boardPreviewRepeater2">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" CssClass="board_list" NavigateUrl='<%# string.Format("BoardDetail.aspx?boardName=sawon&boardId={0}", Eval("id")) %>'>
                                    <div style="display: none;">
                                        <%# Eval("id") %>
                                    </div>
                                    <div class="board_preview_date">
                                        <%# Eval("reg_date") %>
                                    </div>
                                    <div class="ellipsis">
                                        <%# Eval("title") %>
                                        <%# Eval("userfile") %>
                                        <%# Eval("newtag") %>
                                    </div>                                    
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:HyperLink runat="server" ID="boardPreviewEmpty2" CssClass="board_list board_preview_empty ellipsis" Text="<%$ Resources: Resource, board_preview_empty %>" Visible="false" />
                    </div>


                    <div class="board_preview">
                        <asp:HyperLink runat="server" CssClass="board_preview_header" NavigateUrl="BoardList.aspx?boardName=saju">
                            <%= GetGlobalResourceObject("Resource", "Board_saju") %>
                            <span class="icon icon-arrow-right-3" />
                        </asp:HyperLink>

                        <asp:Repeater runat="server" ID="boardPreviewRepeater3">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" CssClass="board_list" NavigateUrl='<%# string.Format("BoardDetail.aspx?boardName=saju&boardId={0}", Eval("id")) %>'>
                                    <div style="display: none;">
                                        <%# Eval("id") %>
                                    </div>
                                    <div class="board_preview_date">
                                        <%# Eval("reg_date") %>
                                    </div>
                                    <div class="ellipsis">
                                        <%# Eval("title") %>
                                        <%# Eval("userfile") %>
                                        <%# Eval("newtag") %>
                                    </div>                                    
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:HyperLink runat="server" ID="boardPreviewEmpty3" CssClass="board_list board_preview_empty ellipsis" Text="<%$ Resources: Resource, board_preview_empty %>" Visible="false" />
                    </div>


                    <div class="board_preview">
                        <asp:HyperLink runat="server" CssClass="board_preview_header" NavigateUrl="BoardList.aspx?boardName=nojo">
                            <%= GetGlobalResourceObject("Resource", "Board_nojo") %>
                            <span class="icon icon-arrow-right-3" />
                        </asp:HyperLink>

                        <asp:Repeater runat="server" ID="boardPreviewRepeater4">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" CssClass="board_list" NavigateUrl='<%# string.Format("BoardDetail.aspx?boardName=nojo&boardId={0}", Eval("id")) %>'>
                                    <div style="display: none;">
                                        <%# Eval("id") %>
                                    </div>
                                    <div class="board_preview_date">
                                        <%# Eval("reg_date") %>
                                    </div>
                                    <div class="ellipsis">
                                        <%# Eval("title") %>
                                        <%# Eval("userfile") %>
                                        <%# Eval("newtag") %>
                                    </div>                                    
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:HyperLink runat="server" ID="boardPreviewEmpty4" CssClass="board_list board_preview_empty ellipsis" Text="<%$ Resources: Resource, board_preview_empty %>" Visible="false" />
                    </div>


                    <div class="board_preview">
                        <asp:HyperLink runat="server" CssClass="board_preview_header" NavigateUrl="BoardList.aspx?boardName=market">
                            <%= GetGlobalResourceObject("Resource", "Board_market") %>
                            <span class="icon icon-arrow-right-3" />
                        </asp:HyperLink>

                        <asp:Repeater runat="server" ID="boardPreviewRepeater5">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" CssClass="board_list" NavigateUrl='<%# string.Format("BoardDetail.aspx?boardName=market&boardId={0}", Eval("id")) %>'>
                                    <div style="display: none;">
                                        <%# Eval("id") %>
                                    </div>
                                    <div class="board_preview_date">
                                        <%# Eval("reg_date") %>
                                    </div>
                                    <div class="ellipsis">
                                        <%# Eval("title") %>
                                        <%# Eval("userfile") %>
                                        <%# Eval("newtag") %>
                                    </div>                                    
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:HyperLink runat="server" ID="boardPreviewEmpty5" CssClass="board_list board_preview_empty ellipsis" Text="<%$ Resources: Resource, board_preview_empty %>" Visible="false" />
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>

</html>
