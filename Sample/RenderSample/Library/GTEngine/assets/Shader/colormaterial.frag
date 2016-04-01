
precision highp float; 

// varying
varying vec4 v_FrontColor;
varying vec3 v_LD;
varying vec3 v_Normal;
varying vec3 v_LDR;

uniform vec4 ambient; 
uniform vec4 diffuse; 
uniform vec4 specular; 


void main()
{

	vec3 LDN = normalize(v_LD);	
	vec3 NormalN = normalize(v_Normal);
	vec3 LDRN = normalize(v_LDR);
	
	float NdotLD = max(dot(NormalN, LDN), 0.0);
	float EVdotLDR = pow(max(dot(LDN, LDRN), 0.0), 32.0);	
		
	gl_FragColor = vec4(ambient.rgb + diffuse.rgb * NdotLD * v_FrontColor.rgb+vec3(EVdotLDR)*specular.rgb, diffuse.a);

}
