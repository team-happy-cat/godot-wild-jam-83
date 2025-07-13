using Game;
using Godot;

public partial class Transition : CanvasLayer
{
    [Export] public Control InputBlocker;
    [Export] public Control RightEdge;
    [Export] public TextureRect Image;
    [Export] public Control Trail;
    [Export] public AudioStreamPlayer Audio;
    
    [ExportSubgroup("Animation Parameters", "Animation")]
    [Export] public double AnimationStart = 0.3;
    [Export] public double AnimationEnd = 1.0;
    
    private LevelManager levelManager;

    public override void _Ready()
    {
        base._Ready();
        
        levelManager = GetNode<LevelManager>("/root/LevelManager");

        levelManager.BeginUnloadingLevel += (id, spawn) => Play();
        Audio.Finished += Hide;
        
        Hide();
    }

    private void Play()
    {
        InputBlocker.Show();
        Image.Position = new Vector2(-Image.Size.X, Image.Position.Y);
        Trail.Modulate = Colors.White;
        Audio.Play();
        Show();
        
        var tween = CreateTween();
        
        tween.TweenInterval(AnimationStart);
        tween.TweenProperty(
            Image,
            "position",
            new Vector2(RightEdge.Position.X, Image.Position.Y),
            AnimationEnd - AnimationStart
        );
        tween.TweenCallback(Callable.From(InputBlocker.Hide));
        tween.TweenProperty(Trail, "modulate", Colors.Transparent, 0.5);
        
        tween.Play();
    }
}
