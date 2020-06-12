<%@ Control Language="VB" AutoEventWireup="false" CodeFile="editTab_Rewards.ascx.vb" Inherits="UserControls_editTab_Rewards" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Src="RewardEntry.ascx" TagName="RewardEntry" TagPrefix="uc" %>
<%@ Register Src="MinorImageSelector_Rewards.ascx" TagName="MinorImageSelector_Rewards" TagPrefix="uc" %>
<%@ Reference Control="RewardEntry.ascx" %>
<%@ Reference Control="AlertMsg.ascx" %>


        

<div class="row">
    <asp:HiddenField runat="server" ID="hfldTabName" />
    <asp:HiddenField runat="server" ID="hfldThisNodeId" />
    <input type="hidden" value="0" id="hfldActiveRewardNodeId" class="hfldActiveRewardNodeId" />

    
    <asp:GridView runat="server" ID="gv" />


    <div class="columns">
        <h2>Rewards</h2>
        <p>Rewards are an excellent means to encourage people to support and fund your campaign.  It allows people to feel they are getting something out of their generosity as well as being a part of something that interests them.  So click the <strong>Add New Entry</strong> link to add your rewards that you want to offer with your campaign. </p>
    </div>

    <div class="columns imgSelectPnl hide">
        <uc:MinorImageSelector_Rewards runat="server" ID="ucMinorImageSelector" activeView="SelectImg" thisTab="Rewards" />
    </div>
    <div class="entryPnls">
        <div class="small-24 medium-24 large-7 columns">
            <h6>Entries</h6>

            <ul class="rewardFilter">
                <li>
                    <input type="radio" value="-1" id='rbl_0_reward' name="rblRewardBtn" />
                    <label for='rbl_0_reward'>
                        <div>+ Add New Entry</div>
                    </label>
                </li>
            </ul>




            <asp:ListView runat="server" ID="lvRewardBtns">
                <LayoutTemplate>
                    <fieldset class="rewardList">
                        <legend></legend>
                        <ul class="rewardFilter">
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </ul>
                    </fieldset>
                </LayoutTemplate>
                <ItemTemplate>
                    <li>
                        <input type="radio" value='<%#Eval("nodeId")%>' id='rbl_<%#Eval("nodeId")%>' name="rblRewardBtn" />
                        <label for='rbl_<%#Eval("nodeId")%>'>
                            <div><strong><%#CDec(Eval("contributionAmount")).ToString("C") %></strong></div>
                            <div><%#Eval("rewardTitle")%></div>
                        </label>                        
                        <%--<input type="radio" value='<%#Eval("Id")%>' id='rbl_<%#Eval("Id")%>' name="rblRewardBtn" />
                        <label for='rbl_<%#Eval("Id")%>'>
                            <div><strong><%#CDec(Eval(Common.nodeProperties.contribution_Amount)).ToString("C") %></strong></div>
                            <div><%#Eval("NodeName")%></div>
                        </label>--%>
                    </li>
                </ItemTemplate>
                <EmptyDataTemplate></EmptyDataTemplate>
            </asp:ListView>


        </div>
        <div class="small-24 medium-24 large-1 columns">&nbsp;</div>
        <div class="small-24 medium-24 large-16 columns">
        

            <asp:PlaceHolder runat="server" ID="phAlertMsg" />

            
            <asp:ListView runat="server" ID="lvRewardEntries">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <uc:RewardEntry ID="RewardEntry" runat="server" thisNodeId='<%#Eval("nodeId")%>' />
                </ItemTemplate>
            </asp:ListView>

            <uc:RewardEntry runat="server" ID="newRewardEntry" thisNodeId="-1" />
            <input type="hidden" runat="server" value="false" id="hfldNewRewardEntry" class="hfldNewRewardEntry" />

            <div class="row btnSave hide">
                <br class="show-for-medium-down" />
                <div class="small-24 medium-12 medium-push-18 large-6 large-push-18 columns">
                    <asp:LinkButton runat="server" ID="lbtnSave" ValidationGroup="vg01" CssClass="shikobaButton nextPg button--shikoba green small button--round-s button--border-thin">
                        <i class="button__icon fi-next size-24"></i>
                        <span>Save</span>
                    </asp:LinkButton>
                </div>
            </div>

        </div>
    </div>

    <br />
</div>

