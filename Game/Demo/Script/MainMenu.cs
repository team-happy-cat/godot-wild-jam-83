using Game;
using Godot;

public partial class MainMenu : Control
{
    [Export] public Button StartButton;
    
    private LevelManager levelManager;

    public override void _Ready()
    {
        StartButton.Pressed += OnStartButtonPressed;
        levelManager = GetNode<LevelManager>("/root/LevelManager");
    }

    private void OnStartButtonPressed()
    {
        levelManager.ChangeLevel("Level_01");
    }

} 

