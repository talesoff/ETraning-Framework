
precision highp float; 

// varying
varying vec4 v_FrontColor;
varying vec3 v_LD;
varying vec3 v_Normal;
varying vec3 v_LDR;
varying vec4 texcoord0;

uniform sampler2D tex0;
uniform vec4 diffuse; 


void main()
{
	vec3 LDN = normalize(v_LD);	
	vec3 NormalN = normalize(v_Normal);
	vec3 LDRN = normalize(v_LDR);
	
	float NdotLD = max(dot(NormalN, LDN), 0.0);
	float EVdotLDR = pow(max(dot(LDN, LDRN), 0.0), 32.0);
	vec4 texcolor = texture2D(tex0, texcoord0.xy);
		
	gl_FragColor = vec4(diffuse.rgb * texcolor.rgb * NdotLD * v_FrontColor.rgb+vec3(EVdotLDR), texcolor.a);
		
}
