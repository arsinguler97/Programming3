using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class HoverShake : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float angle = 6f;
    [SerializeField] float duration = 0.35f;

    RectTransform _rt;
    Tween _tween;
    Quaternion _startRot;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _startRot = _rt.localRotation;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tween?.Kill();
        _tween = _rt
            .DORotate(new Vector3(0, 0, angle), duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tween?.Kill();
        _rt.localRotation = _startRot;
    }
}
