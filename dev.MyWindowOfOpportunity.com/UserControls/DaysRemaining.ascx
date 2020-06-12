<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DaysRemaining.ascx.vb" Inherits="UserControls_DaysRemaining" %>
<meta http-equiv="X-UA-Compatible" content="IE=11" >

<!--  SVG ROUND PROGRESS BAR TEST: http://codepen.io/andrecavallari/pen/eNZmJz/  -->
<div class="daysRemainingControl">
    <object>
    <svg version="1.1" viewBox="0 0 100 100" width="60"height="58">
        <circle r="35" cx="45" cy="45" class="progress-circle-back"></circle>
        <circle cx="45" cy="45" r="35" class="progress-circle" style="stroke-dashoffset: 255px;"></circle>
        <text x="45" y="46" text-anchor="middle" stroke="#ffffff" fill="#ffffff" stroke-width="0" font-size="26" class="daysRemaining">0</text>
        <text x="45" y="65" text-anchor="middle" stroke="#ffffff" fill="#ffffff" stroke-width="0" font-size="18">days</text>
   </svg>
         </object>
    <div class="hide hiddenFields">
        <input type="hidden" runat="server" id="hfldDaysRemainingPercent" class="hfldDaysRemainingPercent" value="0" />
        <input type="hidden" runat="server" id="hfldDaysRemaining" class="hfldDaysRemaining" value="0" />
    </div>
        
</div>
