using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaIntroduccion : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Nivel1");
        }
    }
}
