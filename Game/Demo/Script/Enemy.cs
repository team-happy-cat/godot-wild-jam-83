using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    public static event Action Destroyed;

    public override void _ExitTree()
    {
        GD.Print("[Enemy] Destroyed: ", Name);
        Destroyed?.Invoke();
    }
}
