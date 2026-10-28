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
    private bool attacking2;
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
            StartCoroutine(Idling());

            if (Vector2.Distance(transform.position, player.transform.position) < sight)
            {
                StopCoroutine(Idling());
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
        }

        AnimationManager();

    }

    void Move()
    {
        if (attacking1 == true || attacking2 == true) return;

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
        attackN = Random.Range(1, 3);
        
        if (attackN == 1)
        {
            StartCoroutine(Attacking1());
        }
        else if (attackN == 2)
        {
            StartCoroutine(Attacking2());
        }
    }

    IEnumerator Idling()
    {
        taunting = true;
        yield return new WaitForSeconds(1.5f);
        taunting = false;
    }
    
    IEnumerator Attacking1()
    {
        attacking1 = true;
        attacking2 = false;
        wakeup = true;
        yield return new WaitForSeconds(1.8f);
        attacking1 = false;
    }

    IEnumerator Attacking2()
    {
        attacking2 = true;
        attacking1 = false;
        wakeup = true;
        yield return new WaitForSeconds(2.3f);
        attacking2 = false;
    }

    void AnimationManager()
    {
        anim.SetBool("pWakeUp", wakeup);
        anim.SetBool("pAttack1", attacking1);
        anim.SetBool("pAttack2", attacking2);
        anim.SetBool("pDeath", hp <= 0);
        anim.SetBool("pTaunt", taunting);
    }
}
