using UnityEngine;

public class AudioCharacterSelectionEvents : MonoBehaviour {

    public void SwordMove() {
        Fabric.EventManager.Instance.PostEvent("MenuCharSelectSwordMove", this.gameObject);
    }

    public void SwordSlide() {
        Fabric.EventManager.Instance.PostEvent("MenuCharSelectSwordSlide", this.gameObject);
    }
}
