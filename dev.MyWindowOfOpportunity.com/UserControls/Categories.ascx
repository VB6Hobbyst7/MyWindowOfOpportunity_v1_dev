<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Categories.ascx.vb" Inherits="UserControls_Home_Categories" %>
<div class="categories">
    <h2 class="small-text-center medium-text-center">CATEGORIES</h2>

    <ul class="small-block-grid-2 medium-block-grid-3 large-block-grid-1 categories">
        <li class="hvr-underline-from-center small-text-right medium-text-left">
            <asp:HyperLink runat="server" ID="hlnkCategory01">
                <asp:Image runat="server" ID="imgCategory01" CssClass="hvr-icon-grow-rotate hide-for-small-down" />
                <asp:Literal runat="server" ID="ltrlCategory01" />
            </asp:HyperLink>
        </li>
        <li class="hvr-underline-from-center">
            <asp:HyperLink runat="server" ID="hlnkCategory02">
                <asp:Image runat="server" ID="imgCategory02" CssClass="hvr-icon-grow-rotate hide-for-small-down" />
                <asp:Literal runat="server" ID="ltrlCategory02" />
            </asp:HyperLink>
        </li>
        <li class="hvr-underline-from-center small-text-right medium-text-left">
            <asp:HyperLink runat="server" ID="hlnkCategory03">
                <asp:Image runat="server" ID="imgCategory03" CssClass="hvr-icon-grow-rotate hide-for-small-down" />
                <asp:Literal runat="server" ID="ltrlCategory03" />
            </asp:HyperLink>
        </li>
        <li class="hvr-underline-from-center">
            <asp:HyperLink runat="server" ID="hlnkCategory04">
                <asp:Image runat="server" ID="imgCategory04" CssClass="hvr-icon-grow-rotate hide-for-small-down" />
                <asp:Literal runat="server" ID="ltrlCategory04" />
            </asp:HyperLink>
        </li>
        <li class="hvr-underline-from-center small-text-right medium-text-left">
            <asp:HyperLink runat="server" ID="hlnkCategory05">
                <asp:Image runat="server" ID="imgCategory05" CssClass="hvr-icon-grow-rotate hide-for-small-down" />
                <asp:Literal runat="server" ID="ltrlCategory05" />
            </asp:HyperLink>
        </li>
        <li class="hvr-underline-from-center">
            <asp:HyperLink runat="server" ID="hlnkCategory06">
                <asp:Image runat="server" ID="imgCategory06" CssClass="hvr-icon-grow-rotate hide-for-small-down" />
                <asp:Literal runat="server" ID="ltrlCategory06" />
            </asp:HyperLink>
        </li>
        <li class="hvr-underline-from-center small-text-right medium-text-left">
            <asp:HyperLink runat="server" ID="hlnkCategory07">
                <asp:Image runat="server" ID="imgCategory07" CssClass="hvr-icon-grow-rotate hide-for-small-down" />
                <asp:Literal runat="server" ID="ltrlCategory07" />
            </asp:HyperLink>
        </li>
        <li class="hvr-underline-from-center">
            <asp:HyperLink runat="server" ID="hlnkCategory08">
                <asp:Image runat="server" ID="imgCategory08" CssClass="hvr-icon-grow-rotate hide-for-small-down" />
                <asp:Literal runat="server" ID="ltrlCategory08" />
            </asp:HyperLink>
        </li>
        <li class="hvr-underline-from-right hide-for-small-down medium-text-left viewMore">
            <asp:HyperLink runat="server" ID="hlnkViewAll">
              View All
            </asp:HyperLink>
        </li>
    </ul>
    
    <ul class="small-block-grid-1 hide-for-medium-up categories">
        <li class="hvr-underline-from-right small-text-center viewMore">
            <asp:HyperLink runat="server" ID="hlnkViewAll_Mbl">
                View All
            </asp:HyperLink>
        </li>
    </ul>

    <br />
</div>
