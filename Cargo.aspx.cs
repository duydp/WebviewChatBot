using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Xml;
using System.IO;
namespace WebChatBot
{
    public partial class CargoCustomerName : System.Web.UI.Page
    {
        //Get & Set Property for KeywordSearch
        public string KeywordSearch
        {
            get { if (ViewState["KeywordSearch"] != null) return ViewState["KeywordSearch"].ToString(); else return ""; }
            set { ViewState["KeywordSearch"] = value; }
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
               // KeywordSearch = Server.UrlDecode(Request.QueryString["strkey"]);
                KeywordSearch = Request.QueryString["strkey"];
                ServiceType = Server.UrlDecode(Request.QueryString["svtype"]);
                hiChatbotID.Value = strID;
                if (strID == null && KeywordSearch == "" && ServiceType == "")
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "<div class='alert text-danger'>"
                                                     + "<strong>Thông báo!</strong> Bạn không được phép truy cập hệ thống. Bạn hãy liên hệ với Quản trị để được hướng dẫn!</div>";

                }
                else
                {
                    if (KeywordSearch != "") LoadData(KeywordSearch);

                }
            }
        }

        protected void LoadData(string KeywordSearch)
        {
            int PageNumber = int.Parse(ConfigurationManager.AppSettings["PageNumber"].ToString());
            int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
            string Authen_key = ConfigurationManager.AppSettings["Authen_key"].ToString();
            WSChatBot.CommonInfo wsINSUJ = new WSChatBot.CommonInfo();
            wsINSUJ.Url = ConfigurationSettings.AppSettings["INSUJ_SERVICE_URL"].ToString();
            wsINSUJ.Timeout = 1000000;
            XmlDocument Xml = new XmlDocument();
            if (ServiceType == "cargo_kh")
            {
                Xml.LoadXml(wsINSUJ.GetPolicyCargoInfo(Authen_key,"", KeywordSearch, "", "", "", "", PageNumber, PageSize));
            }

            if (ServiceType == "cargo_ten")
            {
                Xml.LoadXml(wsINSUJ.GetPolicyCargoInfo(Authen_key, "", "", "", "", KeywordSearch, "", PageNumber, PageSize));
            }

            if (ServiceType == "cargo_sodon")
            {
                Xml.LoadXml(wsINSUJ.GetPolicyCargoInfo(Authen_key, KeywordSearch, "", "", "", "", "", PageNumber, PageSize));
            }

            DataSet ds = new DataSet();
            ds.ReadXml(new XmlTextReader(new StringReader(Xml.DocumentElement.OuterXml)));
            DataTable dt = ds.Tables[0];
            if (dt.Columns.Count == 1 && dt.Rows[0]["DATA"].ToString() == "NO DATA FOUND")
            {

                lblWarningTableData.Text = "<div class='alert text-danger'>"
                                                 + "<strong>Thông báo!</strong> Không tìm thấy dữ liệu</div>";

                lblWarningTableData.Visible = true;
            }
            else
            {
                lblWarningTableData.Visible = false;
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
                string msgText = "";
                e.Row.Cells[0].Text = (gvListData.PageSize * gvListData.PageIndex + e.Row.RowIndex + 1).ToString();
                DataRow dr = ((DataTable)gvListData.DataSource).Rows[gvListData.PageSize * gvListData.PageIndex + e.Row.RowIndex];
                Literal ltlMsgText = (Literal)e.Row.FindControl("ltlMsgText");

                if (ServiceType == "cargo_kh")
                {
                    string check_SUMINSURED_AMT = dr["SUMINSURED_AMT"].ToString().Trim();
                    string check_NopPhi = dr["PREMIUM_PAYMENT_AMT"].ToString().Trim();
                    string check_NgayDongPhi = dr["NGAY_NOP_PHI"].ToString().Trim();

                    if (dr["POLICYHOLDER_NAME"].ToString() != "")
                    {
                        msgText += "\n" + "Khách hàng " + dr["POLICYHOLDER_NAME"].ToString();
                    }
                    if (dr["VESSELORCONVEYANCE"].ToString() != "")
                    {
                        msgText +=", Tàu " + dr["VESSELORCONVEYANCE"].ToString();
                    }
                    if (dr["BU_NAME"].ToString() != "")
                    {
                        msgText +=", BH tại " + dr["BU_NAME"].ToString();
                    }
                    if (dr["INCEPTION_DATE"].ToString() != "")
                    {
                        msgText += ", từ " + dr["INCEPTION_DATE"].ToString();
                    }
                    if (dr["EXPIRY_DATE"].ToString() != "")
                    {
                        msgText += ", đến " + dr["EXPIRY_DATE"].ToString();
                    }
                    if (dr["POLICY_URN"].ToString() != "")
                    {
                        msgText += ". Số đơn " + dr["POLICY_URN"].ToString();
                    }
                    if (dr["CERTIFICATENUMBER"].ToString() != "")
                    {
                        msgText += ", GCN " + dr["CERTIFICATENUMBER"].ToString();
                    }
                    if (dr["PRODUCT_NAME"].ToString() != "")
                    {
                        msgText += ", tham gia " + dr["PRODUCT_NAME"].ToString();
                    }

                    if (check_SUMINSURED_AMT != "0")
                    {
                        msgText += " (" + dr["SUMINSURED_AMT"].ToString() + ")";
                    }

                    if (dr["PLACEOFDEPARTURES"].ToString() != "")
                    {
                        msgText += ", đi từ " + dr["PLACEOFDEPARTURES"].ToString();
                    }
                    if (dr["FINALDESTINATIONS"].ToString() != "")
                    {
                        msgText += " đến " + dr["FINALDESTINATIONS"].ToString();
                    }

                    if (check_NopPhi != "0" && check_NgayDongPhi!="")
                    {
                        msgText += " , nộp phí ngày" + dr["NGAY_NOP_PHI"].ToString();
                    }

                    if (dr["TINH_TRANG_THU_PHI"].ToString() != "")
                    {
                        if (dr["TINH_TRANG_THU_PHI"].ToString() != "Đã nộp phí")
                        {
                            msgText += " " + dr["TINH_TRANG_THU_PHI"].ToString();
                        }
                           
                    }

                    if (dr["TRADE_NAME"].ToString() != "")
                    {
                        msgText += ". LH CBKT " + dr["TRADE_NAME"].ToString();
                    }
                    if (dr["TRADE_PHONE"].ToString() != "" && dr["TRADE_PHONE"].ToString().Length > 1)
                    {
                        string v_string = dr["TRADE_PHONE"].ToString();
                        string rst_hP = ReformatPhone(v_string);
                        msgText += " (" + rst_hP + ")";
                    }
                    
                    msgText += ".";

                }
                if (ServiceType == "cargo_ten")
                {

                    string check_SUMINSURED_AMT = dr["SUMINSURED_AMT"].ToString().Trim();
                    string check_NopPhi = dr["PREMIUM_PAYMENT_AMT"].ToString().Trim();
                    string check_NgayDongPhi = dr["NGAY_NOP_PHI"].ToString().Trim();
                    if (dr["VESSELORCONVEYANCE"].ToString() != "")
                    {
                        msgText += "Tàu " + dr["VESSELORCONVEYANCE"].ToString();
                    }
                    if (dr["POLICYHOLDER_NAME"].ToString() != "")
                    {
                        msgText += "\n" + ", khách hàng " + dr["POLICYHOLDER_NAME"].ToString();
                    }
                    
                    if (dr["BU_NAME"].ToString() != "")
                    {
                        msgText += ", BH tại " + dr["BU_NAME"].ToString();
                    }
                    if (dr["INCEPTION_DATE"].ToString() != "")
                    {
                        msgText += ", từ " + dr["INCEPTION_DATE"].ToString();
                    }
                    if (dr["EXPIRY_DATE"].ToString() != "")
                    {
                        msgText += ", đến " + dr["EXPIRY_DATE"].ToString();
                    }
                    if (dr["POLICY_URN"].ToString() != "")
                    {
                        msgText += ". Số đơn " + dr["POLICY_URN"].ToString();
                    }
                    if (dr["CERTIFICATENUMBER"].ToString() != "")
                    {
                        msgText += ", GCN " + dr["CERTIFICATENUMBER"].ToString();
                    }
                    if (dr["PRODUCT_NAME"].ToString() != "")
                    {
                        msgText += ", tham gia " + dr["PRODUCT_NAME"].ToString();
                    }

                    if (check_SUMINSURED_AMT != "0")
                    {
                        msgText += " (" + dr["SUMINSURED_AMT"].ToString() + ")";
                    }

                    if (dr["PLACEOFDEPARTURES"].ToString() != "")
                    {
                        msgText += ", đi từ " + dr["PLACEOFDEPARTURES"].ToString();
                    }
                    if (dr["FINALDESTINATIONS"].ToString() != "")
                    {
                        msgText += " đến " + dr["FINALDESTINATIONS"].ToString();
                    }

                    if (check_NopPhi != "0" && check_NgayDongPhi != "")
                    {
                        msgText += " , nộp phí ngày" + dr["NGAY_NOP_PHI"].ToString();
                    }

                    if (dr["TINH_TRANG_THU_PHI"].ToString() != "")
                    {
                        if (dr["TINH_TRANG_THU_PHI"].ToString() != "Đã nộp phí")
                        {
                            msgText += " " + dr["TINH_TRANG_THU_PHI"].ToString();
                        }

                    }

                    if (dr["TRADE_NAME"].ToString() != "")
                    {
                        msgText += ". LH CBKT " + dr["TRADE_NAME"].ToString();
                    }
                    if (dr["TRADE_PHONE"].ToString() != "" && dr["TRADE_PHONE"].ToString().Length > 1)
                    {
                        string v_string = dr["TRADE_PHONE"].ToString();
                        string rst_hP = ReformatPhone(v_string);
                        msgText += " (" + rst_hP + ")";
                    }

                    msgText += ".";
                }

                if (ServiceType == "cargo_sodon")
                {

                    string check_SUMINSURED_AMT = dr["SUMINSURED_AMT"].ToString().Trim();
                    string check_NopPhi = dr["PREMIUM_PAYMENT_AMT"].ToString().Trim();
                    string check_NgayDongPhi = dr["NGAY_NOP_PHI"].ToString().Trim();
                    if (dr["POLICY_URN"].ToString() != "")
                    {
                        msgText += "Số đơn " + dr["POLICY_URN"].ToString();
                    }
                    if (dr["POLICYHOLDER_NAME"].ToString() != "")
                    {
                        msgText += "\n" + "Khách hàng " + dr["POLICYHOLDER_NAME"].ToString();
                    }
                    
                    if (dr["BU_NAME"].ToString() != "")
                    {
                        msgText += ", BH tại " + dr["BU_NAME"].ToString();
                    }
                    if (dr["INCEPTION_DATE"].ToString() != "")
                    {
                        msgText += ", từ " + dr["INCEPTION_DATE"].ToString();
                    }
                    if (dr["EXPIRY_DATE"].ToString() != "")
                    {
                        msgText += ", đến " + dr["EXPIRY_DATE"].ToString();
                    }
                    if (dr["VESSELORCONVEYANCE"].ToString() != "")
                    {
                        msgText += ". Tàu " + dr["VESSELORCONVEYANCE"].ToString();
                    }
                    if (dr["CERTIFICATENUMBER"].ToString() != "")
                    {
                        msgText += ", GCN " + dr["CERTIFICATENUMBER"].ToString();
                    }
                    if (dr["PRODUCT_NAME"].ToString() != "")
                    {
                        msgText += ", tham gia " + dr["PRODUCT_NAME"].ToString();
                    }

                    if (check_SUMINSURED_AMT != "0")
                    {
                        msgText += " (" + dr["SUMINSURED_AMT"].ToString() + ")";
                    }

                    if (dr["PLACEOFDEPARTURES"].ToString() != "")
                    {
                        msgText += ", đi từ " + dr["PLACEOFDEPARTURES"].ToString();
                    }
                    if (dr["FINALDESTINATIONS"].ToString() != "")
                    {
                        msgText += " đến " + dr["FINALDESTINATIONS"].ToString();
                    }

                    if (check_NopPhi != "0" && check_NgayDongPhi != "")
                    {
                        msgText += " , nộp phí ngày" + dr["NGAY_NOP_PHI"].ToString();
                    }

                    if (dr["TINH_TRANG_THU_PHI"].ToString() != "")
                    {
                        if (dr["TINH_TRANG_THU_PHI"].ToString() != "Đã nộp phí")
                        {
                            msgText += " " + dr["TINH_TRANG_THU_PHI"].ToString();
                        }

                    }

                    if (dr["TRADE_NAME"].ToString() != "")
                    {
                        msgText += ". LH CBKT " + dr["TRADE_NAME"].ToString();
                    }
                    if (dr["TRADE_PHONE"].ToString() != "" && dr["TRADE_PHONE"].ToString().Length > 1)
                    {
                        string v_string = dr["TRADE_PHONE"].ToString();
                        string rst_hP = ReformatPhone(v_string);
                        msgText += " (" + rst_hP + ")";
                    }

                    msgText += ".";
                }


                ltlMsgText.Text = msgText;
            }
        }
    }
}