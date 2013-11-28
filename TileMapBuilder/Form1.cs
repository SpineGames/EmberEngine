using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EmberEngine.TwoD.Rendering;
using TileMapBuilder.Properties;

namespace TileMapBuilder
{
    public partial class Form1 : Form
    {
        TileMap tileMap;
        TileRenderer renderer;
        PointF screenPos;
        float moveSpeed = 8;
        float zoom = 1;

        public Form1()
        {
            InitializeComponent();

            renderer = new TileRenderer(Resources.tileSheet, 5, 5);
            tileMap = new TileMap(renderer, 100, 100, PointF.Empty, 0);

            idd_tile.ImageList = tileMap.GetImages();

            for (int i = -1; i < tileMap.Tiles; i++)
                idd_tile.Items.Add(new ComboBoxExItem("" + i, i));

            idd_tile.SelectedIndex = 0;
            
            //SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void pnl_main_Paint(object sender, PaintEventArgs e)
        {
            tileMap.Position = screenPos;
            tileMap.Scale = new PointF(zoom, zoom);
            tileMap.Paint(this, e);

            if (chk_grid.Checked)
            {
                tileMap.DrawGrid(e, Pens.Gray);
            }
        }

        private void pnl_main_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PointF pos = new PointF(e.X - screenPos.X, e.Y - screenPos.Y);

                int x = (int)(pos.X / (tileMap.TileWidth * tileMap.Scale.X));
                int y = (int)(pos.Y / (tileMap.TileHeight * tileMap.Scale.Y));

                tileMap.SetTile(x, y, int.Parse(idd_tile.Text));

                Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                PointF pos = new PointF(e.X - screenPos.X, e.Y - screenPos.Y);

                int x = (int)((pos.X / tileMap.TileWidth) * tileMap.Scale.X);
                int y = (int)((pos.Y / tileMap.TileHeight) * tileMap.Scale.Y);

                tileMap.SetTile(x, y, -1);

                Invalidate();
            }
        }

        private void idd_tile_ValueChanged(object sender, EventArgs e)
        {
            tsi_image.Image = tileMap.GetTileImage(int.Parse(idd_tile.Text));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Left)
            {
                screenPos.X += moveSpeed;
                e.Handled = true;
            }
            if (e.KeyData == Keys.Right)
            {
                screenPos.X -= moveSpeed;
                e.Handled = true;
            }
            if (e.KeyData == Keys.Up)
            {
                screenPos.Y += moveSpeed;
                e.Handled = true;
            }
            if (e.KeyData == Keys.Down)
            {
                screenPos.Y -= moveSpeed;
                e.Handled = true;
            }
            if (e.KeyData == Keys.Oemplus)
            {
                zoom /= 0.7F;
                e.Handled = true;
            }
            if (e.KeyData == Keys.OemMinus)
            {
                zoom *= 0.7F;
                e.Handled = true;
            }

            if (e.Handled)
                Invalidate();
        }

        private void tsi_image_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (idd_tile.SelectedIndex + 1 >= idd_tile.Items.Count)
                    idd_tile.SelectedIndex = 0;
                else
                    idd_tile.SelectedIndex++;
            }
            else if (e.Button == MouseButtons.Right)
            {

                if (idd_tile.SelectedIndex - 1 < 0)
                    idd_tile.SelectedIndex = idd_tile.Items.Count - 1;
                else
                    idd_tile.SelectedIndex--;
            }
        }

        private void tsi_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Tile map|*.tlmp";
            dialog.DefaultExt = ".tlmp";

            DialogResult result = dialog.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                string fName = dialog.FileName;

                tileMap.Save(fName);
            }
        }

        private void tsi_load_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Tile map|*.tlmp";
            dialog.DefaultExt = ".tlmp";

            DialogResult result = dialog.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                string fName = dialog.FileName;

                tileMap = TileMap.Load(fName, renderer);
            }

            Invalidate();
        }

        private void chk_grid_CheckedChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
