shader_type canvas_item;

uniform sampler2D mask;

void fragment() {
	COLOR.a = texture(mask, UV).r;
}