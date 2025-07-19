using Godot;

[Tool]
public partial class Pursue : BTAction
{
    [Export] public float MinDistance = 1.0f;

    [Export] public StringName TargetVar { get; set; } = "target";
    [Export] public StringName SpeedVar { get; set; } = "speed";

    private Vector3 waypoint;

    public override string _GenerateName()
    {
        return $"Pursue {LimboUtility.DecorateVar(TargetVar)}";
    }

    public override void _Enter()
    {
        Node3D target = (Node3D)Blackboard.GetVar(TargetVar);
        if (IsInstanceValid(target))
        {
            SelectNewWaypoint(GetDesiredPosition(target));
        }
    }

    public override Status _Tick(double delta)
    {
        if (Agent is not AgentBase agentBase)
        {
            return Status.Failure;
        }

        Node3D target = (Node3D)Blackboard.GetVar(TargetVar);
        if (!IsInstanceValid(target))
        {
            return Status.Failure;
        }

        Vector3 desiredPosition = GetDesiredPosition(target);
        if (agentBase.GlobalPosition.DistanceTo(desiredPosition) < MinDistance)
        {
            return Status.Success;
        }

        if (agentBase.GlobalPosition.DistanceTo(waypoint) < MinDistance)
        {
            SelectNewWaypoint(desiredPosition);
        }

        float speed = (float)Blackboard.GetVar(SpeedVar, 3.0f);
        Vector3 desiredVelocity = agentBase.GlobalPosition.DirectionTo(waypoint) * speed;
        desiredVelocity.Y -= (float)delta * 9.8f;
        agentBase.Move(desiredVelocity);
        agentBase.UpdateFacing();
        return Status.Running;
    }

    private Vector3 GetDesiredPosition(Node3D target)
    {
        if (Agent is not AgentBase agentBase)
        {
            return Vector3.Zero;
        }

        Vector3 toAgent = agentBase.GlobalPosition - target.GlobalPosition;
        toAgent.Y = 0;
        if (toAgent.LengthSquared() < 0.01f)
        {
            toAgent = Vector3.Right;
        }
        Vector3 desiredPosition = target.GlobalPosition + toAgent.Normalized();
        return desiredPosition;
    }

    private void SelectNewWaypoint(Vector3 desiredPosition)
    {
        if (Agent is not AgentBase agentBase)
        {
            return;
        }

        Vector3 distanceVector = desiredPosition - agentBase.GlobalPosition;
        float angleVariation = (float)GD.RandRange(-0.2f, 0.2f);

        if (distanceVector.Length() > 50.0f)
        {
            distanceVector = distanceVector.Normalized() * 50.0f;
        }

        Transform3D rotationTransform = Transform3D.Identity.Rotated(Vector3.Up, angleVariation);
        waypoint = agentBase.GlobalPosition + rotationTransform * distanceVector;
    }

}

