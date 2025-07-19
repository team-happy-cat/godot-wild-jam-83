using Godot;

[Tool]
public partial class InRange : BTCondition
{
    [Export] public float DistanceMin { get; set; }
    [Export] public float DistanceMax { get; set; }
    [Export] public StringName TargetVar { get; set; } = "target";

    public override string _GenerateName()
    {
        return $"InRange ({DistanceMin}, {DistanceMax}) of {LimboUtility.DecorateVar(TargetVar)}";
    }

    public override Status _Tick(double delta)
    {
        if (Agent is not AgentBase agentBase)
        {
            GD.Print("Agent is not AgentBase agentBase");
            return Status.Failure;
        }

        Node3D target = (Node3D)Blackboard.GetVar(TargetVar);
        if (!IsInstanceValid(target))
        {
            GD.Print("Could not find target: ", TargetVar);
            return Status.Failure;
        }

        float distance = agentBase.GlobalPosition.DistanceTo(target.GlobalPosition);
        if (distance >= DistanceMin && distance <= DistanceMax)
        {
            return Status.Success;
        }
        else
        {
            GD.Print("Not in range, distance: ", distance);
            return Status.Failure;
        }
    }

}

