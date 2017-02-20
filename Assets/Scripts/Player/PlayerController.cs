using UnityEngine;
using UnityEngine.Networking;
/*

    This class manages the player's behaviour

*/
public class PlayerController : NetworkBehaviour
{
    public GameObject cameraHolder;                                                     // Camera holder
    public GameObject ball;                                                             // Ball gameObject
    public float lockedForwardSpeed, lockedBackwardSpeed, lockedSidewaySpeed;           // Locked speeds
    public float unlockedForwardSpeed, unlockedBackwardSpeed, unlockedRotationSpeed;    // Unlocked speeds
    public float ballRotationSpeed = 50f;                                               // Ball rotation speed
    public const int maxHealth = 100;                                                   // Maximum health
    public const int overheat = 100;                                                    // Maximum overheat
    public const float runSpeed = 3f;                                                   // Maximum run speed
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;                                               // Health synced with the other clients
    [SyncVar(hook = "OnChangeHeat")]
    public int currentHeat = 0;                                                         // Heat synced with the other clients
    private GameObject infoHandler;
    public bool lockedMovement;                                                         // Locked/Unlocked camera boolean
    private Vector3 movement;                                                           // Vector representing the current direction & speed of the robot
    public GameObject playerInfo, opponentInfo;
    public GameObject canvas;
    public Automaton automaton;

    // On Player spawn
    public override void OnStartLocalPlayer()
    {
        lockedMovement = false;
        gameObject.tag = "LocalPlayer";
        this.automaton = GetComponent<RobotAutomaton>();
        this.automaton.enabled = true;
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        TargetManager.instance.SetPlayer(gameObject);
        cameraHolder.GetComponent<FreeCameraLook>().SetTarget(transform);
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        playerInfo = canvas.transform.GetChild(1).gameObject;
        playerInfo.SetActive(true);
    }

    // On Opponent spawn
    void Start()
    {
        if (!isLocalPlayer) {
            this.automaton = GetComponent<RobotAutomaton>();
            canvas = GameObject.FindGameObjectWithTag("Canvas");
            TargetManager.instance.AddOpponent(gameObject);
            opponentInfo = canvas.transform.GetChild(0).gameObject;
        }
    }

    // Updates the character
    void Update()
    {
    }

    // Update physics
    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
    }

    // Movement management
    public void Movement(float multiplier = 1)
    {
        // Player's movement
        if (!lockedMovement)
        {
            UnlockedMovement(multiplier);
        }
        else
        {
            LockedMovement(multiplier);
        }
        // Make the sphere move around its center depending on the robot's direction and speed
        ball.transform.RotateAround(ball.transform.position, Vector3.up, movement.y * ballRotationSpeed);
        ball.transform.RotateAround(ball.transform.position, Vector3.forward, movement.z * ballRotationSpeed);
        ball.transform.RotateAround(ball.transform.position, Vector3.right, movement.x * ballRotationSpeed);
    }

    public void RunMovement()
    {
        Movement(runSpeed);
    }
    // Locked movement management : player translates with the left stick forward, backward and sideways
	public void LockedMovement(float multiplier = 1)
    {
        // Locked movement implementation
		float x = InputManager.moveX () * Time.deltaTime * multiplier * 10.0f;
		float z = InputManager.moveY () * Time.deltaTime * multiplier * 3.0f;

        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);

        movement = new Vector3(x, 0.0f, z);

    }

    // Unlocked movement management : player translates with the left stick forward and backward and rotates around itself sideways
	public void UnlockedMovement(float multiplier = 1)
    {
        // Unlocked movement implementation
		float x = InputManager.moveX () * Time.deltaTime * multiplier * 150.0f;
		float z = InputManager.moveY () * Time.deltaTime * multiplier * 3.0f;
        //float x = Input.GetAxis("Horizontal") * Time.deltaTime * multiplier * 150.0f;
		//float z = Input.GetAxis("Vertical") * Time.deltaTime * multiplier *3.0f;

        transform.Rotate(0, x, 0);
        ball.transform.Rotate(0, -x, 0);
        transform.Translate(0, 0, z);

        movement = new Vector3(0.0f, 0.0f, z);
    }

    // Call this anytime the robot takes damage to decrease its health
    public void TakeDamage(int amount)
    {
        if (!isLocalPlayer)
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
    public void IncreaseHeat(int amount)
    {
        if (!isLocalPlayer)
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

    void OnChangeHealth(int health)
    {
        currentHealth = health;
    }

    void OnChangeHeat(int heat)
    {
        currentHeat = heat;
    }
}
