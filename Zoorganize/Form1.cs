using Microsoft.Data.Sqlite;
using System.Data;

namespace Zoorganize
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=Database/Zoorganize.db";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using SqliteConnection conn =
                new SqliteConnection(connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText =
                "SELECT Id FROM Pfleger LIMIT 1";

            object result = cmd.ExecuteScalar();

            if (result != null)
                MessageBox.Show(result.ToString());
            else
                MessageBox.Show("Keine Daten in der Tabelle.");
        }
    }
}
