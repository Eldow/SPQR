using UnityEngine;
using UnityEngine.Networking;
/*

    This class manages the player's behaviour

*/
public class PlayerController : Photon.MonoBehaviour
{
	public string tagName = "LocalPlayer"; 
    public GameObject cameraHolder;                                                     // Camera holder
    public GameObject ball;                                                             // Ball gameObject 
    public float lockedForwardSpeed, lockedBackwardSpeed, lockedSidewaySpeed;           // Locked speeds
    public float unlockedForwardSpeed, unlockedBackwardSpeed, unlockedRotationSpeed;    // Unlocked speeds
    public float ballRotationSpeed = 50f;                                               // Ball rotation speed
    public const int maxHealth = 100;                                                   // Maximum health 
    public const int overheat = 100;                                                    // Maximum overheat
    public const float runSpeed = 3f;                                                   // Maximum run speed
    public int currentHealth = maxHealth;                                               // Health synced with the other clients
    public int currentHeat = 0;                                                         // Heat synced with the other clients
    public bool lockedMovement;                                                         // Locked/Unlocked camera boolean
    private Vector3 movement;                                                           // Vector representing the current direction & speed of the robot
    public GameObject playerInfo, opponentInfo;
    public GameObject canvas;

    private Rigidbody body;
    
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    private Vector3 previousPosition;

    public float turnSpeed = 10f;

    private void setTags(string tagName){
		Transform[] temp;
		transform.tag = tagName;
		temp = GetComponentsInChildren<Transform> ();
		foreach (Transform t in temp) {
			t.tag = tagName;
		}
	}

    // On Opponent spawn
    void Start()
    {
        if (!photonView.isMine)
        {
            canvas = GameObject.FindGameObjectWithTag("Canvas");
            TargetManager.instance.AddOpponent(gameObject);
            opponentInfo = canvas.transform.GetChild(0).gameObject;
        } else
        {
            lockedMovement = false;
            setTags(tagName);
            GetComponent<RobotAutomaton>().enabled = true;
            GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
            TargetManager.instance.SetPlayer(gameObject);
            cameraHolder.GetComponent<FreeCameraLook>().SetTarget(transform);
            canvas = GameObject.FindGameObjectWithTag("Canvas");
            playerInfo = canvas.transform.GetChild(1).gameObject;
            playerInfo.SetActive(true);
        }
        body = GetComponent<Rigidbody>();
    }

    // Updates the character 
    void Update()
    {
    }

    // Update physics
    void FixedUpdate()
    {

    }

    // Movement management
    public void Movement(float multiplier = 1)
    {
        if (photonView.isMine)
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
            //ball.transform.RotateAround(ball.transform.position, Vector3.up, movement.y * ballRotationSpeed);
            //ball.transform.RotateAround(ball.transform.position, Vector3.forward, movement.z * ballRotationSpeed);
            //ball.transform.RotateAround(ball.transform.position, Vector3.right, movement.x * ballRotationSpeed);
        } else
        {
           // syncTime += Time.deltaTime;
           // body.MovePosition(Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay));
        }

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
        x = Mathf.Clamp(x, -10f, 10f);
        z = Mathf.Clamp(z, -10f, 10f);
        movement = new Vector3(body.position.x + x, body.position.y, body.position.z + z);

        body.MovePosition(movement);
    }

    // Unlocked movement management : player translates with the left stick forward and backward and rotates around itself sideways
	public void UnlockedMovement(float multiplier = 1)
    {
        // Unlocked movement implementation
        float moveHorizontal = InputManager.moveX();
        float moveVertical = InputManager.moveY();
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        previousPosition = body.position;
        body.velocity = movement * 5f;

        body.position = new Vector3
        (
            Mathf.Clamp(body.position.x, -10f, 10f),
            0.0f,
            Mathf.Clamp(body.position.z, -10f, 10f)
        );

        body.rotation = Quaternion.Euler(0.0f, 0.0f, body.velocity.x * -0.5f);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(body.velocity), Time.fixedDeltaTime * turnSpeed);
    }


    // Call this anytime the robot takes damage to decrease its health
    public void TakeDamage(int amount)
    {
        if (!photonView.isMine)
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
        if (!photonView.isMine)
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
    /*
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(body.position);
            stream.SendNext(body.velocity);
        }
        else
        {
            Vector3 syncPosition = (Vector3)stream.ReceiveNext();
            Vector3 syncVelocity = (Vector3)stream.ReceiveNext();

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = body.position;
        }
    }*/
}