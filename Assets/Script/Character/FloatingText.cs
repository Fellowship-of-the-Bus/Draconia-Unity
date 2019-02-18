using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {
  public Rigidbody phys;
  public Text txt;

  public void animate(int val, Color colour) {
    txt.gameObject.transform.localPosition = Vector3.zero;
    txt.text = val.ToString();
    txt.color = colour;
    phys.useGravity = false;
    phys.velocity = new Vector3(0, 1f);
    gameObject.SetActive(true);
  }
}
