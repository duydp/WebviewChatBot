<%@ Page  ViewStateMode ="Enabled" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebChatBot._Default" %>

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
       <%-- <div class="container">
            <div class="row">
                <div class="col-md-6">
                    <div id="custom-search-input">
                        <div class="input-group col-md-12">
                            <asp:TextBox class="form-control input-lg" ID="txtSearch" placeholder="..." runat="server" TabIndex="1" />
                            <span class="input-group-btn">
                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" OnClick="btnSearch_Click" Text="Tìm kiếm" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
                     
        </div>--%>
            
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

                                 <asp:TemplateField  HeaderText="STT" HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="NỘI DUNG">
                                    <ItemTemplate>
                                        <p>
                                         <b>Địa chỉ bảo hiểm :</b>
                                         <asp:Label runat="server" ID="lblDiachi" Text='<%# Eval("LOCATION") %>'/><br>
                                         <b>Khách hàng:</b>
                                          <asp:Label runat="server" ID="lblKH" Text='<%# Eval("POLICYHOLDER_NAME") %>' /><br>
                                         <b>Bảo hiểm tại:</b>
                                         <asp:Label runat="server" ID="lblCTY" Text='<%# Eval("BU_NAME") %>' />, từ
                                         <asp:Label runat="server" ID="lblTu" Text='<%# Eval("INCEPTION_DATE") %>' />
                                        đến
                                         <asp:Label runat="server" ID="lblDen" Text='<%# Eval("EXPIRY_DATE") %>' /><br>                                  
                                        <b>Số đơn:</b>
                                        <asp:Label runat="server" ID="lblSodon" Text='<%# Eval("POLICY_URN") %>' /><br>
                                        <b>Sản phẩm:</b>
                                          <asp:Label runat="server" ID="lblSanpham" Text='<%# Eval("PRODUCT_NAME") %>' /><br>
                                        <b>Phạm vi bảo hiểm:</b>
                                         <asp:Label runat="server" ID="lblPhamvi" Text='<%# Eval("COV_CLASS_NAME") %>' /><br>                                       
                                        <b>Tình trạng nộp phí:</b>
                                          <asp:Label runat="server" ID="lblTinhtrangthuphi" Text='<%# Eval("TINH_TRANG_THU_PHI") %>' />                                 
                                         <asp:Literal ID="ltlTonThat" runat="server"></asp:Literal><br>
                                        <b>LH CBKT:</b>
                                         <asp:Label runat="server" ID="lblCBKT" Text='<%# Eval("TRADE_NAME") %>' />. Số điện thoại:  <asp:Label runat="server" ID="lblPhone" Text='<%# Eval("TRADE_PHONE") %>' />
                                    </p>

                                      
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:TemplateField HeaderText="TÊN KH" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ĐỊA CHỈ" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                     
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CTY CẤP ĐƠN" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                      
                                    </ItemTemplate>
                                </asp:TemplateField>

                               <asp:TemplateField HeaderText="THAO TÁC"   ItemStyle-Width="40px">
                                     <ItemTemplate>             
                                            <asp:LinkButton runat="server" ID="btnViewDetail" Text="Xem" CssClass="btn btn-primary btn-sm btn-light" CommandArgument='<%# Eval("TT") %>'  OnCommand="btnViewDetail_Click"  />                                     
                                       </ItemTemplate>
                                </asp:TemplateField>--%>

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="container">
            <!-- The Modal Error -->
            <div class="modal fade" id="modalDetail">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Thông tin hồ sơ chi tiết</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <!-- Modal body -->
                        <div class="modal-body">
                            <fieldset>
                                <p>
                                    <b>Địa chỉ bảo hiểm :</b>
                                    <asp:Literal ID="ltlLOCATION" runat="server"> </asp:Literal>
                                </p>
                                <p>
                                    <b>Khách hàng:</b>
                                    <asp:Literal ID="ltlPOLICYHOLDER_NAME" runat="server"> </asp:Literal>
                                </p>
                                <p>
                                    <b>Bảo hiểm tại:</b>
                                    <asp:Literal ID="ltlBuName" runat="server"> </asp:Literal>, từ
                                    <asp:Literal ID="ltlINCEPTION_DATE" runat="server" />
                                    đến
                                    <asp:Literal ID="ltlEXPIRY_DATE" runat="server" />
                                </p>
                                <p>
                                    <b>Số đơn:</b>
                                    <asp:Literal ID="ltPOLICY_URN" runat="server"> </asp:Literal>
                                    
                                </p>
                                 <p>
                                    <b>Sản phẩm:</b>
                                    <asp:Literal ID="ltlPRODUCT_NAME" runat="server"></asp:Literal>
                                </p>
                                <p>
                                    <b>Phạm vi bảo hiểm:</b>
                                    <asp:Literal ID="ltlCOV_CLASS_NAME" runat="server"></asp:Literal>
                                </p>
                                
                                 <p>
                                    <b>Tình trạng nộp phí:</b>
                                    <asp:Literal ID="ltlTINH_TRANG_THU_PHI" runat="server"></asp:Literal>
                                </p>
                                 <p>
                                    <b>Số vụ tổn thất:</b>
                                    <asp:Literal ID="ltlSO_VU_BT" runat="server"></asp:Literal> vụ.Tỷ lệ BT:  <asp:Literal ID="ltlTY_LE_BT" runat="server"></asp:Literal>
                                </p>
                                 <p>
                                    <b>LH CBKT:</b>
                                    <asp:Literal ID="ltlTRADE_NAME" runat="server"></asp:Literal> .Số điện thoại: <asp:Literal ID="ltlTRADE_PHONE" runat="server"></asp:Literal>
                                </p>
                            </fieldset>
                        </div>
                        <!-- Modal footer -->
                        <div class="modal-footer">
                          
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Đóng</button>
                        </div>
                    </div>
                </div>
            </div>
               
        </div>

       </div> 
    </form>    

    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/pace.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/jquery.dataTables.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/dataTables.bootstrap.min.js") %>"></script>
    <script type="text/javascript"> function openModal() { $('#modalDetail').modal('show'); }  $('#gvListData').DataTable({ "language": { "url": "<%=Page.ResolveUrl("~/Content/js/plugins/Vietnamese.json") %>" },  "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]]  });  $('#gvListData').DataTable(); </script>

    <script>
        $(document).ready(function () {

            //$('#gridViewDiv').hide();


             <%--if (document.getElementById("<%=psid.ClientID %>").value == "" || document.getElementById("<%=hiChatbotID.ClientID %>").value == "") {
                 $("#gridViewDiv").html("<div class='alert text-danger'><strong>Thông báo!</strong> Bạn không được phép truy cập hệ thống. Bạn hãy liên hệ với Quản trị để được hướng dẫn!</div>");
             }--%>

             //if (document.getElementById("<%=psid.ClientID %>").value != document.getElementById("<%=hiChatbotID.ClientID %>").value) {
             //     $("#gridViewDiv").html("<div class='alert text-danger'><strong>Thông báo!</strong> Bạn không được phép truy cập hệ thống. Bạn hãy liên hệ với Quản trị để được hướng dẫn!</div>");
             // }
        });
    </script>
   
</body>
</html>

