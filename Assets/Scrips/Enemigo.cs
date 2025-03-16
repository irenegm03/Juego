using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float vida;
    private Animator animator;
    private Rigidbody2D body2d;
    Audio audioManager;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void damage(int damage)
    {
        vida -= damage;
        
        if (vida <= 0)
        {
            animator.SetBool("isDead", true);
            audioManager.Reproducir(audioManager.matar);
            if (GetComponent<Patrullar>() != null)
            {
                GetComponent<Patrullar>().parar();
            }
            GetComponent<BoxCollider2D>().enabled = false;
            this.enabled = false;
        }
    }
}
