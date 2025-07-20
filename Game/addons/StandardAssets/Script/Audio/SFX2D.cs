using Godot;
using System.Collections.Generic;

namespace Game
{
	/// <summary>
	/// Autoload singleton accessible at /root/SFX
	/// </summary>
	public partial class SFX2D : Node
	{
		private static SFX2D _instance;
		
		public static float MasterVolume { get; set; } = 0.7f;
		public static Dictionary<string, SoundGroup2D> SoundGroups = new();

		public override void _Ready()
		{
			_instance = this;
			
			FindSoundGroups(this);

			foreach (SoundGroup2D soundGroup in SoundGroups.Values)
			{
				soundGroup.Initialize(this);
			}

			GD.PrintRich($"[SFX] [color={ColorsHex.MediumSeaGreen}]Ready[/color] with {SoundGroups.Count} sound groups");
		}

		private static void FindSoundGroups(Node node)
		{
			foreach (Node child in node.GetChildren())
			{
				if (child is SoundGroup2D soundGroup)
					SoundGroups[child.Name] = soundGroup;
				else FindSoundGroups(child);
			}
		}

		public static void PlaySound(string soundGroupName, float volume = 1f)
		{
			if (SoundGroups.TryGetValue(soundGroupName, out SoundGroup2D soundGroup))
			{
				AudioStreamPlayer source = soundGroup.GetAvailableSource();

				if (source != null)
				{
					if (source.Playing)
					{
						GD.Print("Sound group is already playing");
					}
					GD.Print("[SFX] Playing: " + soundGroupName);
					source.VolumeLinear = MasterVolume * volume;
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
