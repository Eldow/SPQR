using UnityEngine;

public abstract class FollowTarget : MonoBehaviour
{

    [SerializeField]
    public Transform target;
    [SerializeField]
    private bool autoTargetPlayer = true;
    public bool lockCamera = false;
    public XboxInput xboxInput;
    GameObject player, opponent;

    virtual protected void Start()
    {
        if (autoTargetPlayer)
        {
            FindTargetPlayer();
        }
    }

    void FixedUpdate()
    {
        if (autoTargetPlayer && (target == null || !target.gameObject.activeSelf))
        {
            FindTargetPlayer();
        }
        if (target != null && xboxInput != null)
        {
            if (Input.GetKeyUp(xboxInput.LB))
            {
                FindTargetPlayer();
            }
            if (Input.GetKey(xboxInput.LB))
            {
                LookAtOpponent();
            }
            Follow(Time.deltaTime, lockCamera);
        }
    }

    protected abstract void Follow(float deltaTime, bool lockCamera);


    public void FindTargetPlayer()
    {
        lockCamera = false;
        player = TargetManager.instance.player;
        if (player != null)
        {
            if(xboxInput == null)
            {
                xboxInput = player.GetComponent<PlayerController>().xboxInput;
            }
            SetTarget(player.transform);
            player.GetComponent<PlayerController>().lockedMovement = false;
        }
    }

    public void LookAtOpponent()
    {
        lockCamera = true;
        opponent = TargetManager.instance.GetNearestOpponent();
        if (player != null && opponent != null)
        {
            Quaternion neededRotation = Quaternion.LookRotation(opponent.transform.position - player.transform.position);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, neededRotation, Time.deltaTime * 5f);
            player.GetComponent<PlayerController>().lockedMovement = true;
        }
    }

    public virtual void SetTarget(Transform newTransform)
    {
        target = newTransform;
    }
    public Transform Target { get { return this.target; } }
}
