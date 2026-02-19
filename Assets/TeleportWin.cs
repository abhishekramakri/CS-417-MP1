using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportWin : MonoBehaviour
{
    Vector3 birdPos = new Vector3(0, 25, -35);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void teleportToBird() {
         transform.position = birdPos;
    }
}
