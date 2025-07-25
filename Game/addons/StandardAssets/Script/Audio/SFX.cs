using Godot;
using System.Collections.Generic;

namespace Game
{
	/// <summary>
	/// Autoload singleton accessible at /root/SFX
	/// </summary>
	public partial class SFX : Node
	{
		public float MasterVolume { get; set; } = 0.7f;
		public Dictionary<string, SoundGroup3D> SoundGroups = new();

		private CameraBridge cameraBridge;

		public override void _Ready()
		{
			cameraBridge = GetNode<CameraBridge>("/root/CameraBridge");

			FindSoundGroups(this);

			foreach (SoundGroup3D soundGroup in SoundGroups.Values)
			{
				soundGroup.Initialize(this);
			}

			GD.PrintRich($"[SFX] [color={ColorsHex.MediumSeaGreen}]Ready[/color] with {SoundGroups.Count} sound groups");
		}

		private void FindSoundGroups(Node node)
		{
			foreach (Node child in node.GetChildren())
			{
				if (child is SoundGroup3D soundGroup)
				{
					SoundGroups[child.Name] = soundGroup;
				}
				else
				{
					FindSoundGroups(child);
				}
			}
		}

		public void Play(string soundGroupName)
		{
			PlaySound(soundGroupName, cameraBridge.CameraPosition);
		}

		public void PlaySound(string soundGroupName)
		{
			PlaySound(soundGroupName, cameraBridge.CameraPosition);
		}

		public void PlaySound(string soundGroupName, Vector3 location)
		{
			if (SoundGroups.TryGetValue(soundGroupName, out SoundGroup3D soundGroup))
			{
				AudioStreamPlayer3D source = soundGroup.GetAvailableSource();

				if (source != null)
				{
					if (source.Playing)
					{
						GD.Print("Sound group is already playing");
					}
					GD.Print("[SFX] Playing: " + soundGroupName);
					source.Position = location;
					source.Play();
				}
			}
			else
			{
				GD.Print("[SFX] Requested a sound group that does not exist: " + soundGroupName);
			}
		}

	}

}
