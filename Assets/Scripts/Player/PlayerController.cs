using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	public XboxInput xboxInput;
	public float maxIncline = 30f;

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<FollowCamera>().SetTarget(gameObject);
		xboxInput = new XboxInput (1);
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

		//float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		//float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		float x = xboxInput.getLeftStickX() * Time.deltaTime * 150.0f;
		float z = xboxInput.getLeftStickY() * Time.deltaTime * 3.0f;


        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

    }
}