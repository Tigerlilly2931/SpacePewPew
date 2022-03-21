using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTilting : MonoBehaviour
{
    // Move object using accelerometer
    public float speed = 10.0f;
    Rigidbody shiprb;
    private Vector2 screenBounds;
    float objectWidth, objectHeight;
    private void Start()
    {
        shiprb = gameObject.GetComponent<Rigidbody>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = gameObject.GetComponent<BoxCollider>().size.x / 2;
        objectHeight = gameObject.GetComponent<BoxCollider>().size.y / 2;
    }
    void Update()
    {
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
        transform.Translate(dir * speed);
    }
    private void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectWidth, screenBounds.y * -1 - objectHeight);
        transform.position = viewPos;
    }
}
