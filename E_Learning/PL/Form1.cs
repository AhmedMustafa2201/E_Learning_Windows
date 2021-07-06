using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using E_Learning.DAL;
using E_Learning.PL;
using E_Learning.ViewModels;

namespace E_Learning
{
    public partial class Form1 : Form
    {
        ElearningVEntities _entity;
        public Form1()
        {
            _entity = new ElearningVEntities();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //cmbGender.Items.Add("Male");
            //cmbGender.Items.Add("Female");
            Display();

        }

        public void Display()
        {
            List<CoourseVM> _crsList = new List<CoourseVM>();
            _crsList.AddRange(_entity.Courses.Select(x => new CoourseVM
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Order = x.Order
            }).ToList());

            dataGridView1.DataSource = _crsList;
            dataGridView1.Columns[0].HeaderText = "م";
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[1].HeaderText = "عنوان المحاضرة";
            dataGridView1.Columns[2].HeaderText = "التفاصيل";
            dataGridView1.Columns[3].HeaderText = "الترتيب";
            dataGridView1.Columns[3].Width = 50;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lblID.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
        }


        private void btnSave_Click(object sender, EventArgs e)   // Save button click event
        {
            if (txtCity.BackColor == Color.Red)
            {
                MessageBox.Show("هناك خطأ في رقم الترتيب. يرجى المحاولة مجدداً", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Cours crs = new Cours();
                crs.Name = txtName.Text;
                crs.Description = txtAge.Text;
                crs.Order = Convert.ToInt32(txtCity.Text);
                bool result = SaveStudentDetails(crs); // calling SaveStudentDetails method to save the record in table.Here passing a student details object as parameter
                ShowStatus(result, "Save");
            }
        }
        public bool SaveStudentDetails(Cours crs) // calling SaveStudentMethod for insert a new record
        {
            bool result = false;
            _entity.Courses.Add(crs);
            _entity.SaveChanges();
            result = true;

            return result;
        }


        private void btnUpdate_Click(object sender, EventArgs e) // Update button click event
        {
            Cours crs = SetValues(Convert.ToInt32(lblID.Text), txtName.Text, txtAge.Text, Convert.ToInt32(txtCity.Text)); // Binding values to StudentInformationModel
            bool result = UpdateStudentDetails(crs); // calling UpdateStudentDetails Method
            ShowStatus(result, "Update");
        }
        public bool UpdateStudentDetails(Cours crs) // UpdateStudentDetails method for update a existing Record
        {
            bool result = false;
            Cours _crs = _entity.Courses.Where(x => x.Id == crs.Id).Select(x => x).FirstOrDefault();
            _crs.Name = crs.Name;
            _crs.Description = crs.Description;
            _crs.Order = crs.Order;
            _entity.SaveChanges();
            result = true;
            return result;
        }

        private void btnDelete_Click(object sender, EventArgs e) //Delete Button Event
        {
            Cours crs = SetValues(Convert.ToInt32(lblID.Text), txtName.Text, txtAge.Text, Convert.ToInt32(txtCity.Text)); // Binding values to StudentInformationModel
            bool result = DeleteStudentDetails(crs); //Calling DeleteStudentDetails Method
            ShowStatus(result, "Delete");
        }
        public bool DeleteStudentDetails(Cours crs) // DeleteStudentDetails method to delete record from table
        {
            bool result = false;
            Cours _crs = _entity.Courses.Where(x => x.Id == crs.Id).Select(x => x).FirstOrDefault();
            _entity.Courses.Remove(_crs);
            _entity.SaveChanges();
            result = true;
            return result;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //Calling Datagridview cell click to Update and Delete
        {
            if (dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows) // foreach datagridview selected rows values
                {
                    lblID.Text = row.Cells[0].Value.ToString();
                    txtName.Text = row.Cells[1].Value.ToString();
                    txtAge.Text = row.Cells[2].Value.ToString();
                    txtCity.Text = row.Cells[3].Value.ToString();
                }
            }
        }

        private void txtCity_TextChanged(object sender, EventArgs e)
        {
            int s= 0;
            if(txtCity.Text!="")
                s = int.Parse(txtCity.Text);
            var Is_it = _entity.Courses.Where(w => w.Order == s).FirstOrDefault();
            if (Is_it != null) txtCity.BackColor = Color.Red; else txtCity.BackColor = Color.White;
        }


        public Cours SetValues(int Id, string Name, string desc, int order) //Setvalues method for binding field values to StudentInformation Model class
        {
            Cours crs = new Cours();
            crs.Id = Id;
            crs.Name = Name;
            crs.Description = desc;
            crs.Order = order;
            return crs;
        }

        public void ShowStatus(bool result, string Action) // validate the Operation Status and Show the Messages To User
        {
            if (result)
            {
                if (Action.ToUpper() == "SAVE")
                {
                    MessageBox.Show("تم الحفظ بنجاح", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (Action.ToUpper() == "UPDATE")
                {
                    MessageBox.Show("تم التعديل بنجاح", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("تم الحذف", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("هناك خطأ ما. يرجى المحاولة مجدداً", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            ClearFields();
            dataGridView1.Columns.Clear();

            Display();
        }

        public void ClearFields() // Clear the fields after Insert or Update or Delete operation
        {
            txtName.Text = "";
            txtAge.Text = "";
            txtCity.Text = "";
        }

        private void lblID_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.CourseID = Convert.ToInt32(lblID.Text);
            f2.Show();
        }

    }
}
