#version 330 core
in vec3 objectColor;
in vec3 normals;
in vec3 fragPos;
out vec4 outputColor;


struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float shininess;
};

struct Light {
    vec3 position;
	vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

	float cutOff;
    float outerCutOff;
};

uniform Material material;
uniform Light light;

void main()
{

	//ambient
    vec3 ambient = light.ambient * material.ambient; //Remember to use the material here.

	//diffuse
	vec3 norm = normalize(normals);
	vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * (diff * material.diffuse);

	//specular
	vec3 viewPos = light.position;
	vec3 viewDir = normalize(viewPos - fragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = light.specular * (spec * material.specular);

	//spotlight intensity
	float theta     = dot(lightDir, normalize(-light.direction));
    float epsilon   = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0); 

	diffuse *= intensity;
	specular *= intensity;

    vec3 result = ambient + diffuse + specular;
    outputColor = vec4(result, 1.0f);
}