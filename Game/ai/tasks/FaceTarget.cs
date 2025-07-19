using Godot;

[Tool]
public partial class FaceTarget : BTAction
{
    [Export] public StringName TargetVar { get; set; } = "target";

    public override string _GenerateName()
    {
        return $"FaceTarget {LimboUtility.DecorateVar(TargetVar)}";
    }

    public override Status _Tick(double delta)
    {
        if (Agent is not AgentBase agentBase)
        {
            return Status.Failure;
        }

        var target = (Node3D)Blackboard.GetVar(TargetVar);
        if (!IsInstanceValid(target))
        {
            return Status.Failure;
        }

        Vector3 direction = agentBase.GlobalPosition - target.GlobalPosition;
        float yaw = Mathf.Atan2(direction.X, -direction.Z);

        agentBase.Velocity = Vector3.Zero;
        agentBase.FaceDirection(yaw);
        return Status.Success;
    }

}

