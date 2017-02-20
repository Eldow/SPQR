using UnityEngine;

public abstract class FollowTarget : MonoBehaviour
{

    [SerializeField]
    public Transform target;
    [SerializeField]
    private bool autoTargetPlayer = true;
    public bool lockCamera = false;
    GameObject player, opponent;
    private GameObject healthBar;

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
        if (target != null)
        {
            if (InputManager.cameraButtonDown())
            {
                SwitchCameraMode();
            }
            if (lockCamera)
            {
                LookAtOpponent();
            } else
            {
                FindTargetPlayer();
            }
            Follow(Time.deltaTime, lockCamera);
        }
    }

    protected abstract void Follow(float deltaTime, bool lockCamera);


    public void FindTargetPlayer()
    {
        player = TargetManager.instance.player;
        if (player != null)
        {
            SetTarget(player.transform);
            player.GetComponent<PlayerController>().lockedMovement = false;
        }
    }

    public void LookAtOpponent()
    {
        opponent = TargetManager.instance.GetNearestOpponent();
        if (player != null && opponent != null)
        {
            Quaternion neededRotation = Quaternion.LookRotation(opponent.transform.position - player.transform.position);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, neededRotation, Time.deltaTime * 5f);
            player.GetComponent<PlayerController>().lockedMovement = true;
        }
    }

    public void SwitchCameraMode()
    {
        lockCamera = !lockCamera;
        opponent = TargetManager.instance.GetNearestOpponent();
        if(opponent != null)
        {
            if (lockCamera)
            {
                healthBar = opponent.GetComponent<PlayerController>().opponentInfo;
            }
            healthBar.SetActive(lockCamera);
        }
    }

    public virtual void SetTarget(Transform newTransform)
    {
        target = newTransform;
    }
    public Transform Target { get { return this.target; } }
}
