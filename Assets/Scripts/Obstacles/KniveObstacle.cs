using UnityEngine;

public class KniveObstacle : MonoBehaviour
{
    public enum KniveType
    {
        Degollador,
        Tombola,
        Girador
    }

    [SerializeField] private KniveType type;

    private void Start()
    {
        switch (type)
        {
            case KniveType.Degollador:
                DegolladorMovement();
            break;

        }
    }

    private void DegolladorMovement()
    {

    }
}
