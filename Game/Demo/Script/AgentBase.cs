using Game;
using Godot;
using System;

public partial class AgentBase : CharacterBody3D
{
	public static event Action Destroyed;

	private int framesSinceFacingUpdate = 0;

	[Export] public Area3D strikeArea;

	public virtual void Move(Vector3 velocity)
	{
		Velocity = velocity.Lerp(velocity, 0.1f);
		MoveAndSlide();
	}

	public virtual void FaceDirection(float direction)
	{
		Rotation = new(0, direction, 0);
	}
	
	public virtual void FaceRandomDirection()
	{
		float randomAngle = GD.Randf() * Mathf.Tau;
		Rotation = new Vector3(0, randomAngle, 0);
	}

	public virtual float GetFacing()
	{
		return -Transform.Basis.Z.X > 0 ? 1.0f : -1.0f;
	}

	public void Die()
	{
		GD.Print("Destroyed: ", Name);
		Destroyed?.Invoke();
		QueueFree();
	}

	public void UpdateFacing()
	{
		framesSinceFacingUpdate++;
		if (framesSinceFacingUpdate > 3)
		{
			Vector3 flatVelocity = new(Velocity.X, 0, Velocity.Z);
			if (flatVelocity.LengthSquared() > 0.01f)
			{
				float yaw = Mathf.Atan2(flatVelocity.X, flatVelocity.Z);
				FaceDirection(yaw);
				framesSinceFacingUpdate = 0;
			}
		}
	}

	public void Strike()
	{
		GD.Print("[AgentBase] Striking...");
		var overlapping = strikeArea.GetOverlappingBodies();
		foreach (Node3D body in overlapping)
		{
			if (body is CharacterController player)
			{
				player.Die();
			}
		}
	}

}
