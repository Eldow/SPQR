using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour {

    public Text timerLabel;

    private float time;

    void Start() {

        timerLabel.text = "99";

    }

    void Update() {

        time += Time.deltaTime;
        var seconds = 99 - time % 60;

        if (true) { // Replace true by the "round starts" condition.
            timerLabel.text = string.Format ("{0:00}", Mathf.Ceil(seconds));
        }

        if (Mathf.Ceil(seconds)==0) {
            //End the round
        }

    }
}
