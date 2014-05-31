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
            LinkAll();
        }

        private MySqlDataAdapter childrenAdapter;
        private DataTable childrenTable;
        public void LinkChildrenDb() {
            string query = "SELECT * FROM childrens";
            using (MySqlCommand cmd = new MySqlCommand(query, con)) {
                childrenAdapter = new MySqlDataAdapter(cmd);
                childrenTable = new DataTable();
                childrenAdapter.Fill(childrenTable);
                dataGridView1.DataSource = childrenTable.DefaultView;
                dataGridView1.Update();
            }
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e) {
            try {
                var changes = childrenTable.GetChanges();
                if (changes != null) {
                    var mcb = new MySqlCommandBuilder(childrenAdapter);
                    childrenAdapter.UpdateCommand = mcb.GetUpdateCommand();
                    childrenAdapter.Update(changes);
                    childrenTable.AcceptChanges();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                childrenTable.RejectChanges();
            }
        }

        public void LinkAll() {
            LinkChildrenDb();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0) { return; }
            var t = dataGridView1.SelectedCells[0].RowIndex;
            var t2 = dataGridView1.Rows[t].Cells[4].Value;
            var t3 = dataGridView1.Rows[t].Cells[5].Value;
            if (t2 == DBNull.Value || t3 == DBNull.Value)
            {
                return;
            }
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

            using (var cmd = new MySqlCommand(string.Format("SELECT max(`value`)-min(`value`) AS difference_rost, `date` FROM med " +
                                                            "WHERE children_id = {0} and type='rost' " +
                                                            "GROUP BY MONTH(`date`)", t4), con))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt.DefaultView;
                    dataGridView2.Update();
                }
            }

            using (var cmd = new MySqlCommand(string.Format("SELECT max(`value`)-min(`value`) AS difference_ves, `date` FROM med " +
                                                            "WHERE children_id = {0} and type='ves' " +
                                                            "GROUP BY MONTH(`date`)", t4), con))
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (form.IsDisposed) {
                form = new AddForm();
            }

            if (!form.Visible) {
                form.Show(this);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (NeedUpdate) {
                NeedUpdate = false;

               // UpdateDb();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (gform.IsDisposed) {
                gform = new GroupData();
            }

            if (!gform.Visible)
            {
                gform.Show(this);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        { 
            LinkAll();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string query = "SELECT name, value FROM childrens LEFT JOIN med ON childrens.children_id = med.children_id " +
                           "WHERE type = 'rost' AND date = (select max(date) from med where childrens.children_id = med.children_id and type = 'rost') " +
                           "GROUP BY childrens.children_id " +
                           "ORDER BY value DESC";
            StringBuilder sb = new StringBuilder();
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                var adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                foreach (DataRow row in table.Rows) {
                    sb.AppendLine(string.Format("{0} {1}см",row[0], row[1]));
                }
            }
            MessageBox.Show(sb.ToString());
        }

        private void button6_Click(object sender, EventArgs e) {
            string query = "SELECT group_id, COUNT(*) AS count FROM groups " +
                           "LEFT JOIN childrens ON childrens.group = group_id " +
                           "LEFT JOIN med ON med.children_id = childrens.children_id " +
                           "WHERE med.type = 'ill' " +
                           "GROUP BY group_id " +
                           "ORDER BY count DESC";
            StringBuilder sb = new StringBuilder();
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                var adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                foreach (DataRow row in table.Rows)
                {
                    sb.AppendLine(string.Format("группа {0} -- {1} болезней", row[0], row[1]));
                }
            }
            MessageBox.Show(sb.ToString());
        }
    }
}
