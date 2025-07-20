extends Node

var jukebox_audio_player: AudioStreamPlayer

const LEVEL_1_THEME = preload("res://Assets/Audio/Level 1 Theme Looped shorter.wav")
const ENDING_TUNE = preload("res://Assets/Audio/Ending Tune.wav")
#const GODOT_WILD_JAM_83_MENU = preload("res://Assets/Audio/godot wild jam 83 menu.wav")

func _ready() -> void:
	jukebox_audio_player = AudioStreamPlayer.new()
	add_child(jukebox_audio_player)
	jukebox_audio_player.bus = "Music"
	jukebox_audio_player.process_mode = Node.PROCESS_MODE_ALWAYS

func play_music(stream: AudioStream, volume_db: float = 0.0):
	if jukebox_audio_player.stream == stream and jukebox_audio_player.playing:
		return
	
	jukebox_audio_player.stream = stream
	jukebox_audio_player.volume_db = volume_db
	jukebox_audio_player.play()

func play_level1_song():
	play_music(LEVEL_1_THEME)

## LZB NOTE 20-07-25 - Please leave the menu alone as its fine as is
#func play_menu_song():
	#play_music(GODOT_WILD_JAM_83_MENU)

func play_ending_song():
	play_music(ENDING_TUNE)

func stop_music():
	jukebox_audio_player.stop()
