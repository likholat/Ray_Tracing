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
//using OpenTK.Graphics.OpenGL;



namespace Ray_tracing
{
    public partial class Form1 : Form
    {
        Shaders sh;
        Graphics gr;
        RayTracing rt;

        public Form1()
        {
            InitializeComponent();
            sh = new Shaders();
            gr = new Graphics();
            //rt = new RayTracing();
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {
            gr.Resize(glControl1.Width, glControl1.Height);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            gr.Update();
            glControl1.SwapBuffers();
            
        }
    }
}
