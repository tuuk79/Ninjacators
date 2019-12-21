<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getSKUs.aspx.cs" Inherits="WebApplication3.getSKUs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Email address: <asp:Label runat="server" ID="emailText2" /><br />
            Contact ID: <asp:Label runat="server" ID="lblContactID" /><br />
            <br /><br />
            <asp:GridView ID="gvProducts" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="ProductId" HeaderText="ProductId" ItemStyle-Width="150" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" ItemStyle-Width="350" />
                    <asp:BoundField DataField="Sku" HeaderText="SKU" ItemStyle-Width="150" />
                </Columns>
            </asp:GridView>

            <asp:HyperLink ID="lnkCSV" runat="server" />
            <br />
            <asp:Label ID="lblError" runat="server" />
        </div>
    </form>
</body>
</html>
