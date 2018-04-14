using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    public Transform target_to_chase;
    float speed = 7;

    void Update ()
    {
        Vector3 displacement_from_target = target_to_chase.position - transform.position;
        Vector3 direction_to_target = displacement_from_target.normalized;
        Vector3 velocity = direction_to_target * speed;

        if (displacement_from_target.magnitude > 1.5f)
        {
            transform.Translate ( velocity * Time.deltaTime );
        }
    }
}
