using UnityEngine;

public class CameraRobotEnabler : MonoBehaviour {
    public GameObject RobotCamera = null;

    void Start () {
        this.RobotCamera.SetActive(true);
    }

    void Update () {
		
    }
}
