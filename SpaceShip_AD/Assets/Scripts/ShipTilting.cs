using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ShipTilting : MonoBehaviour
{
    // Move object using accelerometer
    public float speed = 10.0f;
    Rigidbody shiprb;
    private Vector2 screenBounds;
    float objectWidth, objectHeight;
    public GameObject parentForFinger;
    bool canShipTilt;
    Vector3 touchStartPos;
    Vector3 direction;

    int health = 5;
    public Text healthTxt;
    bool isPaused = false;
    public GameObject pausedMenu;
    public GameObject bullet;
    private void Start()
    {
        shiprb = gameObject.GetComponent<Rigidbody>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = gameObject.GetComponent<BoxCollider>().size.x / 2;
        objectHeight = gameObject.GetComponent<BoxCollider>().size.y / 2;
        canShipTilt = true;
        health = 5;
        isPaused = false;
        Time.timeScale = 1;
        pausedMenu.SetActive(false);
        InvokeRepeating("Pewpew", 0.1f, 1f);
    }
    void Update()
    {
        healthTxt.text = health + " / 5";
        Vector3 dir = Vector3.zero;
        // we assume that the device is held parallel to the ground
        // and the Home button is in the right hand

        // remap the device acceleration axis to game coordinates:
        // 1) XY plane of the device is mapped onto XZ plane
        // 2) rotated 90 degrees around Y axis

        dir.y = Input.acceleration.y;
        dir.x = Input.acceleration.x;

        // clamp acceleration vector to the unit sphere
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        // Make it move 10 meters per second instead of 10 meters per frame...
        dir *= Time.deltaTime;

        // Move object
        if (canShipTilt)
        {

            transform.Translate(dir * speed);
        }

        if (Input.touchCount > 0)
        {
            // The screen has been touched so store the touch
            Touch touch = Input.GetTouch(0);

            /*if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 30));
                parentForFinger.transform.position = touchPosition;
                gameObject.transform.parent = parentForFinger.transform;
                // JUMP DRIVE: ship teleports to where the finger touched down
                // TODO: FIX THIS!  It's dumb
                //transform.position = touchPosition;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 30));
                parentForFinger.transform.position = touchPosition*8;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                gameObject.transform.parent = null;
            }*/
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position. Save it as our touchStartPos
                    canShipTilt = false;
                    touchStartPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 30));
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 30));
                    direction = touchPosition - touchStartPos;
                    transform.Translate(direction*.2f);
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    // maybe reset the touchStartPos?
                    // anyway, if we NEED to reset something, do it here
                    canShipTilt = true;
                    break;
            }
        }
    }
    private void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectWidth, screenBounds.y * -1 - objectHeight);
        viewPos.z = 0;
        transform.position = viewPos;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            health--;
            Destroy(other.gameObject);
        }
        if(health <= 0)
        {
            //end game
            SceneManager.LoadScene(0);
        }
    }

    public void PauseGame()
    {
        Debug.Log("Pause Game Button Pressed");
        if (!isPaused)
        {
            pausedMenu.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("We paused");
        }
        if (isPaused)
        {
            pausedMenu.SetActive(false);
            Time.timeScale = 1;
            Debug.Log("We play");
        }
            isPaused = !isPaused;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    void Pewpew()
    {
        Vector3 bulletPlace = new Vector3(gameObject.transform.position.x-6f, gameObject.transform.position.y + 50f, 0);
        GameObject pewpewBullet = Instantiate(bullet, gameObject.transform);
        pewpewBullet.transform.position = bulletPlace;
        pewpewBullet.GetComponent<Rigidbody>().velocity = Vector3.up * 200f;
        Destroy(pewpewBullet, 6f);
    }
}
