using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int vidas = 3;

    private static GameManager audioListenerInstance;
    Audio audioManager;


    void Awake()
    {


        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
        // Si ya existe una instancia del GameManager, destruye este objeto duplicado
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Este objeto no será destruido al cargar nuevas escenas
        }

        // Verificar si ya hay un AudioListener activo
        if (audioListenerInstance == null)
        {
            // Si no hay un AudioListener Singleton, lo creamos
            audioListenerInstance = this;
            DontDestroyOnLoad(this);         }
        else
        {
            // Si ya existe un AudioListener Singleton, destruye el duplicado
            Destroy(gameObject);
        }
    }

    public void ReduceVidas()
    {
        vidas--;
        if (vidas <= 0)
        {
            // Si no hay vidas, ir a la pantalla de Game Over
            SceneManager.LoadScene("GameOver");
        }
    }

    public int GetVidas()
    {
        return vidas;
    }
} 