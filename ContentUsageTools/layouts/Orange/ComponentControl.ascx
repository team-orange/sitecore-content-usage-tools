<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComponentControl.ascx.cs" Inherits="ContentUsageTools.layouts.Orange.ComponentControl" %>

<h2>My component</h2>
<div>
    <h1>
        <sc:Text ID="TitleField" runat="server" Field="Title" />
    </h1>
    <p>
        <sc:Text ID="TextField" runat="server" Field="Text" />
    </p>
</div>
