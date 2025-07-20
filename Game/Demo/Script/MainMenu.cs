using Game;
using Godot;

public partial class MainMenu : Control
{
	[Export] public TextureButton PlayButton;
	[Export] public TextureButton QuitButton;
	[Export] public TextureButton TeamButton;
	[Export] public TextureButton StartButton;

	[Export] public AnimationPlayer TutorialAnimations;
	
	private LevelManager levelManager;

	public override void _Ready()
	{
		Mouse.SetVisible();
		var jukebox = GetNode<Node>("/root/JukeBox");
		jukebox.Call("stop_music");
		PlayButton.Pressed += () => TutorialAnimations.Play("open");
		QuitButton.Pressed += () => GetTree().Quit();
		TeamButton.Pressed += () => OS.ShellOpen("https://team-happy-cat.itch.io/");
		StartButton.Pressed += StartGame;
		
		levelManager = GetNode<LevelManager>("/root/LevelManager");
	}

	private void StartGame()
	{
		levelManager.ChangeLevel("Level_00", "SP_Level_00");
		var jukebox = GetNode<Node>("/root/JukeBox");
		jukebox.Call("play_level1_song");
	}
} 
