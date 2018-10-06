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
    public partial class XCG : System.Web.UI.Page
    {
        private readonly object rowSets;

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

            if (ServiceType == "biensoxe")
            {
                Xml.LoadXml(wsINSUJ.GetCarInfo(Authen_key, Keyword, "", "", "", "", PageNumber, PageSize));

            }
            if (ServiceType == "sogcnbh")
            {
                Xml.LoadXml(wsINSUJ.GetCarInfo(Authen_key, "", "", Keyword, "", "", PageNumber, PageSize));

            }

            if (ServiceType == "sok")
            {
                //Xml.LoadXml(wsINSUJ.GetCarInfo(Authen_key, "", Keyword, "", "", "", PageNumber, PageSize));
                Xml.LoadXml(wsINSUJ.GetCarInfo(Authen_key, "", Keyword, "", "", "", PageNumber, PageSize));

               
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
                string msgText = "";
                e.Row.Cells[0].Text = (gvListData.PageSize * gvListData.PageIndex + e.Row.RowIndex + 1).ToString();
                DataRow dr = ((DataTable)gvListData.DataSource).Rows[gvListData.PageSize * gvListData.PageIndex + e.Row.RowIndex];
                Literal ltlMsgText = (Literal)e.Row.FindControl("ltlMsgText");


                if (ServiceType == "biensoxe")
                {
                    string check_NopPhi = dr["PREMIUM_PAYMENT_AMT"].ToString().Trim();
                    string check_TNDSBB = dr["TNDS_BAT_BUOC"].ToString().Trim();
                    string check_TNDSTN = dr["TNDS_TU_NGUYEN"].ToString().Trim();
                    string check_sotienVCX = dr["VAT_CHAT_XE"].ToString().Trim();
                    string check_LaiPhu = dr["LAI_PHU"].ToString().Trim();
                    string check_HangHoa = dr["HANG_HOA"].ToString().Trim();
                    string check_SoVuBT = dr["SO_VU_BT"].ToString().Trim();
                    string check_TyLeBT = dr["TY_LE_BT"].ToString().Trim();

                    if (dr["REG_NUMBER"].ToString() != "")
                    {
                        msgText += "\n" + "BKS: "+ dr["REG_NUMBER"].ToString();
                    }

                    msgText += ", chủ xe " + dr["POLICYHOLDER_NAME"].ToString();

                    if (dr["POLICYHOLDER_PHONE"].ToString() != "" && dr["POLICYHOLDER_PHONE"].ToString().Length > 1)
                    {
                        string v_string = dr["POLICYHOLDER_PHONE"].ToString();
                        string rst_hP = ReformatPhone(v_string);
                        msgText += " (" + rst_hP + ")";
                    }

                    msgText += ", BH tại " + dr["BU_NAME"].ToString();
                    msgText += "-" + dr["DEPT_NAME"].ToString();
                    msgText += ", từ " + dr["INCEPTION_DATE"].ToString();
                    msgText += " đến " + dr["EXPIRY_DATE"].ToString();
                    msgText += ". Số đơn " + dr["POLICY_URN"].ToString();

                    if (check_TNDSBB.Substring(0, 3) != "0tr")
                    {
                        msgText += ", TNDSBB " + "(" + dr["TNDS_BAT_BUOC"].ToString() + ")";
                    }

                    if (check_TNDSTN.Substring(0, 3) != "0tr")
                    {
                        msgText += ", TNDSTN " + "(" + dr["TNDS_TU_NGUYEN"].ToString() + ")";
                    }

                    if (check_sotienVCX.Substring(0, 3) != "0tr")
                    {
                        msgText += ", VCXE " + "(" + dr["VAT_CHAT_XE"].ToString() + ")";
                    }

                    if (check_LaiPhu.Substring(0, 3) != "0tr")
                    {
                        msgText += ", NTX " + "(" + dr["LAI_PHU"].ToString() + ")";
                    }

                    if (check_HangHoa.Substring(0, 3) != "0tr")
                    {
                        msgText += ", TNDS HH " + "(" + dr["HANG_HOA"].ToString() + ")";
                    }

                    if (check_NopPhi != "0")
                    {
                        msgText += ", nộp phí ngày " + dr["NGAY_NOP_PHI"].ToString();
                    }

                    if (dr["TINH_TRANG_THU_PHI"].ToString() != "")
                    {
                        if (dr["TINH_TRANG_THU_PHI"].ToString() != "Đã nộp phí")
                        {
                            msgText += ", " + dr["TINH_TRANG_THU_PHI"].ToString().ToLower();
                        }

                        if (check_SoVuBT != "0")
                        {
                            msgText += ". Số vụ tổn thất: " + dr["SO_VU_BT"].ToString() + " vụ,";
                        }

                        if ((check_TyLeBT != "0%") && (check_TyLeBT != "%"))
                        {
                            msgText += " tỷ lệ BT: " + dr["TY_LE_BT"].ToString();
                        }

                        if (dr["TRADE_NAME"].ToString() != "")
                        {
                            msgText += ". LH CBKT " + dr["TRADE_NAME"].ToString();
                        }

                        if (dr["TRADE_PHONE"].ToString() != "" && dr["TRADE_PHONE"].ToString().Length > 1)
                        {
                            string v_string_p = dr["TRADE_PHONE"].ToString();
                            string rst = ReformatPhone(v_string_p);
                            msgText += " (" + rst + ")";
                        }
                    }
                    msgText += ".";

                }
                if (ServiceType == "sogcnbh")
                {
                    
                    string check_NopPhi = dr["PREMIUM_PAYMENT_AMT"].ToString().Trim();
                    string check_CertNum = dr["CERT_NUMBER"].ToString().Trim();
                    string check_TNDSBB = dr["TNDS_BAT_BUOC"].ToString().Trim();
                    string check_TNDSTN = dr["TNDS_TU_NGUYEN"].ToString().Trim();
                    string check_sotienVCX = dr["VAT_CHAT_XE"].ToString().Trim();
                    string check_LaiPhu = dr["LAI_PHU"].ToString().Trim();
                    string check_HangHoa = dr["HANG_HOA"].ToString().Trim();
                    string check_SoVuBT = dr["SO_VU_BT"].ToString().Trim();
                    string check_TyLeBT = dr["TY_LE_BT"].ToString().Trim();


                    if (dr["REG_NUMBER"].ToString() != "")
                    {
                        msgText += "\n" + "BKS: "+ dr["REG_NUMBER"].ToString();
                    }

                    msgText += ", chủ xe " + dr["POLICYHOLDER_NAME"].ToString();

                    if (dr["POLICYHOLDER_PHONE"].ToString() != "" && dr["POLICYHOLDER_PHONE"].ToString().Length > 1)
                    {
                        string v_string = dr["POLICYHOLDER_PHONE"].ToString();
                        string rst_hP = ReformatPhone(v_string);
                        msgText += " (" + rst_hP + ")";
                    }


                    msgText += ", BH tại " + dr["BU_NAME"].ToString();
                    msgText += "-" + dr["DEPT_NAME"].ToString();
                    msgText += ", từ " + dr["INCEPTION_DATE"].ToString();
                    msgText += " đến " + dr["EXPIRY_DATE"].ToString();
                    msgText += ". Số đơn " + dr["POLICY_URN"].ToString();

                    if (check_CertNum != "")
                    {
                        if (check_CertNum != "0")
                          {
                             msgText +=", GCN " + dr["CERT_NUMBER"].ToString(); 
                          }
                    }

                    if (check_TNDSBB.Substring(0, 3) != "0tr")
                    {
                        msgText += ", TNDSBB " + "(" + dr["TNDS_BAT_BUOC"].ToString() + ")";
                    }

                    if (check_TNDSTN.Substring(0, 3) != "0tr")
                    {
                        msgText += ", TNDSTN " + "(" + dr["TNDS_TU_NGUYEN"].ToString() + ")";
                    }

                    if (check_sotienVCX.Substring(0, 3) != "0tr")
                    {
                        msgText += ", VCXE " + "(" + dr["VAT_CHAT_XE"].ToString() + ")";
                    }

                    if (check_LaiPhu.Substring(0, 3) != "0tr")
                    {
                        msgText += ", NTX " + "(" + dr["LAI_PHU"].ToString() + ")";
                    }

                    if (check_HangHoa.Substring(0, 3) != "0tr")
                    {
                        msgText += ", TNDS HH " + "(" + dr["HANG_HOA"].ToString() + ")";
                    }

                    if (check_NopPhi != "0")
                    {
                        msgText += ", nộp phí ngày " + dr["NGAY_NOP_PHI"].ToString();
                    }

                    if (dr["TINH_TRANG_THU_PHI"].ToString() != "")
                    {
                       if (dr["TINH_TRANG_THU_PHI"].ToString() != "Đã nộp phí")
                       {
                         msgText +=", " +dr["TINH_TRANG_THU_PHI"].ToString().ToLower();
                       }
                    }

                    if (check_SoVuBT != "0")
                    {
                        msgText += ". Số vụ tổn thất: " + dr["SO_VU_BT"].ToString() + " vụ,";
                    }

                    if ((check_TyLeBT != "0%") && (check_TyLeBT != "%"))
                    {
                        msgText += " tỷ lệ BT: " + dr["TY_LE_BT"].ToString();
                    }

                    if (dr["TRADE_NAME"].ToString() != "")
                    {
                        msgText += ". LH CBKT " + dr["TRADE_NAME"].ToString();
                    }

                    if (dr["TRADE_PHONE"].ToString() != "" && dr["TRADE_PHONE"].ToString().Length > 1)
                    {
                        string v_string_p = dr["TRADE_PHONE"].ToString();
                        string rst = ReformatPhone(v_string_p);
                        msgText += " (" + rst + ")";
                    }

                    msgText += ".";
                }

                if (ServiceType == "sok")
                {

                    string check_NopPhi = dr["PREMIUM_PAYMENT_AMT"].ToString().Trim();
                   // string check_CertNum = dr["CERT_NUMBER"].ToString().Trim();
                    string check_TNDSBB = dr["TNDS_BAT_BUOC"].ToString().Trim();
                    string check_TNDSTN = dr["TNDS_TU_NGUYEN"].ToString().Trim();
                    string check_sotienVCX = dr["VAT_CHAT_XE"].ToString().Trim();
                    string check_LaiPhu = dr["LAI_PHU"].ToString().Trim();
                    string check_HangHoa = dr["HANG_HOA"].ToString().Trim();
                    string check_SoVuBT = dr["SO_VU_BT"].ToString().Trim();
                    string check_TyLeBT = dr["TY_LE_BT"].ToString().Trim();

                    if (dr["CHASSIS_NO"].ToString() != "")
                    {
                        msgText += "\n" + "BKS/SK: "+ dr["REG_NUMBER"].ToString() + "/" + dr["CHASSIS_NO"].ToString();

                    }

                    msgText += ", chủ xe " + dr["POLICYHOLDER_NAME"].ToString();


                    if (dr["POLICYHOLDER_PHONE"].ToString() != "" && dr["POLICYHOLDER_PHONE"].ToString().Length > 1)
                    {
                        string v_string = dr["POLICYHOLDER_PHONE"].ToString();
                        string rst_hP = ReformatPhone(v_string);
                        msgText += " (" + rst_hP + ")";
                    }

                    msgText += ", BH tại " + dr["BU_NAME"].ToString();
                    msgText += "-" + dr["DEPT_NAME"].ToString();

                    msgText +=", từ " + dr["INCEPTION_DATE"].ToString();

                    msgText +=" đến " +dr["EXPIRY_DATE"].ToString();

                    msgText += ". Số đơn " + dr["POLICY_URN"].ToString();

                    if (check_TNDSBB.Substring(0, 3) != "0tr")
                    {
                        msgText += ", TNDSBB " + "(" + dr["TNDS_BAT_BUOC"].ToString() + ")";
                    }

                    if (check_TNDSTN.Substring(0, 3) != "0tr")
                    {
                        msgText += ", TNDSTN " + "(" + dr["TNDS_TU_NGUYEN"].ToString() + ")";
                    }

                    if (check_sotienVCX.Substring(0, 3) != "0tr")
                    {
                        msgText += ", VCXE " + "(" + dr["VAT_CHAT_XE"].ToString() + ")";
                    }

                    if (check_LaiPhu.Substring(0, 3) != "0tr")
                    {
                        msgText += ", NTX " + "(" + dr["LAI_PHU"].ToString() + ")";
                    }

                    if (check_HangHoa.Substring(0, 3) != "0tr")
                    {
                        msgText += ", TNDS HH " + "(" + dr["HANG_HOA"].ToString() + ")";
                    }

                    if (check_NopPhi != "0")
                    {
                        msgText += ", nộp phí ngày " + dr["NGAY_NOP_PHI"].ToString();
                    }


                    if (dr["TINH_TRANG_THU_PHI"].ToString() != "")
                    {
                        if (dr["TINH_TRANG_THU_PHI"].ToString() != "Đã nộp phí")
                        {
                            msgText += ", " + dr["TINH_TRANG_THU_PHI"].ToString().ToLower();
                        }
                    }

                    if (check_SoVuBT != "0")
                    {
                        msgText += ". Số vụ tổn thất: " + dr["SO_VU_BT"].ToString() + " vụ,";
                    }

                    if ((check_TyLeBT != "0%") && (check_TyLeBT != "%"))
                    {
                        msgText += " tỷ lệ BT: " + dr["TY_LE_BT"].ToString();
                    }

                    if (dr["TRADE_NAME"].ToString() != "")
                    {
                        msgText += ". LH CBKT " + dr["TRADE_NAME"].ToString();
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