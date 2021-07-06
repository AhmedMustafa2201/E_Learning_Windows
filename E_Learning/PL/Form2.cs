using E_Learning.DAL;
using E_Learning.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E_Learning.PL
{
    public partial class Form2 : Form
    {
        int c_ID;
        public int CourseID { get => c_ID; set => c_ID = value; }
        public string FilePath { get; set; }
        ElearningVEntities _entity;

        public Form2()
        {
            _entity = new ElearningVEntities();
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var list = _entity.Courses.Select(w => new ComboboxItem
            {
                Text = w.Name,
                Value = w.Id
            }).ToList();

            BindingList<ComboboxItem> objects = new BindingList<ComboboxItem>();
            foreach (var item in list)
                objects.Add(new ComboboxItem() { Text = item.Text, Value = item.Value });

            comboBox1.ValueMember = null;
            comboBox1.DisplayMember = "Text";
            comboBox1.DataSource = objects;

            Display();
        }

        public void Display()
        {
            List<LessonVM> _crsList = new List<LessonVM>();
            _crsList.AddRange(_entity.VideoLessons.Where(w => w.CourseId == c_ID).Select(x => new LessonVM
            {
                Id = x.Id,
                Title = x.Title,
                Image=x.Image,
                Order = x.Order,
                VideoLink = x.VideoLink,
                CourseName = x.Cours.Name
                
            }).ToList());
            if (_crsList.Count != 0)
            {
                dataGridView1.DataSource = _crsList;
                lblID.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
                dataGridView1.Columns[0].Visible = false;

                dataGridView1.Columns[1].HeaderText = "عنوان الدرس";
                //dataGridView1.Columns[2].Visible = false;

                dataGridView1.Columns[2].HeaderText = "رابط الدرس";

                dataGridView1.Columns[3].HeaderText = "الترتيب";
                dataGridView1.Columns[3].Width = 50;
                dataGridView1.Columns[4].Visible = false;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                
                //dataGridView1.Columns[2].Visible = true;

            }
            else
            {
                MessageBox.Show("لا يوجد بيانات لعرضها", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void btnSave_Click(object sender, EventArgs e)   // Save button click event
        {
            if (txtCity.BackColor == Color.Red)
            {
                MessageBox.Show("هناك خطأ في رقم الترتيب. يرجى المحاولة مجدداً", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Bitmap b = new Bitmap(this.FilePath);

                string saveDirectory = Application.StartupPath.Replace("bin\\Debug", "");
                string value = System.Configuration.ConfigurationManager.AppSettings["file"];
                string naming = "Lessons\\"+ txtName.Text + ".jpg";
                string file = Path.Combine(saveDirectory, value, naming);

                b.Save(file);


                VideoLesson crs = new VideoLesson();
                crs.Title = txtName.Text;
                crs.VideoLink = txtAge.Text;
                crs.Order = Convert.ToInt32(txtCity.Text);
                crs.CourseId = Convert.ToInt32(comboBox1.SelectedItem.ToString());
                crs.Image = value+naming;
                bool result = SaveStudentDetails(crs); // calling SaveStudentDetails method to save the record in table.Here passing a student details object as parameter
                ShowStatus(result, "Save");
            }
        }
        public bool SaveStudentDetails(VideoLesson crs) // calling SaveStudentMethod for insert a new record
        {
            bool result = false;
            _entity.VideoLessons.Add(crs);
            _entity.SaveChanges();
            result = true;

            return result;
        }


        private void btnUpdate_Click(object sender, EventArgs e) // Update button click event
        {
            VideoLesson crs = SetValues(Convert.ToInt32(lblID.Text), txtName.Text, "", txtAge.Text, Convert.ToInt32(txtCity.Text), Convert.ToInt32(comboBox1.SelectedItem.ToString())); // Binding values to StudentInformationModel
            bool result = UpdateStudentDetails(crs); // calling UpdateStudentDetails Method
            ShowStatus(result, "Update");
        }

        public bool UpdateStudentDetails(VideoLesson crs) // UpdateStudentDetails method for update a existing Record
        {
            bool result = false;
            VideoLesson _crs = _entity.VideoLessons.Where(x => x.Id == crs.Id).Select(x => x).FirstOrDefault();
            _crs.Title = crs.Title;
            _crs.VideoLink = crs.VideoLink;
            _crs.Order = crs.Order;
            _crs.CourseId = crs.CourseId;
            _crs.Image = _crs.Image;
            _entity.SaveChanges();
            result = true;
            return result;
        }


        private void btnDelete_Click(object sender, EventArgs e) //Delete Button Event
        {
            VideoLesson crs = SetValues(Convert.ToInt32(lblID.Text), txtName.Text, "", txtAge.Text, Convert.ToInt32(txtCity.Text),0); // Binding values to StudentInformationModel
            bool result = DeleteStudentDetails(crs); //Calling DeleteStudentDetails Method
            ShowStatus(result, "Delete");
        }
        public bool DeleteStudentDetails(VideoLesson crs) // DeleteStudentDetails method to delete record from table
        {
            bool result = false;
            VideoLesson _crs = _entity.VideoLessons.Where(x => x.Id == crs.Id).Select(x => x).FirstOrDefault();
            _entity.VideoLessons.Remove(_crs);
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
                    //comboBox1.SelectedText = row.Cells[3].Value.ToString();
                }
            }
        }


        public VideoLesson SetValues(int Id, string Title, string image, string video_link, int order, int course_id) //Setvalues method for binding field values to StudentInformation Model class
        {
            VideoLesson crs = new VideoLesson();
            crs.Id = Id;
            crs.Title = Title;
            crs.Image = image;
            crs.VideoLink = video_link;
            crs.Order = order;
            crs.CourseId = course_id;
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



        private void labelBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.Filter = "Image Files (*.jpg;)|*.jpg;";
            if (opnfd.ShowDialog() == DialogResult.OK)
            {
                //Bitmap b = 
                this.FilePath = opnfd.FileName;
                pictureBox1.Image = new Bitmap(opnfd.FileName);

            }
        }

        private void txtCity_TextChanged_1(object sender, EventArgs e)
        {
            int parsedValue;
            if (!int.TryParse(txtCity.Text, out parsedValue))
            {
                txtCity.BackColor = Color.Red;
                return;
            }

            var Is_it = _entity.VideoLessons.Where(w => w.Order == parsedValue).FirstOrDefault();
            if (Is_it != null) txtCity.BackColor = Color.Red; else txtCity.BackColor = Color.White;

        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
