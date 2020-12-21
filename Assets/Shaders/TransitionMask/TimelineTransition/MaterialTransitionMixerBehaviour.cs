using System;
using UnityEngine;
using UnityEngine.Playables;

namespace A_rosuko.TransitionMask
{
    public class MaterialTransitionMixerBehaviour : PlayableBehaviour
    {
        int materialTransitionPropId = Shader.PropertyToID("_AlphaMaskTransition");
        Material trackBinding;
        bool firstFrameHappened;
        float cacheTransition;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            trackBinding = playerData as Material;

            if (!trackBinding)
                return;

            if (!firstFrameHappened)
            {
                cacheTransition = trackBinding.GetFloat(materialTransitionPropId);
                firstFrameHappened = true;
            }

            float inputTotal = 0.0f;
            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<MaterialTransitionBehaviour> inputPlayable = (ScriptPlayable<MaterialTransitionBehaviour>)playable.GetInput(i);
                MaterialTransitionBehaviour input = inputPlayable.GetBehaviour();

                inputTotal += input.transition * inputWeight;

            }
            trackBinding.SetFloat(materialTransitionPropId, inputTotal);
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            firstFrameHappened = false;

            if (trackBinding == null) return;
            trackBinding.SetFloat(materialTransitionPropId, cacheTransition);
        }
    }
}