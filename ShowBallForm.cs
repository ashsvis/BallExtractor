using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace BallExtractor
{
    public partial class ShowBallForm : Form
    {
        Rectangle sourceRect;

        public ShowBallForm()
        {
            InitializeComponent();
        }

        public void Build(Rectangle rect, Image sourceImage)
        {
            sourceRect = rect;
            BackgroundImage = new Bitmap(rect.Width, rect.Height);
            rect.Offset(-rect.Left, -rect.Top);
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(rect);
                using (var region = new Region(path))
                {
                    using (var g = Graphics.FromImage(BackgroundImage))
                    {
                        g.SetClip(region, CombineMode.Intersect);
                        g.DrawImage(sourceImage, rect, sourceRect, GraphicsUnit.Pixel);
                        g.ResetClip();
                    }
                }
            }
        }

        private void ShowBallForm_MouseClick(object sender, MouseEventArgs e)
        {
            saveFileDialog1.InitialDirectory = Path.ChangeExtension(Application.ExecutablePath, ".png");
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                BackgroundImage.Save(saveFileDialog1.FileName, ImageFormat.Png);
        }
    }
}
