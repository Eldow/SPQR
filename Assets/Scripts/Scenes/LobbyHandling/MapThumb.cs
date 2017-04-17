using UnityEngine;
using UnityEngine.UI;

public class MapThumb : MonoBehaviour {
    public RawImage Thumb;
    public Texture Random;
    public Texture Donut;
    public Texture Coliseum;
    public Texture FreeFall;
    public Text Label;

    private string _previousLabel;

    void Start() {
        this._previousLabel = this.Label.text;
    }

    void Update () {
        if (this._previousLabel == this.Label.text) return;

        this._previousLabel = this.Label.text;

        if (this._previousLabel == "Coliseum") {
            this.Thumb.texture = this.Coliseum;
        } else if (this._previousLabel == "Free Fall") {
            this.Thumb.texture = this.FreeFall;
        } else if (this._previousLabel == "Any") {
            this.Thumb.texture = this.Random;
        } else if (this._previousLabel == "Donuts") {
            this.Thumb.texture = this.Donut;
        } else {
            this.Thumb.texture = this.Random;
        }
    }
}
