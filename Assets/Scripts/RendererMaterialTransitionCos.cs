using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererMaterialTransitionCos : MonoBehaviour
{
    [SerializeField]
    AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    float transition = 0;
    [SerializeField]
    float transitionTimeScale = 1;

    float startTransition;

    Renderer targetRenderer;
    Material material;
    int materialPropId = Shader.PropertyToID("_AlphaMaskTransition");

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
        material = targetRenderer.material;
        startTransition = material.GetFloat(materialPropId);
    }
    void OnDestroy()
    {
        Destroy(targetRenderer.material);
        Destroy(targetRenderer);
        material.SetFloat(materialPropId, startTransition);
    }

    void Update()
    {
        transition = Mathf.Abs(Mathf.Cos(Time.time * transitionTimeScale));
        material.SetFloat(materialPropId, animationCurve.Evaluate(transition));
    }

}
