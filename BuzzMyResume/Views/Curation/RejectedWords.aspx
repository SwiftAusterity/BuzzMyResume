<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CurationList>" %>

<%@ Import Namespace="BuzzMyResume.ViewModels" %>
<%@ Import Namespace="Data.API.Interfaces.DO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Rejected Words List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List</h2>

    
    <% foreach (ISynonym synonym in Model.Synonyms)
       {
           Html.RenderPartial("Synonym", synonym);
       } %>

</asp:Content>
