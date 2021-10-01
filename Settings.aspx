<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Settings" %>

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
                        <span class="board_preview_header"><%= GetGlobalResourceObject("Resource", "Board") %> 새 글 알림</span>

                        <div class="board_list">
                            <asp:CheckBox runat="server" ID="alarmHoisa" CssClass="board_preview_date settings_checkbox" AutoPostBack="true" OnCheckedChanged="alarm_CheckedChanged" />
                            <div class="ellipsis"><%= GetGlobalResourceObject("Resource", "Board_hoisa") %></div>
                        </div>
                        <div class="board_list">
                            <asp:CheckBox runat="server" ID="alarmSawon" CssClass="board_preview_date settings_checkbox" AutoPostBack="true" OnCheckedChanged="alarm_CheckedChanged" />
                            <div class="ellipsis"><%= GetGlobalResourceObject("Resource", "Board_sawon") %></div>
                        </div>
                        <div class="board_list">
                            <asp:CheckBox runat="server" ID="alarmSaju" CssClass="board_preview_date settings_checkbox" AutoPostBack="true" OnCheckedChanged="alarm_CheckedChanged" />
                            <div class="ellipsis"><%= GetGlobalResourceObject("Resource", "Board_saju") %></div>
                        </div>
                        <div class="board_list">
                            <asp:CheckBox runat="server" ID="alarmNojo" CssClass="board_preview_date settings_checkbox" AutoPostBack="true" OnCheckedChanged="alarm_CheckedChanged" />
                            <div class="ellipsis"><%= GetGlobalResourceObject("Resource", "Board_nojo") %></div>
                        </div>
                        <div class="board_list">
                            <asp:CheckBox runat="server" ID="alarmMarket" CssClass="board_preview_date settings_checkbox" AutoPostBack="true" OnCheckedChanged="alarm_CheckedChanged" />
                            <div class="ellipsis"><%= GetGlobalResourceObject("Resource", "Board_market") %></div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>

</html>
