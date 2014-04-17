﻿using System;
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

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.ComponentModel;
public partial class RptPrntGrp : System.Web.UI.Page
{
    DBManagementClass clsClasses = new DBManagementClass();
    AssesmentDataClassesDataContext dataclass = new AssesmentDataClassesDataContext();
    DataTable dt = new DataTable();
    int userid = 0; int testid = 0;
    DataTable dtEmptySessionList = new DataTable();
    string scoringType; string scoretype;
    int totalQuestionMark = 0;

    DataTable dtTestSection;
    Rectangle objRectangle_3 = new Rectangle(250, 200, 50, 25);

    // DataTable dtTestSection = new DataTable();
    DataTable dtvariablevalues = new DataTable();
    // string scoretype; int userid = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        testid = int.Parse(Session["UserTestID_Report"].ToString());        

        bool valexists = false;
        if (Session["TestName"] != null)
        {
            lblTestName.Text = Session["TestName"].ToString();
            if (Session["ReportTypeName"] != null)
            { lblReportType.Text = Session["ReportTypeName"].ToString(); valexists = true; }
        }
        if (valexists == false)
        {
            var GetTestName = from testnamedet in dataclass.TestLists
                              where testnamedet.TestId == testid
                              select testnamedet;
            if (GetTestName.Count() > 0)
            {
                if (GetTestName.First().TestName != null)
                {
                    lblTestName.Text = GetTestName.First().TestName.ToString();
                    Session["TestName"] = GetTestName.First().TestName.ToString();
                }
                if (GetTestName.First().ReportType != null)
                {
                    lblReportType.Text = GetTestName.First().ReportType.ToString();
                    Session["ReportTypeName"] = GetTestName.First().ReportType.ToString();
                }
            }
        }
        
        if (Session["ScoringType"] != null)
            scoretype = Session["ScoringType"].ToString();
        
        if (Session["testsectionreportvalues"] != null)
            dtTestSection = (DataTable)Session["testsectionreportvalues"];

        FillReportDescriptionDetails();

        if (Session["variablereportvalues"] != null)
        {
            dtvariablevalues = (DataTable)Session["variablereportvalues"];
            GridView1.DataSource = dtvariablevalues;
            GridView1.DataBind();
        }
        DisplayGroupReportColorGraph();
        this.Dispose();
    }
    private void FillReportDescriptionDetails()
    {
        lblReportDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");//.ToShortDateString();

        var SummaryDetails1 = from SummaryDetails in dataclass.ReportDescriptions
                              where SummaryDetails.TestId == testid
                              select SummaryDetails;
        if (SummaryDetails1.Count() > 0)
        {
            if (SummaryDetails1.First().Summary1 != null)
                lblSummary1.Text = SummaryDetails1.First().Summary1.ToString();
            if (SummaryDetails1.First().Summary2 != null)
                lblSummary2.Text = SummaryDetails1.First().Summary2.ToString();
            if (SummaryDetails1.First().DescriptiveReport != null)
                tcellDescriptionReport.InnerHtml = SummaryDetails1.First().DescriptiveReport.ToString();
            if (SummaryDetails1.First().Conclusion != null)
                lblConclusion.Text = SummaryDetails1.First().Conclusion.ToString();
            if (SummaryDetails1.First().ScoringType != null)
                scoringType = SummaryDetails1.First().ScoringType.ToString();
        }
    }

    private void DisplayGroupReportColorGraph()
    {
        Table tblDisplay = new Table();
        TableCell tblCell = new TableCell();
        TableRow tblRow;
        Label label;
        int i = 0;
        int rowid = 0;
        int totalmarks = 0;
        int sectionid = 0;
        // code to draw section(variable)wise bargraph 

        //tblDisplay.Width = 650;
        tblDisplay.CellPadding = 0;
        tblDisplay.CellSpacing = 0;
        tblDisplay.BorderWidth = 0;
        string mark = "0"; string benchmark = "";

        // lblMessage.Text += " bargrphParts= " + GridView1.Rows.Count.ToString() + " bargrphValues= ";

        //
        float totalmark_variablewise = 0;
        int GRADE = 0;
        int gradecount = 0;

        //GridView1.Sort("USERID", SortDirection.Ascending);// BIP 19-01-2011
        //GridView1.DataBind();


        //code to get all section(variable) list
        //sectionNAME=GridView1.Rows[j].Cells[1].Text
        DataTable dtSectionList = new DataTable();
        dtSectionList.Columns.Add("sectionid");
        dtSectionList.Columns.Add("sectionname");
        DataRow drSectionList;
        string secname = "", secid = "";
        for (int secIndex = 0; secIndex < GridView1.Rows.Count; secIndex++)
        {
            secid = GridView1.Rows[secIndex].Cells[0].Text;
            secname = GridView1.Rows[secIndex].Cells[1].Text;

            bool secexists = false;
            for (int secIndex1 = 0; secIndex1 < dtSectionList.Rows.Count; secIndex1++)
            {
                if (dtSectionList.Rows[secIndex1]["sectionname"].ToString() == secname)
                { secexists = true; break; }
            }
            if (secexists == false)
            {
                drSectionList = dtSectionList.NewRow();
                drSectionList["sectionid"] = secid;
                drSectionList["sectionname"] = secname;
                dtSectionList.Rows.Add(drSectionList);
            }
        }

        //code add first three headers(USERID,NAME.TESTDATE)

        tblRow = new TableRow();
        tblCell = new TableCell();
        label = new Label();
        label.Font.Size = 12;
        label.Font.Bold = true;
        label.Text = "USER ID";
        //label.Width = 30;
        tblCell.Controls.Add(label);
        tblCell.Style.Value = "border: 1px ridge #000000;text-align: center; vertical-align: middle;";
        //tblCell.Text = "USER ID";
        tblRow.Cells.Add(tblCell);
        tblCell = new TableCell();
        label = new Label();
        label.Font.Size = 12;
        label.Font.Bold = true;
        label.Text = "NAME";
        //label.Width = 30;
        tblCell.Controls.Add(label);
        tblCell.Style.Value = "border: 1px ridge #000000;text-align: center; vertical-align: middle";
        tblRow.Cells.Add(tblCell);
        tblCell = new TableCell();
        label = new Label();
        label.Font.Size = 12;
        label.Font.Bold = true;
        label.Text = "TEST DATE";
        //label.Width = 30;
        tblCell.Controls.Add(label);
        tblCell.Style.Value = "border: 1px ridge #000000;text-align: center; vertical-align: middle;";//padding:10
        tblRow.Cells.Add(tblCell);

        //code to design table header(title and variable names on the top of the table cell)
        string variableInstructions = "";
        for (int titleIndex = 0; titleIndex < dtSectionList.Rows.Count; titleIndex++)
        {
            tblCell = new TableCell();

            label = new Label();
            label.Font.Size = 12;
            label.Font.Bold = true;
            label.Text = "V" + (titleIndex + 1).ToString();
            if (variableInstructions != "")
                variableInstructions += "; ";
            variableInstructions += "V" + (titleIndex + 1).ToString() + ": " + dtSectionList.Rows[titleIndex]["sectionname"].ToString();
            //label.Text = dtSectionList.Rows[titleIndex]["sectionname"].ToString();
            //label.Width = 50;
            tblCell.Controls.Add(label);
            tblCell.Style.Value = "border: 1px ridge #000000;text-align: center; vertical-align: middle;width:40px;";//padding:10px
            //tblCell.Text = dtSectionList.Rows[titleIndex]["sectionname"].ToString();
            tblRow.Cells.Add(tblCell);
        }
        label = new Label();
        label.Text = variableInstructions;
        tcellExplanations.Controls.Add(label);
        tblDisplay.Rows.Add(tblRow);// bip 13-02-2011
        //

        string curUSERNAME = "";
        string curNAME = "";
        string curUSERID = "0";
        string testDATE = "";
        string addquery = " and UserProfile.TestId=" + testid;
        if (Session["AdminGroupID"] != null)
        {
            int grpid = int.Parse(Session["AdminGroupID"].ToString());
            addquery += " and GrpUserID=" + grpid;
        }
        string querystring = "SELECT DISTINCT EvaluationResult.UserId, UserProfile.UserName, UserProfile.FirstName, UserProfile.MiddleName, UserProfile.LastName,UserProfile.FirstLoginDate " +
                                    " FROM EvaluationResult INNER JOIN UserProfile ON EvaluationResult.UserId = UserProfile.UserId " + addquery + " ORDER BY UserProfile.UserName";

        DataSet dsEvaluationdetails = new DataSet();
        dsEvaluationdetails = clsClasses.GetValuesFromDB(querystring);
        if (dsEvaluationdetails != null)
            if (dsEvaluationdetails.Tables.Count > 0)
                if (dsEvaluationdetails.Tables[0].Rows.Count > 0)
                    for (int uindex = 0; uindex < dsEvaluationdetails.Tables[0].Rows.Count; uindex++)
                    {
                        curUSERID = dsEvaluationdetails.Tables[0].Rows[uindex]["UserId"].ToString();
                        curUSERNAME = dsEvaluationdetails.Tables[0].Rows[uindex]["UserName"].ToString();
                        curNAME = dsEvaluationdetails.Tables[0].Rows[uindex]["FirstName"].ToString();
                        if (!dsEvaluationdetails.Tables[0].Rows[uindex]["MiddleName"].Equals(""))
                            curNAME += " " + dsEvaluationdetails.Tables[0].Rows[uindex]["MiddleName"].ToString();
                        if (!dsEvaluationdetails.Tables[0].Rows[uindex]["LastName"].Equals(""))
                            curNAME += " " + dsEvaluationdetails.Tables[0].Rows[uindex]["LastName"].ToString();

                        testDATE = dsEvaluationdetails.Tables[0].Rows[uindex]["FirstLoginDate"].ToString();
                        DateTime dttest = DateTime.Parse(testDATE);//, "dd/MM/yyyy");
                        testDATE = dttest.ToString("dd/MM/yyyy");//.ToShortDateString();
                        // }

                        string cellstyle = "border: 1px ridge #000000;text-align: left; vertical-align: middle;padding-left:5px;padding-right:5px";
                        tblRow = new TableRow();
                        // add username,name(firstname+middlename+lastname),testdate;
                        tblCell = new TableCell();
                        label = new Label();
                        label.Font.Size = 12;
                        //label.Font.Bold = true;
                        label.Text = curUSERNAME;
                        tblCell.Controls.Add(label);
                        tblCell.Style.Value = cellstyle;
                        tblRow.Cells.Add(tblCell);
                        tblCell = new TableCell();
                        label = new Label();
                        label.Font.Size = 12;
                        //label.Font.Bold = true;
                        label.Text = curNAME;
                        tblCell.Controls.Add(label);
                        tblCell.Style.Value = cellstyle;
                        tblRow.Cells.Add(tblCell);
                        tblCell = new TableCell();
                        label = new Label();
                        label.Font.Size = 12;
                        //label.Font.Bold = true;
                        label.Text = testDATE;
                        tblCell.Controls.Add(label);
                        tblCell.Style.Value = cellstyle;
                        tblRow.Cells.Add(tblCell);
                        //


                        //
                        int currentINDEX = 0;

                        for (int j = 0; j < GridView1.Rows.Count; j++)
                        {
                            totalmark_variablewise = 0;
                            if (GridView1.Rows[j].Cells[6].Text != "&nbsp;")
                                if (GridView1.Rows[j].Cells[6].Text != curUSERID)
                                    continue;
                            currentINDEX++;
                            if (GridView1.Rows[j].Cells[1].Text != "&nbsp;")
                            {
                                string TESTSECTIONID = GridView1.Rows[j].Cells[2].Text;
                                mark = "0";
                                if (GridView1.Rows[j].Cells[4].Text != "0" && GridView1.Rows[j].Cells[4].Text != "&nbsp;")
                                    totalmark_variablewise = float.Parse(GridView1.Rows[j].Cells[4].Text);

                                string remarks = ""; string querystring1 = "";
                                benchmark = "";
                                int section_id = GetSectionId(GridView1.Rows[j].Cells[1].Text);

                                querystring1 = "SELECT TestID, BenchMark,DisplayName,MarkFrom,MarkTo FROM TestVariableResultBands WHERE TestID = " + testid + " AND TestSectionId = " + TESTSECTIONID;
                                querystring1 += " AND VariableId = " + section_id + "";

                                DataSet ds1 = new DataSet(); bool benchmarkexists = false;
                                ds1 = clsClasses.GetValuesFromDB(querystring1);
                                if (ds1 != null)
                                    if (ds1.Tables.Count > 0)
                                        if (ds1.Tables[0].Rows.Count > 0)
                                        {
                                            benchmarkexists = true;
                                            gradecount = ds1.Tables[0].Rows.Count;
                                            for (int g = 0; g < ds1.Tables[0].Rows.Count; g++)
                                            {
                                                if (totalmark_variablewise >= float.Parse(ds1.Tables[0].Rows[g]["MarkFrom"].ToString()) && totalmark_variablewise <= float.Parse(ds1.Tables[0].Rows[g]["MarkTo"].ToString()))
                                                { GRADE = g + 1; break; }

                                            }
                                        }


                                if (benchmarkexists == false)
                                {

                                    querystring1 = "SELECT TestID, BenchMark,DisplayName,MarkFrom,MarkTo FROM TestSectionResultBands WHERE TestID = " + testid;
                                    querystring1 += " AND SectionId = " + TESTSECTIONID;

                                    ds1 = new DataSet();
                                    ds1 = clsClasses.GetValuesFromDB(querystring1);
                                    if (ds1 != null)
                                        if (ds1.Tables.Count > 0)
                                            if (ds1.Tables[0].Rows.Count > 0)
                                            {
                                                gradecount = ds1.Tables[0].Rows.Count;
                                                for (int g = 0; g < ds1.Tables[0].Rows.Count; g++)
                                                {
                                                    if (totalmark_variablewise >= float.Parse(ds1.Tables[0].Rows[g]["MarkFrom"].ToString()) && totalmark_variablewise <= float.Parse(ds1.Tables[0].Rows[g]["MarkTo"].ToString()))
                                                    { GRADE = g + 1; break; }
                                                }
                                            }
                                }

                                //tblCell = new TableCell();
                                //label = new Label();
                                //label.Text = totalmark_variablewise.ToString();
                                //tblCell.Controls.Add(label);
                                //tblCell.Style.Value = "text-align: left; vertical-align: middle";
                                //tblCell.BackColor = Color.Red;
                                HtmlImage himage = new HtmlImage();
                                //tblRow = new TableRow();
                                string imgvalue = "width:40px;height:22px";
                                for (int n = 0; n < gradecount; n++)
                                {
                                    tblCell = new TableCell();
                                    himage = new HtmlImage(); himage.Style.Value = imgvalue;// "width: 100%; height: 100%";
                                    if (gradecount == 2)
                                    {
                                        //tblCell.Text=i+1;
                                        if (GRADE == n + 1)
                                        {
                                            if (GRADE == 1)
                                            {
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptRed.JPG";
                                                //tblCell.BackColor = Color.Red;
                                                //tblCell.Text = "Grade1_1";break;
                                                tblCell.Controls.Add(himage);break;
                                            }
                                            else if (GRADE == 2)
                                            {
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreen.JPG";
                                                //tblCell.BackColor = Color.Green;
                                                //tblCell.Text = "Grade1_2";break;
                                                tblCell.Controls.Add(himage); break;
                                            }
                                        }

                                    }
                                    else if (gradecount == 3)
                                    {
                                        if (GRADE == n + 1)
                                        {
                                            if (GRADE == 1)
                                            {                                                
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptRed.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                            else if (GRADE == 2)
                                            {                                                
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptYellowOrange.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                            else if (GRADE == 3)
                                            {                                                
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreen.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                        }
                                    }
                                    else if (gradecount == 4)
                                    {
                                        if (GRADE == n + 1)
                                        {
                                            if (GRADE == 1)
                                            {                                                
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptRed.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                            else if (GRADE == 2)
                                            {                                                
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptYellowOrange.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                            else if (GRADE == 3)
                                            {                                                
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreenYellow.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                            else if (GRADE == 4)
                                            {
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreen.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                        }
                                    }
                                    else if (gradecount == 5)
                                    {
                                        if (GRADE == n + 1)
                                        {
                                            if (GRADE == 1)
                                            {
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptRed.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                            else if (GRADE == 2)
                                            {
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptRedOrange.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                            else if (GRADE == 3)
                                            {
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptYellowOrange.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                            else if (GRADE == 4)
                                            {
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreenYellow.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }
                                            else if (GRADE == 5)
                                            {
                                                himage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreen.JPG";
                                                tblCell.Controls.Add(himage); break;
                                            }

                                        }
                                    }

                                }
                                tblCell.Style.Value = "border: 1px ridge #000000";
                                tblRow.Cells.Add(tblCell);
                            }
                            tblDisplay.Rows.Add(tblRow);
                            //break;
                        }
                        //tblDisplay.Rows.Add(tblRow);

                    }// end of for loop userdetails...

        //BIP 23-01-2011

        // tcellBarGraph.Width = "200";
        setGradeInstructions(gradecount);
        tcellBarGraph.Controls.Add(tblDisplay);
        // imgGraph.Visible = false;


    }
    private void setGradeInstructions(int gradeindex)
    {
        Table colortable = new Table();
        colortable.CellPadding = 0;
        colortable.CellSpacing = 0;
        colortable.BorderWidth = 0;
        TableRow colorrow = new TableRow();
        TableCell colorcell = new TableCell();
        HtmlImage colorimage = new HtmlImage();
        string imagestyle = "width:30px;height:20px;";
        Label colorlabel = new Label();
        if (gradeindex == 2)
        {
            colorrow = new TableRow();
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptRed.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 1 &nbsp&nbsp";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            //colortable.Rows.Add(colorrow);

            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreen.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 2";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            colortable.Rows.Add(colorrow);


            tcellColorGrade.Controls.Add(colortable);

        }
        else if (gradeindex == 3)
        {
            colorrow = new TableRow();
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptRed.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 1 &nbsp&nbsp";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            //colortable.Rows.Add(colorrow);

            //colorrow = new TableRow();
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptYellowOrange.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 2 &nbsp&nbsp";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            //colortable.Rows.Add(colorrow);

           
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreen.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 3";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            colortable.Rows.Add(colorrow);


            tcellColorGrade.Controls.Add(colortable);

        }
        else if (gradeindex == 4)
        {
            colorrow = new TableRow();
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptRed.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 1 &nbsp&nbsp";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            //colortable.Rows.Add(colorrow);

            //colorrow = new TableRow();
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptYellowOrange.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 2 &nbsp&nbsp";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            //colortable.Rows.Add(colorrow);

            //colorrow = new TableRow();
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreenYellow.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 3 &nbsp&nbsp";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            //colortable.Rows.Add(colorrow);

            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreen.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 4";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            colortable.Rows.Add(colorrow);


            tcellColorGrade.Controls.Add(colortable);
        }
        else if (gradeindex == 5)
        {
            colorrow = new TableRow();
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptRed.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 1 &nbsp&nbsp";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            //colortable.Rows.Add(colorrow);

            //colorrow = new TableRow();
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptRedOrange.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 2 &nbsp&nbsp";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            //colortable.Rows.Add(colorrow);

            //colorrow = new TableRow();
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptYellowOrange.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 3 &nbsp&nbsp";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            //colortable.Rows.Add(colorrow);

            //colorrow = new TableRow();
            colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreenYellow.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 4 &nbsp&nbsp";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            //colortable.Rows.Add(colorrow);

            //colorimage = new HtmlImage();
            colorimage.Src = "~/QuestionAnswerFiles/ReportImages/rptGreen.JPG";
            colorimage.Style.Value = imagestyle;
            colorcell = new TableCell();
            colorcell.Controls.Add(colorimage);
            colorrow.Cells.Add(colorcell);
            colorlabel = new Label();
            colorlabel.Text = "Grade 5";
            colorcell = new TableCell();
            colorcell.Controls.Add(colorlabel);
            colorrow.Cells.Add(colorcell);
            colortable.Rows.Add(colorrow);


            tcellColorGrade.Controls.Add(colortable);
        }
        

    }


    private float GetPercentileScoreUserwise_testsec(float totalmark, int testsecionid)
    {
        double score = 0;
        int XF = 1, CF = 0, totnum = 0;
        string quesrystring = "select sum(TotalScore) as SecTotalScore,userid from ScoreTable where TestId=" + testid + " and TestSectionId=" + testsecionid + " group by Userid";
        DataSet dssecdet = new DataSet();
        dssecdet = clsClasses.GetValuesFromDB(quesrystring);
        if (dssecdet != null)
            if (dssecdet.Tables.Count > 0)
                if (dssecdet.Tables[0].Rows.Count > 0)
                {
                    totnum = dssecdet.Tables[0].Rows.Count;
                    for (int i = 0; i < dssecdet.Tables[0].Rows.Count; i++)
                    //foreach (var totalmarkdet in GetPercentileScore)
                    {
                        float mark = float.Parse(dssecdet.Tables[0].Rows[i]["SecTotalScore"].ToString());
                        if (mark >= (totalmark - .5) && mark <= (totalmark + .5))
                            XF = XF + 1;
                        else if (mark < (totalmark - .5))
                            CF = CF + 1;
                    }
                    double totsamescore = .5 * XF; totnum = totnum + 1;
                    //score = (CF + (.5 * XF) * 100) / totnum;
                    score = ((CF + totsamescore) * 100) / totnum;

                }
        float scoretotal = 0;
        scoretotal = float.Parse(score.ToString());
        return scoretotal;
    }

    private float GetPercentileScoreUserwise(float totalmark, string sectionname, string testsecId)
    {

        double score = 0;
        int XF = 1, CF = 0, totnum = 1;
        DataSet dsRefScoreDetails = new DataSet();
        string quesrystring = "select * from ScoreTable where TestId = " + testid + " and TestSectionId =" + testsecId + " and SectionName ='" + sectionname + "'";//like '%" + sectionname + "%'"; //
        //lblMessage.Text += " query --- = " + quesrystring;
        dsRefScoreDetails = clsClasses.GetValuesFromDB(quesrystring);
        if (dsRefScoreDetails != null)
            if (dsRefScoreDetails.Tables.Count > 0)
                if (dsRefScoreDetails.Tables[0].Rows.Count > 0)
                {
                    totnum = int.Parse(dsRefScoreDetails.Tables[0].Rows.Count.ToString());
                    //lblMessage.Text += " rowcount= " + totnum;
                    for (int i = 0; i < dsRefScoreDetails.Tables[0].Rows.Count; i++)
                    {
                        float mark = float.Parse(dsRefScoreDetails.Tables[0].Rows[i]["TotalScore"].ToString());
                        if (mark >= (totalmark - .5) && mark <= (totalmark + .5))
                            XF = XF + 1;
                        else if (mark < (totalmark - .5))
                            CF = CF + 1;
                    }
                    double totsamescore = .5 * XF; totnum = totnum + 1;

                    score = ((CF + totsamescore) / totnum) * 100;

                }
        float scoretotal = 0;
        scoretotal = float.Parse(score.ToString());
        return scoretotal;

    }

    private String GetBandDescription(int mark, string secName)
    {
        string remarks = "";
        int secid = GetSectionId(secName);
        string querystring1 = "SELECT TestID, BenchMark,DisplayName FROM TestVariableResultBands WHERE TestID = " + testid;
        querystring1 += " AND (" + mark + " > MarkFrom AND  " + mark + " <= MarkTo)";//MarkFrom <= " + mark + " AND  MarkTo >= " + mark;
        querystring1 += " AND VariableId = " + secid;
        DataSet ds1 = new DataSet();
        ds1 = clsClasses.GetValuesFromDB(querystring1);
        if (ds1 != null)
            if (ds1.Tables.Count > 0)
                if (ds1.Tables[0].Rows.Count > 0)
                    if (ds1.Tables[0].Rows[0]["DisplayName"] != "")
                        remarks = ds1.Tables[0].Rows[0]["DisplayName"].ToString();

        return remarks;
    }

    private int GetSectionwiseTotalQuestionMarks(string sectioname, string testsectionid, int index)
    {
        int totalMemWordQuesCount = 0, totalMemImagesQuesCount = 0, totalRatingQuesCount = 0, totalOtherQuesCount = 0;
        int totalObjQuesCount = 0, totalFillQuesCount = 0, totalImageQuesCount = 0, totalVideoQuesCount = 0, totalAudioQuesCount = 0,
            totalPhotoQuesCount = 0;
        string QuerystringQuestionCount = "select sum(ObjQuestionCount) as ObjQuestions,sum(FillBlanksQuestionCount) as FillQuestions," +
                                            "sum(RatingQuestionCount) as RatingQuestions, sum(ImageQuestionCount) as ImageQuestions," +
                                            "sum(VideoQuestionCount) as VideoQuestions,sum(AudioQuestionCount) as AudioQuestions," +
                                            "sum(PhotoTypeQuestionCount) as PhotoQuestions,sum(WordTypeMemQuestionCount) as WordMemQuestions," +
                                            "sum(ImageTypeMemQuestionCount) as ImageMemQuestions from questioncount where TestId=" + testid + " and TestSectionId=" + testsectionid;
        if (index == 0)
            QuerystringQuestionCount += "and SectionName='" + sectioname + "'";

        DataSet dsQuestionCount = clsClasses.GetValuesFromDB(QuerystringQuestionCount);
        if (dsQuestionCount != null)
            if (dsQuestionCount.Tables.Count > 0)
                if (dsQuestionCount.Tables[0].Rows.Count > 0)
                {
                    for (int c = 0; c < dsQuestionCount.Tables[0].Rows.Count; c++)
                    {
                        if (dsQuestionCount.Tables[0].Rows[c]["ObjQuestions"].ToString() != null && dsQuestionCount.Tables[0].Rows[c]["ObjQuestions"].ToString() != "")
                        {
                            totalObjQuesCount = int.Parse(dsQuestionCount.Tables[0].Rows[c]["ObjQuestions"].ToString());
                            totalOtherQuesCount += totalObjQuesCount;
                        }
                        if (dsQuestionCount.Tables[0].Rows[c]["FillQuestions"].ToString() != null && dsQuestionCount.Tables[0].Rows[c]["FillQuestions"].ToString() != "")
                        {
                            totalFillQuesCount = int.Parse(dsQuestionCount.Tables[0].Rows[c]["FillQuestions"].ToString());
                            totalOtherQuesCount += totalFillQuesCount;
                        }
                        if (dsQuestionCount.Tables[0].Rows[c]["ImageQuestions"].ToString() != null && dsQuestionCount.Tables[0].Rows[c]["ImageQuestions"].ToString() != "")
                        {
                            totalImageQuesCount = int.Parse(dsQuestionCount.Tables[0].Rows[c]["ImageQuestions"].ToString());
                            totalOtherQuesCount += totalImageQuesCount;
                        }
                        if (dsQuestionCount.Tables[0].Rows[c]["VideoQuestions"].ToString() != null && dsQuestionCount.Tables[0].Rows[c]["VideoQuestions"].ToString() != "")
                        {
                            totalVideoQuesCount = int.Parse(dsQuestionCount.Tables[0].Rows[c]["VideoQuestions"].ToString());
                            totalOtherQuesCount += totalVideoQuesCount;
                        }
                        if (dsQuestionCount.Tables[0].Rows[c]["AudioQuestions"].ToString() != null && dsQuestionCount.Tables[0].Rows[c]["AudioQuestions"].ToString() != "")
                        {
                            totalAudioQuesCount = int.Parse(dsQuestionCount.Tables[0].Rows[c]["AudioQuestions"].ToString());
                            totalOtherQuesCount += totalAudioQuesCount;
                        }
                        if (dsQuestionCount.Tables[0].Rows[c]["PhotoQuestions"].ToString() != null && dsQuestionCount.Tables[0].Rows[c]["PhotoQuestions"].ToString() != "")
                        {
                            totalPhotoQuesCount = int.Parse(dsQuestionCount.Tables[0].Rows[c]["PhotoQuestions"].ToString());
                            totalOtherQuesCount += totalPhotoQuesCount;
                        }

                        if (dsQuestionCount.Tables[0].Rows[c]["RatingQuestions"].ToString() != null && dsQuestionCount.Tables[0].Rows[c]["RatingQuestions"].ToString() != "")
                            totalRatingQuesCount += int.Parse(dsQuestionCount.Tables[0].Rows[c]["RatingQuestions"].ToString());
                        if (dsQuestionCount.Tables[0].Rows[c]["WordMemQuestions"].ToString() != null && dsQuestionCount.Tables[0].Rows[c]["WordMemQuestions"].ToString() != "")
                            totalMemWordQuesCount += int.Parse(dsQuestionCount.Tables[0].Rows[c]["WordMemQuestions"].ToString());
                        if (dsQuestionCount.Tables[0].Rows[c]["ImageMemQuestions"].ToString() != null && dsQuestionCount.Tables[0].Rows[c]["ImageMemQuestions"].ToString() != "")
                            totalMemImagesQuesCount += int.Parse(dsQuestionCount.Tables[0].Rows[c]["ImageMemQuestions"].ToString());
                    }
                }

        int totalmark = 0;
        DataSet dsCount = new DataSet();
        int memimageQuesValue = 0, memWordsQuesValue = 0, quesValue = 0, ratingQuesValue = 0;
        string quesryString = "";
        // memImages questions
        if (index > 0)
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList_memImages where status=1 and TestId=" + testid + " and TestSectionId=" + testsectionid;
        else
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList_memImages where status=1 and TestId=" + testid + " and TestSectionId=" + testsectionid + " and SectionName='" + sectioname + "'";

        dsCount = clsClasses.GetValuesFromDB(quesryString);
        if (dsCount != null)
            if (dsCount.Tables.Count > 0)
                if (dsCount.Tables[0].Rows.Count > 0)
                    if (dsCount.Tables[0].Rows[0][0] != null)
                    {
                        memimageQuesValue = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                        if (memimageQuesValue > totalMemImagesQuesCount)
                            memimageQuesValue = totalMemImagesQuesCount;
                    }

        //memWords questions
        dsCount = new DataSet();
        if (index > 0)
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList_memWords where status=1 and TestId=" + testid + " and TestSectionId=" + testsectionid;
        else
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList_memWords where status=1 and TestId=" + testid + " and TestSectionId=" + testsectionid + "  and SectionName='" + sectioname + "'";
        dsCount = clsClasses.GetValuesFromDB(quesryString);
        if (dsCount != null)
            if (dsCount.Tables.Count > 0)
                if (dsCount.Tables[0].Rows.Count > 0)
                    if (dsCount.Tables[0].Rows[0][0] != null)
                    {
                        memWordsQuesValue = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                        if (memWordsQuesValue > totalMemWordQuesCount)
                            memWordsQuesValue = totalMemWordQuesCount;
                    }

        //Rating questions
        dsCount = new DataSet();
        if (index > 0)
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and Category = 'RatingType' and TestId=" + testid + " and TestSectionId=" + testsectionid;
        else
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and Category = 'RatingType' and TestId=" + testid + " and TestSectionId=" + testsectionid + "  and SectionName='" + sectioname + "'";

        //lblMessage.Text += quesryString;

        dsCount = clsClasses.GetValuesFromDB(quesryString);
        if (dsCount != null)
            if (dsCount.Tables.Count > 0)
                if (dsCount.Tables[0].Rows.Count > 0)
                    if (dsCount.Tables[0].Rows[0][0] != null)
                    {
                        int totalrate = 0; int questionid = 0;
                        ratingQuesValue = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                        if (ratingQuesValue > totalRatingQuesCount)
                            ratingQuesValue = totalRatingQuesCount;
                        // questionid = int.Parse(dsCount.Tables[0].Rows[0][1].ToString());
                        // lblMessage.Text += sectioname + " ratingquesCount= " + ratingQuesValue.ToString();// 021209 bip
                        if (index > 0)
                        {
                            var optionCount = from optcount in dataclass.View_TestBaseQuestionLists
                                              where optcount.TestSectionId == int.Parse(testsectionid) && optcount.TestId == testid && optcount.Category == "RatingType" && optcount.TestBaseQuestionStatus == 1
                                              select optcount;
                            if (optionCount.Count() > 0)
                            {
                                if (optionCount.First().Option1 != null && optionCount.First().Option1.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option2 != null && optionCount.First().Option2.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option3 != null && optionCount.First().Option3.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option4 != null && optionCount.First().Option4.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option5 != null && optionCount.First().Option5.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option6 != null && optionCount.First().Option6.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option7 != null && optionCount.First().Option7.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option8 != null && optionCount.First().Option8.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option9 != null && optionCount.First().Option9.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option10 != null && optionCount.First().Option10.ToString() != "")
                                    totalrate++;
                                //if (totalrate > 0)
                                ratingQuesValue = ratingQuesValue * totalrate;

                                // lblMessage.Text += " totaloptions= " + totalrate + " TotRate= " + ratingQuesValue.ToString() + " ... ";
                            }
                        }
                        else
                        {
                            var optionCount = from optcount in dataclass.View_TestBaseQuestionLists
                                              where optcount.TestSectionId == int.Parse(testsectionid) && optcount.SectionName == sectioname && optcount.TestId == testid && optcount.Category == "RatingType" && optcount.TestBaseQuestionStatus == 1
                                              select optcount;
                            if (optionCount.Count() > 0)
                            {
                                if (optionCount.First().Option1 != null && optionCount.First().Option1.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option2 != null && optionCount.First().Option2.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option3 != null && optionCount.First().Option3.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option4 != null && optionCount.First().Option4.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option5 != null && optionCount.First().Option5.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option6 != null && optionCount.First().Option6.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option7 != null && optionCount.First().Option7.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option8 != null && optionCount.First().Option8.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option9 != null && optionCount.First().Option9.ToString() != "")
                                    totalrate++;
                                if (optionCount.First().Option10 != null && optionCount.First().Option10.ToString() != "")
                                    totalrate++;
                                //if (totalrate > 0)
                                ratingQuesValue = ratingQuesValue * totalrate;
                            }
                        }
                    }

        dsCount = new DataSet();
        int curvalue = 0;
        if (index > 0)
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and Category = 'Objective' and TestId=" + testid + " and TestSectionId=" + testsectionid;
        else
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and  Category = 'Objective' and TestId=" + testid + " and SectionName='" + sectioname + "'";
        dsCount = clsClasses.GetValuesFromDB(quesryString);
        if (dsCount != null)
            if (dsCount.Tables.Count > 0)
                if (dsCount.Tables[0].Rows.Count > 0)
                    if (dsCount.Tables[0].Rows[0][0] != null)
                    {
                        curvalue = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                        if (curvalue > totalObjQuesCount)
                            curvalue = totalObjQuesCount;

                        quesValue += curvalue;
                    }

        // filblanks
        dsCount = new DataSet();
        curvalue = 0;
        if (index > 0)
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and Category = 'FillBlanks' and TestId=" + testid + " and TestSectionId=" + testsectionid;
        else
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and  Category = 'FillBlanks' and TestId=" + testid + " and SectionName='" + sectioname + "'";

        dsCount = clsClasses.GetValuesFromDB(quesryString);
        if (dsCount != null)
            if (dsCount.Tables.Count > 0)
                if (dsCount.Tables[0].Rows.Count > 0)
                    if (dsCount.Tables[0].Rows[0][0] != null)
                    {
                        curvalue = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                        if (curvalue > totalFillQuesCount)
                            curvalue = totalFillQuesCount;

                        quesValue += curvalue;
                    }

        // imagetype
        dsCount = new DataSet();
        curvalue = 0;
        if (index > 0)
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and Category = 'ImageType' and TestId=" + testid + " and TestSectionId=" + testsectionid;
        else
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and  Category = 'ImageType' and TestId=" + testid + " and SectionName='" + sectioname + "'";

        dsCount = clsClasses.GetValuesFromDB(quesryString);
        if (dsCount != null)
            if (dsCount.Tables.Count > 0)
                if (dsCount.Tables[0].Rows.Count > 0)
                    if (dsCount.Tables[0].Rows[0][0] != null)
                    {
                        curvalue = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                        if (curvalue > totalImageQuesCount)
                            curvalue = totalImageQuesCount;

                        quesValue += curvalue;
                    }
        // videotype
        dsCount = new DataSet();
        curvalue = 0;
        if (index > 0)
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and Category = 'VideoType' and TestId=" + testid + " and TestSectionId=" + testsectionid;
        else
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and  Category = 'VideoType' and TestId=" + testid + " and SectionName='" + sectioname + "'";

        dsCount = clsClasses.GetValuesFromDB(quesryString);
        if (dsCount != null)
            if (dsCount.Tables.Count > 0)
                if (dsCount.Tables[0].Rows.Count > 0)
                    if (dsCount.Tables[0].Rows[0][0] != null)
                    {
                        curvalue = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                        if (curvalue > totalVideoQuesCount)
                            curvalue = totalVideoQuesCount;

                        quesValue += curvalue;
                    }
        // audiotype
        dsCount = new DataSet();
        curvalue = 0;
        if (index > 0)
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and Category = 'AudioType' and TestId=" + testid + " and TestSectionId=" + testsectionid;
        else
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and  Category = 'AudioType' and TestId=" + testid + " and SectionName='" + sectioname + "'";

        dsCount = clsClasses.GetValuesFromDB(quesryString);
        if (dsCount != null)
            if (dsCount.Tables.Count > 0)
                if (dsCount.Tables[0].Rows.Count > 0)
                    if (dsCount.Tables[0].Rows[0][0] != null)
                    {
                        curvalue = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                        if (curvalue > totalAudioQuesCount)
                            curvalue = totalAudioQuesCount;

                        quesValue += curvalue;
                    }
        // phototype
        dsCount = new DataSet();
        curvalue = 0;
        if (index > 0)
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and Category = 'PhotoType' and TestId=" + testid + " and TestSectionId=" + testsectionid;
        else
            quesryString = "select Count(*) as TotalCount from View_TestBaseQuestionList where TestBaseQuestionStatus=1 and  Category = 'PhotoType' and TestId=" + testid + " and SectionName='" + sectioname + "'";

        dsCount = clsClasses.GetValuesFromDB(quesryString);
        if (dsCount != null)
            if (dsCount.Tables.Count > 0)
                if (dsCount.Tables[0].Rows.Count > 0)
                    if (dsCount.Tables[0].Rows[0][0] != null)
                    {
                        curvalue = int.Parse(dsCount.Tables[0].Rows[0][0].ToString());
                        if (curvalue > totalPhotoQuesCount)
                            curvalue = totalPhotoQuesCount;

                        quesValue += curvalue;
                    }

        //////

        totalmark = memimageQuesValue + memWordsQuesValue + quesValue + ratingQuesValue;

        return totalmark;
    }

    private Boolean CheckExistence(string section, int testsectinid, int user_id)
    {
        Boolean result = false;
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["SectionName"].ToString() == section && dt.Rows[i]["TestSectionId"].ToString() == testsectinid.ToString() && dt.Rows[i]["USERID"].ToString() == user_id.ToString())
                {
                    result = true;
                    Session["RowID"] = i;
                    break;
                }
            }
        }
        return result;
    }

    private int GetSectionId(string sectionname)
    {
        int secID = 0;
        string querystring1 = "select SectionId from SectionDetail where ParentId=0 and SectionName='" + sectionname + "'";

        DataSet ds1 = new DataSet();
        ds1 = clsClasses.GetValuesFromDB(querystring1);
        if (ds1 != null)
            if (ds1.Tables.Count > 0)
                if (ds1.Tables[0].Rows.Count > 0)
                    if (ds1.Tables[0].Rows[0]["SectionId"] != "")
                        secID = int.Parse(ds1.Tables[0].Rows[0]["SectionId"].ToString());

        return secID;
    }


    private int GetBenchMarkVariablewise(string variableName, string curmark, string TESTSECTIONID)
    {
        int benchmark = 0;
        string remarks = "";
        try
        {
            string[] strValue = curmark.Split(new char[] { '.' });
            int mark = int.Parse(strValue[0]);

            int secid = GetSectionId(variableName);
            string querystring1 = "";
            if (curmark == "0")
            {
                querystring1 = "SELECT TestID, BenchMark,DisplayName FROM TestVariableResultBands WHERE TestID = " + testid + " AND TestSectionId = " + TESTSECTIONID;
                querystring1 += " AND (0 >= MarkFrom AND 0 <= MarkTo)";
                querystring1 += " AND VariableId = " + secid;
            }
            else
            {
                querystring1 = "SELECT TestID, BenchMark,DisplayName FROM TestVariableResultBands WHERE TestID = " + testid + " AND TestSectionId = " + TESTSECTIONID;
                querystring1 += " AND (" + mark + " > MarkFrom AND  " + mark + " <= MarkTo)";//   MarkFrom <= " + mark + " AND  MarkTo >= " + mark;
                querystring1 += " AND VariableId = " + secid + "";
            }
            bool benchmarkexists = false;
            DataSet ds1 = new DataSet();
            ds1 = clsClasses.GetValuesFromDB(querystring1);
            if (ds1 != null)
                if (ds1.Tables.Count > 0)
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["BenchMark"] != "")
                            benchmark = int.Parse(ds1.Tables[0].Rows[0]["BenchMark"].ToString());
                        remarks = ds1.Tables[0].Rows[0]["DisplayName"].ToString();
                        benchmarkexists = true;
                    }

            if (benchmarkexists == false)
            {
                benchmark = GetBenchMarkTestSectionwise(curmark, TESTSECTIONID);
            }

            // lblMessage.Text += querystring1;

        }
        catch (Exception ex) { benchmark = 0; }

        return benchmark;
    }



    private string GetTestSectionName(int testsecionID)
    {
        string secName = "";
        var testsectionname = from testsecname in dataclass.TestSectionsLists
                              where testsecname.TestSectionId == testsecionID
                              select testsecname;
        if (testsectionname.Count() > 0)
        {
            if (testsectionname.First().SectionName != null && testsectionname.First().SectionName != "")
                secName = testsectionname.First().SectionName.ToString();
        }
        return secName;
    }

    private int GetBenchMarkTestSectionwise(string curmark, string TESTSECTIONID)
    {
        int benchmark = 0;
        string remarks = "";
        try
        {
            string[] strValue = curmark.Split(new char[] { '.' });
            int mark = int.Parse(strValue[0]);

            // int secid = GetSectionId(variableName);
            string querystring1 = "";

            if (curmark == "0")
            {
                querystring1 = "SELECT TestID, BenchMark,DisplayName FROM TestSectionResultBands WHERE TestID = " + testid;
                querystring1 += " AND (0 >= MarkFrom AND 0 <= MarkTo)";
                querystring1 += " AND SectionId = " + TESTSECTIONID;
            }
            else
            {
                querystring1 = "SELECT TestID, BenchMark,DisplayName FROM TestSectionResultBands WHERE TestID = " + testid;
                querystring1 += " AND (" + mark + " > MarkFrom AND  " + mark + " <= MarkTo)";//   MarkFrom <= " + mark + " AND  MarkTo >= " + mark;
                querystring1 += " AND SectionId = " + TESTSECTIONID;
            }
            DataSet ds1 = new DataSet();
            ds1 = clsClasses.GetValuesFromDB(querystring1);
            if (ds1 != null)
                if (ds1.Tables.Count > 0)
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["BenchMark"] != "")
                            benchmark = int.Parse(ds1.Tables[0].Rows[0]["BenchMark"].ToString());
                        remarks = ds1.Tables[0].Rows[0]["DisplayName"].ToString();

                    }
        }
        catch (Exception ex) { benchmark = 0; }

        return benchmark;
    }



    
}
