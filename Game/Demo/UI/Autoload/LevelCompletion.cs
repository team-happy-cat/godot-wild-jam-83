using Game;
using Godot;

public partial class LevelCompletion : CanvasLayer
{
    private static LevelCompletion _instance;
    private static LevelManager _levelManager;
    
    [Export] private AnimationPlayer _animations;
    [Export] private TextureButton _buttonContinue;

    private static string _nextLevel;

    public override void _Ready()
    {
        base._Ready();
        
        _instance = this;
        _levelManager = GetNode<LevelManager>("/root/LevelManager");

        _buttonContinue.Pressed += () => _levelManager.ChangeLevel(_nextLevel, "SP_" + _nextLevel);
        GetNode<Transition>("/root/Transition").OnCleanup += Close;
    }
    
    public static void Open(string nextLevel)
    {
        _nextLevel = nextLevel;
        _instance.GetTree().Paused = true;
        _instance._animations.Play("open");
        Mouse.SetVisible();
    }
    
    private void Close()
    {
        _animations.Play("RESET");
    }
}
