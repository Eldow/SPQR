using UnityEngine;

public class GameManager : MonoBehaviour {
    public Running Running = null;

    public static GameManager Instance = null;

    void Awake() {
        if (GameManager.Instance == null) {
            GameManager.Instance = this;
        } else if (GameManager.Instance != this) {
            Destroy(gameObject);
        }

        GameObject.DontDestroyOnLoad(gameObject);

        if (this.Running != null) return;

        this.Running = this.gameObject.GetComponent<Running>();

        if (this.Running == null) {
            Debug.LogError(this.GetType().Name + ": No Running script found!");
        }
    }
}
