using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace KindBdLab
{
    public partial class Form1 : Form {
        internal static MySqlConnection con;
        private AddForm form = new AddForm();
        private GroupData gform = new GroupData();
        internal static bool NeedUpdate;

        public Form1()
        {
            InitializeComponent();
            con = SqlMethods.EstablishConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlMethods.DropAll(con);
            SqlMethods.RecreateTables(con);
            SqlMethods.FillTestData(con);
            UpdateDb();
        }

        public void UpdateDb() {
            string query = "SELECT * FROM childrens";
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt.DefaultView;
                    dataGridView1.Update();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0) { return; }
            var t = dataGridView1.SelectedCells[0].RowIndex;
            var t2 = dataGridView1.Rows[t].Cells[4].Value;
            var t3 = dataGridView1.Rows[t].Cells[5].Value;
            using (var cmd = new MySqlCommand(string.Format("call sel({0},{1})", t2, t3), con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView4.DataSource = dt.DefaultView;
                    dataGridView4.Update();
                }
            }

            var t4 = dataGridView1.Rows[t].Cells[0].Value;
            using (var cmd = new MySqlCommand(string.Format("call p1({0})", t4), con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView5.DataSource = dt.DefaultView;
                    dataGridView5.Update();
                }
            }

            using (var cmd = new MySqlCommand(string.Format("call p2({0})", t4), con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView6.DataSource = dt.DefaultView;
                    dataGridView6.Update();
                }
            }

            Graphics g = pictureBox1.CreateGraphics();
            g.FillRectangle(Brushes.AliceBlue,0,0,pictureBox1.Width,pictureBox1.Height);
            int i = 0;
            int pre = 0, pos = 0;
            foreach (DataGridViewRow row in dataGridView5.Rows) {
                if (row.Cells.Count > 3) {
                    pre = pos;
                    if (row.Cells[3].Value != null) {
                        pos = (int) row.Cells[3].Value;
                    }
                    g.DrawLine(Pens.BlueViolet, i * 5, pictureBox1.Height - pre, (i + 1) * 5, pictureBox1.Height - pos);
                    i++;
                }
            }

            using (var cmd = new MySqlCommand(string.Format("SELECT max(`value`)-min(`value`) AS difference_rost, `date` FROM med WHERE children_id = {0} and type='rost' GROUP BY MONTH(`date`)", t4), con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt.DefaultView;
                    dataGridView2.Update();
                }
            }

            using (var cmd = new MySqlCommand(string.Format("SELECT max(`value`)-min(`value`) AS difference_ves, `date` FROM med WHERE children_id = {0} and type='ves' GROUP BY MONTH(`date`)", t4), con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView3.DataSource = dt.DefaultView;
                    dataGridView3.Update();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!form.Visible) {
                form.Show(this);
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (NeedUpdate) {
                NeedUpdate = false;

                UpdateDb();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!gform.Visible)
            {
                gform.Show(this);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateDb();
        }
    }
}
