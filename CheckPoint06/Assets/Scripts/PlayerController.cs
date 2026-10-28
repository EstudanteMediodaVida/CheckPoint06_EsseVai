using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float speed;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bullet;
    private Vector2 direction;

    private Rigidbody2D rb;
    private bool attack;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        Move();
        Attack();
    }

    void FixedUpdate()
    {
        OnMove();
    }

    void Move()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void OnMove()
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    void Attack()
    {
        attack = Input.GetButtonDown("Fire1");

        if (attack == true)
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}
