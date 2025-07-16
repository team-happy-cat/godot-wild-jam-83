using Godot;

public partial class GobbleAbility : Area3D
{
    [Export] public EatAndLungeAbility eatAndLungeAbility;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        GD.Print("[GobbleAbility] body entered");
        if (body is AgentBase agentBase)
        {
            GD.Print("[GobbleAbility] Agent base is true");
            if (eatAndLungeAbility.Gobbling)
            {
                agentBase.Die();
                GD.Print("[GobbleAbility] Gobbled: ", agentBase.Name);
            }
        }
    }

}
