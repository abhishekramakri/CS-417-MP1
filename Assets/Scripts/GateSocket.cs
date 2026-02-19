using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GateSocket : MonoBehaviour
{
    [Tooltip("The specific clue object this socket accepts")]
    public GameObject acceptedClue;

    [Tooltip("Optional: object to disable when unlocked (e.g. a wall, door, laser barrier)")]
    public GameObject obstruction;

    [Tooltip("Optional: object to enable when unlocked (e.g. reveal a hidden area)")]
    public GameObject secret;

    private XRSocketInteractor socketInteractor;
    private bool isUnlocked = false;

    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.selectEntered.AddListener(OnObjectPlaced);
        socketInteractor.selectExited.AddListener(OnObjectRemoved);
    }

    void OnObjectPlaced(SelectEnterEventArgs args)
    {
        // Check if the placed object is the correct clue
        if (args.interactableObject.transform.gameObject != acceptedClue)
        {
            // Wrong object — eject it by deselecting
            StartCoroutine(EjectWrongObject(args));
            return;
        }

        // Correct object placed
        isUnlocked = true;

        // Remove the obstruction (Secrets / Doors and Keys)
        if (obstruction != null)
            obstruction.SetActive(false);

        // Reveal the secret content
        if (secret != null)
            secret.SetActive(true);

        // Notify the game manager
        EscapeRoomManager manager = FindFirstObjectByType<EscapeRoomManager>();
        if (manager != null)
            manager.OnGateUnlocked();
    }

    void OnObjectRemoved(SelectExitEventArgs args)
    {
        // Don't allow removing once unlocked
        // The object stays locked in the socket
    }

    System.Collections.IEnumerator EjectWrongObject(SelectEnterEventArgs args)
    {
        yield return new WaitForSeconds(0.1f);

        if (socketInteractor.hasSelection)
        {
            // Force the socket to release the wrong object
            var interactable = socketInteractor.firstInteractableSelected;
            if (interactable != null && interactable.transform.gameObject != acceptedClue)
            {
                socketInteractor.interactionManager.CancelInteractorSelection((IXRSelectInteractor)socketInteractor);
            }
        }
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}
