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

    private void Update()
    {
        popupTransform.LookAt(mainCamera.transform.position);

        float dist = Vector3.Distance(mainCamera.transform.position, popupTransform.position);
        float t = Mathf.InverseLerp(maxDistance, minDistance, dist);

        canvasGroup.DOFade(t, fadeDuration);
    }
}