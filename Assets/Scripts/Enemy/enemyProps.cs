using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct rayCastOrigin
{
    public Vector2 bottomLeft, topLeft, bottomRight, topRight;
}

public struct collisionInfo
{
    public bool above, below, left, right;

    public void Reset()
    {
        above = below = left = right = false;
    }
}
[System.Serializable]
public class enemyProps 
{
    public int health;
    public float speed;
    public float points;

    public float fireRate;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public Transform bulletEmissionPoint;
    //bullet destruction particleEffect;


    public rayCastOrigin rayCastOrigins;
    public float skinWidth;
    public int horizontalRayCount;

    public collisionInfo collisionInfo;


}
