using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace A_rosuko.TransitionMask
{
    public class ImageMaterialTransitionCos : MonoBehaviour
    {
        [SerializeField]
        AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField]
        float transition = 0;
        [SerializeField]
        float transitionTimeScale = 1;

        float startTransition;
        Image image;
        Material material;
        int materialPropId = Shader.PropertyToID("_AlphaMaskTransition");

        private void Awake()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            material = image.material;
            startTransition = material.GetFloat(materialPropId);
        }
        void OnDestroy()
        {
            Destroy(image);
            material.SetFloat(materialPropId, startTransition);
        }
        void Update()
        {
            transition = Mathf.Abs(Mathf.Cos(Time.time * transitionTimeScale));
            material.SetFloat(materialPropId, animationCurve.Evaluate(transition));
        }

    }
}