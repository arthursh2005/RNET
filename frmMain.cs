using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Configuration;
using System.Threading;
using System.Globalization;

namespace WindowsFormsApp3
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            string lang = ConfigurationManager.AppSettings["LANG"];//conf.AppSettings.Settings["LANG"].Value;
            if (lang != "he-IL")
                lang = "en-US";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            // Instantiate a new DBContext
            WindowsFormsApp3.RNETEntities dbContext = new WindowsFormsApp3.RNETEntities();
            // Call the Load method to get the data for the given DbSet from the database.
            dbContext.Items.Load();
            // This line of code is generated by Data Source Configuration Wizard
            itemsBindingSource.DataSource = dbContext.Items.Local.ToBindingList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //return;
            frmLogin fl = new frmLogin();
            if (fl.ShowDialog() == DialogResult.Cancel)
                Close();
            
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEdit frm = new frmAddEdit();
            frm.isAdd = true;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                using (RNETEntities db = new RNETEntities())
                {
                    db.Items.Load();
                    itemsBindingSource.DataSource = db.Items.Local.ToBindingList();
                }
                treeList1.Refresh();
                treeList1.RefreshDataSource();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Item currItem = null;
            int code = Convert.ToInt32(treeList1.GetFocusedRowCellValue("Code"));
            using (RNETEntities db = new RNETEntities())
            {
                db.Items.Load();
                currItem = db.Items.Find(code);
            }
            frmAddEdit frm = new frmAddEdit();
            frm.isAdd = false;
            frm.CurrItem = currItem;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                using (RNETEntities db = new RNETEntities())
                {
                    db.Items.Load();
                    currItem = db.Items.Find(code);
                    itemsBindingSource.DataSource = db.Items.Local.ToBindingList();
                    treeList1.Refresh();
                    treeList1.RefreshDataSource();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int code = Convert.ToInt32(treeList1.GetFocusedRowCellValue("Code"));
            using (RNETEntities db = new RNETEntities())
            {
                db.Items.Load();
                Item currItem = db.Items.Find(code);
                db.Items.Remove(currItem);
                db.SaveChanges();
                db.Items.Load();
                itemsBindingSource.DataSource = db.Items.Local.ToBindingList();
                treeList1.Refresh();
                treeList1.RefreshDataSource();
            }

        }
    }
}
