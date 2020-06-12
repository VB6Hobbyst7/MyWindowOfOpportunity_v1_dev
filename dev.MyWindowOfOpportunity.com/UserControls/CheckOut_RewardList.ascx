<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CheckOut_RewardList.ascx.vb" Inherits="CheckOut_RewardList" %>
 

<div class="rewardItem row radius panel callout">   
    <div class="small-4 medium-4 large-1 columns">
        <br class="hide-for-medium-down" />
        <input type="radio"  name="rbtn2" class="rbtn2" value="0" data-ramt="0" data-rtit="No Thanks!" />
    </div>
    <div class="small-20 medium-20 large-23 columns">
        <div class="row">
            <div class="small- medium- large-18 columns">
                <h3>No Thanks</h3>
                <h4>I just want to help the campaign.</h4>
            </div>
            <div class="small- medium- large-6 columns text-right">
                <br class="show-for-medium-down" />
                <asp:LinkButton runat="server" ID="lbtnQuestionnair"  CssClass="mblFullWidth shikobaButton nextPg button--shikoba small button--round-s button--border-thin right lbtnContinue" ValidationGroup="vgEndorsement01">
                    <i class="button__icon fi-next size-24"></i>
                    <span>Continue</span>
                </asp:LinkButton>
            </div>
        </div>
    </div>
</div>



<asp:ListView runat="server" ID="lstviewRewards">
    <LayoutTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
    </LayoutTemplate>
    <ItemTemplate>
        <br />
        <asp:Panel runat="server" ID="pnlRewardItem" ClientIDMode="Static" CssClass="rewardItem row radius panel callout " data-amt='<%#Eval("contributionAmount")%>' data-act='<%#Eval("isAvailable")%>' >
            <asp:HiddenField runat="server" ID="hfldInactive" Value='<%#Eval("isAvailable")%>' />
            <div class="small-4 medium-4 large-1 columns">
                &nbsp;<br />
                <input type="radio" name="rbtn2" class="rbtn2" value='<%#Eval("nodeId")%>' data-ramt='<%#Eval("contributionAmount")%>' data-rtit='<%#Eval("rewardTitle")%>' />
            </div>
            <div class="small-20 medium-20 large-19 columns">
                <div class="row">
                    <div class="small- medium- large-18 columns">
                        <h3>$<%#Eval("contributionAmount")%></h3>
                        <h4><%#Eval("rewardTitle")%></h4>
                        <br class="show-for-medium-down" />
                    </div>
                    <div class="small-24 medium-24 large-6 columns text-right">
                        <asp:LinkButton runat="server" ID="lbtnContinue" CssClass="mblFullWidth shikobaButton nextPg button--shikoba small button--round-s button--border-thin right lbtnContinue" ValidationGroup="vgEndorsement01">
                            <i class="button__icon fi-next size-24"></i>
                            <span>Continue</span>
                        </asp:LinkButton>
                    </div>
                </div>
                <p><%#Eval("shortDescription")%></p>
                <div class="row stats">
                    <div class="small-12 medium-12 large-6 columns">
                        Available: &nbsp;<span><asp:Literal ID="ltrAvailable" Text='<%#Eval("quantityAvailable")%>' runat="server"></asp:Literal></span>
                    </div>
                    <div class="small-12 medium-12 large-6 columns">
                        Claimed: &nbsp;<span><asp:Literal ID="ltrClaimed" Text='<%#Eval("quantityClaimed")%>' runat="server"></asp:Literal></span>
                    </div>
                    <div class="small-24 medium-24 large-12 columns">
                        Estimated Shipping Date:<br class="show-for-small-down" /> &nbsp;<span><%#Eval("estimatedShippingMonth")%> &nbsp; <%#Eval("estimatedShippingYear")%></span>
                    </div>
                </div>
            </div>
            <div class="small-20 medium-20 large-4 columns">
                <br class="show-for-medium-down" />
                <img alt="<%#Eval("featuredImgName")%>" src="<%#Eval("featuredImgUrl")%>" />
            </div>
        </asp:Panel>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
</asp:ListView>




