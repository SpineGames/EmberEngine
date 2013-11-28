using EmberEngine.TwoD.Rendering;
using System.Drawing;
using TileMapBuilder.Properties;
using System.Windows.Forms;
namespace TileMapBuilder
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsi_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsi_save = new System.Windows.Forms.ToolStripMenuItem();
            this.tsi_load = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ts_right = new System.Windows.Forms.ToolStrip();
            this.tsi_image = new System.Windows.Forms.ToolStripButton();
            this.idd_tile = new TileMapBuilder.ImageDropdown();
            this.chk_grid = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.ts_right.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsi_File,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(714, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsi_File
            // 
            this.tsi_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsi_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsi_save,
            this.tsi_load});
            this.tsi_File.Image = ((System.Drawing.Image)(resources.GetObject("tsi_File.Image")));
            this.tsi_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsi_File.Name = "tsi_File";
            this.tsi_File.Size = new System.Drawing.Size(38, 22);
            this.tsi_File.Text = "File";
            // 
            // tsi_save
            // 
            this.tsi_save.Name = "tsi_save";
            this.tsi_save.Size = new System.Drawing.Size(100, 22);
            this.tsi_save.Text = "Save";
            this.tsi_save.Click += new System.EventHandler(this.tsi_save_Click);
            // 
            // tsi_load
            // 
            this.tsi_load.Name = "tsi_load";
            this.tsi_load.Size = new System.Drawing.Size(100, 22);
            this.tsi_load.Text = "Load";
            this.tsi_load.Click += new System.EventHandler(this.tsi_load_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ts_right
            // 
            this.ts_right.AutoSize = false;
            this.ts_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.ts_right.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.ts_right.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsi_image});
            this.ts_right.Location = new System.Drawing.Point(650, 25);
            this.ts_right.Name = "ts_right";
            this.ts_right.Size = new System.Drawing.Size(64, 423);
            this.ts_right.TabIndex = 4;
            this.ts_right.Text = "tools";
            // 
            // tsi_image
            // 
            this.tsi_image.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tsi_image.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsi_image.Image = ((System.Drawing.Image)(resources.GetObject("tsi_image.Image")));
            this.tsi_image.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsi_image.Name = "tsi_image";
            this.tsi_image.Size = new System.Drawing.Size(62, 68);
            this.tsi_image.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tsi_image_MouseDown);
            // 
            // idd_tile
            // 
            this.idd_tile.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.idd_tile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.idd_tile.ImageList = null;
            this.idd_tile.Location = new System.Drawing.Point(594, 0);
            this.idd_tile.Name = "idd_tile";
            this.idd_tile.Size = new System.Drawing.Size(120, 21);
            this.idd_tile.TabIndex = 2;
            this.idd_tile.TabStop = false;
            this.idd_tile.SelectedIndexChanged += new System.EventHandler(this.idd_tile_ValueChanged);
            // 
            // chk_grid
            // 
            this.chk_grid.AutoSize = true;
            this.chk_grid.Location = new System.Drawing.Point(515, 4);
            this.chk_grid.Name = "chk_grid";
            this.chk_grid.Size = new System.Drawing.Size(73, 17);
            this.chk_grid.TabIndex = 5;
            this.chk_grid.Text = "Draw Grid";
            this.chk_grid.UseVisualStyleBackColor = true;
            this.chk_grid.CheckedChanged += new System.EventHandler(this.chk_grid_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 448);
            this.Controls.Add(this.chk_grid);
            this.Controls.Add(this.ts_right);
            this.Controls.Add(this.idd_tile);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.pnl_main_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnl_main_MouseClick);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ts_right.ResumeLayout(false);
            this.ts_right.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageDropdown idd_tile;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton tsi_File;
        private System.Windows.Forms.ToolStripMenuItem tsi_save;
        private System.Windows.Forms.ToolStripMenuItem tsi_load;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private ToolStrip ts_right;
        private ToolStripButton tsi_image;
        private CheckBox chk_grid;
    }
}

