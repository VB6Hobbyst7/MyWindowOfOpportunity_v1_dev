<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TimelineSegment.ascx.vb" Inherits="UserControls_TimelineSegment" %>


<div class="cd-timeline-block">
    <div class="cd-timeline-img">
        <div class="cd-date">
            <asp:Literal runat="server" ID="ltrlDay" />
            <br />
            <asp:Literal runat="server" ID="ltrlYear" />
        </div>
    </div>
    <div class="cd-timeline-content">
        <h2><asp:Literal runat="server" ID="ltrlTitle" /></h2>
        <div>
            <asp:Literal runat="server" ID="ltrlContent" />
        </div>        
    </div>
</div>




<%--<asp:MultiView runat="server" ID="mvTimelineEntry" Visible="false">

    <asp:View runat="server" id="vLeftAlign">
        <div class="row path-item">
            <div class="large-14 push-10 columns">
                <div class="circle circle-left ">                    
                    <div class="entryDate">
                        <asp:Literal runat="server" ID="ltrlEntryDate_left" />
                    </div>
                </div>
            </div>
            <div class="large-10 pull-14 columns path-text left">
                <h3><asp:Literal runat="server" ID="ltrlHeading_left" /></h3>
                <asp:Literal runat="server" ID="ltrlContent_left" />
            </div>
            <span class="line hide-for-small"></span>
        </div>
    </asp:View>

    <asp:View runat="server" id="vrightAlign">
        <div class="row path-item">
            <div class="large-14 columns">
                <div class="circle circle-right right">
                    <div class="entryDate">
                        <asp:Literal runat="server" ID="ltrlEntryDate_right" />
                    </div>                    
                </div>
            </div>
            <div class="large-10 columns path-text right">
                <h3><asp:Literal runat="server" ID="ltrlHeading_right" /></h3>
                <asp:Literal runat="server" ID="ltrlContent_right" />
            </div>
            <span class="line hide-for-small"></span>
        </div>
    </asp:View>

</asp:MultiView>--%>


<%--<div class="cd-timeline-block">
    <div class="cd-timeline-img">
        <div class="cd-date">Jan 14<br />2017</div>
    </div>
    <div class="cd-timeline-content">
        <h2>Title of section 1</h2>
        <p>Lorem ipsum dolor sit amet, consectetur adipisicing elbitis unde? Iste voluptatibus minus veritatis qui ut.</p>
    </div>
</div>--%>