using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip_AccessVT
{
   public class vtBaglan
    {
        public OleDbConnection baglan()
        {
            OleDbConnection baglanti = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; " +
                "Data Source = Data\\stok.accdb; Persist Security Info = False;");
            baglanti.Open();
            return (baglanti);
        }
    }
}
