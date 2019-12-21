<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hooks.aspx.cs" Inherits="WebApplication3.Hooks" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:LinkButton runat="server" ID="lnkGetAccessToken" OnClick="GetAccessToken_Click" Text="Get Access Token"></asp:LinkButton>
        </div>
        <div>
            <asp:LinkButton runat="server" ID="lnkSubscribeAddInvoicePayment" OnClick="SubscribeAddInvoicePayment_Click" Text="Subscribe to Adding Invoice Payment" CommandName="invoice.payment.add"></asp:LinkButton>
        </div>
        <div>
            <asp:LinkButton runat="server" ID="lnkSubscribeOrderEdit" OnClick="SubscribeOrderEdit_Click" Text="Subscribe to Order Edit" CommandName="order.edit"></asp:LinkButton>
        </div>
        <div>
            <asp:LinkButton runat="server" ID="lnkSubscribeDeleteInvoicePayment" OnClick="SubscribeDeleteInvoicePayment_Click" Text="Subscribe to Delete Invoice Payment" CommandName="invoice.payment.delete"></asp:LinkButton>
        </div>
        <div>
            <asp:LinkButton runat="server" ID="lnkSubscribeEditInvoicePayment" OnClick="SubscribeEditInvoicePayment_Click" Text="Subscribe to Edit Invoice Payment" CommandName="invoice.payment.edit"></asp:LinkButton>
        </div>
        <div>
            <asp:LinkButton runat="server" ID="lnkSubscribeDeleteInvoice" OnClick="SubscribeDeleteInvoice_Click" Text="Subscribe to Delete Invoice" CommandName="invoice.delete"></asp:LinkButton>
        </div>
        <div>
            <asp:LinkButton runat="server" ID="lnkSubscribeEditInvoice" OnClick="SubscribeEditInvoice_Click" Text="Subscribe to Edit Invoice" CommandName="invoice.edit"></asp:LinkButton>
        </div>
    </form>
</body>
</html>
