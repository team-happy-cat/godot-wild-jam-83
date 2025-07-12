using Godot;
using System;

/// <summary>
/// UI element which bops to the beat of a song.
/// </summary>
public partial class Bopper : TextureRect
{
    private const double AnimationDuration = 0.1;
    
    [Export] public double BeatsPerMinute = 140.0;

    private Timer _timer = new();
    
    public override void _Ready()
    {
        base._Ready();

        _timer.Timeout += Bop;
        _timer.OneShot = false;
        AddChild(_timer);
        _timer.Start(60.0 / BeatsPerMinute);
    }

    private void Bop()
    {
        PivotOffset = Size / 2f;
        Scale = Vector2.One * 1.1f;
        
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Cubic);
        
        tween.TweenProperty(this, "scale", Vector2.One, AnimationDuration);
        
        tween.Play();
    }
}
