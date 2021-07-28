using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SrijanAutoStartCount
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SqlDataReader _dReader;
        public SqlDataReader dReader
        {
            get { return _dReader; }
            set { _dReader = value; }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        [Obsolete]
        public void RetriveData()
        {
            try
            {
                string strSql = "truncate table tbl_trn_ProductFilterSearchTemp; insert into tbl_trn_ProductFilterSearchTemp " +
                    " select * from fn_ProductFilterSearch() where(EstimatePrice < 10000) " +
                    " or(EstimatePricefuture < 10000) or (EstimatePrice18 < 10000) or (EstimatePrice21 < 10000)" +
                    " order by productrefno desc";
                DataUtility objdut = new DataUtility();
                dReader = objdut.GetData(strSql);
                string strSql1 = "truncate table tbl_mst_ProgressReport; insert into tbl_mst_ProgressReport" +
                    " select * from fn_ProductProgress(); select Productrefno from tbl_trn_ProductFilterSearchTemp; ";
                objdut = new DataUtility();
                dReader = objdut.GetData(strSql1);
                DataTable dtx = new DataTable();
                dtx.Load(dReader);
                if (dtx.Rows.Count > 0)
                {
                    string newsql = "insert into tbl_trn_Updatetemptabletime values ('" + DateTime.Now + "','Scheduler','" + dtx.Rows.Count + "')";
                    DataUtility objdut1 = new DataUtility();
                    dReader = objdut1.GetData(newsql);
                    lbltotal.Text = "Total Record Update " + dtx.Rows.Count.ToString() + " and next update will " +
                        "be in " + DateTime.Now.AddHours(12).ToString();
                    lbllastupdate.Text = "Last Update " + DateTime.Now.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:- " + ex.Message);
            }
        }
        [Obsolete]
        private void timer1_Tick(object sender, EventArgs e)
        {
            int minutes = DateTime.Now.Minute;
            int adjust = 720 - (minutes % 10);
            timer1.Interval = adjust * 60 * 1000;
            RetriveData();
        }
        [Obsolete]
        private void button1_Click(object sender, EventArgs e)
        {
            RetriveData();
        }
        [Obsolete]
        private void timer2_Tick(object sender, EventArgs e)
        {
            int minutes = DateTime.Now.Minute;
            int adjust = 60 - (minutes % 10);
            timer2.Interval = adjust * 60 * 1000;
            ActiveBlockUser();
        }
        [Obsolete]
        protected void ActiveBlockUser()
        {
            try
            {
                string strSql9 = "select FailedCount,NodalOfficerEmail,BlockDate,BlockTime,IsActive,IsLoginActive from tbl_mst_NodalOfficer where IsActive='N' and FailedCount!='0' and BlockDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                DataUtility objdut9 = new DataUtility();
                dReader = objdut9.GetData(strSql9);
                DataTable dtxxx = new DataTable();
                dtxxx.Load(dReader);
                if (dtxxx.Rows.Count > 0)
                {
                    for (int i = 0; dtxxx.Rows.Count > i; i++)
                    {
                        try
                        {
                            DateTime Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime Time = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss"));
                            if (Convert.ToDateTime(dtxxx.Rows[i]["BlockDate"]) == Date)
                            {
                                if (Convert.ToDateTime(dtxxx.Rows[i]["BlockTime"]) <= Time)
                                {
                                    string newsql4 = "update tbl_mst_NodalOfficer set FailedCount=0,BlockDate=NULL,BlockTime=NULL,IsLoginActive='Y',IsActive='Y' where NodalOfficerEmail='" + dtxxx.Rows[i]["NodalOfficerEmail"].ToString() + "'";
                                    DataUtility objdut4 = new DataUtility();
                                    dReader = objdut4.GetData(newsql4);
                                    lblblockuserinfo.Text = "Total Block Account found and Update " + dtxxx.Rows.Count.ToString() + " and next update will be in " + DateTime.Now.AddHours(1).ToString();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    lblblockuserinfo.Text = "Total Block Account found and Update " + dtxxx.Rows.Count.ToString() + " " +
                        "and next update will be in " + DateTime.Now.AddHours(1).ToString() + "Last Update " + DateTime.Now.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:- " + ex.Message);
            }
        }
        [Obsolete]
        private void button2_Click(object sender, EventArgs e)
        {
            ActiveBlockUser();
        }
    }
}
