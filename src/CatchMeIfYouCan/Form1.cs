using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatchMeIfYouCan
{
    public partial class Form1 : Form
    {
        private const int BUTTON_WIDTH = 64;
        private const int BUTTON_HEIGHT = 64;
        private const int SENSITIVE = 24;

        public Form1()
        {
            InitializeComponent();

            rn = new Random();

            Text = "Catch me if you can";

            MinimumSize = new Size(400, 400);
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            Cursor = Cursors.Default;

            // Set button properties
            button1.TabStop = false;
            button1.BackColor = Color.FromArgb(255, 194, 19);
            button1.Text = "I'm \nhere!";
            button1.Size = new Size(BUTTON_WIDTH, BUTTON_HEIGHT);

            MouseMove += HandleMouseMove;
            ClientSizeChanged += HandleClientSizeChanged;
            button1.Click += HandleButtonClick;
          
            this.MouseLeave += Form1_MouseLeave;
            this.MouseEnter += Form1_MouseEnter;
            toolStripStatusLabel1.Text = "Point: initialize ...";
            toolStripStatusLabel2.Text = "";
            

            button1.Location = GetNewLocation();

            statusStrip1.Visible = Debugger.IsAttached;
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            ChangeButotnLocation(GetNewLocation());
            label1.Text = String.Empty;
            button1.Show();
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            label1.Text = "Please move your mouse in the game window.";
            button1.Hide();
        }


        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            var formRectangle = new Rectangle(new Point(0, 0), ClientSize);
            var buttonRectangle = new Rectangle(button1.Location, button1.Size);

            if (!formRectangle.Contains(buttonRectangle))
            {
                ChangeButotnLocation(GetNewLocation());
            }
        }

        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            var done = false;
            var newLocation = Point.Empty;

            toolStripStatusLabel1.Text = String.Format("Point: ({0}, {1})", e.X, e.Y);

            var rec = new Rectangle(button1.Location, button1.Size);
            // form client rectangle
            var formRec = new Rectangle(
                new Point(SENSITIVE, SENSITIVE), 
                new Size(ClientSize.Width - (SENSITIVE * 2), ClientSize.Height - (SENSITIVE * 2)));

            rec.Inflate(SENSITIVE, SENSITIVE);
           
            if (rec.Contains(e.Location))
            {
                do
                {
                    newLocation = GetNewLocation();

                    if (!rec.Contains(newLocation) &&
                        formRec.Contains(new Rectangle(newLocation, button1.Size)))  
                    {
                        done = true;
                    }
                } while (!done);
            }


            if (done)
            {
                ChangeButotnLocation(newLocation);
            }
        }

        private void HandleButtonClick(object sender ,EventArgs e)
        {
            if (Cursor == Cursors.Default)
            {
                MessageBox.Show("You won!!\n\nTry again!", "📢 Notification");
                button1.Enabled = false;
                button1.Enabled = true;
                ChangeButotnLocation(GetNewLocation());
            }
        }

        private void ChangeButotnLocation(Point point)
        {
            if (Point.Empty != point)
            {
                button1.Location = point;
            }
        }

        private Point GetNewLocation()
        {
            var done = false;
            var newPoint = Point.Empty;

            var dangerRectangle = new Rectangle(
                new Point(button1.Location.X - SENSITIVE, button1.Location.Y - SENSITIVE),
                new Size(button1.Width + (SENSITIVE * 2), button1.Height + (SENSITIVE * 2)));


            if (ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                do
                {
                    int a = rn.Next(ClientSize.Width - button1.Width);
                    int b = rn.Next(ClientSize.Height - button1.Height);

                    newPoint = new Point(a, b);

                    if (!dangerRectangle.Contains(newPoint))
                    {
                        done = true;
                    }

                } while (!done);


                return newPoint;
            }

            return newPoint;
        }

        private readonly Random rn;


    }
}
