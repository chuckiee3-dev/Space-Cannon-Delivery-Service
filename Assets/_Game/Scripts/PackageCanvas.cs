using TMPro;
using UnityEngine;

public class PackageCanvas : MonoBehaviour {

    [ SerializeField ] private TextMeshProUGUI amountTmp;
    [ SerializeField ] private Canvas canvas;

    public void SetAmount(int amount) {
        amountTmp.text = "<size=10>x</size>" + amount.ToString();
    }

    public void Show() {
        canvas.enabled = true;
    }
    public void Hide() {
        canvas.enabled = false;
    }

}
