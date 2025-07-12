using Godot;
using System.Collections.Generic;

namespace Game
{
    [GlobalClass]
    public partial class SoundGroup2D : Node
    {
        [Export] public int MaxVoices = 3;
        [Export] public Vector2 VaryPitch = new(0.97f, 1.03f);
        [Export] public Vector2 VaryVolume = new(0.95f, 1.0f);

        private RandomNumberGenerator rng = new();
        private SFX2D sfx;

        public List<AudioStreamPlayer> AvailableSources = [];
        public List<AudioStreamPlayer> ActiveSources = [];

        public int TotalVariations => ActiveSources.Count + AvailableSources.Count;

        public void Initialize(SFX2D _sfx)
        {
            sfx = _sfx;

            foreach (Node child in GetChildren())
            {
                if (child is AudioStreamPlayer player)
                {
                    player.Finished += () => OnAudioFinished(player);
                    AvailableSources.Add(player);
                }
            }

            if (MaxVoices > AvailableSources.Count)
                MaxVoices = AvailableSources.Count;
        }

        public void OnAudioFinished(AudioStreamPlayer src)
        {
            ActiveSources.Remove(src);
            AvailableSources.Add(src);
        }

        public void Stop(AudioStreamPlayer src)
        {
            src.Stop();
            ActiveSources.Remove(src);
            AvailableSources.Add(src);
        }

        public AudioStreamPlayer GetAvailableSource()
        {
            AudioStreamPlayer src;

            // Stop an active source if necessary
            if ((AvailableSources.Count > 0 && ActiveSources.Count >= MaxVoices)
                || AvailableSources.Count == 0)
            {
                src = ActiveSources[0];
                Stop(src);
            }

            int idx = rng.RandiRange(0, AvailableSources.Count - 1);
            src = AvailableSources[idx];
            src.PitchScale = (float)GD.RandRange(VaryPitch.X, VaryPitch.Y);
            src.VolumeDb = Toolbox.Audio.Linear2Db((float)GD.RandRange(VaryVolume.X, VaryVolume.Y));
            AvailableSources.RemoveAt(idx);
            ActiveSources.Add(src);

            return src;
        }

    }

}

