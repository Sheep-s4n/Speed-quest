using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject character;
    public Vector3 posOffset;

    private Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, character.transform.position + posOffset, ref vel, 0);
    }
}
