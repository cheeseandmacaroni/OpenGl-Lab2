using System;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;


namespace OpenGLFirst
{
    public class MyScene : GameWindow
    {
        Matrix4 view;
        Matrix4 projection;
        Matrix4 model;

        Vector3 LightPos;
        Vector3 LightColor;
        Vector3 LightDir;
        float LightCutOff;
        float LightOuterCutOff;
        float ElapsedTime;
        float Period;

        Hammer hammer;

        public MyScene(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState input = Keyboard.GetState();
            
            if (input.IsKeyDown(Key.Escape))
            {
                Close();
            }
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            ElapsedTime += (float)e.Time;
            ElapsedTime %= Period;
            model *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(ElapsedTime < Period / 3 ? -2f : 1f));
            hammer.draw(ref view, ref projection, ref model, LightPos, LightDir, LightColor * 0.2f, LightColor * 0.8f, LightColor, LightCutOff, LightOuterCutOff); 
            SwapBuffers();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            view = Matrix4.CreateTranslation(-1.5f, -2.5f, 0.0f); //move 'camera'
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Width / Height, 0.1f, 100.0f); //create perspective FOV
            model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-90f)) * Matrix4.CreateTranslation(0.0f, 0.5f, -8.0f); //move object around the scene

            LightColor = new Vector3(1.0f, 1.0f, 1.0f);
            LightPos = new Vector3(1.5f, 2.5f, 0.0f);
            LightDir = new Vector3(0.0f, 0.0f, -1.0f);
            LightCutOff = (float)Math.Cos(MathHelper.DegreesToRadians(15));
            LightOuterCutOff = (float)Math.Cos(MathHelper.DegreesToRadians(18));
            hammer = new Hammer(ref view, ref projection, ref model);

            ElapsedTime = 0f;
            Period = 1.5f;

            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.Enable(EnableCap.DepthTest);
        }
        protected override void OnUnload(EventArgs e)
        {
            hammer.Delete();
            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }
        

        //////////////////////////////////////////////////////////////////////////////////

        static void Main()
        {
            MyScene scene = new MyScene(800, 800, "Window");
            scene.Run(60.0);
        }
    }
}

