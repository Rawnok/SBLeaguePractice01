using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EP09
{
    public class CubeScript : MonoBehaviour
    {
        public Transform sphere_transform;

        void Start ()
        {
            sphere_transform.SetParent ( transform );
        }

        void Update ()
        {
            //transform.eulerAngles += new Vector3 ( 0, 180 * Time.deltaTime, 0 );
            //transform.eulerAngles += Vector3.up * 180 * Time.deltaTime;

            transform.Rotate ( Vector3.up * Time.deltaTime * 180, Space.Self );
        }
    }

}