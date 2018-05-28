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
    class Shaders
    {
        Vector3 cameraPosition = new Vector3(2, 3, 4); 
        Vector3 cameraDirecton = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);

        //string glVersion = GL.GetString(StringName.Version);
        //string glslVersion = GL.GetString(StringName.ShadingLanguageVersion);
        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;

        int[] vboHandlers = new int[2];
        int vaoHandle;

        void loadShader(String filename, ShaderType type, int program, out int address)     // возвращает значение,которое можно использовать  для ссылки на объект вершинного шейдера (дескриптор)
        {
            address = GL.CreateShader(type); // создает объект шейдера, аргумент определяет тип шейдера 
            using (System.IO.StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        private void InitShaders() // инициализация шейдерной программы
        {

            // Создание объекта программы    
            BasicProgramID = GL.CreateProgram();
            loadShader("C:\\Users\\ann\\Documents\\Visual Studio 2012\\Projects\\Ray_tracing\\Ray_tracing\\vs.txt", ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
            loadShader("C:\\Users\\ann\\Documents\\Visual Studio 2012\\Projects\\Ray_tracing\\Ray_tracing\\fs.txt", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);

            //Компановка программы  
            GL.LinkProgram(BasicProgramID);

            // Проверить успех компановки  
            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);

            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));


            float[] positionData = { -0.8f, -0.8f, 0.0f, 0.8f, -0.8f, 0.0f, 0.0f, 0.8f, 0.0f };
            float[] colorData = { 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f };


            GL.GenBuffers(2, vboHandlers);  // дескрипторы буферных объектов сохраняются в массиве

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);  //Каждый буферный объект связывается с точкой привязки GL_ARRAY_BUFFER (BufferTarget.ArrayBuffer) 

            GL.BufferData(BufferTarget.ArrayBuffer,
                    (IntPtr)(sizeof(float) * positionData.Length),
                    positionData, BufferUsageHint.StaticDraw);  // glBufferData выделяет память под данные

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);

            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(sizeof(float) * colorData.Length),
                colorData, BufferUsageHint.StaticDraw);

            vaoHandle = GL.GenVertexArray();  //VAO содержит информацию о связях между данными в буферах и входными атрибутами вершин.
            GL.BindVertexArray(vaoHandle);

            // Активация атрибутов вершин с индексами 1 и 0 

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0); 

        }

        void Draw()
        {
            InitShaders();
            GL.UseProgram(BasicProgramID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            // openGlControl.SwapBuffers();     
            GL.UseProgram(0);
        }

        public void Resize(int width, int height)
        {

            GL.ClearColor(Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);
            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, width / (float)height, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);

        }

        public void Update() { 
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirecton, cameraUp); 
            GL.MatrixMode(MatrixMode.Modelview); 
            GL.LoadMatrix(ref viewMat);
            Render();
        }

 /*       private void drawTestTriangle() {
          
            GL.Begin(PrimitiveType.Polygon); 
            GL.Color3(Color.Blue); 
            GL.Vertex3(-1.0f, -1.0f, -1.0f); 
            GL.Color3(Color.Red); 
            GL.Vertex3(-1.0f, 1.0f, -1.0f); 
            GL.Color3(Color.White); 
            GL.Vertex3(1.0f, 1.0f, -1.0f); 
      
            GL.End(); 
  * }  */

        public void Render()
        {
            Draw();
        } 
    }
}
