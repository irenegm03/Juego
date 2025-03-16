using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI texto;

    
    void Start()
    {
        texto.text = "x" + GameManager.instance.GetVidas();
    }

    public void setLives(int vidas) {
        texto.text = "x" + vidas;
    }
}
