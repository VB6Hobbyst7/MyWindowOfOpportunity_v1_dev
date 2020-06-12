<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TimelineEntry.ascx.vb" Inherits="UserControls_TimelineEntry" EnableViewState="true" %>
<%@ Register Src="TextBox_Animated.ascx" TagName="TextBox_Animated" TagPrefix="uc" %>
<%--<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="uc" %>--%>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Reference Control="AlertMsg.ascx" %>

<asp:Panel runat="server" ID="pnl" type="timelineEntry" CssClass="hide pnlTimelineEntry">
    <h6>Timeline Info</h6>
    <br class="hide-for-medium-down" />

    <div class="row">

        <div class="small-24 medium-24 large-8 columns">
            <uc:TextBox_Animated runat="server" Title="Heading" ID="txbTitle" />
            <div class="show-for-medium-down">
                <br />
            </div>
        </div>


        <div class="small-24 medium-24 large-8 columns">
            <div class="row">
                <div class="small-24 medium-6 large-8 columns large-text-right">
                    <h6 class="secondary">Entry Date</h6>
                    <br class="show-for-medium-only" />
                </div>
                <div class="small-24 medium-12 large-16 columns end">
                    <asp:TextBox runat="server" ID="txbCalendar" />
                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txbCalendar" Format="MMM dd, yyyy"></ajaxToolkit:CalendarExtender>
                    <br class="show-for-small-down" />
                </div>
            </div>
        </div>


        <div class="small-24 medium-24 large-8 columns">
            <div class="row">
                <div class="small-24 medium-9 large-16 columns large-text-right">
                    <h6 class="displayInline secondary">Show on timeline</h6>
                </div>
                <div class="small-24 medium-15 large-8 columns">
                    <div class="switch round">
                        <asp:CheckBox runat="server" ID="cbActive" />
                        <label runat="server" id="lbl">
                            <span class="switch-on">Yes</span>
                            <span class="switch-off">No</span>
                        </label>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <br />
    <h6 class="secondary">Timeline Text</h6>
    <%--<uc:CKEditorControl runat="server" ID="ckEditor" BasePath="/ckeditor/" />  --%>
    





    <textarea id="TimelineEntryContent" class="TimelineEntryContent" name="TimelineEntryContent" runat="server"></textarea>
    <div class="hide hflds existingTimelineEntryData">
        <input type="hidden" runat="server" id="hfldNodeId" class="hfldNodeId" />
        <input type="hidden" runat="server" id="hfldTimelineContent" class="hfldTimelineContent" />
    </div>






</asp:Panel>
