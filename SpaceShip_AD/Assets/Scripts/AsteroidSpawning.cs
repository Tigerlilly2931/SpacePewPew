using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawning : MonoBehaviour
{
    public GameObject[] asteroidList = new GameObject[3];
    int astNum;
    Vector2 screenBounds;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 3)
        {
            timer = 0;
            astNum = Random.Range(0, 2);
            GameObject asteroid = Instantiate(asteroidList[astNum]);
            asteroid.transform.position = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), -screenBounds.y, 0);
            //asteroid.GetComponent<Rigidbody>().AddForce(Vector3.down * 2);
            asteroid.GetComponent<Rigidbody>().velocity = Vector3.down * 100;
            Destroy(asteroid, 7f);
        }
    }
}
