<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ArchiveList>" %>

<%@ Import Namespace="BuzzMyResume.ViewModels" %>
<%@ Import Namespace="Data.API.Interfaces.DO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Translation Archive
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Archive</h2>
    
    <% foreach (ITranslation translation in Model.Translations)
       {
           Html.RenderPartial("Translation", translation);
       } %>
    
</asp:Content>
