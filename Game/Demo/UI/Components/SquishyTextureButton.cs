using Godot;
using System;
using Game;

[GlobalClass]
public partial class SquishyTextureButton : TextureButton
{
    private const double AnimationTime = 0.2;
    
    [ExportSubgroup("Sounds", "sound")]
    [Export] private string soundHover;
    [Export] private float soundHoverVolume = 0.25f;
    [Export] private string soundClick;
    [Export] private float soundClickVolume = 0.5f;

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
        
        if (soundHover != null)
            SFX2D.PlaySound(soundHover, soundHoverVolume);
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
        
        if (soundClick != null)
            SFX2D.PlaySound(soundClick, soundClickVolume);
    }
}
