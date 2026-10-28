using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace InventaireMC2180
{
    public class MainForm : Form
    {
        private Label lblUser, lblEmpl, lblCode, lblQty, lblStatus;
        private TextBox txtUser, txtEmpl, txtCode, txtQty;
        private Button btnValid, btnQuit;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Inventaire Pièces";
            this.Size = new Size(240, 320);
            this.MinimizeBox = false;

            lblUser = new Label { Text = "User:", Location = new Point(5, 10), Size = new Size(50, 20) };
            lblEmpl = new Label { Text = "Empl:", Location = new Point(5, 35), Size = new Size(50, 20) };
            lblCode = new Label { Text = "Code:", Location = new Point(5, 60), Size = new Size(50, 20) };
            lblQty = new Label { Text = "Qté:", Location = new Point(5, 85), Size = new Size(50, 20) };

            txtUser = new TextBox { Location = new Point(60, 8), Size = new Size(160, 20) };
            txtEmpl = new TextBox { Location = new Point(60, 33), Size = new Size(160, 20) };
            txtCode = new TextBox { Location = new Point(60, 58), Size = new Size(160, 20) };
            txtQty = new TextBox { Text = "1", Location = new Point(60, 83), Size = new Size(160, 20) };

            btnValid = new Button { Text = "Valider", Location = new Point(10, 115), Size = new Size(100, 30) };
            btnQuit = new Button { Text = "Quitter", Location = new Point(120, 115), Size = new Size(100, 30) };

            lblStatus = new Label { Text = "Prêt à scanner", Location = new Point(5, 155), Size = new Size(215, 40), ForeColor = Color.Blue };

            btnValid.Click += new EventHandler(BtnValid_Click);
            btnQuit.Click += new EventHandler((s, e) => Application.Exit());
            txtCode.KeyPress += new KeyPressEventHandler(TxtCode_KeyPress);

            this.Controls.Add(lblUser); this.Controls.Add(txtUser);
            this.Controls.Add(lblEmpl); this.Controls.Add(txtEmpl);
            this.Controls.Add(lblCode); this.Controls.Add(txtCode);
            this.Controls.Add(lblQty);  this.Controls.Add(txtQty);
            this.Controls.Add(btnValid); this.Controls.Add(btnQuit);
            this.Controls.Add(lblStatus);
        }

        private void TxtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return || e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                e.Handled = true;
                EnregistrerLigne();
            }
        }

        private void BtnValid_Click(object sender, EventArgs e)
        {
            EnregistrerLigne();
        }

        private void EnregistrerLigne()
        {
            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                lblStatus.Text = "Erreur: Code vide !";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            try
            {
                string txtPath = @"\invpieces.txt";
                string csvPath = @"\invpieces.csv";

                if (!File.Exists(csvPath))
                {
                    using (StreamWriter swHeader = new StreamWriter(csvPath, true))
                    {
                        swHeader.WriteLine("Emplacement;Utilisateur;CodeBarres;Quantite;Date;Heure");
                    }
                }

                string dateStr = DateTime.Now.ToString("dd/MM/yyyy");
                string timeStr = DateTime.Now.ToString("HH:mm");
                string qtyStr = string.IsNullOrEmpty(txtQty.Text.Trim()) ? "1" : txtQty.Text.Trim();

                string line = string.Format("{0};{1};{2};{3};{4};{5}",
                    txtEmpl.Text.Trim(),
                    txtUser.Text.Trim(),
                    txtCode.Text.Trim(),
                    qtyStr,
                    dateStr,
                    timeStr);

                using (StreamWriter swTxt = new StreamWriter(txtPath, true))
                {
                    swTxt.WriteLine(line);
                }

                using (StreamWriter swCsv = new StreamWriter(csvPath, true))
                {
                    swCsv.WriteLine(line);
                }

                lblStatus.Text = "Ajouté : " + txtCode.Text.Trim();
                lblStatus.ForeColor = Color.Green;

                txtCode.Text = "";
                txtCode.Focus();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Erreur : " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
    }

    static class Program
    {
        [MTAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }
    }
}
