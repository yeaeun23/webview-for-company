<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Menu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <script>
        new mlPushMenu(document.getElementById('mp-menu'), document.getElementById('trigger'), {
            type: 'cover'
        });
    </script>
</head>

<body>
    <asp:Label runat="server" ID="ErrorMsg" />

    <nav id="mp-menu" class="mp-menu">
        <div class="mp-level">
            <h2>
                <span class="icon icon-user" />
                <asp:Label runat="server" ID="session" />
            </h2>
            <ul>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Home.aspx" Text="<%$ Resources: Resource, Home %>" />
                </li>
                <li class="icon icon-arrow-left-3">
                    <a href="#"><%= GetGlobalResourceObject("Resource", "Board") %></a>
                    <div class="mp-level">
                        <h2><%= GetGlobalResourceObject("Resource", "Board") %></h2>
                        <asp:HyperLink runat="server" CssClass="mp-back" Text="<%$ Resources: Resource, mp_back %>" />
                        <ul>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="BoardList.aspx?boardName=temp" Text="<%$ Resources: Resource, Board_temp %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="BoardList.aspx?boardName=hoisa" Text="<%$ Resources: Resource, Board_hoisa %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="BoardList.aspx?boardName=sawon" Text="<%$ Resources: Resource, Board_sawon %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="BoardList.aspx?boardName=saju" Text="<%$ Resources: Resource, Board_saju %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="BoardList.aspx?boardName=nojo" Text="<%$ Resources: Resource, Board_nojo %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="BoardList.aspx?boardName=market" Text="<%$ Resources: Resource, Board_market %>" />
                            </li>
                        </ul>
                    </div>
                </li>
                <li class="icon icon-arrow-left-3">
                    <a href="#"><%= GetGlobalResourceObject("Resource", "Family") %></a>
                    <div class="mp-level" style="overflow: auto;">
                        <h2><%= GetGlobalResourceObject("Resource", "Family") %></h2>
                        <asp:HyperLink runat="server" CssClass="mp-back" Text="<%$ Resources: Resource, mp_back %>" />
                        <ul>
                            <asp:Repeater runat="server" ID="gukListRepeater">
                                <ItemTemplate>
                                    <li>
                                        <asp:HyperLink runat="server" NavigateUrl='<%# string.Format("Family.aspx?gukCode={0}&gukName={1}", Eval("code"), Eval("name")) %>' Text='<%# Eval("name") %>' />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="MyunList.aspx" Text="<%$ Resources: Resource, Myun %>" />
                </li>
                <li class="icon icon-arrow-left-3">
                    <a href="#"><%= GetGlobalResourceObject("Resource", "Work") %></a>
                    <div class="mp-level">
                        <h2><%= GetGlobalResourceObject("Resource", "Work") %></h2>
                        <asp:HyperLink runat="server" CssClass="mp-back" Text="<%$ Resources: Resource, mp_back %>" />
                        <ul>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="Work.aspx?type=workReg" Text="<%$ Resources: Resource, Work_workReg %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="Work.aspx?type=workAppr" Text="<%$ Resources: Resource, Work_workAppr %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="Work.aspx?type=accReg" Text="<%$ Resources: Resource, Work_accReg %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="Work.aspx?type=accAppr" Text="<%$ Resources: Resource, Work_accAppr %>" />
                            </li>
                        </ul>
                    </div>
                </li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="SMS.aspx" Text="<%$ Resources: Resource, SMS %>" />
                </li>
                <li class="icon icon-arrow-left-3">
                    <a href="#"><%= GetGlobalResourceObject("Resource", "Appr") %></a>
                    <div class="mp-level">
                        <h2><%= GetGlobalResourceObject("Resource", "Appr") %></h2>
                        <asp:HyperLink runat="server" CssClass="mp-back" Text="<%$ Resources: Resource, mp_back %>" />
                        <ul>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="ApprList.aspx?apprName=board" Text="<%$ Resources: Resource, Appr_board %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="ApprList.aspx?apprName=hyupjo&type=0" Text="<%$ Resources: Resource, Appr_hyupjo %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="ApprList.aspx?apprName=hyupjo&type=3" Text="<%$ Resources: Resource, Appr_hyupjo_hold %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="ApprList.aspx?apprName=goods" Text="<%$ Resources: Resource, Appr_goods %>" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="ApprGongmun.aspx" Text="<%$ Resources: Resource, ApprGongmun %>" />
                            </li>
                        </ul>
                    </div>
                </li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="Settings.aspx" Text="<%$ Resources: Resource, Settings %>" />
                </li>
            </ul>
        </div>
    </nav>
</body>

</html>
