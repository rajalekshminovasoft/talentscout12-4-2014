using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


public partial class MasterPage4 : System.Web.UI.MasterPage
{
    DBManagementClass clsClass = new DBManagementClass();
   AssesmentDataClassesDataContext dataclass = new AssesmentDataClassesDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (Session["UserID"] != null)
        {lbtnLogout.Visible = true; lbtnChangePassword.Visible = true;}

        if (Session["ControlToReDirect"] != null)
        {           
            ShowPopup();
        }
        else if (LastLoadedControl != null)
        {
            AddControl(LastLoadedControl);
        }
        //else AddControl("HomeControl.ascx");
        divmenu.Visible = false;
        if (Session["usertype"] != null)
            if (Session["usertype"].ToString() == "SuperAdmin")
                divmenu.Visible = true;
    }
    private void AddControl(string ControlPath)
    {
        try
        {
            System.Web.UI.Control Control_ToAdd;
            Control_ToAdd = LoadControl(ControlPath);
            Control_ToAdd.ID = "fja";
            cplhLoader.Controls.Clear();
            cplhLoader.Controls.Add(Control_ToAdd);
            LastLoadedControl = ControlPath;
        }
        catch (Exception ex) { lblmessage.Text = ex.Message;}//
    }
    private string LastLoadedControl
    {
        get
        {
            return Session["SubCtrl"] as string;
        }
        set
        {
            Session["SubCtrl"] = value;
        }
    }
    protected void lbtnHome_Click(object sender, EventArgs e)
    {
        ////AddControl("Homepage.ascx");
        //Session["SubCtrl"] = "Homepage.ascx"; Response.Redirect("FJAAdmin.aspx");
       // Response.Redirect("CareerJudge.htm");

        Session["ControlToReDirect"] = "Homepage.ascx";
        ShowPopup();
    }
    protected void lbtnInformationKit_Click(object sender, EventArgs e)
    {
        //AddControl("DownLoadFiles.ascx");
       // ReDirectToCareerJudge("InfoKit.ascx");

        Session["ControlToReDirect"] = "TakeTest.ascx";
        ShowPopup();
    }
    protected void lbtnTakeaTest_Click(object sender, EventArgs e)
    {
        //AddControl("TakeaTestOnline.ascx");
       // ReDirectToCareerJudge("TakeTest.ascx");

        Session["ControlToReDirect"] = "TakeTest.ascx";
        ShowPopup();
    }
    protected void lbtnProducts_Click(object sender, EventArgs e)
    {
        AddControl("Products.ascx");
    }
    protected void lbtnServices_Click(object sender, EventArgs e)
    {
        AddControl("Services.ascx");
    }
    protected void lbtnAboutUs_Click(object sender, EventArgs e)
    {
        //AddControl("AboutUS.ascx");
       // ReDirectToCareerJudge("AboutUS.ascx");

        Session["ControlToReDirect"] = "AboutUS.ascx";
        ShowPopup();
    }
    protected void lbtnContactUs_Click(object sender, EventArgs e)
    {
        // AddControl("ContactUsControl.ascx");
        // ReDirectToCareerJudge("ContactUsControl.ascx");

        Session["ControlToReDirect"] = "ContactUsControl.ascx";
        ShowPopup();
    }

    protected void mnuSideMenu_MenuItemClick(object sender, MenuEventArgs e)
    {
        MenuItem mnuitem = new MenuItem();
        mnuitem = e.Item;
        if (mnuitem.Target == ".") return;
        AddControl(mnuitem.Value);
    }
    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        ////////// only for local test

        ////Session.Clear();
        ////Session["SubCtrl"] = null;
        ////Session["MasterCtrl"] = "~/MasterPage5.master";
        ////Response.Redirect("FJAHome.aspx");
        
        //////ReDirectToCareerJudge("HomePage.ascx");

        //////////     

        
        Session["ControlToReDirect"] = "HomePage.ascx";
        ShowPopup();
    }
    protected void mnuAboutUs_MenuItemClick(object sender, MenuEventArgs e)
    {
        MenuItem mnuitem = new MenuItem();
        mnuitem = e.Item; if (mnuitem.Value == "AboutUs") return;
       // ReDirectToCareerJudge(mnuitem.Value);
        Session["ControlToReDirect"] = mnuitem.Value;
        ShowPopup();
    }
    protected void mnuProducts_MenuItemClick(object sender, MenuEventArgs e)
    {
        MenuItem mnuitem = new MenuItem();
        mnuitem = e.Item; if (mnuitem.Value == "Products") return;
        //ReDirectToCareerJudge(mnuitem.Value);
        Session["ControlToReDirect"] = mnuitem.Value;
        ShowPopup();
    }
    protected void mnuServices_MenuItemClick(object sender, MenuEventArgs e)
    {
        MenuItem mnuitem = new MenuItem();
        mnuitem = e.Item; if (mnuitem.Value == "Services") return;
        //ReDirectToCareerJudge(mnuitem.Value);
        Session["ControlToReDirect"] = mnuitem.Value;
        ShowPopup();
    }
    private void ReDirectToCareerJudge(string controlname)
    {
        //// bipson 12082010
        if (Session["dirLogin"] != null)
            if (Session["dirLogin"].ToString() == "Yes")
            {
                Session.Clear();
                Session["SubCtrl"] = null;
                Session["MasterCtrl"] = "~/MasterPage5.master";
                Response.Redirect("FJAHome.aspx");
                return;
            }

        Session.Clear();
        Response.Redirect("http://careerjudge.com/FJAHome.aspx?ctrlid=" + controlname);

    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (Session["ControlToReDirect"] != null)
        {
            ReDirectToCareerJudge(Session["ControlToReDirect"].ToString());
            Session["ControlToReDirect"] = null;
            
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["ControlToReDirect"] = null;
        if (LastLoadedControl != null)
        {
            AddControl(LastLoadedControl);
        }
    }
    private void ShowPopup()
    {
        pnlRedirect.Visible = true;
        cplhLoader.Controls.Clear();
        cplhLoader.Controls.Add(pnlRedirect);

    }
    protected void lbtnPrivacyPolicy_Click(object sender, EventArgs e)
    {
        Session["ControlToReDirect"] = "PrivacyPolicy.ascx";
        ShowPopup();
    }
    protected void lbtnTermsAndConditions_Click(object sender, EventArgs e)
    {
        Session["ControlToReDirect"] = "TermsAndConditions.ascx";
        ShowPopup();
    }
    protected void lbtnIPR_Click(object sender, EventArgs e)
    {
        Session["ControlToReDirect"] = "IPR.ascx";
        ShowPopup();
    }
    protected void lbtnChangePassword_Click(object sender, EventArgs e)
    {
        Session["pwdChanged"] = null;
        Session["SubCtrl"] = "ChangePasswordControl.ascx";
        Response.Redirect("FJAHome.aspx");
    }
}
