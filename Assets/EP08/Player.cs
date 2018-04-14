using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 10;

    void Update ()
    {
        Vector3 input = new Vector3 ( Input.GetAxisRaw ( "Horizontal" ), 0, Input.GetAxisRaw ( "Vertical" ) );
        Vector3 direction = input.normalized;
        Vector3 velocity = direction * speed;
        Vector3 move_amount = velocity * Time.deltaTime;
        transform.position += move_amount;
    }
}
