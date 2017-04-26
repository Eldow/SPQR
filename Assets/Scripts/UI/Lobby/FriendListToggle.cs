using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendListToggle : MonoBehaviour {

    public GameObject FriendList;

    public void ToggleFriendList()
    {
        SoundManager.instance.PlayClick();
        if (FriendList.activeInHierarchy)
            FriendList.SetActive(false);
        else
            FriendList.SetActive(true);
    }
}
