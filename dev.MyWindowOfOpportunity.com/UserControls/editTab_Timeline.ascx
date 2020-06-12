<%@ Control Language="VB" AutoEventWireup="false" CodeFile="editTab_Timeline.ascx.vb" Inherits="UserControls_editTab_Timeline" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Src="TimelineEntry.ascx" TagName="TimelineEntry" TagPrefix="uc" %>
<%@ Reference Control="TimelineEntry.ascx" %>
<%@ Reference Control="AlertMsg.ascx" %>



<div class="row timelineEntry">
    <%--<asp:GridView runat="server" ID="gv" EmptyDataText="No Rewards Data..." />--%>

    <div class="large-24 columns">
        <h2>Timeline</h2>
        <p>A campaign timeline is an effective way to keep your audience up-to-date with your current progress and milestones.  Click the <strong>Add New Entry</strong> link to enter a new timeline entry.</p>
    </div>

    <div class="small-24 medium-8 large-7 columns">
        <h6>Entries</h6>

        <ul class="timelineFilter newEntry">
            <li data-nodeId="-1">
                <input type="radio" value="-1" id='rbl_0_timeline' name="rblTimelineBtn" class="rblTimelineBtn" />
                <label for='rbl_0_timeline'>
                    <div>+ Add New Entry</div>
                </label>
            </li>
        </ul>

        <asp:ListView runat="server" ID="lvTimelineBtns">
            <LayoutTemplate>
                <fieldset class="timelineList">
                    <legend></legend>
                    <ul class="timelineFilter">
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </ul>
                </fieldset>
            </LayoutTemplate>
            <ItemTemplate>
                <li data-nodeId='<%#Eval("id")%>'>
                    <input type="radio" value='<%#Eval("id")%>' id='rbl_<%#Eval("id")%>' name="rblTimelineBtn" class="rblTimelineBtn" />
                    <label for='rbl_<%#Eval("id")%>'>
                        <div><strong><%#CDate(Eval("entryDate")).ToString("MMM dd, yyyy") %></strong></div>
                        <div><%#Eval("title")%></div>
                    </label>
                </li>
            </ItemTemplate>
            <EmptyDataTemplate></EmptyDataTemplate>
        </asp:ListView>
    </div>

    <div class="hide-for-small-down medium-1 large-1 columns">&nbsp;</div>

    <div class="small-24 medium-15 large-16 columns">
        <asp:PlaceHolder runat="server" ID="phAlertMsg" />
        
        <uc:TimelineEntry runat="server" ID="newTimelineEntry" thisNodeId="-1" />
        
        <asp:ListView runat="server" ID="lvTimelineEntries">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
            </LayoutTemplate>
            <ItemTemplate>
                <uc:TimelineEntry ID="TimelineEntry" runat="server" thisNodeId='<%#Eval("id")%>' />
            </ItemTemplate>
        </asp:ListView>
                
        <div class="row btnSave hide">
            <div class="small-24 medium-24 large-6 large-push-18 columns">
                <button type="button" id="btnSaveTimelineContent" name="btnSaveTimelineContent" class="right shikobaButton nextPg button--shikoba green  button--round-s button--border-thin">
                    <i class="button__icon fi-next size-24"></i>
                    <span>Save</span>
                </button>
                <asp:Button runat="server" ID="lbtnSaveTimelineContent" CssClass="hide" ClientIDMode="Static" />
            </div>
        </div>

    </div>
    <br />



        <div class="hide hiddenFields">
            <input type="hidden" runat="server" id="hfldNewTimelineEntry" class="hfldNewTimelineEntry" value="false" />
            <input type="hidden" runat="server" id="hfldTabName" class="hfldTabName" />
            <input type="hidden" runat="server" id="hfldThisNodeId" class="hfldThisNodeId" />
            <input type="hidden" runat="server" id="hfldIndexId" class="hfldIndexId" />
        </div>


</div>
