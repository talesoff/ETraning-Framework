
precision highp float; 

// attribute
attribute vec4 position;
attribute vec4 normal;

// uniform
uniform mat4 mvp;
uniform mat4 mv;

uniform vec4 color4;

// varying
varying vec4 v_FrontColor;
varying vec3 v_LD;
varying vec3 v_Normal;
varying vec3 v_LDR;

void main()
{
	mat3 ModelViewIT=mat3( mv[0].xyz,mv[1].xyz,mv[2].xyz );
	v_LD = -(mv * position).xyz;

	//v_Normal = (nmat * normal).xyz;	
	v_Normal = normalize(ModelViewIT * normal.xyz);

	v_LDR = (reflect(-v_LD, v_Normal)).xyz;
	
	v_FrontColor = color4;

	gl_Position = mvp * position;
}
