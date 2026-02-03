using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Zoorganize.Database;

namespace Zoorganize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            using var context = new AppDbContext();
            
            var ersterPfleger = context.Pfleger.FirstOrDefault();

            if (ersterPfleger != null)
                MessageBox.Show($"Id: {ersterPfleger.Id}\nName: {ersterPfleger.Name}");
            else
                MessageBox.Show("Keine Pfleger in der Datenbank gefunden.");
        }
    }
}
