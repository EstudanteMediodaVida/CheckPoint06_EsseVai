using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float detonate;

    void Start()
    {
        Destroy(gameObject, detonate);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
