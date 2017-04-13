using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour {

    private Image Cd_3;
    private Image Cd_2;
    private Image Cd_1;
    private Image Cd_Go;
    private Image Ko;
    private Image To;

    public float StartTime;
    public float ElapsedTime;

    public bool isCountingDown = false;

    public int SpriteState_3 = 0;
    public int SpriteState_2 = 0;
    public int SpriteState_1 = 0;
    public int SpriteState_Go = 0;
    public int SpriteState_Ko = 0;
    public int SpriteState_To = 0;

    private void Start() {

       // Finds the child sprites
       Cd_3 = this.transform.Find("Cd_3").gameObject.GetComponent<Image>();
       Cd_2 = this.transform.Find("Cd_2").gameObject.GetComponent<Image>();
       Cd_1 = this.transform.Find("Cd_1").gameObject.GetComponent<Image>();
       Cd_Go = this.transform.Find("Cd_Go").gameObject.GetComponent<Image>();
       To = this.transform.Find("To").gameObject.GetComponent<Image>();
       Ko = this.transform.Find("Ko").gameObject.GetComponent<Image>();

       //Renders them invisible
       Cd_3.canvasRenderer.SetAlpha(0.0f);
       Cd_2.canvasRenderer.SetAlpha(0.0f);
       Cd_1.canvasRenderer.SetAlpha(0.0f);
       Cd_Go.canvasRenderer.SetAlpha(0.0f);
       Ko.canvasRenderer.SetAlpha(0.0f);
       To.canvasRenderer.SetAlpha(0.0f);
    }

    //Resets the alpha of all sprites, stops the current countdown if any is ongoing.
    private void ResetCd() {

       Cd_3.canvasRenderer.SetAlpha(0.0f);
       Cd_2.canvasRenderer.SetAlpha(0.0f);
       Cd_1.canvasRenderer.SetAlpha(0.0f);
       Cd_Go.canvasRenderer.SetAlpha(0.0f);
       Ko.canvasRenderer.SetAlpha(0.0f);
       To.canvasRenderer.SetAlpha(0.0f);

       isCountingDown = false;

       SpriteState_3 = 0;
       SpriteState_2 = 0;
       SpriteState_1 = 0;
       SpriteState_Go = 0;
       SpriteState_Ko = 0;
       SpriteState_To = 0;
    }

    //Starts a new countdown.
    public void NewCountdown() {
        ResetCd();
        isCountingDown = true;
        StartTime = Time.time;
    }

    public void ManageCountdownSprites(){
      if (SpriteState_3==0){
        SpriteState_3++;
        Cd_3.CrossFadeAlpha(1f, 0.1f, true);
      }
      else if ((ElapsedTime > 0.1f) && (SpriteState_3==1)) {
        SpriteState_3++;
        Cd_3.CrossFadeAlpha(0f, 0.7f, true);
      }

      if ((SpriteState_2==0) && (ElapsedTime > 1f)){
        SpriteState_2++;
        Cd_2.CrossFadeAlpha(1f, 0.1f, true);
      }
      else if ((ElapsedTime > 1.1f) && (SpriteState_2==1)) {
        SpriteState_2++;
        Cd_2.CrossFadeAlpha(0f, 0.7f, true);
      }

      if ((SpriteState_1==0) && (ElapsedTime > 2f)){
        SpriteState_1++;
        Cd_1.CrossFadeAlpha(1f, 0.1f, true);
      }
      else if ((ElapsedTime > 2.1f) && (SpriteState_1==1)) {
        SpriteState_1++;
        Cd_1.CrossFadeAlpha(0f, 0.7f, true);
      }

      if ((SpriteState_Go==0) && (ElapsedTime > 3f)){
        SpriteState_Go++;
        Cd_Go.CrossFadeAlpha(1f, 0.1f, true);
      }
      else if ((ElapsedTime > 3.1f) && (SpriteState_Go==1)) {
        SpriteState_Go++;
        Cd_Go.CrossFadeAlpha(0f, 0.7f, true);
        isCountingDown = false;
      }
    }

    public void ManageKoSprite(){
        StartCoroutine (KoSprite());
    }

    IEnumerator KoSprite(){
      Ko.CrossFadeAlpha(1f, 0.1f, true);
      yield return new WaitForSeconds(0.5f);
      Ko.CrossFadeAlpha(0f, 0.5f, true);
    }

    public void ManageToSprite(){
        StartCoroutine (ToSprite());
    }

    IEnumerator ToSprite(){
      To.CrossFadeAlpha(1f, 0.1f, true);
      yield return new WaitForSeconds(0.5f);
      To.CrossFadeAlpha(0f, 0.5f, true);
    }

    private void Update() {

        if (isCountingDown) {
          ElapsedTime = Time.time - StartTime;
          ManageCountdownSprites();
        }
    }


}
