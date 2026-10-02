shader_type spatial;
render_mode blend_mix, depth_draw_opaque, depth_test_default, cull_back, diffuse_toon, specular_toon;


// Varyings
varying bool var_impactFrame;

global uniform bool impactFrame;
uniform sampler2D Albedo;
uniform sampler2D tex_frg_14;
uniform sampler2D Emission;
uniform float Emissiveness = 1.0;



void fragment() {
// BooleanParameter:4
	bool n_out4p0 = impactFrame;


	vec4 n_out6p0;
// Texture2D:6
	n_out6p0 = texture(Albedo, UV);


// ColorConstant:17
	vec4 n_out17p0 = vec4(0.3, 0.039702, 0.05, 1.000000);


// ColorConstant:16
	vec4 n_out16p0 = vec4(0.8, 0.066667, 0.08, 1.000000);


// Input:18
	vec2 n_out18p0 = SCREEN_UV;


// Texture2D:14
	vec4 n_out14p0 = texture(tex_frg_14, n_out18p0);


// Mix:15
	vec3 n_out15p0 = mix(vec3(n_out17p0.xyz), vec3(n_out16p0.xyz), vec3(n_out14p0.xyz));


	vec3 n_out3p0;
// If:3
	float n_in3p1 = 0.01000;
	float n_in3p2 = 0.01000;
	if(abs((n_out4p0 ? 1.0 : 0.0) - n_in3p1) < n_in3p2)
	{
		n_out3p0 = vec3(n_out6p0.xyz);
	}
	else if((n_out4p0 ? 1.0 : 0.0) < n_in3p1)
	{
		n_out3p0 = vec3(n_out6p0.xyz);
	}
	else
	{
		n_out3p0 = n_out15p0;
	}


// FloatConstant:12
	float n_out12p0 = 0.041000;


// FloatConstant:13
	float n_out13p0 = 1.000000;


	vec4 n_out8p0;
// Texture2D:8
	n_out8p0 = texture(Emission, UV);


// FloatParameter:21
	float n_out21p0 = Emissiveness;


// VectorOp:20
	vec4 n_out20p0 = n_out8p0 * vec4(n_out21p0);


	vec3 n_out11p0;
// If:11
	float n_in11p1 = 0.01000;
	float n_in11p2 = 0.01000;
	if(abs((n_out4p0 ? 1.0 : 0.0) - n_in11p1) < n_in11p2)
	{
		n_out11p0 = vec3(n_out20p0.xyz);
	}
	else if((n_out4p0 ? 1.0 : 0.0) < n_in11p1)
	{
		n_out11p0 = vec3(n_out20p0.xyz);
	}
	else
	{
		n_out11p0 = n_out15p0;
	}


// Output:0
	ALBEDO = n_out3p0;
	METALLIC = n_out12p0;
	ROUGHNESS = n_out13p0;
	SPECULAR = n_out12p0;
	EMISSION = n_out11p0;


// VaryingSetter:19
	var_impactFrame = n_out4p0;
}

void light() {
// ty binbun!

		float light = max(dot(NORMAL, LIGHT), 0.0) * ATTENUATION;

	light = clamp(smoothstep(0.02,.15, light),.01,.9); // Here
	
	vec3 diffuse = vec3(light) * LIGHT_COLOR / PI;
	
		bool n_out4p0 = impactFrame;
			float n_in11p1 = 0.01000;
	float n_in11p2 = 0.01000;
		if(abs((n_out4p0 ? 1.0 : 0.0) - n_in11p1) < n_in11p2)
	{
		DIFFUSE_LIGHT += vec3(0);	}
	else if((n_out4p0 ? 1.0 : 0.0) < n_in11p1)
	{

					DIFFUSE_LIGHT += diffuse;


	}
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
}
