<%@ Page  ViewStateMode ="Enabled" Language="C#" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="WebChatBot._Default2" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Tìm kiếm</title>
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
      <%--  MessengerExtensions.getSupportedFeatures(function success(result) {
            let features = result.supported_features;            
            if (features.includes("context")) {
                MessengerExtensions.getContext('<%=ConfigurationSettings.AppSettings["APP_ID"].ToString()%>',
                    function success(thread_context) {
                        // success
                        document.getElementById("<%=psid.ClientID %>").value = thread_context.psid;
                    
                        if (thread_context.psid == document.getElementById("<%=hiChatbotID.ClientID %>").value) {

                               $("#gridViewDiv").load("Result.aspx?searchkey="+$("#hiKeyword").val(), { "KeyWord": "" + $("#hiKeyword").val() + "" }, function (response, status, xhr) {  
                                if (status == "error")  
                                        $("#gridViewDiv").html("Error: " + xhr.status + ": " + xhr.statusText);  
                                    //$("#gridViewDiv").html("<div class='alert text-danger'><strong>Thông báo!</strong> Bạn không được phép truy cập hệ thống. Bạn hãy liên hệ với Quản trị để được hướng dẫn!</div>");
                            }); 

                        }
                        //document.getElementById("<%=lblShowText.ClientID %>").innerHTML = thread_context.psid + "=" + document.getElementById("<%=hiChatbotID.ClientID %>").value;
                    },
                    function error(err) {
                        // error
                        console.log(err);
                    }
                );
            }
        }, function error(err) {
            // error retrieving supported features
            console.log(err);
        });--%>
     
    };
</script>
    <form runat="server">
        
        <asp:HiddenField  runat="server" id="psid" />
        <asp:HiddenField  runat="server" id="hiChatbotID" />
        <asp:HiddenField  runat="server" id="hiKeyword" />

        <asp:Label runat="server" ID="lblShowText" Visible="true"  /> 
        <asp:Label runat="server" ID="lblMsg"  />   
      
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                   <asp:Label runat="server" ID="lblWarning"  Visible="false"  />                
                </div>
            </div>
           
         </div>
        <div id="gridViewDiv"></div>          
    </form>
 
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/pace.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/jquery.dataTables.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/dataTables.bootstrap.min.js") %>"></script>
    <script type="text/javascript"> function openModal() { $('#modalDetail').modal('show'); }  $('#gvListData').DataTable({ "language": { "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Vietnamese.json" },  "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]]  });  $('#gvListData').DataTable(); </script>
     <script>
         $(document).ready(function () {
             <%--if (document.getElementById("<%=psid.ClientID %>").value == "" || document.getElementById("<%=hiChatbotID.ClientID %>").value == "") {
                 $("#gridViewDiv").html("<div class='alert text-danger'><strong>Thông báo!</strong> Bạn không được phép truy cập hệ thống. Bạn hãy liên hệ với Quản trị để được hướng dẫn!</div>");
             }--%>

             //if (document.getElementById("<%=psid.ClientID %>").value != document.getElementById("<%=hiChatbotID.ClientID %>").value) {
             //     $("#gridViewDiv").html("<div class='alert text-danger'><strong>Thông báo!</strong> Bạn không được phép truy cập hệ thống. Bạn hãy liên hệ với Quản trị để được hướng dẫn!</div>");
             // }

              $("#gridViewDiv").load("Result.aspx?searchkey="+$("#hiKeyword").val(), { "searchkey": "%hai phong%" }, function (response, status, xhr) {  
                    if (status == "error")  
                            $("#gridViewDiv").html("Error: " + xhr.status + ": " + xhr.statusText);  
                        //$("#gridViewDiv").html("<div class='alert text-danger'><strong>Thông báo!</strong> Bạn không được phép truy cập hệ thống. Bạn hãy liên hệ với Quản trị để được hướng dẫn!</div>");
                });  
        });
    </script>


    
</body>
</html>

