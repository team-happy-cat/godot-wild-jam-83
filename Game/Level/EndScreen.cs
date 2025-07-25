using Game;
using Godot;

public partial class EndScreen : Control
{
	[Export] private SquishyTextureButton _exitButton;
	
	private LevelManager _levelManager;

	public override void _Ready()
	{
		base._Ready();
		var jukebox = GetNode<Node>("/root/JukeBox");
		jukebox.Call("stop_music");

		Mouse.SetVisible();
		
		_levelManager = GetNode<LevelManager>("/root/LevelManager");
		_exitButton.Pressed += () => _levelManager.ChangeLevel("Main_Menu");
	}
}
