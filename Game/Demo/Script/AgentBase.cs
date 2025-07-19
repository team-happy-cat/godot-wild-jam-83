using Godot;
using System;

public partial class AgentBase : CharacterBody3D
{
	public static event Action Destroyed;

	public virtual void Move(Vector3 velocity)
	{
		Velocity = velocity.Lerp(velocity, 0.1f);
		MoveAndSlide();
	}

	public virtual void FaceDirection(float direction)
	{
		float targetRotationY = direction > 0 ? 0 : Mathf.Pi;
		Vector3 targetRotation = new(0, targetRotationY, 0);
		Rotation = Rotation.Lerp(targetRotation, 0.1f);
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

}
