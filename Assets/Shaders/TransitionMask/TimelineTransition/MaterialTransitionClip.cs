using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace A_rosuko.TransitionMask
{
    [Serializable]
    public class MaterialTransitionClip : PlayableAsset, ITimelineClipAsset
    {
        public MaterialTransitionBehaviour template = new MaterialTransitionBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.All; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<MaterialTransitionBehaviour>.Create(graph, template);
            MaterialTransitionBehaviour clone = playable.GetBehaviour();
            return playable;
        }
    }
}