using UnityEngine;
using UnityEngine.SceneManagement;
public class Boss : MonoBehaviour
{
    public int vida;
    private Animator animator;
    private Rigidbody2D body2d;
    Audio audioManager;
    public HealthBar healthBar;
    public Transform player;
    public GameObject player1;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        body2d = GetComponent<Rigidbody2D>();
        player1 = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void damage(int damage)
    {
        vida -= damage;
        healthBar.SetHealth(vida);
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
            SceneManager.LoadScene("Fin");
        }
    }

	public bool isFlipped = false;

	public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}

    public int attackDamage = 20;

	public Vector3 attackOffset;
	public float attackRange = 3f;
	public LayerMask attackMask;

	public void Attack()
	{
		Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Collider2D colInfo = Physics2D.OverlapCircle(body2d.transform.position, attackRange, attackMask);
       
		if (colInfo != null)
		{
            Debug.Log("entra");
			player1.GetComponent<HeroKnight>().RecibirDaño(attackDamage);
		}
	}

    void OnDrawGizmosSelected()
	{
		Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Gizmos.DrawWireSphere(pos, attackRange);
	}

}
