shader_type spatial;

uniform float height_scale = 0.5;
uniform sampler2D noise;

float wave(vec2 position){
	position += texture(noise, position / 10.0).x * 2.0 - 1.0;
	vec2 wv = 1.0 - abs(sin(position));
	return pow(1.0 - pow(wv.x * wv.y, 0.65), 4.0);
}

float height(vec2 position, float time) {
	float d = wave((position + time) * 0.4) * 0.3;
	d += wave((position - time) * 0.3) * 0.3;
	d += wave((position + time) * 0.5) * 0.2;
	d += wave((position - time) * 0.6) * 0.2;
	return d;
}

// Ran for each vertex
void vertex() {
	vec2 pos = VERTEX.xz;
	float h = height(pos, TIME);
	NORMAL = normalize(vec3(h - height(pos + vec2(0.1, 0.0), TIME), 0.1, h - height(pos + vec2(0.0, 0.1), TIME)));
	VERTEX.y = (h - h / 2.0) * height_scale;
}

// Runs for each pixel on the mesh
void fragment() {
	float fresnel = sqrt(1.0 - dot(NORMAL, VIEW));
	
	RIM = 0.2;
	METALLIC = 0.0;
	ROUGHNESS = 0.01 * (1.0 - fresnel);
	
	ALBEDO = vec3(0.1, 0.3, 0.5) * (1.0 - fresnel);
}