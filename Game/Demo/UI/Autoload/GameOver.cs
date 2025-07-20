using Game;
using Godot;

public partial class GameOver : CanvasLayer
{
	private static GameOver _instance;
	private static LevelManager _levelManager;

	[Export] public AnimationPlayer Animations;
	[Export] public AudioStreamPlayer GameOverAudio;

	[ExportSubgroup("Buttons", "_button")]
	[Export] private TextureButton _buttonExit;
	[Export] private TextureButton _buttonRestart;
	
	public override void _Ready()
	{
		base._Ready();
		
		_instance = this;
		_levelManager = GetNode<LevelManager>("/root/LevelManager");
		
		_buttonExit.Pressed += () =>
			_levelManager.ChangeLevel("Main_Menu");
		_buttonRestart.Pressed += () =>
			_levelManager.ChangeLevel(_levelManager.CurrentLevelID, "SP_" + _levelManager.CurrentLevelID);

		GetNode<Transition>("/root/Transition").OnCleanup += CloseWithoutAnimations;
	}

	public static void Open()
	{
		_instance.Animations.Play("open");
		Mouse.SetVisible();
		_instance.GetTree().Paused = true;
		_instance.GameOverAudio.Play();
	}

	public static void Close()
	{
		_instance.Animations.Play("close");
		_instance.GetTree().Paused = false;
	}

	private static void CloseWithoutAnimations()
	{
		_instance.Animations.Play("RESET");
	}
}
