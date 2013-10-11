using System.Drawing;
using System.Windows.Forms;

namespace SimpleVisualization
{
    internal class BitMapDrawer
    {
        public void SampleDraw(int width, int height, PictureBox pictureBox1, Graph gr)
        {
            /*
            int nWidth = width, nHeight = height;

            Pen p1 = new Pen(Color.OrangeRed);
            Bitmap b1 = new Bitmap(nWidth, nHeight);

            using (Graphics g1 = Graphics.FromImage(b1))
            {
                //рисуем
                  //gr.DrawGraph(g1);

                  g1.DrawEllipse(p1, 1000, 1000, 100, 100);
            }

            Graphics g2 = Graphics.FromImage(b1);
            g2.DrawEllipse(p1, 1000, 1000, 100, 100);

            pictureBox1.Image = b1;
           // b1.Save("C:\\test.bmp");

            p1.Dispose();
             * */

            int nWidth = width, nHeight = height;
            var redPen = new Pen(Color.Red, 8);

            var b1 = new Bitmap(nWidth, nHeight);
            using (var g1 = Graphics.FromImage(b1))
            {
                //рисуем
                //g1.DrawRectangle(redPen, 0, 0, width - 100, height - 100);
                //g1.DrawEllipse(redPen, 300, 300, 100, 100);

                gr.DrawGraph(g1);
            }

            pictureBox1.Image = b1;
            b1.Save("D:\\test.bmp");

            redPen.Dispose();
        }
    }
}