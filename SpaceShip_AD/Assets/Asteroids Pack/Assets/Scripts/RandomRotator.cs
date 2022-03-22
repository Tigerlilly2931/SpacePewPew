using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float tumble;

    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "bullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}