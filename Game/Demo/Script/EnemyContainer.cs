using Game;
using Godot;
using System.Collections.Generic;

public partial class EnemyContainer : Node
{
	[Export] public string NextLevel = "";

	private List<AgentBase> enemies = [];
	private LevelManager levelManager;

	public override void _Ready()
	{
		levelManager = GetNode<LevelManager>("/root/LevelManager");
		
		foreach (var child in GetChildren())
		{
			if (child != null && child is AgentBase agentBase) 
			{
				enemies.Add(agentBase);
			}
		}

		AgentBase.Destroyed += OnDestroyed;
	}

	public override void _ExitTree()
	{
		AgentBase.Destroyed -= OnDestroyed;
	}

    private void OnDestroyed()
    {
        CallDeferred(nameof(CheckForLevelComplete));
    }

    private void CheckForLevelComplete()
    {
        GD.Print("[EnemyContainer] Enemy remaining: ", GetChildCount());

        if (GetChildCount() == 0)
        {
            GD.Print("[EnemyContainer] All enemies have been destoyed! Changing level to: ", NextLevel);
            levelManager.ChangeLevel(NextLevel, "SP_" + NextLevel);
        }
    }
}
