using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
    public UnityEvent myEvent;

    private bool pressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (pressed) return;

        myEvent?.Invoke();
    }

    public void ButtonHasBeenPressed()
    {
        pressed = true;
    }
}
