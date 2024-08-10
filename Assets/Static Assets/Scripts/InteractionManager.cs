using UnityEngine;


public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    private Camera mainCamera;
    public bool isInputLocked { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void LockInput(float duration)
    {
        // Lock input for a certain duration (used for combo feedback or animations)
        isInputLocked = true;
        Invoke(nameof(UnlockInput), duration);
    }

    private void UnlockInput()
    {
        isInputLocked = false;
    }
   
}
