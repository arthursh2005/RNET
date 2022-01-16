using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class frmAddEdit : Form
    {
        public bool isAdd { set; get; }
        public Item CurrItem = null;
        public frmAddEdit()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (isAdd)
            {
                using (RNETEntities db = new RNETEntities())
                {
                    try
                    {
                        var items = db.Items;
                        int max = items.Max(item => item.Code);
                        if(Convert.ToInt32(numCode.Value) <= max)
                        {
                            MessageBox.Show("Wrong code number");
                            return;
                        }                        
                        Item it = new Item();
                        it.Code = Convert.ToInt32(numCode.Value);
                        it.Name = txtName.Text;
                        it.Price = numPrice.Value;
                        it.Picture = File.ReadAllBytes(txtImage.Text);
                        if (numParent.Value < 1)
                            it.ParentID = null;
                        else
                            it.ParentID = Convert.ToInt32(numParent.Value);
                        db.Items.Add(it);
                        db.SaveChanges();
                        DialogResult = DialogResult.OK;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    //var items = db.Items;
                    //int i = items.Count();
                    
                }
            }
            else
            {
                using (RNETEntities db = new RNETEntities())
                {
                    try
                    {
                        Item it = db.Items.Find(CurrItem.Code);
                        //Item it = new Item();
                        it.Code = Convert.ToInt32(numCode.Value);
                        it.Name = txtName.Text;
                        it.Price = numPrice.Value;
                        if (String.IsNullOrEmpty(txtImage.Text.Trim()))
                            it.Picture = CurrItem.Picture;
                        else
                            it.Picture = File.ReadAllBytes(txtImage.Text);
                        if (numParent.Value < 1)
                            it.ParentID = null;
                        else
                            it.ParentID = Convert.ToInt32(numParent.Value);
                        
                        //db.Items.Add(it);
                        db.SaveChanges();
                        DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    //var items = db.Items;
                    //int i = items.Count();

                }

            }
        }

        private void frmAddEdit_Load(object sender, EventArgs e)
        {
            if(!isAdd && CurrItem != null)
            {
                numCode.Value = CurrItem.Code;
                numCode.ReadOnly = true;
                txtName.Text = CurrItem.Name;
                numPrice.Value = CurrItem.Price;
                byte[] data = CurrItem.Picture;//(byte[])dt.Rows[e.RowIndex]["Picture"];
                MemoryStream ms = new MemoryStream(data);
                picBox.Image = Image.FromStream(ms);
                if (CurrItem.ParentID == null)
                    numParent.Value = 0;
                else
                    numParent.Value = Convert.ToInt32(CurrItem.ParentID);
            }
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (openImageFileDialog.ShowDialog() == DialogResult.OK)
                txtImage.Text = openImageFileDialog.FileName;
        }
    }
}
