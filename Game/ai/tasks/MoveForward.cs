using Godot;

[Tool]
public partial class MoveForward : BTAction
{
    [Export] public StringName SpeedVar { get; set; } = "speed";
    [Export] public float Duration { get; set; } = 1f;

    public override string _GenerateName()
    {
        return $"MoveForward  speed: {LimboUtility.DecorateVar(SpeedVar)}  duration: {Duration}s";
    }

    public override Status _Tick(double delta)
    {
        GD.Print($"[MoveForward] Tick... {ElapsedTime}s / {Duration}s");

        if (Agent is not AgentBase agentBase)
        {
            return Status.Failure;
        }

        Vector3 forward = agentBase.Transform.Basis.Z;
        float speed = (float)Blackboard.GetVar(SpeedVar, 10.0f);
        Vector3 targetVelocity = forward * speed;
        targetVelocity.Y = agentBase.Velocity.Y;

        agentBase.Move(targetVelocity);

        if (ElapsedTime > Duration)
        {
            return Status.Success;
        }

        return Status.Running;
    }

}

