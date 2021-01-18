using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(controller))]
public class player : MonoBehaviour
{
    controller controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<controller>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), 0);
        controller.move(input);

    }
}
