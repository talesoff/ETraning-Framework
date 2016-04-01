program ppColorMaterialProg  Shader/colormaterial.vert Shader/colormaterial.frag
link ppColorMaterialProg


uniform_mat4 mvp 				1.0 0.0 0.0 0.0     0.0 1.0 0.0 0.0     0.0 0.0 1.0 0.0       0.0 0.0 0.0 1.0
uniform_mat4 mv  				1.0 0.0 0.0 0.0     0.0 1.0 0.0 0.0     0.0 0.0 1.0 0.0       0.0 0.0 0.0 1.0

uniform_vec4 color4  			1  1  1  1
uniform_vec4 ambient			1  1  1  1
uniform_vec4 diffuse			1  1  1  1
uniform_vec4 specular			1  1  1  1

attribute position 0
attribute color    1
attribute texcoord 2
attribute normal   3

link ppColorMaterialProg
