<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AccordionItem_TeamMember.ascx.vb" Inherits="UserControls_AccordionItem_TeamMember" %>



<asp:HyperLink runat="server" ID="hlnkHandle" CssClass="handle">
    <div class="row member">
        <div class="small-24 columns accTopBanner">&nbsp;</div>
        <div class="small-22 medium-12 large-8 columns accName">
            <asp:Literal runat="server" ID="ltrlMemberName_heading" />
        </div>
        <div class="small-2 medium-1 large-1 columns accClosed end">▲</div>
        <div class="small-2 medium-1 large-1 columns accOpen end">▼</div>                       
    </div>
</asp:HyperLink>
<asp:Panel runat="server" ID="pnl" CssClass="content" ClientIDMode="Static">
    <div class="row accMember">
        <div class="small-12 small-push-6 medium-6 medium-push-0 columns">
            <asp:Image runat="server" ID="imgMemberPhoto" />
        </div>
        <div class="small-24 medium-18 columns">
            <div class="row">
                <div class="small-3 medium-3 columns">
                    <img src="/Images/temp/teamPage/teamMemberData.gif" alt="" />
                </div>
                <div class="small-19 medium-19 columns accMemberName">
                    <br class="hide-for-small-down" />
                    <asp:Literal runat="server" ID="ltrlRoles" />
                </div>
                <div class="small-2 medium-2 columns smallLogo">
                    <img src="/Images/Logo/smallLogo_transparent.png" alt="" />
                </div>
            </div>
            <div class="row">
                <div class="columns">
                    <br class="hide-for-small-down" />
                    <asp:Literal runat="server" ID="ltrlShortDescription" />
                </div>
            </div>
                            
        </div>
    </div>
</asp:Panel>