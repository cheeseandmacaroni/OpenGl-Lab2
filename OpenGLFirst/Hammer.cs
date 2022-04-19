using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using OpenTK.Input;

namespace OpenGLFirst
{
    class Hammer
    {
        float[] GripVertices;
        float[] HeadVertices = {
            // Position                        Normal
            -0.5f, -0.5f, -0.5f + headOffset,  0.0f,  0.0f, -1.0f, // Front face
             0.5f, -0.5f, -0.5f + headOffset,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f + headOffset,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f + headOffset,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f + headOffset,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f + headOffset,  0.0f,  0.0f, -1.0f,
                                             
            -0.5f, -0.5f,  0.5f + headOffset,  0.0f,  0.0f,  1.0f, // Back face
             0.5f, -0.5f,  0.5f + headOffset,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f + headOffset,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f + headOffset,  0.0f,  0.0f,  1.0f,
            -0.5f,  0.5f,  0.5f + headOffset,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f + headOffset,  0.0f,  0.0f,  1.0f,
                                             
            -0.5f,  0.5f,  0.5f + headOffset, -1.0f,  0.0f,  0.0f, // Left face
            -0.5f,  0.5f, -0.5f + headOffset, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f + headOffset, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f + headOffset, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f + headOffset, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f + headOffset, -1.0f,  0.0f,  0.0f,
                                             
             0.5f,  0.5f,  0.5f + headOffset,  1.0f,  0.0f,  0.0f, // Right face
             0.5f,  0.5f, -0.5f + headOffset,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f + headOffset,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f + headOffset,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f + headOffset,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f + headOffset,  1.0f,  0.0f,  0.0f,
                                             
            -0.5f, -0.5f, -0.5f + headOffset,  0.0f, -1.0f,  0.0f, // Bottom face
             0.5f, -0.5f, -0.5f + headOffset,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f + headOffset,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f + headOffset,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f + headOffset,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f + headOffset,  0.0f, -1.0f,  0.0f,
                                             
            -0.5f,  0.5f, -0.5f + headOffset,  0.0f,  1.0f,  0.0f, // Top face
             0.5f,  0.5f, -0.5f + headOffset,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f + headOffset,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f + headOffset,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f + headOffset,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f + headOffset,  0.0f,  1.0f,  0.0f
            };
        
        MyObject Grip;
        MyObject Head;
        static Vector3 HeadColor = new Vector3(0.3f, 0.3f, 0.3f);

        static float headOffset = 5.4f;

        public Hammer(ref Matrix4 view, ref Matrix4 projection,ref Matrix4 model)
        {
            Vector3 headScale = new Vector3(1.5f, 0.5f, 0.5f);
            for(int i = 0; i < 36; ++i) //scale the head
            {
                HeadVertices[6 * i] *= headScale[0];
                HeadVertices[6 * i + 1] *= headScale[1];
                HeadVertices[6 * i + 2] *= headScale[2];
            }

            Cylinder cylinder = new Cylinder(0.2f, 3f, 0.0f, 20);
            GripVertices = cylinder.vertices;

            Grip = new MyObject(GripVertices, "../../../Shader.vert", "../../../Shader.frag", new Vector3(0.2f, 0.1f, 0f), new Vector3(0.2f, 0.1f, 0f), new Vector3(0.2f, 0.1f, 0f), 1f);
            Head = new MyObject(HeadVertices, "../../../Shader.vert", "../../../Shader.frag", new Vector3(0.25f, 0.25f, 0.25f), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.774597f, 0.774597f, 0.774597f), 76.8f);
        }
        public void draw(ref Matrix4 view, ref Matrix4 projection, ref Matrix4 model, Vector3 LightPos, Vector3 LightDir, Vector3 LightAmbient, Vector3 LightDiffuse, Vector3 LightSpecular, float LightCutOff, float LightOuterCutOff)
        {
            Grip.Render(ref view, ref projection, ref model, LightPos, LightDir, LightAmbient, LightDiffuse, LightSpecular, LightCutOff, LightOuterCutOff);
            Head.Render(ref view, ref projection, ref model, LightPos, LightDir, LightAmbient, LightDiffuse, LightSpecular, LightCutOff, LightOuterCutOff);
        }
        public void Delete()
        {
            Head.Unload();
            Grip.Unload();
        }
    }
}
