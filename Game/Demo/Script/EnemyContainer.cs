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
			if (child is AgentBase agentBase)
			{
				enemies.Add(agentBase);
				agentBase.TreeExited += OnDestroyed;
			}
		}
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
			LevelCompletion.Open(NextLevel);
			// levelManager.ChangeLevel(NextLevel, "SP_" + NextLevel);
		}
	}
}
