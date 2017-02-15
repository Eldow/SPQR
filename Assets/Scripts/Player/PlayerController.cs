using UnityEngine;
using UnityEngine.Networking;
/*

    This class manages the player's behaviour

*/
public class PlayerController : NetworkBehaviour
{
    private bool lockedMovement;
	  public XboxInput xboxInput;
	  public float maxIncline = 30f;

    // On Player spawn
    public override void OnStartLocalPlayer()
    {
        lockedMovement = true;
        gameObject.tag = "LocalPlayer";
        TargetManager.instance.SetPlayer(gameObject);
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
		    xboxInput = new XboxInput (1);
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
            //UnlockedMovement();
        } else
        {
            //LockedMovement();
        }

    }

    public void LockedMovement()
    {
        // Locked movement implementation
        float x = xboxInput.getLeftStickX() * Time.deltaTime * 10.0f;
		    float z = xboxInput.getLeftStickY() * Time.deltaTime * 3.0f;

        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);

    }

    public void UnlockedMovement()
    {
        // Unlocked movement implementation
        float x = xboxInput.getLeftStickX() * Time.deltaTime * 150.0f;
		    float z = xboxInput.getLeftStickY() * Time.deltaTime * 3.0f;
		    //float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		    //float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }

    public void UnlockedRunMovement()
    {
        // Unlocked movement implementation
        float x = xboxInput.getLeftStickX() * Time.deltaTime * 2 * 150.0f;
        float z = xboxInput.getLeftStickY() * Time.deltaTime * 2 * 3.0f;
        //float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        //float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
}
