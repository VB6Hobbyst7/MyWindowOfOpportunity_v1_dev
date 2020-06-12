<%@ Control Language="VB" AutoEventWireup="false" CodeFile="editTab_Categories.ascx.vb" Inherits="UserControls_editTab_Categories" %>
<%@ Reference Control="AlertMsg.ascx" %>
<%@ Reference Control="CategoryEntry.ascx" %>


<div>
    <asp:HiddenField runat="server" ID="hfldTabName" />
    <asp:HiddenField runat="server" ID="hfldThisNodeId" />

    <div class="row small-uncollapse">
        <div class="columns">
            <h2 class="small-text-center medium-text-left">What categories does your campaign belong to?</h2>
            <div class="twoColumns">
                <p>The next step in creating your campaign is to categorize it. Selecting the proper categories for you campaign will allow people to more easily find your project in a sea of competing campaigns. But be honest here. Selecting categories that do not match your project may result in the suspension of your campaign.</p>
                <p>
                    To select a category, simply click a category below and then click any subcategory that applies to your campaign.  
                <span class="smaller">Don't see a matching category? 
                    <asp:HyperLink runat="server" ID="hlnkContactUs" Text="Contact us" CssClass="default" />
                    and let us know what you need and we'll see about adding it.</span>
                </p>
            </div>
        </div>
    </div>

    <asp:ListView runat="server" ID="lstviewSelectedCategories">
        <LayoutTemplate>
            <div class="row">
                <h6 class="small-text-center medium-text-left">This Campaign's Categories</h6>
                <ul class="small-block-grid-1 medium-block-grid-3 large-block-grid-6" data-equalizer data-equalizer-mq="medium-up">
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </ul>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <li class="liCategories round" data-equalizer-watch>
                <div class="bold">
                    <%#Eval("category")%> —
                </div>
                <div>
                    <%#Eval("subcategories")%>
                </div>
            </li>
        </ItemTemplate>
        <EmptyDataTemplate>
            <p>You have not selected any categories for your campaign yet.</p>
        </EmptyDataTemplate>
    </asp:ListView>


    <asp:PlaceHolder runat="server" ID="phSelectCategories">
        <hr />
        <div class="row">
            <div class="small-24 medium-16 large-20 columns">
                <h2 class="small-text-center medium-text-left">Select Your Categories</h2>
                <h3 class="smaller important">Important: Once published you will no longer be able to update your campaign's category.</h3>
                <br />
            </div>
            <div class="small-24 medium-8 large-4 columns">
                <asp:LinkButton runat="server" ID="lbtnSave" ValidationGroup="vg01" CssClass="shikobaButton nextPg button--shikoba green small button--round-s button--border-thin">
            <i class="button__icon fi-next size-24"></i>
            <span>Save</span>
                </asp:LinkButton>
            </div>

            <div class="small-24 medium-24 large-24 columns">
                <asp:PlaceHolder runat="server" ID="phAlertMsg" />
            </div>

            <div class="small-24 medium-8 large-5 columns">
                <h6>Categories</h6>
                <asp:RadioButtonList runat="server" ID="rblCategories" CssClass="categoryFilter" RepeatLayout="UnorderedList" />
            </div>

            <div class="hide-for-medium-down large-1 columns">&nbsp;</div>

            <div class="small-24 medium-16 large-18 columns">
                <h6>Subcategories</h6>
                <div class="panel categoryFilter">
                    <asp:PlaceHolder runat="server" ID="phSubcategories"></asp:PlaceHolder>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>

</div>

