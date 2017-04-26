using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsDisplayer : MonoBehaviour {
    public GameObject KeyBinds = null;

    public void Display() {
        if (this.KeyBinds == null) return;

        this.KeyBinds.SetActive(true);
    }

    public void Hide() {
        if (this.KeyBinds == null) return;

        this.KeyBinds.SetActive(false);
    }
}
