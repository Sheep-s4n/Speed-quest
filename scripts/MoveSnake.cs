using UnityEngine;

public class MoveSnake : MonoBehaviour
{
    public Transform[] way_points;
    public float speed;
    public Rigidbody2D rb;
    public int time_btw_jumps;
    public SpriteRenderer sprite_renderer;
    public int damage = 50;


    private WeakSpot weakspot;
    private int dest_point = 0;
    private Transform target_wp;
    private float prev_time;

    // Start is called before the first frame update
    void Start()
    {
        weakspot = transform.GetComponentInChildren<WeakSpot>();
        target_wp = way_points[0];
        weakspot.textMeshPro.SetText(weakspot.boss_life.ToString());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damage == 0) return;
        if (collision.transform.CompareTag("Player") && !transform.GetComponentInChildren<WeakSpot>().is_taking_damage)
        {
            bool is_dead = collision.transform.GetComponent<PlayerHealth>().TakeDamage(damage);
            if (is_dead)
            {
                weakspot.touch = 0;
                weakspot.textMeshPro.SetText(weakspot.boss_life.ToString());
            }
        }
    }


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

        if (prev_time + time_btw_jumps < Time.time)
        {
            rb.AddForce(new Vector2(0f , 100), ForceMode2D.Impulse);
            prev_time = Time.time;
        }
    }

}
