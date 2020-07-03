<%@ Page Language="C#" AutoEventWireup="True" Inherits="ReportViewerForMvc.ReportViewerWebForm" %>

<%--CodeBehind="ReportViewerWebForm.aspx.cs"--%>
<%--<%@ Import Namespace="System.Web.Mvc" %>--%>

<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Printing" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Runtime.InteropServices" %>
<%@ Import Namespace="System.Security.Permissions" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #ReportViewer1 {
            width: 100% !important;
            height: 100% !important;
        }
        #ReportViewer1_fixedTable{
        overflow:hidden !important;
        padding-left:20px;
        padding-right:20px;
        padding-bottom:20px;
        }
    </style>
    <script runat="server">
        protected override void OnLoadComplete(System.EventArgs e)
        {
            Label1.Text = (string)System.Web.HttpContext.Current.Session["pdffile"];
        }
    </script>
</head>
<body style="margin: 0px; padding: 0px;">
    <form id="form1" runat="server">
        <%--<asp:HiddenField ID="HiddenPath" runat="server"/>--%>
        <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;"></asp:Label>
        <div>
            <%--<asp:Button ID="Button1" runat="server" Text="Print" CausesValidation="False" OnClick="Button1_Click" Style="display: none;" />--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="ReportViewerForMvc" Name="ReportViewerForMvc.Scripts.PostMessage.js" />
                </Scripts>
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" BorderStyle="Solid"></rsweb:ReportViewer>
        </div>

         
    </form>
     


</div>
</body>
</html>

<script src="Scripts/jquery-1.11.2.min.js"></script>





<script type="text/javascript">
    $(document).ready(function () {
        console.log(jQuery.browser);
        if (jQuery.browser) {
            //$.browser.mozilla || $.browser.webkit
            try {
                showPrintButton();
            }
            catch (e) { alert(e); }
        }
    });

    function showPrintButton() {
        var table = $("table[title='Refresh']");
        var parentTable = $(table).parents('table');
        var parentDiv = $(parentTable).parents('div').parents('div').first();
        //var path = $("#HiddenPath").val() + "DoctorWise.pdf";
        //parentDiv.append('<select name="ctl00$ContentPlaceHolder1$reportViewer$ReportViewer1$ctl01$ctl03$ctl00" title="Zoom" id="ctl00_ContentPlaceHolder1_reportViewer_ReportViewer1_ctl01_ctl03_ctl00" style="font-family: Verdana; font-size: 8pt; onclick="PrintReport(); onChange="showzoom();"> <OPTION value=PageWidth>Page Width</OPTION> <OPTION value=FullPage>Whole Page</OPTION> <OPTION value=500>500%</OPTION> <OPTION value=200>200%</OPTION> <OPTION value=150>150%</OPTION> <OPTION selected value=100>100%</OPTION> <OPTION value=75>75%</OPTION> <OPTION value=50>50%</OPTION> <OPTION value=25>25%</OPTION> <OPTION value=10>10%</OPTION>');
        //parentDiv.append('<input name="ctl00$ContentPlaceHolder1$reportViewer$ReportViewer1$ctl01$ctl04$ctl00" title="Find Text" id="ctl00_ContentPlaceHolder1_reportViewer_ReportViewer1_ctl01_ctl04_ctl00" style="font-family: Verdana; font-size: 8pt;" onkeypress="keyprs();" onpropertychange="propertychnge();" type="text" size="10" maxLength="255"/>');
        //parentDiv.append('<a title="Find" id="ctl00_ContentPlaceHolder1_reportViewer_ReportViewer1_ctl01_ctl04_ctl01" style="font-family: Verdana; color: gray; font-size: 8pt; text-decoration: none;" href="#" onclick="Find();" onmouseover="mouseover();" onmouseout="mouseout();"  Controller="[object Object]">Find</a>');
        //parentDiv.append('<a title="Find Next" id="ctl00_ContentPlaceHolder1_reportViewer_ReportViewer1_ctl01_ctl04_ctl03" style="font-family: Verdana; color: gray; font-size: 8pt; text-decoration: none;" onmouseover="this.Controller.OnLinkHover();" onmouseout="this.Controller.OnLinkNormal();" onclick="Next();" href="#" Controller="[object Object]"> | Next</a>');
        //parentDiv.append('<input type="image" style="border-width: 0px;  padding: 3px;margin-top:2px; height:16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif";title="Print" onclick="PrintReport();">');
        parentDiv.append('<input type="image" style="border-width: 0px;  padding: 3px;margin-top:2px; height:16px; width: 16px;" alt="Print" src="/HISMCRS/Reserved.ReportViewerWebControl.axd?OpType=Resource&Version=11.0.3452.0&Name=Microsoft.Reporting.WebForms.Icons.Print.gif";title="Print" onclick="PrintReport();">');
    }

    function PrintReport() {

        if ($("#Label1")[0].textContent == "") {
            // print report viewer content for 1 page only.
            win = window;
            var dv = $("div[dir='LTR']").parent().html();
            var head = $('<head>').append($('head').clone()).html();

            alert(head);
            self.focus();
            win.document.open();
            win.document.write('<' + 'html' + '>' + head + '<' + 'body' + '>');
            win.document.write(dv);
            win.document.write('<' + '/body' + '><' + '/html' + '>');
            win.document.close();
            win.print();
         } else {
             //print pdf saved in server folder
            win = window.open("http://" + $("#Label1")[0].textContent, "_blank");
            win.print();
       
          

        }
    }

    $(document).ready(function () {
        if (jQuery.browser) {
            $(".reportViewerCtrl table").css('display', 'inline-block');
        }
    });

    $(DocReady);

    function DocReady() {
        $('option[value = PDF]').remove();


    }

    jQuery.uaMatch = function (ua) {
        ua = ua.toLowerCase();

        var match = /(chrome)[ /]([w.]+)/.exec(ua) ||
                /(webkit)[ /]([w.]+)/.exec(ua) ||
                /(opera)(?:.*version|)[ /]([w.]+)/.exec(ua) ||
                /(msie) ([w.]+)/.exec(ua) ||
                ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([w.]+)|)/.exec(ua) ||
                [];

        return {
            browser: match[1] || "",
            version: match[2] || "0"
        };
    };

    // Don't clobber any existing jQuery.browser in case it's different
    if (!jQuery.browser) {
        matched = jQuery.uaMatch(navigator.userAgent);
        browser = {};

        if (matched.browser) {
            browser[matched.browser] = true;
            browser.version = matched.version;
        }

        // Chrome is Webkit, but Webkit is also Safari.
        if (browser.chrome) {
            browser.webkit = true;
        } else if (browser.webkit) {
            browser.safari = true;
        }

        jQuery.browser = browser;
    }


  
</script>
