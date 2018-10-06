<%@ Page Language="C#"  ViewStateMode ="Enabled" AutoEventWireup="true" CodeBehind="Result.aspx.cs" Inherits="WebChatBot.Result" %>

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

    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/pace.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/jquery.dataTables.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/Content/js/plugins/dataTables.bootstrap.min.js") %>"></script>
    <script type="text/javascript"> function openModal() { $('#modalDetail').modal('show'); }  $('#gvListData').DataTable({ "language": { "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Vietnamese.json" },  "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]]  });  $('#gvListData').DataTable(); </script>

</head>
<body> 
    <form runat="server">
      
   
          <asp:Label runat="server" ID="lblValue"  />  
         <div class="container">
            <div class="row">
                <div class="col-md-6">
                   <asp:Label runat="server" ID="lblWarningTableData"  Visible="false"  />                
                </div>
            </div>
           
         </div>
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <asp:GridView ID="gvListData" runat="server" AutoGenerateColumns="False" CssClass="table table-hover table-bordered" OnRowDataBound="gvListData_RowDataBound" PageSize="3">
                            <Columns>
                                <asp:TemplateField HeaderText="SỐ ĐƠN">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="policyurnCol" Text='<%# Eval("POLICY_URN") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TÊN KH">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="bunameCol" Text='<%# Eval("POLICYHOLDER_NAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ĐỊA CHỈ">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="locationCol" Text='<%# Eval("LOCATION") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                             
                                <asp:TemplateField HeaderText="THAO TÁC">
                                    <ItemTemplate>
             
                                            <asp:LinkButton runat="server" ID="btnViewDetail" Text="Xem" CssClass="btn btn-primary btn-sm btn-light" CommandArgument='<%# Eval("TT") %>'  OnCommand="btnViewDetail_Click"  />
                                     
                                       </ItemTemplate>
                                </asp:TemplateField>
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
                            <h4 class="modal-title">Thông tin chi tiết</h4>
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
                                    <asp:Literal ID="ltPOLICY_URN" runat="server"> </asp:Literal>, tham gia BH:
                                    
                                </p>
                                 <p>
                                    <b>Sản phẩm:</b>
                                    <asp:Literal ID="ltlPRODUCT_NAME" runat="server"></asp:Literal>
                                </p>
                                <p>
                                    <b>Phạm vị bảo hiểm:</b>
                                    <asp:Literal ID="ltlCOV_CLASS_NAME" runat="server"></asp:Literal>
                                </p>
                                 <p>
                                    <b>Số tiền:</b>
                                    <asp:Literal ID="ltlSUMINSURED_AMT" runat="server"></asp:Literal>
                                </p>
                                 <p>
                                    <b>Ngày nộp phí:</b>
                                    <asp:Literal ID="ltlNGAY_NOP_PHI" runat="server"></asp:Literal>
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
    </form>
  
    
</body>
</html>
