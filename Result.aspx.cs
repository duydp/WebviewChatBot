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
using System.Net;


namespace WebChatBot
{
    public partial class Result : System.Web.UI.Page
    {
        public string Key
        {
            get { if (ViewState["Key"] != null) return ViewState["Key"].ToString(); else return ""; }
            set { ViewState["Key"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

              
                //string Key = "%Hải phòng%";
                string Key = Server.UrlDecode(Request.QueryString["searchkey"]);
                if (Key != "" && Key != null) LoadData(Key);

               //lblValue.Text = Key;

            }    
          
        }

        protected void LoadData(string Key)
        {

            //string productName = Request.Form["productName"];

            int PageNumber = int.Parse(ConfigurationManager.AppSettings["PageNumber"].ToString());
            int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

            string Authen_key = ConfigurationManager.AppSettings["Authen_key"].ToString();


            WSChatBot.CommonInfo wsINSUJ = new WSChatBot.CommonInfo();

            // Su dung WS insureJ theo dinh nghia 

            wsINSUJ.Url = ConfigurationSettings.AppSettings["INSUJ_SERVICE_URL"].ToString();

            XmlDocument Xml = new XmlDocument();

            Xml.LoadXml(wsINSUJ.GetPolicyFireInfo(Authen_key, "", "", "", "", Key, PageNumber, PageSize));


            //int RowCount = 0;
            //int PageCount = 0;

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


        protected void gvListData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }


        protected void btnViewDetail_Click(Object sender, CommandEventArgs e)
        {

            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "modal", "javascript:openModal();", true);
            string Script = "";
            Script += "\r\n<SCRIPT language='javascript'>";
            Script += "\r\n  window.onload = function(e) {";
            Script += "\r\n     openModal();";
            Script += "\r\n  }";
            Script += "\r\n</SCRIPT>";

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ModalScript", Script);

            int count = 0;
            string id = e.CommandArgument.ToString();

            DataTable dtResult = (DataTable)Session["dtSelect"];

            DataRow dr = dtResult.AsEnumerable().Where(x => x["TT"].ToString().Equals(id)).FirstOrDefault();

            ltlBuName.Text = dr["BU_NAME"].ToString();

            ltlPOLICYHOLDER_NAME.Text = dr["POLICYHOLDER_NAME"].ToString();

            ltlLOCATION.Text = dr["LOCATION"].ToString();

            ltlINCEPTION_DATE.Text = dr["INCEPTION_DATE"].ToString();
            ltlEXPIRY_DATE.Text = dr["EXPIRY_DATE"].ToString();
            ltPOLICY_URN.Text = dr["POLICY_URN"].ToString();
            ltlPRODUCT_NAME.Text = dr["PRODUCT_NAME"].ToString();
            ltlCOV_CLASS_NAME.Text = dr["COV_CLASS_NAME"].ToString();
            ltlSUMINSURED_AMT.Text = dr["SUMINSURED_AMT"].ToString();

            ltlNGAY_NOP_PHI.Text = dr["NGAY_NOP_PHI"].ToString();
            ltlSO_VU_BT.Text = dr["SO_VU_BT"].ToString();
            ltlTINH_TRANG_THU_PHI.Text = dr["TINH_TRANG_THU_PHI"].ToString();

            ltlTRADE_NAME.Text = dr["TRADE_NAME"].ToString();
            ltlTRADE_PHONE.Text = dr["TRADE_PHONE"].ToString();

            ltlTY_LE_BT.Text = dr["TY_LE_BT"].ToString();


            LoadData(Key);

        }
    }
}