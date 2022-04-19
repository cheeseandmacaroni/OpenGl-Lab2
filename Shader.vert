#version 330 core
layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec3 vNormals;

out vec3 normals;
out vec3 fragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position =  vec4(vPosition, 1.0f) * model * view * projection;
	fragPos = vec3(vec4(vPosition, 1.0f) * model);
	normals = vNormals * mat3(transpose(inverse(model)));
}