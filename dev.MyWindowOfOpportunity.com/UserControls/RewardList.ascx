<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RewardList.ascx.vb" Inherits="UserControls_RewardList" %>


<asp:PlaceHolder runat="server" ID="phRewardList">
    <br />
    <h2>REWARDS LIST</h2>

    <asp:ListView runat="server" ID="lstviewRewards">
        <LayoutTemplate>
            <div class="RewardList">
                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <asp:Panel runat="server" ID="pnlRewardItem">
                <asp:HiddenField runat="server" ID="hfldInactive" Value='<%#Eval("isAvailable")%>' />


                <div class="row">
                    <div class="small-8 columns">
                        <h4>$<%#Eval("contributionAmount")%></h4>
                    </div>
                    <div class="small-16 columns text-right">
                        <h5><%#Eval("rewardTitle")%></h5>
                    </div>
                </div>



                <div class="row">
                    <div class="small-24 medium-6 large-24 columns">
                        <img alt="<%#Eval("featuredImgName")%>" src="<%#Eval("featuredImgUrl")%>" />
                    </div>
                    <div class="small-24 medium-18 large-24 columns">
                        <p><%#Eval("shortDescription")%></p>

                        <div class="row show-for-medium-only">
                            <br />
                            <div class="medium-6 columns">
                                <p><strong>Available:</strong> &nbsp;<span><asp:Literal ID="ltrAvailable_tblt" Text='<%#Eval("quantityAvailable")%>' runat="server"></asp:Literal></span></p>
                            </div>
                            <div class="medium-6 columns">
                                <p><strong>Claimed:</strong> &nbsp;<span><asp:Literal ID="ltrClaimed_tblt" Text='<%#Eval("quantityClaimed")%>' runat="server"></asp:Literal></span> </p>
                            </div>
                            <div class="medium-12 columns">
                                <p><strong>Estimated Shipping Date:</strong> &nbsp;<span><%#Eval("estimatedShippingMonth")%> &nbsp; <%#Eval("estimatedShippingYear")%></span> </p>
                            </div>
                        </div>

                    </div>
                </div>






                <div class="row hide-for-medium-only">
                    <div class="small-12 large-8 columns">
                        <p><strong>Available:</strong> &nbsp;<span><asp:Literal ID="ltrAvailable" Text='<%#Eval("quantityAvailable")%>' runat="server"></asp:Literal></span></p>
                    </div>
                    <div class="small-12 large-16 columns">
                        <p><strong>Claimed:</strong> &nbsp;<span><asp:Literal ID="ltrClaimed" Text='<%#Eval("quantityClaimed")%>' runat="server"></asp:Literal></span> </p>
                    </div>
                    <div class="small-24 large-24 columns">
                        <p><strong>Estimated Shipping Date:</strong> &nbsp;<span><%#Eval("estimatedShippingMonth")%> &nbsp; <%#Eval("estimatedShippingYear")%></span> </p>
                    </div>
                </div>



            </asp:Panel>
        </ItemTemplate>
        <ItemSeparatorTemplate>
            <hr />
        </ItemSeparatorTemplate>
        <EmptyDataTemplate>
            <div>No rewards have been created yet.</div>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:PlaceHolder>
