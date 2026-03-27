using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

[ExecuteInEditMode]
public class Selector : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    [SerializeField] private float radius = 200f;
    [SerializeField] private Vector2 centerOffset = Vector2.zero;

    private List<Vector2> targetPositions = new List<Vector2>();

    private void Start()
    {
        //ArrangeButtonsInCircle();
    }

    private void OnValidate()
    {
        ArrangeButtonsInCircle();
    }

    private void ArrangeButtonsInCircle()
    {
        if (buttons == null || buttons.Count == 0)
            return;

        targetPositions.Clear();

        float angleIncrement = 360f / buttons.Count;

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] == null)
                return;

            float angle = i * angleIncrement;
            float radians = angle * Mathf.Deg2Rad;

            float x = Mathf.Sin(radians) * radius;
            float y = Mathf.Cos(radians) * radius;

            Vector2 targetPos = new Vector2(x, y) + centerOffset;
            targetPositions.Add(targetPos);

            RectTransform buttonTransform = buttons[i].GetComponent<RectTransform>();

            if (buttonTransform != null)
            {
                //buttonTransform.anchoredPosition = new Vector2(x, y) + centerOffset;

                buttonTransform.localEulerAngles = new Vector3(0, 0, -angle);

                foreach (RectTransform child in buttonTransform)
                {
                    child.localEulerAngles = new Vector3(0, 0, angle);
                }
            }
        }
    }

    public void AnimateOpen(float duration = 0.3f)
    {
        DOVirtual.DelayedCall(0.3f, () =>
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                RectTransform rt = buttons[i].GetComponent<RectTransform>();

                rt.DOKill();

                //Resetting scale
                rt.localScale = Vector3.zero;

                // Start at center
                rt.anchoredPosition = centerOffset;

                // Move outward
                rt.DOAnchorPos(targetPositions[i], duration).SetEase(Ease.OutBack).SetDelay(i * 0.05f);

                // Scale pop
                rt.DOScale(0.76098f, duration).SetEase(Ease.OutBack).SetDelay(i * 0.05f);
            }
        });
    }

    public void AnimateClose(float duration = 0.3f)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            RectTransform rt = buttons[i].GetComponent<RectTransform>();

            rt.DOKill();

            rt.DOAnchorPos(centerOffset, duration).SetEase(Ease.InBack);

            rt.DOScale(0f, duration).SetEase(Ease.InBack);
        }
    }

}
