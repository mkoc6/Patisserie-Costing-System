using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _21.ProjePastaneMaliyetlendirmeSistemiİ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-1DQCP20\SQLEXPRESS;Initial Catalog=21.ProjePastaneMaliyetlendirmeSistemi;Integrated Security=True");

        void malzemelistesi()
        {
            SqlDataAdapter da = new SqlDataAdapter("select* from TBLMALZEMELER", connection);
            DataTable DT = new DataTable();
            da.Fill(DT);
            bunifuDataGridView1.DataSource = DT;
        }
        void Urunlistesi()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from TBLURUNLER", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            bunifuDataGridView1.DataSource = dt;
        }
        void kasa()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from TBLKASA", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            bunifuDataGridView1.DataSource = dt;
        }
        void urunler()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("select * from TBLURUNLER",connection);
            DataTable dr = new DataTable();
            da.Fill(dr);

            cmbUrun.DataSource = dr;
            cmbUrun.DisplayMember = "AD";
            cmbUrun.ValueMember = "URUNID";
            connection.Close();
        }
        void MALZEMELER()
        {

            SqlDataAdapter DA = new SqlDataAdapter("select * from TBLMALZEMELER", connection);
            DataTable DT = new DataTable();
            DA.Fill(DT);
            cmbMalzeme.DataSource = DT;
            cmbMalzeme.DisplayMember = "AD";
            cmbMalzeme.ValueMember = "MALZEMEID";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            malzemelistesi();
            urunler();
            MALZEMELER();

        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            Urunlistesi();
        }

        private void bunifuThinButton27_Click(object sender, EventArgs e)
        {
            malzemelistesi();
        }

        private void bunifuThinButton28_Click(object sender, EventArgs e)
        {
            kasa();
        }

        private void btnCıkıs_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("insert into TBLMALZEMELER (AD, STOK, FIYAT, NOTLAR) values (@P1,@P2,@P3,@P4)", connection);
            command.Parameters.AddWithValue("@P1", txtMalzemeAd.Text);
            command.Parameters.AddWithValue("@P2", TxtMalzemeStok.Text);
            command.Parameters.AddWithValue("@P3", txtMalzemeFiyat.Text);
            command.Parameters.AddWithValue("@P4", txtMalzemeNotlar.Text);
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Malzeme Eklenmiştir");
            malzemelistesi();
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("insert into TBLURUNLER (AD) VALUES (@P1)", connection);
            command.Parameters.AddWithValue("@P1", TtxtUrunAd.Text);
            command.ExecuteNonQuery();
            MessageBox.Show("Urun Eklendi");
            connection.Close();
            Urunlistesi();
        }

        private void vtnurunolustur_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand COMMAND = new SqlCommand("insert into TBLFIRIN (URUNID, MALZEMEID, MIKTAR, MALIYET) VALUES (@P1,@P2,@P3,@P4) ", connection);
            COMMAND.Parameters.AddWithValue("@P1", cmbUrun.SelectedValue);
            COMMAND.Parameters.AddWithValue("@P2", cmbMalzeme.SelectedValue);
            COMMAND.Parameters.AddWithValue("@P3",decimal.Parse( txtMiktar.Text));
            COMMAND.Parameters.AddWithValue("@P4",decimal.Parse(txtMaliyet.Text));
            COMMAND.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Ürün Oluşturuldu");
            listBox1.Items.Add(cmbMalzeme.Text +"-" + txtMaliyet.Text).ToString();
            
        }

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            Double maliyet;
            if (txtMiktar.Text == "")
            {
                txtMiktar.Text = "0";
            }
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM TBLMALZEMELER where malzemeıd=@p1", connection);
            command.Parameters.AddWithValue("@p1", cmbMalzeme.SelectedValue);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
               txtMaliyet.Text = dr[3].ToString();
            }
            connection.Close();
            maliyet = Convert.ToDouble(txtMaliyet.Text) / 1000 * Convert.ToDouble(txtMiktar.Text);
            txtMaliyet.Text = maliyet.ToString();
        }

        private void bunifuDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = bunifuDataGridView1.SelectedCells[0].RowIndex;
            textBox10.Text = bunifuDataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TtxtUrunAd.Text = bunifuDataGridView1.Rows[secilen].Cells[1].Value.ToString();
            connection.Open();
            SqlCommand command = new SqlCommand("select sum(MALIYET) FROM TBLFIRIN where urunıd=@p1", connection);
            command.Parameters.AddWithValue("@p1", textBox10.Text);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                TxtUrunMliyetFiYAT.Text = dr[0].ToString();
            }
            connection.Close();
        }
    }
}
