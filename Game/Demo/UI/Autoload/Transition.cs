using Game;
using Godot;

public partial class Transition : CanvasLayer
{
    [Export] public Control InputBlocker;
    [Export] public AnimationPlayer Animations;
    
    [ExportSubgroup("Animation Parameters", "Animation")]
    [Export] public double AnimationStart = 0.3;
    [Export] public double AnimationEnd = 1.0;
    
    private LevelManager _levelManager;

    public override void _Ready()
    {
        base._Ready();
        
        _levelManager = GetNode<LevelManager>("/root/LevelManager");

        _levelManager.BeginUnloadingLevel += (id, spawn) => Play();
        Animations.AnimationFinished += (anim) => Hide();
        
        Hide();
    }

    private void Play()
    {
        Animations.Play("transition");
        Show();
    }
}
