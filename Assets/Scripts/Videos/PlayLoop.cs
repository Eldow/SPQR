using UnityEngine;
using UnityEngine.UI;

public class PlayLoop : MonoBehaviour {
    public RawImage RawImage;
    [HideInInspector]
    public MovieTexture MovieTexture;

    void Start() {
        if (RawImage.texture == null) return;
        this.MovieTexture = RawImage.texture as MovieTexture;
        this.MovieTexture.Play();
    }
}