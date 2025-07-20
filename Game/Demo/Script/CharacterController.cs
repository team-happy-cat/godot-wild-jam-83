using Godot;

namespace Game
{
	public partial class CharacterController : CharacterBody3D
	{
		[ExportCategory("Character")]
		[Export] public float BaseSpeed = 5.0f;
		[Export] public float SprintSpeed = 8.0f;
		[Export] public float JumpVelocity = 4.5f;
		
		// MN air control 
		[Export] public float AirSpeed = 2.0f;

		[ExportGroup("Controls")]
		[Export] public string Jump = "jump";
		[Export] public string Left = "move_left";
		[Export] public string Right = "move_right";
		[Export] public string Forward = "move_forward";
		[Export] public string Backward = "move_backward";
		[Export] public string Pause = "start";
		[Export] public string Sprint = "sprint";

		[Export] public EatAndLungeAbility eatAndLungeAbility;

		[ExportGroup("Audio")]
		[Export] public AudioStreamPlayer JumpAudio;
		[Export] public AudioStreamPlayer WaterMoveAudio;

		private float gravity;
		private float speed;
		private CameraBridge cameraBridge;

		private Vector3 lastGroundedVelocity = Vector3.Zero;
		private bool groundedLastFrame = false;

		public bool InWater { get; set; } = false;
		
		// MN air control
		public bool IsLunging { get; set; } = false;

		private LevelManager levelManager;

		public override void _Ready()
		{
			gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
			speed = BaseSpeed;
			cameraBridge = GetNode<CameraBridge>("/root/CameraBridge");
			Mouse.SetCaptured("CharacterController");
			levelManager = GetNode<LevelManager>("/root/LevelManager");
		}

		public override void _PhysicsProcess(double delta)
		{
			if (eatAndLungeAbility.Gobbling)
			{
				Velocity = new Vector3(Velocity.X, Velocity.Y - gravity * (float)delta, Velocity.Z);
				MoveAndSlide();
				return;
			}

			if (!IsOnFloor())
			{
				Velocity = new Vector3(Velocity.X, Velocity.Y - gravity * (float)delta, Velocity.Z);
				
				if (!IsLunging)
				{
					speed = AirSpeed;
					
					Vector2 inputDir = Input.GetVector(Left, Right, Forward, Backward);
					Vector3 cameraForward = cameraBridge.MainCamera.GlobalTransform.Basis.Z;
					Vector3 cameraRight = cameraBridge.MainCamera.GlobalTransform.Basis.X;

					cameraForward.Y = 0;
					cameraRight.Y = 0;
					cameraForward = cameraForward.Normalized();
					cameraRight = cameraRight.Normalized();

					Vector3 direction = (cameraForward * inputDir.Y + cameraRight * inputDir.X).Normalized();

					if (direction != Vector3.Zero)
					{
						Velocity = new Vector3(direction.X * speed, Velocity.Y, direction.Z * speed);

						float targetAngle = Mathf.Atan2(direction.X, direction.Z);
						Rotation = new Vector3(0, targetAngle, 0);
					}
					else
					{
						Velocity = new Vector3(
							Mathf.MoveToward(Velocity.X, 0, speed),
							Velocity.Y,
							Mathf.MoveToward(Velocity.Z, 0, speed)
						);
					}
				}
			}

			if (Input.IsActionJustPressed(Jump) && IsOnFloor())
			{
				Velocity = new Vector3(Velocity.X, JumpVelocity, Velocity.Z);
				JumpAudio.Play();
			}

			if (IsOnFloor())
			{
				if (!groundedLastFrame)
				{
					lastGroundedVelocity = Vector3.Zero;
				}
				groundedLastFrame = true;

				speed = Input.IsActionPressed(Sprint) ? SprintSpeed : BaseSpeed;

				if (InWater)
				{
					speed *= 0.5f;
				}

				Vector2 inputDir = Input.GetVector(Left, Right, Forward, Backward);
				Vector3 cameraForward = cameraBridge.MainCamera.GlobalTransform.Basis.Z;
				Vector3 cameraRight = cameraBridge.MainCamera.GlobalTransform.Basis.X;

				cameraForward.Y = 0;
				cameraRight.Y = 0;
				cameraForward = cameraForward.Normalized();
				cameraRight = cameraRight.Normalized();

				Vector3 direction = (cameraForward * inputDir.Y + cameraRight * inputDir.X).Normalized();

				if (direction != Vector3.Zero)
				{
					Velocity = new Vector3(direction.X * speed, Velocity.Y, direction.Z * speed);

					float targetAngle = Mathf.Atan2(direction.X, direction.Z);
					Rotation = new Vector3(0, targetAngle, 0);
				}
				else
				{
					Velocity = new Vector3(
						Mathf.MoveToward(Velocity.X, 0, speed),
						Velocity.Y,
						Mathf.MoveToward(Velocity.Z, 0, speed)
					);
				}
			}
			else
			{
				/*
				if (groundedLastFrame)
				{
					lastGroundedVelocity = new Vector3(Velocity.X, 0, Velocity.Z);
				}
				groundedLastFrame = false;

				Velocity = new Vector3(lastGroundedVelocity.X, Velocity.Y, lastGroundedVelocity.Z);
				*/
			}

			MoveAndSlide();
		}

		public void Die()
		{
			GD.Print("[Player] Died!");
			GameOver.Open();
			// levelManager.ChangeLevel("Main_Menu");
		}
	}

}
