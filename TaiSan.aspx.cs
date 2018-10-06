using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Configuration;
using System.Web.SessionState;
using System.Security;
using System.Text;
using System.IO;
using System.Web.Security;
using System.Security.Cryptography;
using System.Collections;
using System.Text.RegularExpressions;

namespace WebChatBot
{
    public partial class TaiSan : System.Web.UI.Page
    {
        public string Keyword
        {
            get { if (ViewState["Keyword"] != null) return ViewState["Keyword"].ToString(); else return ""; }
            set { ViewState["Keyword"] = value; }
        }

        public string ServiceType
        {
            get { if (ViewState["ServiceType"] != null) return ViewState["ServiceType"].ToString(); else return ""; }
            set { ViewState["ServiceType"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
              
                string strID = Server.UrlDecode(Request.QueryString["id"]);
                //Keyword = Server.UrlDecode(Request.QueryString["strkey"]);
                Keyword = Request.QueryString["strkey"];
                ServiceType = Server.UrlDecode(Request.QueryString["svtype"]);

                hiChatbotID.Value = strID;

                if (strID == null && Keyword == "" && ServiceType == "")
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "<div class='alert text-danger'>"
                                                     + "<strong>Thông báo!</strong> Bạn không được phép truy cập hệ thống. Bạn hãy liên hệ với Quản trị để được hướng dẫn!</div>";

                }
                else
                {
                    if (Keyword != "") LoadData(Keyword);

                }

                //if (Keyword != "") LoadData(Keyword);
                //if (Keyword != "" && Keyword != null && strID != null && strID != "") 

            }

        }

        protected void LoadData(string Keyword)
        {
            int PageNumber = int.Parse(ConfigurationManager.AppSettings["PageNumber"].ToString());
            int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

            string Authen_key = ConfigurationManager.AppSettings["Authen_key"].ToString();


            WSChatBot.CommonInfo wsINSUJ = new WSChatBot.CommonInfo();

            // Su dung WS insureJ theo dinh nghia 

            wsINSUJ.Url = ConfigurationSettings.AppSettings["INSUJ_SERVICE_URL"].ToString();

            wsINSUJ.Timeout = 1000000;
            XmlDocument Xml = new XmlDocument();

            if (ServiceType == "ts_location")
            {
                Xml.LoadXml(wsINSUJ.GetPolicyFireInfo(Authen_key, "", "", "", "", Keyword, PageNumber, PageSize));
            }
            if (ServiceType == "ts_sodon")
            {
               
                Xml.LoadXml(wsINSUJ.GetPolicyFireInfo(Authen_key, Keyword, "", "", "", "", PageNumber, PageSize));
            }

            if (ServiceType == "ts_tkh")
            {
                Xml.LoadXml(wsINSUJ.GetPolicyFireInfo(Authen_key, "", "", "", "", Keyword, PageNumber, PageSize));

            }
           

            //GetCarInfo


            DataSet ds = new DataSet();

            ds.ReadXml(new XmlTextReader(new StringReader(Xml.DocumentElement.OuterXml)));

            DataTable dt = ds.Tables[0];

            if (dt.Columns.Count == 1 && dt.Rows[0]["DATA"].ToString() == "NO DATA FOUND")
            {

                //< i class='fa fa-warning'></i>
                lblWarningTableData.Text = "<div class='alert text-danger'>"
                                                 + "<strong>Thông báo!</strong> Không tìm thấy dữ liệu</div>";

                //lblWarningTableData.Text= "<i class='fa fa-warning'></i> Không có dữ liệu";
                lblWarningTableData.Visible = true;
            }
            else
            {
                lblWarningTableData.Visible = false;

                //dt.Columns.Add("STT", typeof(string));
                //int i = 0;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    i++;
                //    dr["STT"] = i.ToString();
                //}
                //RowCount = dt.Rows.Count * PageCount;

                Session["dtSelect"] = dt;
                gvListData.DataSource = dt;
                gvListData.DataBind();
                gvListData.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        public static string ReformatPhone(string phone)
        {
            
            //string[] arr = phone.Split("/; ".ToCharArray());
            string[] arr = phone.Split("/;^+ |D".ToCharArray());
            string result = "";
            foreach (string str in arr)
            {
                if (str != "")
                 {
                    if (result != "") result += "/";
                    result += str;
                }
            }

            //string result = Regex.Replace(phone, @"^(\+)|\D", "$1");
            return result;
        }


        protected void gvListData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           

            if (e.Row.RowIndex > -1)
            {
                e.Row.Cells[0].Text = (gvListData.PageSize * gvListData.PageIndex + e.Row.RowIndex + 1).ToString();
                DataRow dr = ((DataTable)gvListData.DataSource).Rows[gvListData.PageSize * gvListData.PageIndex + e.Row.RowIndex];

                string msgText = "";
                Literal ltlMsgText = (Literal)e.Row.FindControl("ltlMsgText");



                if (ServiceType == "ts_location")
                {
                    msgText = "Thông tin hồ sơ:";
                    msgText += "\n";

                    if (msgText.Contains(dr["LOCATION"].ToString()) && dr["LOCATION"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += dr["LOCATION"].ToString();


                    }

                    if (msgText.Contains(dr["POLICYHOLDER_NAME"].ToString()) && dr["POLICYHOLDER_NAME"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += ", khách hàng " + dr["POLICYHOLDER_NAME"].ToString();

                    }

                    if (msgText.Contains(dr["BU_NAME"].ToString()) && dr["BU_NAME"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += ", BH tại " +dr["BU_NAME"].ToString();

                    }

                    if (msgText.Contains(dr["INCEPTION_DATE"].ToString()) && dr["INCEPTION_DATE"].ToString() == "")
                    {
                    }
                    else
                    {
                            msgText += ", từ ngày " + dr["INCEPTION_DATE"].ToString();
          
                    }

                    if (msgText.Contains(dr["EXPIRY_DATE"].ToString()) && dr["EXPIRY_DATE"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += " đến ngày " +dr["EXPIRY_DATE"].ToString();

                    }

                    if (msgText.Contains(dr["POLICY_URN"].ToString()) && dr["POLICY_URN"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += ". Số đơn " + dr["POLICY_URN"].ToString();

                    }

                    if (msgText.Contains(dr["COV_CLASS_NAME"].ToString()) && dr["COV_CLASS_NAME"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += ", tham gia BH " + dr["COV_CLASS_NAME"].ToString();

                    }
                    if (dr["SUMINSURED_AMT"].ToString().Trim() == "0" &&  dr["SUMINSURED_AMT"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += " (" + dr["SUMINSURED_AMT"].ToString() + ")";
          
                    }

                    if (dr["TINH_TRANG_THU_PHI"].ToString() != "" && dr["TINH_TRANG_THU_PHI"].ToString() == "Đã nộp phí")
                    {
                    }
                    else
                    {
                        msgText += ", " + dr["TINH_TRANG_THU_PHI"].ToString().ToLower();
          
                    }
                    if (msgText.Contains(dr["NGAY_NOP_PHI"].ToString()) && dr["NGAY_NOP_PHI"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += ", nộp phí ngày " +dr["NGAY_NOP_PHI"].ToString();

                    }

                    if (msgText.Contains(dr["SO_VU_BT"].ToString()) || dr["SO_VU_BT"].ToString() == "" || dr["SO_VU_BT"].ToString() == "0")
                    {
                    }
                    else
                    {

                        msgText += ". Số vụ tổn thất: " + dr["SO_VU_BT"].ToString() + " vụ,";
          
                    }

                    if (msgText.Contains(dr["TY_LE_BT"].ToString()) || dr["TY_LE_BT"].ToString() == "" || dr["TY_LE_BT"].ToString() == "0%")
                    {
                    }
                    else
                    {
                        msgText += " tỷ lệ BT: " +dr["TY_LE_BT"].ToString();

                    }

                    if (msgText.Contains(dr["TRADE_NAME"].ToString()) && dr["TRADE_NAME"].ToString() != "")
                    {
                    }
                    else
                    {
                        msgText += ". LH CBKT " +dr["TRADE_NAME"].ToString();

                    }

                    if (dr["TRADE_PHONE"].ToString() != "" && dr["TRADE_PHONE"].ToString().Length > 1)
                    {
                        string v_string_p = dr["TRADE_PHONE"].ToString();
                        string rst = ReformatPhone(v_string_p);

                        msgText += " (" + rst + ")";
                    }

                    msgText += ".";
                                                                                             
                }
                if (ServiceType == "ts_sodon")
                {

                    string locationVal = "";

                    string locationStr = "";

                    string locationStr1row = "";

                    int locationNo = 0;

                    int startCvr = 0;

                    string vTradeName = "";

                    string vNgayNopPhi = "";

                    string vTinhTrangThuPhi = "";

                    string vPhoneTrade = "";

                    string vSoVuBT = "";

                    string vTyLeBT = "";

                    string vINCEPTION_DATE = "";

                    string vEXPIRY_DATE = "";

                    string vPOLICYURN = "";

                    string vPolicyHolderName = "";

                    string vBU = "";


                                 
                    if (vPolicyHolderName != dr["POLICYHOLDER_NAME"].ToString() && dr["POLICYHOLDER_NAME"].ToString() != "")
                    {
                        vPolicyHolderName = ", khách hàng " + dr["POLICYHOLDER_NAME"].ToString();
                    }

                    if (vBU != dr["BU_NAME"].ToString() && dr["BU_NAME"].ToString() != "")
                    {
                        vBU =  ", BH tại " +dr["BU_NAME"].ToString();
                    }

                    if (vTradeName != dr["TRADE_NAME"].ToString() && dr["TRADE_NAME"].ToString() != "")
                    {
                        vTradeName =". LH CBKT " +dr["TRADE_NAME"].ToString();
                    }

                    if (vINCEPTION_DATE != dr["INCEPTION_DATE"].ToString() && dr["INCEPTION_DATE"].ToString() != "")
                    {
                        vINCEPTION_DATE =  ", từ ngày " +dr["INCEPTION_DATE"].ToString();

                    }
                    if (vEXPIRY_DATE != dr["INCEPTION_DATE"].ToString() && dr["EXPIRY_DATE"].ToString() != "")
                    {
                        vEXPIRY_DATE = " đến ngày " + dr["EXPIRY_DATE"].ToString();
                    }
                    if (vPOLICYURN != dr["POLICY_URN"].ToString() && dr["POLICY_URN"].ToString() != "")
                    {
                        vPOLICYURN = "Số đơn " + dr["POLICY_URN"].ToString();

                    }
                    if (vNgayNopPhi != dr["NGAY_NOP_PHI"].ToString() && dr["NGAY_NOP_PHI"].ToString() != "")
                    {
                        vNgayNopPhi =", nộp phí ngày " +dr["NGAY_NOP_PHI"].ToString();
                    }

                    if (dr["TINH_TRANG_THU_PHI"].ToString() == "" || dr["TINH_TRANG_THU_PHI"].ToString() == "Đã nộp phí")
                    {
                    }
                    else
                    {
                        vTinhTrangThuPhi = ", " + dr["TINH_TRANG_THU_PHI"].ToString().ToLower();
               
                    }

                    if (dr["TRADE_PHONE"].ToString() != "" && dr["TRADE_PHONE"].ToString().Length > 1)
                    {
                        string v_string_p = dr["TRADE_PHONE"].ToString();
                        string rst = ReformatPhone(v_string_p);

                        vPhoneTrade += " (" + rst + ")";
                    }

                    if (vSoVuBT != dr["SO_VU_BT"].ToString() && dr["SO_VU_BT"].ToString() != "" && dr["SO_VU_BT"].ToString() != "0")
                    {
                        vSoVuBT = ". Số vụ tổn thất: :" + dr["SO_VU_BT"].ToString() + " vụ,";
                
                    }

                    if (vTyLeBT != dr["TY_LE_BT"].ToString() && dr["TY_LE_BT"].ToString() != "" && dr["TY_LE_BT"].ToString() != "0%")
                    {
                        vTyLeBT =" tỷ lệ BT: " +dr["TY_LE_BT"].ToString();


                    }

                    if (locationVal != dr["LOCATION"].ToString() && dr["LOCATION"].ToString() != "")
                    {
                        if (locationVal != "")
                        {
                            msgText += locationStr;           
                        }

                        locationNo += 1;
                        startCvr = 1;

                        locationStr = "\r\n" + Convert.ToString(locationNo) + "." + dr["LOCATION"].ToString();


                        if (locationNo == 1) 
                        {
                            locationStr1row = "Số đơn " + dr["POLICY_URN"].ToString() + ", " + dr["LOCATION"].ToString() + ", khách hàng " + dr["POLICYHOLDER_NAME"].ToString() + ", BH tại " +dr["BU_NAME"].ToString();

                            locationStr1row += ", từ ngày " + dr["INCEPTION_DATE"].ToString() + " đến ngày " +dr["EXPIRY_DATE"].ToString();

                            locationStr1row += ", tham gia BH ";

                        }

                        locationVal = dr["LOCATION"].ToString();

                    }

                    if (startCvr == 0)
                    {
                        locationStr += ", ";

                        locationStr1row += ", ";
                
                    }
                    locationStr += " tham gia BH " + dr["COV_CLASS_NAME"].ToString();

                    locationStr1row += dr["COV_CLASS_NAME"].ToString();

                    if (dr["SUMINSURED_AMT"].ToString() != "" && dr["SUMINSURED_AMT"].ToString().Trim() !="0")
                    {
                        locationStr += " (" + dr["SUMINSURED_AMT"].ToString() + ")";
                        locationStr1row += " (" + dr["SUMINSURED_AMT"].ToString() + ")";

                    }
                    startCvr = 0;


                   if (locationNo == 1)
                   {
                        msgText = "Thông tin hồ sơ:" + "\n" + locationStr1row + vTinhTrangThuPhi + vNgayNopPhi + vSoVuBT + vTyLeBT + vTradeName + vPhoneTrade;
                   }
                   else
                   {
                        msgText = "Thông tin hồ sơ:" + "\n" + vPOLICYURN + vPolicyHolderName + vBU + vINCEPTION_DATE + vEXPIRY_DATE + vTinhTrangThuPhi + vNgayNopPhi + vSoVuBT + vTyLeBT + vTradeName + vPhoneTrade + " .Địa điểm bảo hiểm:" + msgText + locationStr;
                   }

                   msgText += ".";

                }

                if (ServiceType == "ts_tkh")
                {

                    msgText = "Thông tin hồ sơ:";

                    msgText += "\n";
                
                    if (msgText.Contains(dr["POLICYHOLDER_NAME"].ToString()) || dr["POLICYHOLDER_NAME"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += "Khách hàng " + dr["POLICYHOLDER_NAME"].ToString();

                    }
                    if (msgText.Contains(dr["LOCATION"].ToString()) || dr["LOCATION"].ToString()  == "")
                    {
                    }
                    else
                    {
                        msgText += ", " + dr["LOCATION"].ToString();

                    }

                    if (msgText.Contains(dr["BU_NAME"].ToString()) || dr["BU_NAME"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText +=", BH tại " +dr["BU_NAME"].ToString();
                    }

                    if (msgText.Contains(dr["INCEPTION_DATE"].ToString()) || dr["INCEPTION_DATE"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText +=", từ ngày " +dr["INCEPTION_DATE"].ToString();

                    }

                    if (msgText.Contains(dr["EXPIRY_DATE"].ToString()) || dr["EXPIRY_DATE"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += " đến ngày " +dr["EXPIRY_DATE"].ToString();

                    }

                    if (msgText.Contains(dr["POLICY_URN"].ToString()) || dr["POLICY_URN"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += ". Số đơn " + dr["POLICY_URN"].ToString();

                    }

                    if (msgText.Contains(dr["COV_CLASS_NAME"].ToString()) || dr["COV_CLASS_NAME"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += ", tham gia BH " + dr["COV_CLASS_NAME"].ToString();


                    }
                    if (dr["SUMINSURED_AMT"].ToString().Trim() == "0" || dr["SUMINSURED_AMT"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += " (" + dr["SUMINSURED_AMT"].ToString() + ")";
               
                    }

                    if (dr["TINH_TRANG_THU_PHI"].ToString() != "" || dr["TINH_TRANG_THU_PHI"].ToString() == "Đã nộp phí")
                    {
                    }
                    else
                    {
                        msgText += ", " + dr["TINH_TRANG_THU_PHI"].ToString().Trim();
               
                    }
                    if (msgText.Contains(dr["NGAY_NOP_PHI"].ToString()) || dr["NGAY_NOP_PHI"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += ", nộp phí ngày "+dr["NGAY_NOP_PHI"].ToString();


                    }

                    if (msgText.Contains(dr["SO_VU_BT"].ToString()) || dr["SO_VU_BT"].ToString() == "" || dr["SO_VU_BT"].ToString() == "0")
                    {
                    }
                    else
                    {
                        msgText += ". Số vụ tổn thất: " +dr["SO_VU_BT"].ToString() + " vụ,";
               
                    }

                    if (msgText.Contains(dr["TY_LE_BT"].ToString()) || dr["TY_LE_BT"].ToString() == "" || dr["TY_LE_BT"].ToString() == "0%")
                    {
                    }
                    else
                    {
                        msgText += " tỷ lệ BT: " +dr["TY_LE_BT"].ToString();

                    }

                    if (msgText.Contains(dr["TRADE_NAME"].ToString()) || dr["TRADE_NAME"].ToString() == "")
                    {
                    }
                    else
                    {
                        msgText += ". LH CBKT " +dr["TRADE_NAME"].ToString();

                    }

                    if (dr["TRADE_PHONE"].ToString() != "" && dr["TRADE_PHONE"].ToString().Length > 1)
                    {
                        string v_string_p = dr["TRADE_PHONE"].ToString();
                        string rst = ReformatPhone(v_string_p);

                        msgText += " (" + rst + ")";
                    }
                 
                    msgText += ".";                                                        
                }

                ltlMsgText.Text = msgText;
            

            }
        }

        
    }
}