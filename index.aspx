<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="DXFReaderNETWebApplication.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>DXFReader.NET Component</h1>
            <h4>DXFReader.NET is a .NET component that allows viewing, manipulating and plotting direct from the AutoCAD drawing file format DXF, also known as the drawing exchange format.</h4>



            <div style="margin-top: 10px; margin-bottom: 10px;">



                <asp:Literal ID="LiteralButtonUpload" runat="server"></asp:Literal>
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" Width="364px" accept=".dxf" onchange=" this.form.submit();" />
                <%--<asp:FileUpload ID="FileUpload1" runat="server" Style="visibility: hidden;" CssClass="form-control" accept=".dxf" onchange=" this.form.submit();" Height="0" />--%>
                <br />
                <asp:Label ID="LabelError" runat="server" Text="" Style="font-weight: bold; color: Red;"></asp:Label>

                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="LiteraImage" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="LiteralText" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>

            </div>


        </div>
        <p>
            <p>&copy; <%: DateTime.Now.Year %> - Kadmos.com</p>
        </p>
    </form>
</body>
</html>
