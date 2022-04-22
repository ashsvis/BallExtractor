using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallExtractor
{
    public partial class ShoBallForm : Form
    {
        Bitmap sourceImage = null;
        Rectangle sourceRect;
        Rectangle destRect;

        public ShoBallForm()
        {
            InitializeComponent();
        }

        public void Build(Rectangle rect, Image image)
        {
            this.sourceRect = rect;
            rect.Offset(-rect.Left, -rect.Top);
            this.destRect = rect;
            this. sourceImage = (Bitmap)image;
        }

        private void ShoBallForm_Paint(object sender, PaintEventArgs e)
        {
            if (sourceImage == null) return;
            var rect = destRect;
            rect.Offset((ClientSize.Width - rect.Width) / 2, (ClientSize.Height - rect.Height) / 2);
            e.Graphics.DrawImage(sourceImage, rect, sourceRect, GraphicsUnit.Pixel);
        }
    }
}
