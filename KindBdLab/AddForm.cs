using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace KindBdLab
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            var con = Form1.con;

            using (var t = new MySqlCommand(string.Format(
                @"INSERT INTO `childrens` (`name`, `group`, `birth`, `mother_id`, `father_id`) VALUES
                ('{0}', {1},'{2}',{3},{4});", textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text), con)) {
                t.ExecuteNonQuery();
            }

            Form1.NeedUpdate = true;
        }
    }
}
