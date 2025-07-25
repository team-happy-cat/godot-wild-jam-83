using Godot;
using DialogueManagerRuntime;
using PhantomCamera;

namespace Game
{
    /// <summary>
    /// Autoload singleton accessible at /root/CameraBridge
    /// This class is a stub, included here for demo purposes only.
    /// </summary>
    public partial class CameraBridge : Node
    {
        [Export] public bool FirstPersonMode = false;

        public bool HasDefaultYaw => Mathf.IsEqualApprox(Yaw, 0.0f);
        public readonly float KeyboardTurnRate = 0.045f;
        public float Pitch = 0.0f;
        public float Yaw = 0.0f;
        public Camera3D MainCamera;

        // Autoload
        private LevelManager levelManager;
        private PlayerSpawner playerSpawner;
        private Preferences prefs;
        private SaveManager saveManager;

        // Spring length
        private float currentSpringLength = 2.5f;
        private float defaultSpringLength = 2.5f;
        private float targetSpringLength;
        private float maxSpringLength = 5f;
        private float zoomRate = 0.2f;
        private float gamepadZoomRate = 0.1f;

        private float blink_delay = 0.0f;
        private float blink_in = 0.0f;
        private float blink_out = 2.0f;
        private float pitchDefault = Mathf.DegToRad(-18.0f);
        private float pitchMin = Mathf.DegToRad(-45);
        private float pitchMax = Mathf.DegToRad(45);
        private CharacterHub characterHub;
        private ColorRect blackout;
        private Node3D nodePhantomCamera3D;
        private Node3D lookAtTarget;

        private CameraAngles currentActiveCameraAngles;
        private CameraAngle currentActiveAngle;

        public Vector3 CameraPosition => MainCamera.Position;

        float xSensitivity = 0.06f;
        float ySensitivity = 0.04f;

        public override void _Ready()
        {
            levelManager = GetNode<LevelManager>("/root/LevelManager");
            playerSpawner = GetNode<PlayerSpawner>("/root/PlayerSpawner");
            prefs = GetNode<Preferences>("/root/Preferences");
            saveManager = GetNode<SaveManager>("/root/SaveManager");

            MainCamera = GetNode<Camera3D>("Camera3D");
            nodePhantomCamera3D = GetNode<Node3D>("PhantomCamera3D");

            blackout = GetNode<ColorRect>("ColorRect");

            targetSpringLength = defaultSpringLength;
            nodePhantomCamera3D.Set("spring_length", defaultSpringLength);
            nodePhantomCamera3D.Call("set_fov", 65);

            if (!FirstPersonMode)
            {
                Pitch = 0.0f;
                Yaw = MainCamera.Rotation.Y;
            }
            else
            {
                Pitch = pitchDefault;
                Yaw = 0.0f;
            }

            CharacterHub.Spawned += OnPlayerSpawned;
            CharacterHub.Destroyed += OnPlayerDestroyed;
            levelManager.BeginUnloadingLevel += OnBeginUnloadingLevel;
            DialogueManager.DialogueEnded += OnDialogueEnded;
            //Mouse.SetCaptured("CharacterController");
            GD.PrintRich($"[CameraBridge] [color={ColorsHex.MediumSeaGreen}]Ready[/color]");

            if (FirstPersonMode)
            {
                xSensitivity = 0.002f;
                ySensitivity = 0.0015f;
            }
        }

        public override void _Process(double delta)
        {
            // Yaw
            Yaw -= mouseTwistInput * xSensitivity;
            Yaw = Mathf.Wrap(Yaw, 0, Mathf.Tau);

            Pitch += mousePitchInput * ySensitivity;
            Pitch = Mathf.Clamp(Pitch, pitchMin, pitchMax);

            if (FirstPersonMode)
            {
                nodePhantomCamera3D.RotationDegrees = new(Mathf.RadToDeg(Pitch), Mathf.RadToDeg(-Yaw), 0);
            }
            else
            {
                Vector3 newRotation = new(Mathf.RadToDeg(Pitch), Mathf.RadToDeg(-Yaw), 0);
                nodePhantomCamera3D.Call("set_third_person_rotation_degrees", newRotation);
            }

            mouseTwistInput = 0.0f;
            mousePitchInput = 0.0f;
        }

        private float mouseTwistInput = 0.0f;
        private float mousePitchInput = 0.0f;

        public override void _UnhandledInput(InputEvent inputEvent)
        {
            if (inputEvent.IsActionPressed("zoom_in"))
            {
                GetViewport().SetInputAsHandled();
                targetSpringLength -= zoomRate;
                targetSpringLength = Mathf.Clamp(targetSpringLength, 1, maxSpringLength);
            }

            if (inputEvent.IsActionPressed("zoom_out"))
            {
                GetViewport().SetInputAsHandled();
                targetSpringLength += zoomRate;
                targetSpringLength = Mathf.Clamp(targetSpringLength, 1, maxSpringLength);
            }

            if (inputEvent is InputEventMouseMotion eventMouseMotion)
            {
                mouseTwistInput = -eventMouseMotion.Relative.X;
                mousePitchInput = -eventMouseMotion.Relative.Y;
            }
        }

        public void Blink()
        {
            var tween = GetTree().CreateTween();
            tween.TweenProperty(blackout, "self_modulate:a", 1, blink_in).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
            tween.TweenProperty(blackout, "self_modulate:a", 0, blink_out).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetDelay(blink_delay);
        }

        public void SetActiveCamera(CameraAngles cameraAngles, CameraAngle angle)
        {
            if (currentActiveCameraAngles != null && currentActiveCameraAngles != cameraAngles)
            {
                GD.Print($"[CameraBridge] Resetting previous camera: {currentActiveCameraAngles.Name} angle {currentActiveAngle}");
                currentActiveCameraAngles.SetCameraPriority(currentActiveAngle, 0);
            }

            GD.Print($"[CameraBridge] Setting new camera: {cameraAngles.Name} angle {angle}");
            cameraAngles.SetCameraPriority(angle);
            currentActiveCameraAngles = cameraAngles;
            currentActiveAngle = angle;
        }

        public void ResetActiveCamera()
        {
            GD.Print($"[CameraBridge] ResetActiveCamera - currentActiveCameraAngles: {currentActiveCameraAngles}, currentActiveAngle: {currentActiveAngle}");

            if (currentActiveCameraAngles != null)
            {
                currentActiveCameraAngles.SetCameraPriority(currentActiveAngle, 0);
                currentActiveCameraAngles = null;
            }
        }

        protected void OnDialogueEnded(Resource dialogueResource)
        {
            GD.Print($"[CameraBridge] OnDialogueEnded - resetting camera: {currentActiveCameraAngles?.Name}");
            ResetActiveCamera();
        }

        public void OnBeginUnloadingLevel(string nextLevel, string nextSpawnpoint)
        {
            nodePhantomCamera3D.Set("follow_target", default);
            nodePhantomCamera3D.Set("look_at_target", default);
        }

        public void OnPlayerSpawned(CharacterHub _characterHub)
        {
            characterHub = _characterHub;
            lookAtTarget = characterHub.LookAt;

            nodePhantomCamera3D.Set("follow_target", lookAtTarget);
            nodePhantomCamera3D.Set("look_at_target", lookAtTarget);

            GD.Print("[CameraBridge] Updated look targets");
        }

        public void OnPlayerDestroyed(CharacterHub _characterHub)
        {
            characterHub = null;
            lookAtTarget = null;

            GD.Print("[CameraBridge] Player destroyed, cleared references");
        }

        public Vector3 PhantomCameraPosition => nodePhantomCamera3D.GlobalPosition;
    }

}

