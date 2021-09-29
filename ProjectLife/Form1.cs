using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConsoleProjectLife
{
    public partial class Form1 : Form
    {
        private Graphics _graphics;
        private int _resolution;
        private GameEngine _engine;
        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer1.Enabled)
            {
                return;
            }

            numericUpDownDensity.Enabled = false;
            numericUpDownResolution.Enabled = false;
            _resolution = (int)numericUpDownResolution.Value;
            _engine = new GameEngine
                (
                    rows: pictureBox1.Height / _resolution,
                    columns: pictureBox1.Width / _resolution,
                    density: (int)numericUpDownDensity.Minimum + (int)numericUpDownDensity.Maximum - (int)numericUpDownDensity.Value
                );

            Text = $"Generation {_engine.CurrentGeneration}";
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(pictureBox1.Image);

            timer1.Start();
        }

        private void StopGame()
        {
            if (!timer1.Enabled)
            {
                return;
            }
            timer1.Stop();
            numericUpDownDensity.Enabled = true;
            numericUpDownResolution.Enabled = true;
        }
        private void DrawNextGeneration()
        {
            _graphics.Clear(BackColor);

            var field = _engine.GetCurrentGeneration();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                    {
                        _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution - 1, _resolution - 1);
                    }
                }
            }
            pictureBox1.Refresh();
            Text = $"Generation {_engine.CurrentGeneration}";
            _engine.NextGeneration();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / _resolution;
                var y = e.Location.Y / _resolution;
                _engine.AddCell(x, y);

            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / _resolution;
                var y = e.Location.Y / _resolution;
                _engine.DeleteCell(x, y);
            }
        }
    }
}
