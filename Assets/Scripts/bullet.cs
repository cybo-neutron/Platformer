using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    float lifeTime;
    [SerializeField] float speed;

    
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    
    void Update()
    {
        transform.Translate(Vector2.right * this.speed * Time.deltaTime);
    }

    public void init(float speed,float lifeTime)
    {
        this.speed = speed;
        this.lifeTime = lifeTime;

    }



}
