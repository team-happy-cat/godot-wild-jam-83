using Godot;

[Tool]
public partial class FaceRandomDirection : BTAction
{
    public override string _GenerateName()
    {
        return "FaceRandomDirection";
    }

    public override Status _Tick(double delta)
    {
        if (Agent is AgentBase agentBase)
        {
            agentBase.FaceRandomDirection();
            return Status.Success;
        }

        return Status.Failure;
    }

}

