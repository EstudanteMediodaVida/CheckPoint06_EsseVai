using System.Collections;
using System.ComponentModel.Design.Serialization;
using Unity.Scripting.LifecycleManagement;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float sight;
    [SerializeField] private float trigger;
    [SerializeField] private float speed;
    [SerializeField] private int hp;

    private Animator anim;
    private SpriteRenderer sr;

    private bool wakeup;
    private bool attacking1;
    private bool attacking;
    private bool taunting;
    private int attackN;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        wakeup = false;
    }

    
    void Update()
    {
        if (wakeup == false)
        {

            if (Vector2.Distance(transform.position, player.transform.position) < sight)
            {
                wakeup = true;
            }
        }
        else
        {
            if(Vector2.Distance(transform.position, player.transform.position) < trigger)
            {
                Attack();
            }
            else
            {
                Move();
            }

            if(Vector2.Distance(transform.position, player.transform.position) > sight)
            {
                wakeup = false;
            }
        }

        AnimationManager();

    }

    void Move()
    {
        if (attacking == true) return;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if(player.transform.position.x > transform.position.x)
        {
            sr.flipX = false;
        }
        else if(player.transform.position.x < transform.position.x)
        {
            sr.flipX = true;
        }
    }

    void Attack()
    {
            StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        attacking = true;
        wakeup = true;
        yield return new WaitForSeconds(1.5f);
        attacking = false;
    }

    void AnimationManager()
    {
        anim.SetBool("pWakeUp", wakeup);
        anim.SetBool("pAttack", attacking);
        anim.SetBool("pDeath", hp <= 0);
        anim.SetBool("pTaunt", taunting);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Attack")
        {
            Destroy(collision.gameObject);
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        hp--;

        if(hp > 0)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
            Destroy(gameObject);
        }
    }
}
