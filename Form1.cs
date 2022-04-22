using System;
using System.Drawing;
using System.Windows.Forms;

namespace BallExtractor
{
    public partial class Form1 : Form
    {
        ProgramPhase phase = ProgramPhase.SelectCenter;

        Point center = Point.Empty;
        Size radius = Size.Empty;
        Point nextColumnCenter = Point.Empty;
        int columnOffset = 0;
        Point nextRowCenter = Point.Empty;
        int rowOffset = 0;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Cursor = Cursors.Cross;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (phase)
                {
                    case ProgramPhase.SelectCenter:
                        center = e.Location;
                        phase = ProgramPhase.SelectRadius;
                        break;
                    case ProgramPhase.SelectRadius:
                        var max = (int)Math.Sqrt((center.X - e.X) * (center.X - e.X) + (center.Y - e.Y) * (center.Y - e.Y));
                        radius = new Size(max, max);
                        phase = ProgramPhase.ShowResult;
                        break;
                    case ProgramPhase.ShowResult:

                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                switch (phase)
                {
                    case ProgramPhase.SelectRadius:
                        phase = ProgramPhase.SelectCenter;
                        center = Point.Empty;
                        break;
                    case ProgramPhase.ShowResult:
                        phase = ProgramPhase.SelectRadius;
                        nextRowCenter = Point.Empty;
                        rowOffset = 0;
                        break;
                }
            }
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            switch (phase)
            {
                case ProgramPhase.SelectRadius:
                    if (!center.IsEmpty)
                    {
                        DrawCenter(e.Graphics, center);
                        DrawRadius(e.Graphics, center, PointToClient(MousePosition));
                    }
                    break;
                case ProgramPhase.ShowResult:
                    DrawCircle(e.Graphics, center, radius);
                    break;
            }
        }

        private void DrawCircle(Graphics graphics, Point center, Size radius)
        {
            var rect = new Rectangle(center.X - radius.Width, center.Y - radius.Height, radius.Width * 2, radius.Height * 2);
            graphics.DrawEllipse(Pens.Magenta, rect);
        }

        private void DrawRadius(Graphics graphics, Point center, Point offset)
        {
            graphics.DrawLine(Pens.Magenta, center, offset);
        }

        private void DrawCenter(Graphics graphics, Point center)
        {
            var rect = new Rectangle(center.X - 3, center.Y - 3, 7, 7);
            graphics.FillEllipse(Brushes.Magenta, rect);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    switch (phase)
                    {
                        case ProgramPhase.ShowResult:
                            if (e.Modifiers == Keys.None)
                                center.Offset(0, -1);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                radius.Height--;
                                radius.Width--;
                            }
                            break;
                    }
                    break;
                case Keys.Down:
                    switch (phase)
                    {
                        case ProgramPhase.ShowResult:
                            if (e.Modifiers == Keys.None)
                                center.Offset(0, 1);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                radius.Height++;
                                radius.Width++;
                            }
                            break;
                    }
                    break;
                case Keys.Left:
                    switch (phase)
                    {
                        case ProgramPhase.ShowResult:
                            if (e.Modifiers == Keys.None)
                                center.Offset(-1, 0);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                radius.Height--;
                                radius.Width--;
                            }
                            break;
                    }
                    break;
                case Keys.Right:
                    switch (phase)
                    {
                        case ProgramPhase.ShowResult:
                            if (e.Modifiers == Keys.None)
                                center.Offset(1, 0);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                radius.Height++;
                                radius.Width++;
                            }
                            break;
                    }
                    break;
            }
            Invalidate();
        }
    }
}
