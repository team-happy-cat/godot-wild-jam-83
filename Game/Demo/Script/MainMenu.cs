using Game;
using Godot;

public partial class MainMenu : Control
{
	[Export] public TextureButton PlayButton;
	[Export] public TextureButton QuitButton;
	[Export] public TextureButton TeamButton;
	[Export] public TextureButton StartButton;

	[Export] public AnimationPlayer TutorialAnimations;
	
	// Debug - to be removed for release
	[Export] public Button LevelButton1;
	[Export] public Button LevelButton2;
	[Export] public Button LevelButton3;
	
	private LevelManager levelManager;

	public override void _Ready()
	{
		Mouse.SetVisible();
		
		PlayButton.Pressed += () => TutorialAnimations.Play("open");
		QuitButton.Pressed += () => GetTree().Quit();
		TeamButton.Pressed += () => OS.ShellOpen("https://team-happy-cat.itch.io/");
		StartButton.Pressed += StartGame;
		
		// Debug - to be removed for release
		LevelButton1.Pressed += OnLevelButton1Pressed;
		LevelButton2.Pressed += OnLevelButton2Pressed;
		LevelButton3.Pressed += OnLevelButton3Pressed;
		
		levelManager = GetNode<LevelManager>("/root/LevelManager");
	}

	private void StartGame()
	{
		levelManager.ChangeLevel("Level_01", "SP_Level_01");
	}

	private void OnLevelButton1Pressed()
	{
		levelManager.ChangeLevel("Level_01", "SP_Level_01");
	}
	
	private void OnLevelButton2Pressed()
	{
		levelManager.ChangeLevel("Level_02", "SP_Level_02");
	}
	
	private void OnLevelButton3Pressed()
	{
		levelManager.ChangeLevel("Level_03", "SP_Level_03");
	}
} 
