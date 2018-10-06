<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vessel.aspx.cs" Inherits="WebChatBot.Vessel" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Tìm kiếm mở rộng cho nghiệp vụ tàu hàng</title>
    <!-- Main CSS-->
    <link rel="stylesheet" type="text/css" href="~/Content/css/main.css" />
    <link rel="stylesheet" type="text/css" href="~/Content/css/custom.css" />
    <!-- Font-icon css-->
    <link rel="stylesheet" type="text/css" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script src="<%=Page.ResolveUrl("~/Content/js/jquery-3.2.1.min.js") %>" type="text/javascript"></script>   
    <script src="<%=Page.ResolveUrl("~/Content/js/popper.min.js") %>" type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("~/Content/js/bootstrap.min.js") %>" type="text/javascript"></script>
</head>
<body>
    <script>
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) {
            return;
        }
        js = d.createElement(s);
        js.id = id;
        js.src = "//connect.facebook.com/en_US/messenger.Extensions.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'Messenger'));

        window.extAsyncInit = () => {   
        // TODO: How to parse env file from here?
         MessengerExtensions.getSupportedFeatures(function success(result) {
            let features = result.supported_features;
            if (features.includes("context")) {
                MessengerExtensions.getContext('<%=ConfigurationSettings.AppSettings["APP_ID"].ToString()%>',
                    function success(thread_context) {
                        // success
                        document.getElementById("<%=psid.ClientID %>").value = thread_context.psid;

                        if (thread_context.psid == document.getElementById("<%=hiChatbotID.ClientID %>").value) {
                            $('#gridViewDiv').show();
                        }
                        else {

                             $('#gridViewDiv').hide();

                        }                  

                        //document.getElementById("<%=lblShowText.ClientID %>").innerHTML = thread_context.psid + "=" + document.getElementById("<%=hiChatbotID.ClientID %>").value;
                    },
                    function error(err) {
                        // error
                        console.log(err);
                    });
            }
        }, function error(err) {
            // error retrieving supported features
            console.log(err);
        });
       
        };
       
</script>
    <form runat="server"  action="/optionspostback" method="get"> 
        <asp:HiddenField  runat="server" id="psid" />
        <asp:HiddenField  runat="server" id="hiChatbotID" />
        <asp:Label runat="server" ID="lblShowText" Visible="true"  /> 
        <asp:Label runat="server" ID="lblMsg" Visible="false"   />   
        <div id="gridViewDiv">
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                   <asp:Label runat="server" ID="lblWarningTableData"  Visible="false"  />                
                </div>
            </div>
         </div>
        <p/>
        <div class="container" >
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <asp:GridView ID="gvListData" runat="server"   AutoGenerateColumns="False" CssClass="table table-hover table-bordered" OnRowDataBound="gvListData_RowDataBound"  DataKeyNames="TT" PageSize="10"  >
                            <Columns>
                                 <asp:TemplateField  HeaderText="TT" HeaderStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NỘI DUNG" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Justify">
                                    <ItemTemplate>
                                                                                                                                                                                                              
                                          <asp:Literal ID="ltlMsgText" runat="server"></asp:Literal>                                    
                                                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
       </div> 
    </form>    
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/pace.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/jquery.dataTables.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/dataTables.bootstrap.min.js") %>"></script>
    <script type="text/javascript"> 
        $('#gvListData').DataTable({
            responsive: true,
            "language": { "url": "<%=Page.ResolveUrl("~/Content/js/plugins/Vietnamese.json") %>" },
            "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
            "columnDefs": [{
                "targets": [0],
                "orderable": true,
            },
            {
                "targets": [1]
                }],
            "order": [[0, "asc"]]
        });</script>

    <script>
        $(document).ready(function () {


        });
    </script>
</body>
</html>
