<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="DTETIPlagiarismDetection._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Styles/bootstrap.min.css" rel="stylesheet" />
    <link href="Styles/jquery-ui.css" rel="stylesheet" />
    <link href="Styles/ionicons.min.css" rel="stylesheet" />
    <title>Plagiarism Detection using Fingerprint Algorithm</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-group">
            <label for="teks1">Teks 1:</label>
           <%-- <textarea class="form-control" id="teks1" placeholder="Masukkan teks 1 untuk dibandingkan dengan teks 2" ></textarea>--%>
            <asp:TextBox ID="teks1" runat="server" ClientIDMode="Static" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
            <label for="hashresult1">Hasil hash 1:</label>
            <div class="alert alert-info"><asp:Literal ID="hashresult1" runat="server"></asp:Literal></div>
        </div>
        <div class="form-group">
            <label for="teks2">Teks 2:</label>
            <%--<textarea class="form-control" id="teks2" placeholder="Masukkan teks 2 untuk dibandingkan dengan teks 1" ></textarea>--%>
            <asp:TextBox ID="teks2" runat="server" ClientIDMode="Static" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
             <label for="hashresult2">Hasil hash 2:</label>
            <div class="alert alert-info"><asp:Literal ID="hashresult2" runat="server"></asp:Literal></div>
        </div>
        <%--<button type="submit" class="btn btn-default"><span class="ion ion-checkmark-circled"></span>&nbsp;Bandingkan!</button>--%>
        <asp:LinkButton ID="saveButton" runat="server" OnClick="saveButton_Click" CssClass="btn btn-default" Text='<span class="ion ion-checkmark-circled"></span>&nbsp;Bandingkan!'></asp:LinkButton>
        <div class="row" runat="server" id="divresult">
            <div class="col-md-6">
                <div class="alert alert-success"><span class="ion-calculator"></span>&nbsp;Jaccard: <asp:Literal ID="jaccardResult" runat="server"></asp:Literal></div>
            </div>
            <div class="col-md-6">
                <div class="alert alert-success"><span class="ion-calculator"></span>&nbsp;Pearson: <asp:Literal ID="pearsonResult" runat="server"></asp:Literal></div>
            </div>
        </div>
    </form>


    <script src="JS/jquery-3.1.0.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui.js" type="text/javascript"></script>
    <script src="JS/bootstrap.min.js" type="text/javascript"></script>
</body>
</html>
