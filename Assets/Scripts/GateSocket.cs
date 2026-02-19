using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

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

        // Filter: only allow the correct clue to be socketed
        socketInteractor.startingSelectedInteractable = null;
        socketInteractor.selectEntered.AddListener(OnObjectPlaced);
    }

    // Called by XRI before allowing a hover — reject wrong objects early
    public bool CanSocketAccept(IXRInteractable interactable)
    {
        if (isUnlocked) return false;
        return interactable.transform.gameObject == acceptedClue;
    }

    void OnEnable()
    {
        // Wait a frame so socketInteractor is ready
        StartCoroutine(RegisterFilter());
    }

    System.Collections.IEnumerator RegisterFilter()
    {
        yield return null;
        if (socketInteractor == null)
            socketInteractor = GetComponent<XRSocketInteractor>();

        // Use hover/select filters to block wrong objects entirely
        socketInteractor.hoverFilters.Add(new XRSocketFilter(this));
        socketInteractor.selectFilters.Add(new XRSocketFilter(this));
    }

    void OnObjectPlaced(SelectEnterEventArgs args)
    {
        // Double-check (filter should prevent this, but just in case)
        if (args.interactableObject.transform.gameObject != acceptedClue)
            return;

        // Correct object placed
        isUnlocked = true;

        if (obstruction != null)
            obstruction.SetActive(false);

        if (secret != null)
            secret.SetActive(true);

        EscapeRoomManager manager = FindFirstObjectByType<EscapeRoomManager>();
        if (manager != null)
            manager.OnGateUnlocked();
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}

// Filter class that prevents wrong objects from even entering the socket
public class XRSocketFilter : IXRHoverFilter, IXRSelectFilter
{
    private GateSocket gateSocket;

    public XRSocketFilter(GateSocket socket)
    {
        gateSocket = socket;
    }

    public bool canProcess => true;

    public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable)
    {
        return gateSocket.CanSocketAccept(interactable);
    }

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        return gateSocket.CanSocketAccept(interactable);
    }
}
