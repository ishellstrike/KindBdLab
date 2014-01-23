using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace KindBdLab
{
    public partial class GroupData : Form
    {
        public GroupData()
        {
            InitializeComponent();
        }

        private int groupsel = 0;

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) {
            var con = Form1.con;
            if (dataGridView1.SelectedCells.Count == 0) { return; }
            var t = dataGridView1.SelectedCells[0].RowIndex;
            var t2 = dataGridView1.Rows[t].Cells[0].Value;
            groupsel = (int)t2;
            using (var cmd = new MySqlCommand(string.Format("SELECT `name`,`birth` from childrens where childrens.group = {0}", t2), con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt.DefaultView;
                    dataGridView2.Update();
                }
            }
            using (var cmd = new MySqlCommand(string.Format("SELECT `name`,`birth` from childrens where childrens.group = {0}", t2), con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt.DefaultView;
                    dataGridView2.Update();
                }
            }

            UpdateIllness();
        }

        private void GroupData_Shown(object sender, EventArgs e)
        {
            var con = Form1.con;
            var query = "SELECT `group_id`, `room`, `name` as vospitatel FROM groups LEFT JOIN vosp ON vosp.group = groups.group_id GROUP BY group_id";
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

        private void GroupData_Load(object sender, EventArgs e)
        {

        }
        IFormatProvider ifp = new CultureInfo("en-US");

        private void UpdateIllness() {
            var con = Form1.con;
            var a = dateTimePicker1.Value.ToString("s");
            a = a.Substring(0, a.IndexOf('T'));
            var b = dateTimePicker2.Value.ToString("s");
            b = b.Substring(0, b.IndexOf('T'));
            var str = string.Format(@"SELECT `illness`, `date`, `name` FROM med 
                                                                LEFT JOIN childrens ON childrens.children_id = med.children_id
                                                                WHERE type = 'ill' 
                                                                AND `date` BETWEEN CAST('{1}' AS date) AND CAST('{2}' AS date)
                                                                AND med.children_id IN (SELECT childrens.children_id FROM childrens WHERE childrens.group={0})",
                                    groupsel, a, b);
            using (var cmd = new MySqlCommand(str, con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView3.DataSource = dt.DefaultView;
                    dataGridView3.Update();
                }
            }

            str = string.Format(@"SELECT `illness`, `date`, `name` FROM med 
                                                                LEFT JOIN childrens ON childrens.children_id = med.children_id
                                                                WHERE type = 'ill' 
                                                                AND `date` BETWEEN CAST('{1}' AS date) AND CAST('{2}' AS date)",
                                    groupsel, a, b);
            using (var cmd = new MySqlCommand(str, con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView4.DataSource = dt.DefaultView;
                    dataGridView4.Update();
                }
            }

              
            str = string.Format(@"SELECT max(`value`) - min(`value`) AS difference_ves, YEAR(`date`) as year, `name` FROM med 
                                                                LEFT JOIN childrens ON childrens.children_id = med.children_id
                                                                WHERE type = 'ves' 
                                                                AND med.children_id IN (SELECT childrens.children_id FROM childrens WHERE childrens.group={0})
                                                                GROUP BY med.children_id",
                                   groupsel, a, b);
            using (var cmd = new MySqlCommand(str, con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView5.DataSource = dt.DefaultView;
                    dataGridView5.Update();
                }
            }

            //  AND med.children_id IN (SELECT childrens.children_id FROM childrens WHERE childrens.group={0})
            str = string.Format(@"SELECT max(`value`) - min(`value`) AS difference_ves, YEAR(`date`) as year, `name` FROM med 
                                                                LEFT JOIN childrens ON childrens.children_id = med.children_id
                                                                WHERE type = 'ves' 
                                                                GROUP BY med.children_id",
                                   groupsel, a, b);
            using (var cmd = new MySqlCommand(str, con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView6.DataSource = dt.DefaultView;
                    dataGridView6.Update();
                }
            }

            str = string.Format(@"SELECT max(`value`) - min(`value`) AS difference_ves, YEAR(`date`) as year, `name` FROM med 
                                                                LEFT JOIN childrens ON childrens.children_id = med.children_id
                                                                WHERE type = 'ves' 
                                                                GROUP BY med.children_id",
                                   groupsel, a, b);
            using (var cmd = new MySqlCommand(str, con)) {

            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdateIllness();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            UpdateIllness();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
