using UnityEngine;
using UnityEngine.Networking;
/*

    This class manages the player's behaviour

*/
public class PlayerController : NetworkBehaviour
{
    private bool lockedMovement;

    // On Player spawn
    public override void OnStartLocalPlayer()
    {
        lockedMovement = true;
        gameObject.tag = "LocalPlayer";
        TargetManager.instance.SetPlayer(gameObject);
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }

    // On Opponent spawn
    void Start()
    {
        if (!isLocalPlayer)
        {
            TargetManager.instance.AddOpponent(gameObject);
        }
    }

    // Updates the character 
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // Movement management
        if (!lockedMovement)
        {
            UnlockedMovement();
        } else
        {
            LockedMovement();
        }

    }

    void LockedMovement()
    {
        // Locked movement implementation
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);

    }

    void UnlockedMovement()
    {
        // Unlocked movement implementation
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
}