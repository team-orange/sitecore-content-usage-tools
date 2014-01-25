<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Teaser.ascx.cs" Inherits="ContentUsageTools.layouts.Orange.Teaser" %>

<h2>My teaser component</h2>
<div>
    <h1>
        <sc:Text ID="TitleField" runat="server" Field="Title" />
    </h1>
    <p>
        <sc:Text ID="TextField" runat="server" Field="Text" />
    </p>
</div>
