using Game;
using Godot;

public partial class AgentSpawner : Node3D
{
    [Export] public PackedScene AgentScene;

    private NextEventTimer timer;
    private AgentBase agentBase;

    public override void _Ready()
    {
        timer = GetNode<NextEventTimer>("NextEventTimer");
        timer.EventTriggered += OnTimerTriggered;
        agentBase = AgentScene.Instantiate<AgentBase>();
    }

    private void OnTimerTriggered()
    {
        if (GetChildCount() > 1)
        {
            GD.Print("Fly Exists");
            return;
        }
        AgentBase duplicate = agentBase.Duplicate() as AgentBase;
        AddChild(duplicate);
        duplicate.GlobalPosition = GlobalPosition;
        GD.Print("[AgentSpawner] Respawning");
    }

}
