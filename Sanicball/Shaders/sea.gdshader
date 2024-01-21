shader_type spatial;

uniform sampler2D noise;
uniform sampler2D normalmap;
uniform float height_scale = 0.5;

varying vec2 tex_position;
void vertex() {
	tex_position = VERTEX.xz / 2.0 + 0.5;
	float height = texture(noise, tex_position).x;
	VERTEX.y += height * height_scale;
}

void fragment() {
	NORMAL_MAP = texture(normalmap, tex_position).xyz;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
