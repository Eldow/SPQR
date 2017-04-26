using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitEnter : MonoBehaviour {
    public InputField InputField = null;
    
    void OnGUI() {
        if (this.InputField == null) return;

        if (this.IsCallback()) {
            this.Callback();
            this.InputField.text = "";

            EventSystem.current.SetSelectedGameObject(
                this.InputField.gameObject, null);
            this.InputField.OnPointerClick(
                new PointerEventData(EventSystem.current));
        }
    }

    protected virtual bool IsCallback() {
        return this.InputField.isFocused && this.InputField.text != "" &&
            Input.GetKey(KeyCode.Return);
    }

    protected virtual void Callback() {
        Debug.LogWarning("Callback not implemented");
    }
}
