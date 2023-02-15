using UnityEngine;

public class MoveBabySnake : MonoBehaviour
{
    public Transform[] way_points;
    public float speed;
    public Rigidbody2D rb;
    public SpriteRenderer sprite_renderer;
    public int damage = 20;


    private int dest_point = 0;
    private Transform target_wp;


    // Start is called before the first frame update
    void Start()
    {
        target_wp = way_points[0];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
           collision.transform.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target_wp.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target_wp.position) < 0.3)
        {
            dest_point = (dest_point + 1) % way_points.Length;
            target_wp = way_points[dest_point];
            sprite_renderer.flipX = !sprite_renderer.flipX;
        }

    }

}
