using UnityEngine;
using DG.Tweening;

public class PopupPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform popupTransform;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 6f;
    [SerializeField] private float fadeDuration;

    private Renderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
    }

    private void Update()
    {
        popupTransform.LookAt(mainCamera.transform.position);

        float dist = Vector3.Distance(mainCamera.transform.position, popupTransform.position);
        float t = Mathf.InverseLerp(maxDistance, minDistance, dist);

        FadeCanvasGroup(t);
        FadeRenderers(t);
    }

    private void FadeCanvasGroup(float targetAlpha)
    {
        if (canvasGroup == null) return;

        DOTween.Kill(canvasGroup);
        canvasGroup.DOFade(targetAlpha, fadeDuration);
    }

    private void FadeRenderers(float targetAlpha)
    {
        if (renderers == null) return;

        foreach (var renderer in renderers)
        {
            if (renderer == null) continue;
            var material = renderer.material;
            if (material == null || !material.HasProperty("_Color")) continue;

            DOTween.Kill(material);
            material.DOFade(targetAlpha, fadeDuration);
        }
    }
}
