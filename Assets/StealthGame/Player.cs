using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StealthGame
{

    public class Player : MonoBehaviour
    {
        public static event System.Action OnGameWon;

        public float move_speed = 7;
        public float turn_speed = 5;

        float smooth_time = 0.1f;

        float current_angle;
        float smooth_damp_velocity;
        float smoothing_magnitude;
        Vector3 rigidbody_velocity;

        bool isDisabled = false;
        Rigidbody m_rigidbody;

        void Start ()
        {
            Guard.OnPlayerSpotted += OnGameOver;
            isDisabled = false;
            m_rigidbody = GetComponent<Rigidbody> ();
            //Debug.Log ( Mathf.Atan2 ( 0, 0 ) * Mathf.Rad2Deg );
            //Debug.Log ( Mathf.Atan2 ( 0, 1 ) * Mathf.Rad2Deg );
            //Debug.Log ( Mathf.Atan2 ( 1, 0 ) * Mathf.Rad2Deg );
            //Debug.Log ( Mathf.Atan2 ( 1, 1 ) * Mathf.Rad2Deg );

            ////Debug.Log ( Mathf.Atan2 ( 0, 0 ) );
            //Debug.Log ( Mathf.Atan2 ( 0, -1 ) * Mathf.Rad2Deg );
            //Debug.Log ( Mathf.Atan2 ( -1, 0 ) * Mathf.Rad2Deg );
            //Debug.Log ( Mathf.Atan2 ( -1, -1 ) * Mathf.Rad2Deg );

            
        }

        private void OnGameOver ()
        {
            isDisabled = true;
            Guard.OnPlayerSpotted -= OnGameOver;
        }

        void Update ()
        {
            Vector3 inputDirec = new Vector3 ( Input.GetAxisRaw ( "Horizontal" ), 0, Input.GetAxisRaw ( "Vertical" ) ).normalized;
            float magnitude = inputDirec.magnitude;
            float inputAngle = Mathf.Atan2 ( inputDirec.x, inputDirec.z ) * Mathf.Rad2Deg;
            
            //working with angles
            current_angle = Mathf.LerpAngle ( current_angle, inputAngle, Time.deltaTime * turn_speed * magnitude );
            //transform.eulerAngles = Vector3.up * current_angle ;

            //working with movement
            smoothing_magnitude = Mathf.SmoothDamp ( smoothing_magnitude, magnitude, ref smooth_damp_velocity, smooth_time );
            //transform.Translate ( Vector3.forward * smoothing_magnitude * move_speed * Time.deltaTime );
            rigidbody_velocity = transform.forward * smoothing_magnitude * move_speed;
        }

        private void FixedUpdate ()
        {
            if (!isDisabled)
            {
                m_rigidbody.MoveRotation ( Quaternion.Euler ( Vector3.up * current_angle ) );
                m_rigidbody.MovePosition ( m_rigidbody.position + rigidbody_velocity * Time.deltaTime );
            }
        }


        private void OnDrawGizmos ()
        {
            Gizmos.DrawRay ( transform.position, transform.forward.normalized * 10  );
        }

        private void OnTriggerEnter ( Collider other )
        {
            if (other.tag.Equals( "Finish" ) )
            {
                isDisabled = true;
                OnGameWon ();
            }
        }

    }//class

}//namespace