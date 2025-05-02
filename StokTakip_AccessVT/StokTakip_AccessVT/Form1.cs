using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StokTakip_AccessVT
{
    public partial class Form1: Form
    {
        vtBaglan AccessBaglanti = new vtBaglan();  // urun.accdb Access veritabanına bağlanması
        DataTable AccessTablo = new DataTable();   // Access Tablo tanımlaması
        OleDbCommand SqlKomut = new OleDbCommand(); // Sql Komut değişkeni tanımlanması
        OleDbDataAdapter Adaptor = new OleDbDataAdapter();  // Access verilerini kullanmak için adaptör tanımlama

        public Form1()
        {
            InitializeComponent();
            AccessListele();
        }

        private void AccessListele()
        {
            try
            {
                string sqlCumle = "Select * from URUNLER";
                Adaptor = new OleDbDataAdapter(sqlCumle, AccessBaglanti.baglan());
                AccessTablo.Clear();
                Adaptor.Fill(AccessTablo);
                dataGridView1.DataSource = AccessTablo;
                toolStripStatusLabel2.Text = AccessTablo.Rows.Count.ToString();
            }catch
            {
                MessageBox.Show("Stok listelenirken bir hata oluştu");
            }
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Access Yeni Kayıt Ekleme
            try
            {
                string sqlEKLE = "Insert Into URUNLER(stokadi,stokturu,stokmiktari) values(@stokadi,@stokturu,@stokmiktari)";
                SqlKomut = new OleDbCommand(sqlEKLE, AccessBaglanti.baglan());
                SqlKomut.Parameters.AddWithValue("@stokadi", textBox1.Text);
                SqlKomut.Parameters.AddWithValue("@stokturu", comboBox1.Text);
                SqlKomut.Parameters.AddWithValue("@stokmiktari", maskedTextBox1.Text);
              
                SqlKomut.Connection = AccessBaglanti.baglan();
                SqlKomut.ExecuteNonQuery();
                SqlKomut.Connection.Close();
                MessageBox.Show("Yeni bir Stok Kaydı eklendi.");
            }
            catch { MessageBox.Show("Kayıt yapılırken bir hata oluştu."); }

            AccessListele();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count>0)
            {
                label2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                maskedTextBox1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
             }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // Access Stok Güncelleme
            try
            {
                string sqlGUNCELLE = "Update URUNLER set stokadi=@stokadi,stokturu=@stokturu,stokmiktari=@stokmiktari Where kayitno=@kayitno";
                SqlKomut = new OleDbCommand(sqlGUNCELLE, AccessBaglanti.baglan());

                SqlKomut.Parameters.AddWithValue("@stokadi", textBox1.Text);
                SqlKomut.Parameters.AddWithValue("@stokturu", comboBox1.Text);
                SqlKomut.Parameters.AddWithValue("@stokmiktari", maskedTextBox1.Text);
                SqlKomut.Parameters.AddWithValue("@kayitno", label2.Text);

                SqlKomut.Connection = AccessBaglanti.baglan();
                SqlKomut.ExecuteNonQuery();
                SqlKomut.Connection.Close();
                MessageBox.Show("Güncelleme yapildi.");
            }
            catch { MessageBox.Show("Güncelleme yapılırken bir hata oluştu."); }

            AccessListele();

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            // Listeleme

            AccessListele();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // Kayıt Silme 

            DialogResult SOR = new DialogResult();

            SOR = MessageBox.Show(" Bu kaydı silmek istiyor musun?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (SOR == DialogResult.Yes) KAYITSIL();
        }

        private void KAYITSIL()
        {
            try
            {
                string sqlSIL = "Delete from URUNLER Where kayitno=@kayitno";

                SqlKomut = new OleDbCommand(sqlSIL, AccessBaglanti.baglan());

                SqlKomut.Parameters.AddWithValue("@kayitno", label2.Text);

                SqlKomut.Connection = AccessBaglanti.baglan();
                SqlKomut.ExecuteNonQuery();
                SqlKomut.Connection.Close();
                MessageBox.Show("Kayıt silindi.");
            }
            catch { MessageBox.Show("Kayıt silme işlemi yapılırken bir hata oluştu."); }

            AccessListele();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            // Filtreleme

            try
            {
                string sqlAra = "Select * from URUNLER Where stokadi like '%" + toolStripTextBox1.Text + "%'";
                OleDbDataAdapter adpVeri = new OleDbDataAdapter(sqlAra, AccessBaglanti.baglan());
                AccessTablo.Clear();
                adpVeri.Fill(AccessTablo);
                dataGridView1.DataSource = AccessTablo;
                toolStripStatusLabel2.Text = AccessTablo.Rows.Count.ToString();
                if (dataGridView1.Rows.Count == 0) MessageBox.Show("Hiç bir kayıt bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Veri tabanı hatası oluştu!");
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            // Form Temizleme
            label2.Text = "";
            textBox1.Clear();
            comboBox1.Text = "";
            maskedTextBox1.Clear();

        }
    }
}
