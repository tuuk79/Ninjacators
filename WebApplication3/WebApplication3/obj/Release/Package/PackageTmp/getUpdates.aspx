<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getUpdates.aspx.cs" Inherits="WebApplication3.GetInvoice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2><asp:Label ID="lblProducts" runat="server"></asp:Label></h2>
            <asp:GridView ID="GridView1" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Invoice Id" ItemStyle-Width="30" />
                    <asp:BoundField DataField="ContactId" HeaderText="Contact Id" ItemStyle-Width="150" />
                    <asp:BoundField DataField="ProductSold" HeaderText="Product Sold" ItemStyle-Width="150" />
                </Columns>
            </asp:GridView>
            <br /><br />
            <h2><asp:Label ID="lblOutput" runat="server"></asp:Label></h2>
            <asp:GridView ID="gvProducts" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="150" />
                    <asp:BoundField DataField="ProductId" HeaderText="Product Id" ItemStyle-Width="150" />
                    <asp:BoundField DataField="Sku" HeaderText="SKU" ItemStyle-Width="150" />
                </Columns>
            </asp:GridView>
            <br /><br />
            <asp:HyperLink ID="lnkCSV" runat="server" />
            <br />
            <asp:Label ID="lblError" runat="server" />

        </div>
    </form>
</body>
</html>
