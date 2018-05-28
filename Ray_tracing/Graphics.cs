using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;

namespace Ray_tracing
{
    class Graphics
    {

      //  Vector3 cameraPosition = new Vector3(2, 3, 4);
        //Vector3 cameraDirection = new Vector3(0, 0, 0);
        //Vector3 cameraUp = new Vector3(0, 0, 1); 

       // string glVersion = GL.GetString(StringName.Version);
       // string glslVersion = GL.GetString(StringName.ShadingLanguageVersion);

      Vector3 cameraPosition = new Vector3(0, 0, 1.5f);
        Vector3 cameraDirecton = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 1, 0);

        int vertexbuffer;
        float[] vertdata = { -1f, -1f, 0.0f, -1f, 1f, 0.0f, 1f, -1f, 0.0f, 1f, 1f, 0f };

        int width, height;
        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;

        int vaoHandle;

        public Graphics()
        {
          
        }

        public void Draw()
        {
           

            GL.UseProgram(BasicProgramID);

            GL.Uniform3(GL.GetUniformLocation(BasicProgramID, "campos"), cameraPosition);
            GL.Uniform1(GL.GetUniformLocation(BasicProgramID, "aspect"), width / (float)height);




            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            //glControl1.SwapBuffers();
            GL.UseProgram(0);
        }

        public void Render()
        {
            Draw();
        }

        public void Update()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirecton, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview); //указали текущую матрицу
            GL.LoadMatrix(ref viewMat); // передать матрицу по ссылке

            Render();
        } 

      /*  public void Update()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            cameraPosition = new Vector3(
                (float)(radius * Math.Cos(Math.PI / 180.0f * latitude) * Math.Cos(Math.PI / 180.0f * longitude)),
                (float)(radius * Math.Cos(Math.PI / 180.0f * latitude) * Math.Sin(Math.PI / 180.0f * longitude)),
                (float)(radius * Math.Sin(Math.PI / 180.0f * latitude)));

            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirecton, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);

            Render();
        } */

        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (System.IO.StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));
            string st = GL.GetProgramInfoLog(BasicProgramID);
        }

        private void InitShaders()
        {
            // создание объекта программы
            BasicProgramID = GL.CreateProgram();
            loadShader("C:\\Users\\ann\\Documents\\Visual Studio 2012\\Projects\\Ray_tracing\\Ray_tracing\\raytracing.vert.txt", ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
            loadShader("C:\\Users\\ann\\Documents\\Visual Studio 2012\\Projects\\Ray_tracing\\Ray_tracing\\raytracing.frag.2.txt", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);
            //Линковка программы
            GL.LinkProgram(BasicProgramID);

            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));



            GL.GenBuffers(1, out vertexbuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                          (IntPtr)(sizeof(float) * vertdata.Length),
                           vertdata, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            /*
            float[] positionData = { -0.8f, -0.8f, 0.0f, 0.8f, -0.8f, 0.0f, 0.0f, 0.8f, 0.0f };
            float[] colorData = { 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f };
            //  float[] colorData = { 255.0f, 0.0f, 0.0f, 233.0f, 150.0f, 122.0f, 250.0f, 150.0f, 122.0f }
            int[] vboHandlers = new int[2];
            GL.GenBuffers(2, vboHandlers);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                          (IntPtr)(sizeof(float) * positionData.Length),
                           positionData, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                         (IntPtr)(sizeof(float) * colorData.Length),
                          colorData, BufferUsageHint.StaticDraw);
            vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            */
        }

        public void Resize(int _width, int _height)
        {
            width = _width;
            height = _height;
            GL.ClearColor(Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);
            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, width / (float)height, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);
            InitShaders();
        }
       

    }
}
