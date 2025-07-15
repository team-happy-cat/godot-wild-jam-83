using Game;
using Godot;
using System.Collections.Generic;

public partial class EnemyContainer : Node
{
    [Export] public string NextLevel = "";

    private List<Enemy> enemies = [];
    private LevelManager levelManager;

    public override void _Ready()
    {
        levelManager = GetNode<LevelManager>("/root/LevelManager");
        
        foreach (var child in GetChildren())
        {
            if (child != null && child is Enemy enemy) 
            {
                enemies.Add(enemy);
            }
        }

        Enemy.Destroyed += OnDestroyed;
    }

    public override void _ExitTree()
    {
        Enemy.Destroyed -= OnDestroyed;
    }

    private void OnDestroyed()
    {
        GD.Print("[EnemyContainer] Enemy remaining: ", GetChildCount());

        if (NextLevel == "")
        {
            GD.PrintErr("[EnemyContainer] NextLevel is an empty string");
            return;
        }

        if (GetChildCount() - 1 == 0)
        {
            GD.Print("[EnemyContainer] All enemies have been destoyed! Changing level to: ", NextLevel);
            levelManager.ChangeLevel(NextLevel, "SP_" + NextLevel);
        }

    }
}
