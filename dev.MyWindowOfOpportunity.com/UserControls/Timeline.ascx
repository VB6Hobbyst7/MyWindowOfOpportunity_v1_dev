<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Timeline.ascx.vb" Inherits="UserControls_Timeline" %>
<%@ Register Src="/UserControls/TimelineSegment.ascx" TagName="TimelineSegment" TagPrefix="uc" %>


<asp:ListView runat="server" ID="lstviewTimeEntries">
    <LayoutTemplate>
        <section class="cd-container">    
        <%--<div class="path-container ">--%>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
        <%--</div>--%>
        </section>
    </LayoutTemplate>
    <ItemTemplate>
        <%--<div class="item">--%>
            <uc:TimelineSegment runat="server" nodeId='<%#Eval("nodeId")%>' oddCount='<%#Eval("oddCount")%>' />
        <%--</div>--%>
    </ItemTemplate>
    <EmptyDataTemplate>
        <div>No timeline has been created yet.</div>
    </EmptyDataTemplate>
</asp:ListView>


<%--<section class="cd-container">
    <div class="cd-timeline-block">
        <div class="cd-timeline-img">
            <div class="cd-date">Jan 14<br />2017</div>
        </div>
        <div class="cd-timeline-content">
            <h2>Title of section 1</h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Iusto, optio, dolorum provident rerum aut hic quasi placeat iure tempora laudantium ipsa ad debitis unde? Iste voluptatibus minus veritatis qui ut.</p>
        </div>
    </div>
    <div class="cd-timeline-block">
        <div class="cd-timeline-img">
            <div class="cd-date">Jan 14<br />2017</div>
        </div>
        <div class="cd-timeline-content">
            <h2>Title of section 2</h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Iusto, optio, dolorum provident rerum aut hic quasi placeat iure tempora laudantium ipsa ad debitis unde?</p>
        </div>
    </div>
    <div class="cd-timeline-block">
        <div class="cd-timeline-img">
            <div class="cd-date">Jan 14<br />2017</div>
        </div>
        <div class="cd-timeline-content">
            <h2>Title of section 3</h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Excepturi, obcaecati, quisquam id molestias eaque asperiores voluptatibus cupiditate error assumenda delectus odit similique earum voluptatem doloremque dolorem ipsam quae rerum quis. Odit, itaque, deserunt corporis vero ipsum nisi eius odio natus ullam provident pariatur temporibus quia eos repellat consequuntur perferendis enim amet quae quasi repudiandae sed quod veniam dolore possimus rem voluptatum eveniet eligendi quis fugiat aliquam sunt similique aut adipisci.</p>
        </div>
    </div>
    <div class="cd-timeline-block">
        <div class="cd-timeline-img">
            <div class="cd-date">Jan 14<br />2017</div>
        </div>
        <div class="cd-timeline-content">
            <h2>Title of section 3</h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Excepturi, obcaecati, quisquam id molestias eaque asperiores voluptatibus cupiditate error assumenda delectus odit similique earum voluptatem doloremque dolorem ipsam quae rerum quis. Odit, itaque, deserunt corporis vero ipsum nisi eius odio natus ullam provident pariatur temporibus quia eos repellat consequuntur perferendis enim amet quae quasi repudiandae sed quod veniam dolore possimus rem voluptatum eveniet eligendi quis fugiat aliquam sunt similique aut adipisci.</p>
        </div>
    </div>
    <div class="cd-timeline-block">
        <div class="cd-timeline-img">
            <div class="cd-date">Jan 14<br />2017</div>
        </div>
        <div class="cd-timeline-content">
            <h2>Title of section 3</h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Excepturi, obcaecati, quisquam id molestias eaque asperiores voluptatibus cupiditate error assumenda delectus odit similique earum voluptatem doloremque dolorem ipsam quae rerum quis. Odit, itaque, deserunt corporis vero ipsum nisi eius odio natus ullam provident pariatur temporibus quia eos repellat consequuntur perferendis enim amet quae quasi repudiandae sed quod veniam dolore possimus rem voluptatum eveniet eligendi quis fugiat aliquam sunt similique aut adipisci.</p>
        </div>
    </div>
    <div class="cd-timeline-block">
        <div class="cd-timeline-img">
            <div class="cd-date">Jan 14<br />2017</div>
        </div>
        <div class="cd-timeline-content">
            <h2>Title of section 3</h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Excepturi, obcaecati, quisquam id molestias eaque asperiores voluptatibus cupiditate error assumenda delectus odit similique earum voluptatem doloremque dolorem ipsam quae rerum quis. Odit, itaque, deserunt corporis vero ipsum nisi eius odio natus ullam provident pariatur temporibus quia eos repellat consequuntur perferendis enim amet quae quasi repudiandae sed quod veniam dolore possimus rem voluptatum eveniet eligendi quis fugiat aliquam sunt similique aut adipisci.</p>
        </div>
    </div>
</section>--%>
