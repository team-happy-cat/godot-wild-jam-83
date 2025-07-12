using Godot;
using System;
using Game;

[GlobalClass]
public partial class SquishyButton : TextureButton
{
    [ExportSubgroup("Sounds", "_sound")]
    [Export] private AudioStreamPlayer _soundHover;
    [Export] private AudioStreamPlayer _soundClick;

    public override void _Ready()
    {
        base._Ready();
    }
}
