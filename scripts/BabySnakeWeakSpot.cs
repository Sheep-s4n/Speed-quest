using UnityEngine;


public class BabySnakeWeakSpot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.GetComponent<MovePlayer>().is_dead) return;
            Destroy(transform.parent.gameObject);
            collision.transform.GetComponent<MovePlayer>().enemy_killed_count++;

        }
    }
}
