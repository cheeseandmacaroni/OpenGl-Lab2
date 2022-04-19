using System;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace OpenGLFirst
{
    class MyObject
    {
        int VertexBufferObject;
        int VertexArrayObject;

        Vector3 Ambient;
        Vector3 Diffuse;
        Vector3 Specular;
        float Shine;

        int ViewLoc;
        int ProjLoc;
        int ModelLoc;
        int LightPosLoc;
        int LightDirLoc;
        int LightAmbientLoc;
        int LightDiffuseLoc;
        int LightSpecularLoc;
        int CutOffLoc;
        int OuterCutOffLoc;
        int DiffuseLoc;
        int AmbientLoc;
        int SpecularLoc;
        int ShineLoc;

        private float[] vertices;
        public Shader shader;

        public MyObject(float[] vert, string vertexPath, string fragmentPath, Vector3 ambient, Vector3 diffuse, Vector3 specular, float shine)
        {
            vertices = vert;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shine = shine;
            shader = new Shader(vertexPath, fragmentPath);

            VertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);//vertices
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));//normals
            GL.EnableVertexAttribArray(1);
            shader.Use();

            ViewLoc = getUniformLocation("view");
            ProjLoc = getUniformLocation("projection");
            ModelLoc = getUniformLocation("model");
            LightPosLoc = getUniformLocation("light.position");
            LightDirLoc = getUniformLocation("light.direction");
            LightAmbientLoc = getUniformLocation("light.ambient");
            LightDiffuseLoc = getUniformLocation("light.diffuse");
            LightSpecularLoc = getUniformLocation("light.specular");
            DiffuseLoc = getUniformLocation("material.ambient");
            AmbientLoc = getUniformLocation("material.diffuse");
            SpecularLoc = getUniformLocation("material.specular");
            CutOffLoc = getUniformLocation("light.cutOff");
            OuterCutOffLoc = getUniformLocation("light.outerCutOff");
            ShineLoc = getUniformLocation("material.shininess");
        }
        public void Render(ref Matrix4 view, ref Matrix4 projection, ref Matrix4 model, Vector3 LightPos, Vector3 LightDir, Vector3 LightAmbient, Vector3 LightDiffuse, Vector3 LightSpecular, float LightCutOff, float LightOuterCutOff)
        {
            shader.Use();

            GL.UniformMatrix4(ViewLoc, true, ref view);
            GL.UniformMatrix4(ProjLoc, true, ref projection);
            GL.UniformMatrix4(ModelLoc, true, ref model);

            GL.Uniform3(LightPosLoc, LightPos);
            GL.Uniform3(LightDirLoc, LightDir);
            GL.Uniform3(LightAmbientLoc, LightAmbient);
            GL.Uniform3(LightDiffuseLoc, LightDiffuse);
            GL.Uniform3(LightSpecularLoc, LightSpecular);
            GL.Uniform1(CutOffLoc, LightCutOff);
            GL.Uniform1(OuterCutOffLoc, LightOuterCutOff);

            GL.Uniform3(AmbientLoc, Ambient);
            GL.Uniform3(DiffuseLoc, Diffuse);
            GL.Uniform3(SpecularLoc, Specular);
            GL.Uniform1(ShineLoc, Shine);


            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }
        public void Unload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);

            shader.Dispose();
        }
        public int getUniformLocation(string name)
        {
            return GL.GetUniformLocation(shader.Handle, name);
        }
    }
}
