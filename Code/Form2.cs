using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Zadatak01
{
    public partial class Form2 : Form
    {
        //Connection to DB vars
        string fileNameTxt;
        string connectionString = @"Data Source = (localDb)\localDB01; Initial Catalog = HYGDB; Integrated Security = True;";

        //Passing the fileName string from form1
        public void fileName(string b)
        {
            fileNameTxt = b.ToString();
        }
        public Form2()
        {
            InitializeComponent();
        }

        //Upload procedure
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("proper");
            dataTable.Columns.Add("dist");
            dataTable.Columns.Add("mag");
            string filePath = fileNameTxt;

            StreamReader streamReader = new StreamReader(filePath);
            string[] totalData = new string[File.ReadAllLines(filePath).Length];
            totalData = streamReader.ReadLine().Split(',');

            while (!streamReader.EndOfStream)
            {
                totalData = streamReader.ReadLine().Split(',');
                //Check if proper contains name
                if(totalData[6] != "" ) 
                {
                    dataTable.Rows.Add(totalData[6], totalData[9], totalData[13]);
                }
            }

            button3.Enabled = true;
            button2.Enabled = true;
            button1.Enabled = false;
            numericUpDown1.Enabled = true;

            dataGridView1.DataSource = dataTable;
            numericUpDown1.Maximum = dataGridView1.Rows.Count;
            
        }


        public void button2_Click(object sender, EventArgs e)
        {
            int nSize = (int)(numericUpDown1.Value);

            string[] properData = new string[nSize];
            double[] distData = new double[nSize];
            double[] magData = new double[nSize];

            Form3 frm3 = new Form3();

            for (int i = 0; i < nSize; i++)
            {
                properData[i] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                distData[i] = Math.Abs(Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value));
                magData[i] = Math.Abs(Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value));
            }

            frm3.StarSize(properData, distData, magData, nSize);
            frm3.Show();

        }

        private void Upload()
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    //Open the connection to local DB
                    sqlCon.Open();

                    //Clear the database table
                    string query = "TRUNCATE TABLE " + "dbo.HYGtable";
                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    cmd.ExecuteNonQuery();

                    //Upload the data to dbo.HYGtable
                    SqlCommand sqlCmd = new SqlCommand("ImportData", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        sqlCmd.Parameters.AddWithValue("@proper", dataGridView1.Rows[i].Cells[0].Value);
                        sqlCmd.Parameters.AddWithValue("@dist", dataGridView1.Rows[i].Cells[1].Value);
                        sqlCmd.Parameters.AddWithValue("@mag", dataGridView1.Rows[i].Cells[2].Value);
                        sqlCmd.ExecuteNonQuery();
                        sqlCmd.Parameters.Clear(); // Clear
                    }

                    MessageBox.Show("Data imported successfully!", "Success!");

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please set your database first!","Connection failed!");
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Upload();
        }

    }
}
