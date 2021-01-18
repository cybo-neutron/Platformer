using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum gunType
{
    laser,bullet
}
public class Gun : MonoBehaviour
{
    public gunType GunType;
    public float speed;
    public float fireRate;
    public GameObject bulletPrefab;
    public Transform bulletEmissionPoint;

    float nextFireTime;
    public float bulletLifeTime;

    Vector2 cursorPosition;

    // Start is called before the first frame update
    void Start()
    {
        nextFireTime = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        cursorPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2((cursorPosition.y - transform.position.y) ,(cursorPosition.x - transform.position.x))*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(Input.GetMouseButtonDown(0))
        {
            shoot();
        }
        

        
    }

    void shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletEmissionPoint.position, bulletEmissionPoint.rotation);

        bullet script = bullet.GetComponent<bullet>();
        script.init(speed, bulletLifeTime);

        //Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        //rb.AddForce(transform.right*speed,ForceMode2D.Impulse);
        

        



    }
}
