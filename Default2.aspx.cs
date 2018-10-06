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

namespace WebChatBot
{
    public partial class _Default2 : System.Web.UI.Page
    {

        public string Keyword
        {
            get { if (ViewState["Keyword"] != null) return ViewState["Keyword"].ToString(); else return ""; }
            set { ViewState["Keyword"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //lblWarningTableData.Text = "<i class='fa fa-warning'></i> Không có dữ liệu";     

            if (!IsPostBack)
            {


                string strID = Server.UrlDecode(Request.QueryString["id"]);
                Keyword = Server.UrlDecode(Request.QueryString["strkey"]);

                hiChatbotID.Value = strID;
                hiKeyword.Value = Server.UrlEncode(Keyword);

                //if (psid.Value != null && psid.Value != "" && psid.Value == strID)
                //{

                //    // txtSearch.Text = strKey;

                //    lblWarning.Visible = true;
                //    lblWarning.Text = "OK";


                //}
                //else
                //{
                //    lblWarning.Visible = true;

                //    lblWarning.Text = "<div class='alert text-danger'>"
                //                                 + "<strong>Thông báo!</strong> Bạn không được phép truy cập hệ thống. Bạn hãy liên hệ với Quản trị để được hướng dẫn!</div>";
                //}


                //lblMsg.Text = strID + Keyword;

                //if (hiChatbotID.Value == "" || psid.Value == "")

                if (strID == null && Keyword == "")
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "<div class='alert text-danger'>"
                                                     + "<strong>Thông báo!</strong> Bạn không được phép truy cập hệ thống. Bạn hãy liên hệ với Quản trị để được hướng dẫn!</div>";
                }



            }

        }

     
    }
}