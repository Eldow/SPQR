using UnityEngine;
using UnityEngine.UI;

public class ModeThumb : MonoBehaviour {
    public RawImage Thumb;
    public Texture BO3;
    public Texture BO5;
    public Texture BO7;
    public Texture Stock;
    public Texture Random;
    public Text Label;

    private string _previousLabel;

    void Start() {
        this._previousLabel = this.Label.text;
    }

    void Update () {
        if (this._previousLabel == this.Label.text) return;

        this._previousLabel = this.Label.text;

        if (this._previousLabel == "BO3") {
            this.Thumb.texture = this.BO3;
        } else if (this._previousLabel == "BO5") {
            this.Thumb.texture = this.BO5;
        } else if (this._previousLabel == "Any") {
            this.Thumb.texture = this.Random;
        } else if (this._previousLabel == "BO7") {
            this.Thumb.texture = this.BO7;
        } else if (this._previousLabel == "Stock") {
            this.Thumb.texture = this.Stock;
        } else {
            this.Thumb.texture = this.Random;
        }
    }
}
