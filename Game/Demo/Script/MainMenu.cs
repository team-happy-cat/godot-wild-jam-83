using Game;
using Godot;

public partial class MainMenu : Control
{
	[Export] public Button StartButton;
	
	// Debug - to be removed for release
	[Export] public Button LevelButton1;
	[Export] public Button LevelButton2;
	[Export] public Button LevelButton3;
	
	private LevelManager levelManager;

	public override void _Ready()
	{
		StartButton.Pressed += OnStartButtonPressed;
		
		// Debug - to be removed for release
		LevelButton1.Pressed += OnLevelButton1Pressed;
		LevelButton2.Pressed += OnLevelButton2Pressed;
		LevelButton3.Pressed += OnLevelButton3Pressed;
		
		levelManager = GetNode<LevelManager>("/root/LevelManager");
	}

	private void OnStartButtonPressed()
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
