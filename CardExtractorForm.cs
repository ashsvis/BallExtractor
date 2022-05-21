using System;
using System.Drawing;
using System.Windows.Forms;

namespace BallExtractor
{
    public partial class CardExtractorForm : Form
    {
        ProgramCardPhase phase = ProgramCardPhase.SelectTopLeft;

        Point topleft = Point.Empty;
        Size diagonal = Size.Empty;
        Size step = Size.Empty;

        public CardExtractorForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Cursor = Cursors.Cross;
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (phase)
                {
                    case ProgramCardPhase.SelectTopLeft:
                        topleft = e.Location;
                        phase = ProgramCardPhase.SelectDiagonal;
                        break;
                    case ProgramCardPhase.SelectDiagonal:
                        diagonal = new Size(e.X - topleft.X, e.Y - topleft.Y);
                        phase = ProgramCardPhase.SelectNextColumn;
                        break;
                    case ProgramCardPhase.SelectNextColumn:
                        step = new Size(e.X - topleft.X, 0);
                        phase = ProgramCardPhase.SelectNextRow;
                        break;
                    case ProgramCardPhase.SelectNextRow:
                        step = new Size(step.Width, e.Y - topleft.Y);
                        phase = ProgramCardPhase.ShowResult;
                        break;
                    case ProgramCardPhase.ShowResult:
                        var frm = new ShowBallForm();
                        var rect = new Rectangle(topleft.X, topleft.Y, diagonal.Width, diagonal.Height);
                        frm.Build(rect, BackgroundImage);
                        frm.ShowDialog(this);
                        phase = ProgramCardPhase.SelectTopLeft;
                        topleft = Point.Empty;
                        step = Size.Empty;
                        break;
                }
                Text = $"{phase} location:{topleft} size:{diagonal} step:{step}";
            }
            else if (e.Button == MouseButtons.Right)
            {
                switch (phase)
                {
                    case ProgramCardPhase.SelectDiagonal:
                    case ProgramCardPhase.ShowResult:
                        phase = ProgramCardPhase.SelectTopLeft;
                        topleft = Point.Empty;
                        break;
                    case ProgramCardPhase.SelectNextRow:
                        phase = ProgramCardPhase.SelectNextColumn;
                        break;
                    case ProgramCardPhase.SelectNextColumn:
                        phase = ProgramCardPhase.SelectDiagonal;
                        break;
                }
                Text = $"{phase} location:{topleft} size:{diagonal} step:{step}";
            }
            Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r;
            switch (phase)
            {
                case ProgramCardPhase.SelectDiagonal:
                    if (!topleft.IsEmpty)
                    {
                        DrawTopLeft(e.Graphics, topleft);
                        DrawDiagonal(e.Graphics, topleft, PointToClient(MousePosition));
                    }
                    break;
                case ProgramCardPhase.SelectNextColumn:
                    DrawRect(e.Graphics, topleft, diagonal);
                    break;
                case ProgramCardPhase.SelectNextRow:
                    r = new Rectangle(topleft, diagonal);
                    for (var c = 0; c < 13; c++)
                    {
                        DrawRect(e.Graphics, r.Location, r.Size);
                        r.Offset(step.Width, 0);
                    }
                    break;
                case ProgramCardPhase.ShowResult:
                    r = new Rectangle(topleft, diagonal);
                    for (var i = 0; i < 4; i++)
                    {
                        for (var c = 0; c < 13; c++)
                        {
                            DrawRect(e.Graphics, r.Location, r.Size);
                            r.Offset(step.Width, 0);
                        }
                        r.X = topleft.X;
                        r.Offset(0, step.Height);
                    }
                    break;
            }
        }

        private void DrawRect(Graphics graphics, Point topleft, Size diagonal)
        {
            var rect = new Rectangle(topleft.X, topleft.Y, diagonal.Width, diagonal.Height);
            graphics.DrawRectangle(Pens.Magenta, rect);
        }

        private void DrawDiagonal(Graphics graphics, Point center, Point offset)
        {
            graphics.DrawLine(Pens.Magenta, center, offset);
        }

        private void DrawTopLeft(Graphics graphics, Point center)
        {
            var rect = new Rectangle(center.X - 3, center.Y - 3, 7, 7);
            graphics.FillEllipse(Brushes.Magenta, rect);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            Invalidate();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    switch (phase)
                    {
                        case ProgramCardPhase.SelectNextRow:
                            if (e.Modifiers == Keys.None)
                                topleft.Offset(0, -1);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                diagonal.Height--;
                            }
                            else if (e.Modifiers.HasFlag(Keys.Alt))
                            {
                                step.Width--;
                            }
                            break;
                        case ProgramCardPhase.ShowResult:
                            if (e.Modifiers == Keys.None)
                                topleft.Offset(0, -1);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                diagonal.Height--;
                            }
                            else if (e.Modifiers.HasFlag(Keys.Alt))
                            {
                                step.Height--;
                            }
                            break;
                    }
                    break;
                case Keys.Down:
                    switch (phase)
                    {
                        case ProgramCardPhase.SelectNextRow:
                            if (e.Modifiers == Keys.None)
                                topleft.Offset(0, 1);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                diagonal.Height++;
                            }
                            else if (e.Modifiers.HasFlag(Keys.Alt))
                            {
                                step.Height++;
                            }
                            break;
                        case ProgramCardPhase.ShowResult:
                            if (e.Modifiers == Keys.None)
                                topleft.Offset(0, 1);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                diagonal.Height++;
                            }
                            else if (e.Modifiers.HasFlag(Keys.Alt))
                            {
                                step.Height++;
                            }
                            break;
                    }
                    break;
                case Keys.Left:
                    switch (phase)
                    {
                        case ProgramCardPhase.SelectNextRow:
                            if (e.Modifiers == Keys.None)
                                topleft.Offset(-1, 0);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                diagonal.Width--;
                            }
                            else if (e.Modifiers.HasFlag(Keys.Alt))
                            {
                                step.Width--;
                            }
                            break;
                        case ProgramCardPhase.ShowResult:
                            if (e.Modifiers == Keys.None)
                                topleft.Offset(-1, 0);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                diagonal.Width--;
                            }
                            else if (e.Modifiers.HasFlag(Keys.Alt))
                            {
                                step.Width--;
                            }
                            break;
                    }
                    break;
                case Keys.Right:
                    switch (phase)
                    {
                        case ProgramCardPhase.SelectNextRow:
                            if (e.Modifiers == Keys.None)
                                topleft.Offset(1, 0);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                diagonal.Width++;
                            }
                            else if (e.Modifiers.HasFlag(Keys.Alt))
                            {
                                step.Width++;
                            }
                            break;
                        case ProgramCardPhase.ShowResult:
                            if (e.Modifiers == Keys.None)
                                topleft.Offset(1, 0);
                            else if (e.Modifiers.HasFlag(Keys.Control))
                            {
                                diagonal.Width++;
                            }
                            else if (e.Modifiers.HasFlag(Keys.Alt))
                            {
                                step.Width++;
                            }
                            break;
                    }
                    break;
            }
            Text = $"{phase} location:{topleft} size:{diagonal} step:{step}";
            Invalidate();
        }
    }
}
