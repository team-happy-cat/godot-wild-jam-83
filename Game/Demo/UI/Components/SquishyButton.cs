using Godot;
using System;
using Game;

[GlobalClass]
public partial class SquishyButton : Button
{
    private const double AnimationTime = 0.2;
    
    [ExportSubgroup("Sounds", "_sound")]
    [Export] private AudioStreamPlayer _soundHover;
    [Export] private AudioStreamPlayer _soundClick;

    public override void _Ready()
    {
        base._Ready();
        
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
        Pressed += OnPressed;
    }

    private void OnMouseEntered()
    {
        PivotOffset = Size / 2f;
        
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Cubic);

        tween.TweenProperty(this, "scale", Vector2.One * 1.1f, AnimationTime);
        
        tween.Play();
    }

    private void OnMouseExited()
    {
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Cubic);

        tween.TweenProperty(this, "scale", Vector2.One, AnimationTime);
        
        tween.Play();
    }

    private void OnPressed()
    {
        Scale = Vector2.One;
        
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Cubic);

        tween.TweenProperty(this, "scale", Vector2.One * 1.1f, AnimationTime / 2f);
        
        tween.Play();
    }
}
