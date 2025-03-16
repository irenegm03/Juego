using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class HeroKnight : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    Audio audioManager;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool isGrounded = false;
    private bool m_rolling = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;
    public Transform attackPointD;
    public Transform attackPointI;
    public float radius;
    public LayerMask layer;
    public int saludActual = 100;
    public HealthBar healthBar;
    [SerializeField] TextMeshProUGUI texto;



    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
            m_rolling = false;


        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }

        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling)
            m_body2d.linearVelocity = new Vector2(inputX * m_speed, m_body2d.linearVelocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.linearVelocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }


        //Attack
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;

            var numero = Random.Range(0, 3);
            switch (numero)
            {
                case 0: audioManager.Reproducir(audioManager.ataque1); break;
                case 1: audioManager.Reproducir(audioManager.ataque2); break;
                case 2: audioManager.Reproducir(audioManager.ataque3); break;
            }

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
            m_currentAttack = 0;

            Collider2D[] contact;
            if (m_facingDirection == 1)
            {
                contact = Physics2D.OverlapCircleAll(attackPointD.position, radius, layer);
            }
            else
            {
                contact = Physics2D.OverlapCircleAll(attackPointI.position, radius, layer);
            }

            foreach (Collider2D enemy in contact)
            {
                if (enemy.GetComponent<Enemigo>() != null)
                {
                    enemy.GetComponent<Enemigo>().damage(10);
                }
                if (enemy.GetComponent<Boss>() != null)
                {
                    enemy.GetComponent<Boss>().damage(10);
                }

            }

        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.linearVelocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.linearVelocity.y);
        }

        //Jump
        else if (Input.GetKeyDown("space") && isGrounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            isGrounded = false;
            m_animator.SetBool("Grounded", false);
            m_body2d.linearVelocity = new Vector2(m_body2d.linearVelocity.x, m_jumpForce);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

        if (transform.position.y < -10f)
        {
            PerderVida();
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
        m_animator.SetBool("Grounded", true);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            RecibirDaño(20);
        }

        if (collision.gameObject.CompareTag("Nivel2"))
        {
            SceneManager.LoadScene("Nivel2");
        }

        if (collision.gameObject.CompareTag("Nivel3"))
        {
            SceneManager.LoadScene("Nivel3");
        }


    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPointD.position, radius);
        Gizmos.DrawWireSphere(attackPointI.position, radius);
    }

    public void RecibirDaño(int daño)
    {
        saludActual -= daño;
        m_animator.SetTrigger("Hurt");
        Debug.Log("Daño recibido: " + daño + " | Salud actual: " + saludActual);
        healthBar.SetHealth(saludActual);
        if (saludActual <= 0)
        {
            PerderVida();
        }
    }



    void PerderVida() // Llama a reduce vidas que esta en GameManager
    {
        GameManager.instance.ReduceVidas();
        texto.GetComponent<UIText>().setLives(GameManager.instance.GetVidas());

        if (GameManager.instance.vidas > 0)
        {
            saludActual = 100;
            Debug.Log("Te quedan " + GameManager.instance.vidas + " vidas");
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
