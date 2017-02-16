using UnityEngine;
using UnityEngine.Networking;
/*

    This class manages the player's behaviour

*/
public class PlayerController : NetworkBehaviour
{
    public XboxInput xboxInput;                                                         // Input manager
    public GameObject ball;                                                             // Ball gameObject 
    public float lockedForwardSpeed, lockedBackwardSpeed, lockedSidewaySpeed;           // Locked speeds
    public float unlockedForwardSpeed, unlockedBackwardSpeed, unlockedRotationSpeed;    // Unlocked speeds
    public float ballRotationSpeed = 50f;                                               // Ball rotation speed
    public const int maxHealth = 100;                                                   // Maximum health 
    public const int overheat = 100;                                                    // Maximum overheat
    [SyncVar]
    public int currentHealth = maxHealth;                                               // Health synced with the other clients
    public int currentHeat = 0;                                                         // Overheat level

    private bool lockedMovement;                                                        // Locked/Unlocked camera boolean - TODO : replace with the actual input
    private Vector3 movement;                                                           // Vector representing the current direction & speed of the robot

    // On Player spawn
    public override void OnStartLocalPlayer()
    {
        lockedMovement = false;
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

    }

    // Update physics
    void FixedUpdate()
    {
        Movement();
    }

    // Movement management
    void Movement()
    {
        // Player's movement
        if (!lockedMovement)
        {
            UnlockedMovement();
        }
        else
        {
            LockedMovement();
        }
        // Make the sphere move around its center depending on the robot's direction and speed
        ball.transform.RotateAround(ball.transform.position, Vector3.up, movement.y * ballRotationSpeed);
        ball.transform.RotateAround(ball.transform.position, Vector3.forward, movement.z * ballRotationSpeed);
        ball.transform.RotateAround(ball.transform.position, Vector3.right, movement.x * ballRotationSpeed);
    }

    // Locked movement management : player translates with the left stick forward, backward and sideways
	public void LockedMovement()
    {
        // Locked movement implementation
        float x = xboxInput.getLeftStickX() * Time.deltaTime * 10.0f;
		float z = xboxInput.getLeftStickY() * Time.deltaTime * 3.0f;

        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);

        movement = new Vector3(x, 0.0f, z);

    }

    // Unlocked movement management : player translates with the left stick forward and backward and rotates around itself sideways
	public void UnlockedMovement()
    {
        // Unlocked movement implementation
        //float x = xboxInput.getLeftStickX() * Time.deltaTime * 150.0f;
        //float z = xboxInput.getLeftStickY() * Time.deltaTime * 3.0f;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        ball.transform.Rotate(0, -x, 0);
        transform.Translate(0, 0, z);

        movement = new Vector3(0.0f, 0.0f, z);
    }

	// Unlocked movement management : player translates with the left stick forward and backward and rotates around itself sideways faster

	public void UnlockedRunMovement ()
	{
		float x = Input.GetAxis("Horizontal") * Time.deltaTime *2* 150.0f;
		float z = Input.GetAxis("Vertical") * Time.deltaTime *2* 3.0f;

		transform.Rotate(0, x, 0);
		ball.transform.Rotate(0, -x, 0);
		transform.Translate(0, 0, z);

		movement = new Vector3(0.0f, 0.0f, z);
	}

    // Call this anytime the robot takes damage to decrease its health
    void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            // TODO : Set dead state
            currentHealth = 0;
            Debug.Log("Dead!");
        }
        Debug.Log(currentHealth);
    }

    // Call this anytime the robot uses an ability to increase its heat
    void IncreaseHeat(int amount)
    {
        if (!isServer)
        {
            return;
        }
        currentHeat += amount;
        if (currentHeat >= overheat)
        {
            // TODO : Set overheat state
            Debug.Log("Overheat!");
        }
        Debug.Log(currentHeat);
    }
}