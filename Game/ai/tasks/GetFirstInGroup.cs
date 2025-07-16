using Godot;

[Tool]
public partial class GetFirstInGroup : BTAction
{
    [Export] public StringName Group { get; set; }
    [Export] public StringName OutputVar { get; set; } = "target";

    public override string _GenerateName()
    {
        return $"GetFirstNodeInGroup \"{Group}\"  -> {LimboUtility.DecorateVar(OutputVar)}";
    }

    public override Status _Tick(double delta)
    {
        var nodes = Agent.GetTree().GetNodesInGroup(Group);
        if (nodes.Count == 0)
        {
            return Status.Failure;
        }
        Blackboard.SetVar(OutputVar, nodes[0]);
        return Status.Success;
    }

}

