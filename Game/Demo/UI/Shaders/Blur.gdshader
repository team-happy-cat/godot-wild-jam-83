shader_type canvas_item;

uniform sampler2D screen_texture: hint_screen_texture, filter_linear_mipmap_anisotropic;
uniform float amount = 0.0;

void fragment() {
	COLOR = COLOR * textureLod(screen_texture, UV, amount);
}