<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AccordionItem_Campaign.ascx.vb" Inherits="UserControls_AccordionItem_Campaign" %>

<%--<asp:GridView runat="server" ID="gv1" />--%>
<%--<h6>lstCampaigns</h6>
<asp:GridView runat="server" ID="gv1" />

<h6>lstPhases</h6>
<asp:GridView runat="server" ID="gv2" />

<h6>discovery Phase</h6>
<asp:GridView runat="server" ID="gv3" />

<h6>lstPledges</h6>
<asp:GridView runat="server" ID="gv4" />

<h6>lstPreviousPhases</h6>
<asp:GridView runat="server" ID="gv5" />

<h6>FINAL- Campaign Summary</h6>
<asp:GridView runat="server" ID="gv6" />--%>


<asp:HyperLink runat="server" ID="hlnkHandle">
    <div class="row campaign">
        <div class="small-24 columns accTopBanner">&nbsp;</div>
        <div class="small-22 medium-12 large-8 columns accName">
            <asp:Literal runat="server" ID="ltrlCampaignName_Handle" />
        </div>
        <div class="small-2 medium-1 large-1 columns accClosed end">▲</div>
        <div class="small-2 medium-1 large-1 columns accOpen end">▼</div>
    </div>
</asp:HyperLink>
<asp:Panel runat="server" ID="pnl" CssClass="content" ClientIDMode="Static">
    <div class="row campaign">
        <div class="small-6 columns text-right">
            <asp:Image runat="server" ID="imgFeaturedImage" CssClass="featuredImg" />
        </div>

        <div class="small- medium- large-18 columns">
            <h3>Campaign Summary</h3>
            <asp:Literal runat="server" ID="ltrlBriefSummary" />                        
            <div>
                <asp:HyperLink runat="server" ID="hrefViewCampaign" CssClass="green small right   shikobaButton nextPg button--shikoba button--round-s button--border-thin lbtn-preview-width-210">
                    <i class="button__icon fi-zoom-in size-16"></i>
                    <span>View</span>
                </asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Panel>
