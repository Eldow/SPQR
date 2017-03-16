using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoundScoreTracker : MonoBehaviour {

    private float time;
    private int victory;

    public Text roundsWon;

    void Start() {
        victory = 0;
        roundsWon.text = "";
    }

    void AddVictory() {
        roundsWon.text = string.Concat(roundsWon.text, "V ");
    }

    void AddPerfectVictory() {
        roundsWon.text = string.Concat(roundsWon.text, "P ");
    }

    void AddTimeoutVictory() {
        roundsWon.text = string.Concat(roundsWon.text, "T ");
    }

    void AddRingoutVictory() {
        roundsWon.text = string.Concat(roundsWon.text, "R ");
    }

    void Update() {

      time += Time.deltaTime;
      float seconds = 99 - time % 60;

      if ((Mathf.Ceil(seconds)==90)&&(victory==0)) {
          victory++;
          AddVictory();
      }

      if ((Mathf.Ceil(seconds)==85)&&(victory==1)) {
          victory++;
          AddPerfectVictory();
      }

      if ((Mathf.Ceil(seconds)==80)&&(victory==2)) {
          victory++;
          AddTimeoutVictory();
      }

      if ((Mathf.Ceil(seconds)==75)&&(victory==3)) {
          victory++;
          AddRingoutVictory();
      }

    }
}
