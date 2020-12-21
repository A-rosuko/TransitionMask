using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace A_rosuko.TransitionMask
{
    [TrackColor(0f, 0.5f, 1f)]
    [TrackClipType(typeof(MaterialTransitionClip))]
    [TrackBindingType(typeof(Material))]
    public class MaterialTransitionTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<MaterialTransitionMixerBehaviour>.Create(graph, inputCount);
        }
    }
}