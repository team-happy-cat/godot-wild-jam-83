using Game;
using Godot;
using Godot.Collections;

public partial class EatAndLungeAbility : Node
{
	[Export(PropertyHint.None, "suffix:m")] public float RayLength = 30.0f;
	[Export] public BellyDisplay BellyDisplay;
	[Export] public CharacterController characterController;
	
	[Export] public AudioStreamPlayer LungeAudio;
	[Export] public AudioStreamPlayer EatAudio;

	[Export] public AnimationPlayer TongueAnimation;
	
	public bool Gobbling { get; set; } = false;

	private bool bellyIsFull = false;
	private CameraBridge cameraBridge;
	private SFX sfx;

	public override void _Ready()
	{
		cameraBridge = GetNode<CameraBridge>("/root/CameraBridge");
		sfx = GetNode<SFX>("/root/SFX");
		sfx.PlaySound("Bubbles", new Vector3(0,0,0));
	}

	private float gobbleTimer = 0;
	private float gobbleTimerMax = 1;

	public override void _Process(double delta)
	{
		float dt = (float)delta;

		if (Gobbling)
		{
			if (characterController.IsOnFloor())
			{
				Gobbling = false;
			}
			/*
			gobbleTimer += dt;

			if (gobbleTimer > gobbleTimerMax)
			{
				Gobbling = false;
			}
			*/
		}
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("left_click"))
		{
			if (bellyIsFull)
			{
				GD.Print("Launch!!");
				
				LungeAudio.Play();
				Vector3 launchDirection = -cameraBridge.MainCamera.GlobalTransform.Basis.Z;
				float launchForce = 20.0f;
				characterController.Velocity += launchDirection * launchForce;
				BellyDisplay.Empty();
				bellyIsFull = false;
				Gobbling = true;
				gobbleTimer = 0;
			}
			else
			{
				EatAudio.Play();
				if (TongueAnimation.IsPlaying())
				{
					TongueAnimation.Stop();
				}
				TongueAnimation.Play("eat");
				PhysicsDirectSpaceState3D spaceState = GetViewport().GetWorld3D().DirectSpaceState;
				Vector2 mousePos = GetViewport().GetMousePosition();
				Vector3 origin = cameraBridge.MainCamera.ProjectRayOrigin(mousePos);
				Vector3 end = origin + cameraBridge.MainCamera.ProjectRayNormal(mousePos) * RayLength;
				PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(origin, end);
				query.CollisionMask = 1u << 14;
				Dictionary result = spaceState.IntersectRay(query);

				if (result.ContainsKey("position"))
				{
					GD.Print($"Hit position: {result["position"]}");

					CharacterBody3D hitBody = (CharacterBody3D)result["collider"];

					if (hitBody != null && hitBody is AgentBase agentBase)
					{
						agentBase.Die();
						BellyDisplay.Full(agentBase.foodType);
						bellyIsFull = true;
						return;
					}
					else
					{

						GD.Print(hitBody.GetType());
					}
				}
				else
				{
					GD.Print("Miss");
				}
			}            
		}

	}

}
