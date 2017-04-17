using UnityEngine;
using UnityEngine.UI;

public class PlayLoop : MonoBehaviour {
    public RawImage RawImage;
    [HideInInspector]
    public MovieTexture MovieTexture;

    void Start() {
        this.MovieTexture = RawImage.texture as MovieTexture; 
        this.MovieTexture.Play();
    }
}