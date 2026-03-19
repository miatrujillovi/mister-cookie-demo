using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Selector : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    [SerializeField] private float radius = 200f;
    [SerializeField] private Vector2 centerOffset = Vector2.zero;

    private void Start()
    {
        ArrangeButtonsInCircle();
    }

    private void OnValidate()
    {
        ArrangeButtonsInCircle();
    }

    private void ArrangeButtonsInCircle()
    {
        if (buttons == null || buttons.Count == 0)
            return;

        float angleIncrement = 360f / buttons.Count;

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] == null)
                return;

            float angle = i * angleIncrement;
            float radians = angle * Mathf.Deg2Rad;

            float x = Mathf.Sin(radians) * radius;
            float y = Mathf.Cos(radians) * radius;

            RectTransform buttonTransform = buttons[i].GetComponent<RectTransform>();

            if (buttonTransform != null)
            {
                buttonTransform.anchoredPosition = new Vector2(x, y) + centerOffset;

                buttonTransform.localEulerAngles = new Vector3(0, 0, -angle);

                foreach (RectTransform child in buttonTransform)
                {
                    child.localEulerAngles = new Vector3(0, 0, angle);
                }
            }
        }
    }

}
