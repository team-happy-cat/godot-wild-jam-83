using Godot;

[Tool]
public partial class ArrivePos : BTAction
{
    [Export] public float Tolerance { get; set; } = 50.0f;
    [Export] public StringName TargetPosition { get; set; } = "pos";
    [Export] public StringName Speed { get; set; } = "speed";

    public override string _GenerateName()
    {
        return $"Arrive  pos: {LimboUtility.DecorateVar(TargetPosition)}";
    }

    public override Status _Tick(double delta)
    {
        if (Agent is not AgentBase agentBase)
        {
            return Status.Failure;
        }

        Vector3 targetPosition = Blackboard.GetVar(TargetPosition, Vector3.Zero).AsVector3();
        if (targetPosition.DistanceTo(agentBase.GlobalPosition) < Tolerance)
        {
            return Status.Success;
        }

        float speed = (float)Blackboard.GetVar(Speed, 3.0);
        float flatDistance = Mathf.Abs(agentBase.GlobalPosition.X - targetPosition.X);
        Vector3 direction = agentBase.GlobalPosition.DirectionTo(targetPosition);

        float verticalFactor = Mathf.Remap(flatDistance, 200.0f, 500.0f, 1.0f, 0.0f);
        verticalFactor = Mathf.Clamp(verticalFactor, 0.0f, 1.0f);
        direction.Y *= verticalFactor;

        Vector3 desiredVelocity = direction.Normalized() * speed;
        agentBase.Move(desiredVelocity);
        agentBase.UpdateFacing();
        return Status.Running;
    }

}

