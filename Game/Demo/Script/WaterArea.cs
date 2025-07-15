using Game;
using Godot;

public partial class WaterArea : Area3D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node3D body)
    {
        GD.Print("[WaterArea] Body entered water: " + body.Name);

        if (body is CharacterController cc)
        {
            cc.InWater = true;
        }
    }

    private void OnBodyExited(Node3D body)
    {
        GD.Print("[WaterArea] Body exited water: " + body.Name);

        if (body is CharacterController cc)
        {
            cc.InWater = false;
        }
    }
}

