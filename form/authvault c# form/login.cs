using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace authvault_c__form
{
    public partial class login : Form
    {
        private const int APPLICATION_ID = 37;
        private const string SECRET = "1ef87b0a176f4d62fd9bb8a8";

        private List<Dot> dots;
        private System.Windows.Forms.Timer timer;
        private Random random;

        public login()
        {
            InitializeComponent();
            this.Load += new EventHandler(login_Load);
            this.Paint += new PaintEventHandler(login_Paint);
            this.FormBorderStyle = FormBorderStyle.None;

            InitializeDots();
            SetupTimer();
        }

        private void InitializeDots()
        {
            dots = new List<Dot>();
            random = new Random();

            for (int i = 0; i < 75; i++)
            {
                dots.Add(new Dot
                {
                    X = random.Next(0, this.Width),
                    Y = random.Next(-50, this.Height),
                    Size = random.Next(3, 6)
                });
            }
        }

        private void SetupTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 50; // Adjust to slow down or speed up falling dots
            timer.Tick += new EventHandler(OnTimerTick);
            timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            for (int i = 0; i < dots.Count; i++)
            {
                dots[i].Y += 1;
                if (dots[i].Y > this.Height)
                {
                    dots[i].Y = random.Next(-50, -10);
                    dots[i].X = random.Next(0, this.Width);
                }
            }
            this.Invalidate();
        }

        private void login_Load(object sender, EventArgs e)
        {
            AuthVault.Setup();
        }

        private void login_Paint(object sender, PaintEventArgs e)
        {
            int radius = 30;
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                this.Region = new Region(path);
            }

            foreach (var dot in dots)
            {
                using (Brush brush = new SolidBrush(Color.RoyalBlue))
                {
                    e.Graphics.FillEllipse(brush, dot.X, dot.Y, dot.Size, dot.Size);
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string licenseKey = license.Text;
            int validationStatus = AuthVault.ValidateLicenseKey(licenseKey, APPLICATION_ID, SECRET);

            if (validationStatus == 0)
            {
                MessageBox.Show("Valid key! Program continuing...");
                this.Hide(); // Hide the login form
                main mainForm = new main();
                mainForm.Show();
            }
            else if (validationStatus == 1)
            {
                MessageBox.Show("Hardware ID Mismatch.");
            }
            else if (validationStatus == 2)
            {
                MessageBox.Show("Invalid key.");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }

    public class Dot
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Size { get; set; }
    }
}
