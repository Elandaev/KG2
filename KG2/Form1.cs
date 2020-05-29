using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace KG2
{
    public partial class Form1 : Form
    {
        
        Bin bin;
        View view;
        bool loaded = false;
        int currentLayer = 0;
        int current_Min = 1;
        int current_Width = 2000;
        int FrameCount = 0;
        
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        public Form1()
        {
            InitializeComponent();
        }
        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl.IsIdle)
            {
                displayFPS();
                glControl.Invalidate();
            }
        }
        void displayFPS()
        {
            if (DateTime.Now >= NextFPSUpdate)
            {
                this.Text = String.Format("Visualization performance (fps={0})", FrameCount);
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }
       
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = dialog.FileName;
                bin.readBIN(str);
                view.SetupView(glControl.Width, glControl.Height);
                loaded = true;
                glControl.Invalidate();
                trackBar1.Maximum = Bin.Z - 1;
            }
        }
        bool needReload = false;
        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                if (checkBox.Checked)
                {
                    if (needReload)
                    {
                        view.generateTextureImage(current_Min, current_Width, currentLayer);
                        view.Load2DTexture();
                        needReload = false;
                    }
                    view.DrawTexture();
                }
                else
                view.DrawQuads(current_Min, current_Width, currentLayer);
                glControl.SwapBuffers();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
       
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            needReload = true;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            current_Min = trackBar2.Value;
            needReload = true;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            current_Width = trackBar3.Value;
            needReload = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }
    }
}
