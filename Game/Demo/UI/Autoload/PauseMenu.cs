using Game;
using Godot;

public partial class PauseMenu : CanvasLayer
{
	private static PauseMenu _instance;
	private static LevelManager _levelManager;

	[Export] private AnimationPlayer _animations;
	
	[ExportSubgroup("Buttons", "_button")]
	[Export] private SquishyTextureButton _buttonExit;
	[Export] private SquishyTextureButton _buttonResume;
	
	private static bool _opened;
	private static bool _captureOnClose;

	public override void _Ready()
	{
		base._Ready();
		
		_instance = this;
		_levelManager = GetNode<LevelManager>("/root/LevelManager");

		_buttonExit.Pressed += () => _levelManager.ChangeLevel("Main_Menu");
		_buttonResume.Pressed += Close;

		GetNode<Transition>("/root/Transition").OnCleanup += CloseWithoutUnpausing;
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (!Input.IsActionJustPressed("ui_cancel"))
			return;
		if (_animations.IsPlaying())
			return;

		if (_opened)
			Close();
		else Open();
	}

	private static void Open()
	{
		_instance._animations.Play("open");
		_instance.GetTree().Paused = true;
		_opened = true;
		
		_captureOnClose = Mouse.IsCursorCaptured();
		
		Mouse.SetVisible();
	}

	private static void Close()
	{
		_instance._animations.Play("close");
		_instance.GetTree().Paused = false;
		_opened = false;
		
		if (_captureOnClose)
			Mouse.SetCaptured();
	}

	private static void CloseWithoutUnpausing()
	{
		_instance._animations.Play("RESET");
		_opened = false;
	}
}
