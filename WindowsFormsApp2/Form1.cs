using AForge.Imaging.Filters;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private string filePath;
        private Bitmap MyImage;
        private string fileFormat;
        private int zoom;
        private int penSize = 1;
        PointF lastPoint = PointF.Empty;
        bool isMouseDown = new Boolean();
        private Color penColor;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("File name: " + fd.FileName);
                
                filePath = fd.FileName;
                fileFormat = Path.GetExtension(filePath);
                fileFormat = fileFormat.ToUpper();
                if (fileFormat.Equals(".JPG"))
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    int xSize = pictureBox1.Width;
                    int ySize = pictureBox1.Height;
                    ShowMyImage(filePath, xSize, ySize);
                }
                else
                {
                    MessageBox.Show("ONLY JPG FILES ALLOWED!");
                }
            }
        }

        public void ShowMyImage(string filePath, int xSize, int ySize)
        {
            // Sets up an image object to be displayed.
            if (MyImage != null)
            {
                MyImage.Dispose();
                lastPoint = Point.Empty;
            }

            // Stretches the image to fit the pictureBox.
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            MyImage = new Bitmap(filePath);
            pictureBox1.ClientSize = new Size(xSize, ySize);
            pictureBox1.Image = (Image)MyImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap greyImage = filter.Apply(MyImage);
            pictureBox1.Image = (Image)greyImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Invert filter = new Invert();
            Bitmap invertImage = filter.Apply(MyImage);
            pictureBox1.Image = (Image)invertImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Sepia filter = new Sepia();
            Bitmap sepiaImage = filter.Apply(MyImage);
            pictureBox1.Image = (Image)sepiaImage;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            
            lastPoint = RealMousePosition(e);
            
            isMouseDown = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isMouseDown == true)
            {
                if(lastPoint != null)
                {
                    if (pictureBox1.Image == null)
                    {
                        MessageBox.Show("INSERT IMAGE FIRST!");
                    }

                    using(Graphics g = Graphics.FromImage(MyImage))
                    {
                        g.DrawLine(new Pen(penColor, penSize), lastPoint, RealMousePosition(e));
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                    }
                    pictureBox1.Invalidate();
                    lastPoint = RealMousePosition(e);
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;

            lastPoint = Point.Empty;
        }

        PointF RealMousePosition(MouseEventArgs e)
        {
            float Pic_width = MyImage.Width / pictureBox1.Width;
            float Pic_height = MyImage.Height / pictureBox1.Height;
            var mouseArgs = (MouseEventArgs)e;
            float xpoint = Convert.ToInt16(mouseArgs.X * Pic_width);
            float ypoint = Convert.ToUInt16(mouseArgs.Y * Pic_height);

            return new PointF(xpoint, ypoint);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = comboBox1.SelectedItem.ToString();
            penSize = Int32.Parse(selectedValue);
            Console.WriteLine("CHOICE: " + selectedValue);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                penColor = colorDialog1.Color;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Images|*.jpg";
            ImageFormat format = ImageFormat.Jpeg;
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(saveFileDialog1.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;

                }
                pictureBox1.Image.Save(saveFileDialog1.FileName, format);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)MyImage;
        }
    }
}
