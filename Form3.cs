using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zadatak01
{
    
    public partial class Form3 : Form
    {
        static int nStars;

        //Star names
        string[] properDataC = new string[nStars];
        //Stars distance from the sun
        double[] distDataC = new double[nStars];
        //Stars brightness
        double[] magDataC = new double[nStars];

        public Form3()
        {
            InitializeComponent();
        }

        private static int map(double value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            //mapping the values so that they stay true to scale
            return Convert.ToInt32((value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow);
        }

        public void StarSize(string[] a, double[] b, double[] c, int nStrarsIn)
        {
            //Import of the data from form2
            properDataC = a;
            distDataC = b;
            magDataC = c;
            nStars = nStrarsIn;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

            //Load the image
            Image image = Image.FromFile("star.bmp");

            //Star and their names array
            Label[] labels = new Label[nStars];
            PictureBox[] pictureBoxes = new PictureBox[nStars];

            //generate a random Object
            Random rnd = new Random();

            
            int ScreenCenterY = (this.Height / 2) - 35;
            int ScreenCenterX = (this.Width / 2) - 75;

            //Creating stars
            for (int i = 0; i < nStars; i++)
            {
                pictureBoxes[i] = new PictureBox();
                labels[i] = new Label();

                //Picture box settings

                //brightness - star size
                int starSize = map(magDataC[i], 0, 30, 5, 45);
                int starPositionX = map(distDataC[i], 0, 220, ScreenCenterX+40, this.Width - 100);
                int starPositionY = map(distDataC[i], 0, 220, ScreenCenterY+40, this.Height - 100);

                //Location settings
                Point pStar = new Point();
                Point pCenter = new Point();

                pStar.X = starPositionX;
                pStar.Y = starPositionY;

                pCenter.X = ScreenCenterX;
                pCenter.Y = ScreenCenterY;

                //Insert the image - proccess
                pictureBoxes[i].Image = image;
                pictureBoxes[i].Size = new Size(starSize, starSize);
                pictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxes[i].BackColor = Color.Transparent;

                //random rotation
                int angle = rnd.Next(0, 360);

                if ( i == 0) // check if its Sun and position it in the middle
                    pictureBoxes[i].Location = new Point(ScreenCenterX, ScreenCenterY);
                else // if its not Sun rotate the point
                {
                    Point pStarDraw = rotate_point(pStar, pCenter, angle);
                    pictureBoxes[i].Location = pStarDraw;
                }

                //Lable info
                labels[i].BackColor = Color.Transparent;
                labels[i].ForeColor = Color.White;
                labels[i].Text = properDataC[i];

                //Lable dynamic width
                labels[i].Width = properDataC[i].Length * 8;

                if (i == 0)
                {
                    labels[i].Location = new Point(ScreenCenterX + starSize, ScreenCenterY + starSize);
                }
                else
                {
                    Point pLable = rotate_point(pStar, pCenter, angle);
                    //Acount the lable position for the size of the stars
                    pLable.X += starSize;
                    pLable.Y += starSize;
                    labels[i].Location = pLable;

                }
                labels[i].Click += new EventHandler(lable_Click); //assign click handler
                //Show/Control 
                this.Controls.Add(labels[i]);
                this.Controls.Add(pictureBoxes[i]);
            }


            Point rotate_point(Point pStarIN, Point pCenterIN, int angle)
            {
                //Rotation of the point(star) around SUN with random angle
                
                //Convert to radians
                double radians = (Math.PI / 180) * angle;

                double sin = Math.Sin(radians);
                double cos = Math.Cos(radians);

                //Bring the star to origin
                pStarIN.X -= pCenterIN.X;
                pStarIN.Y -= pCenterIN.Y;

                //Rotate the point
                double xnew = pStarIN.X * cos - pStarIN.Y * sin;
                double ynew = pStarIN.X * sin + pStarIN.Y * cos;

                //Assigne to new point Converting to int and relocationg.
                Point newPoint = new Point(Convert.ToInt32(xnew) + pCenterIN.X, Convert.ToInt32(ynew) + pCenterIN.Y);
                return newPoint;

            }
        }

        private void lable_Click(object sender, EventArgs e)
        {
            // A function to bring the lable to front. When you click the lable you cant see the text, it pops to front
            Label clickedLabel = sender as Label;

            clickedLabel.BringToFront();
        }
    }
}
