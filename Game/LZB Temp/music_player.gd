extends Node

@export var music_1: AudioStreamPlayer
@export var music_2: AudioStreamPlayer
@export var transition_duration: float = 0.5

#func pick_player():
	#var selectedplayer
	#if music_1.playing == false:
		#selectedplayer = music_1
	#elif music_2.playing == false:
		#selectedplayer = music_2
		#return selectedplayer

func transition_songs(starting_player: AudioStreamPlayer, stopping_player: AudioStreamPlayer):
	var tween1:= create_tween()
	var tween2:= create_tween()
	tween1.set_trans(Tween.TRANS_SINE).set_ease(Tween.EASE_OUT)
	tween1.tween_property(starting_player, "volume_db", 0.0, transition_duration)
	tween2.set_trans(Tween.TRANS_CUBIC).set_ease(Tween.EASE_IN)
	tween2.tween_property(stopping_player, "volume_db", -80.0, transition_duration)


func _on_song_1_button_down() -> void:
	transition_songs(music_1, music_2)


func _on_song_2_button_down() -> void:
	transition_songs(music_2, music_1)
