using System.Collections;
using UnityEngine;
using System;

namespace StealthGame
{
    public class Guard : MonoBehaviour
    {

        public static event Action OnPlayerSpotted;

        public Transform path_holder;

        private Vector3[] waypoints;

        [SerializeField]
        private float speed = 5f;

        [SerializeField]
        private float turn_speed = 90;

        [SerializeField]
        private float wait_time = 1;

        [SerializeField]
        private float max_spot_time_threshold = 1f;

        [SerializeField]
        private Light spotlight;

        [SerializeField]
        private float spot_angle;

        [SerializeField]
        private Transform player_transform;

        [SerializeField]
        private float view_distance;

        [SerializeField]
        private LayerMask mask_layer;

        private float current_player_visible_timer = 0;

        private Color original_spot_light_color;
        private Rigidbody m_rigidbody;

        private void Start ()
        {
            original_spot_light_color = spotlight.color;
            spot_angle = spotlight.spotAngle;

            m_rigidbody = GetComponent<Rigidbody> ();
            waypoints = new Vector3[path_holder.childCount];
            for ( int i = 0; i < waypoints.Length; i++ )
            {
                waypoints[i] = path_holder.GetChild ( i ).position;
                waypoints[i] = new Vector3 ( waypoints[i].x, transform.position.y, waypoints[i].z );
            }

            StartCoroutine ( Patrol () );
        }

        private void Update ()
        {
            if ( CanSeePlayer () )
            {
                current_player_visible_timer += Time.deltaTime;
            }
            else
            {
                current_player_visible_timer -= Time.deltaTime;
            }

            current_player_visible_timer = Mathf.Clamp ( current_player_visible_timer, 0, max_spot_time_threshold );
            spotlight.color = Color.Lerp ( original_spot_light_color, Color.red, current_player_visible_timer / max_spot_time_threshold );

            if ( current_player_visible_timer >= max_spot_time_threshold )
            {
                if ( OnPlayerSpotted != null )
                {
                    OnPlayerSpotted ();
                }
            }
        }

        bool CanSeePlayer ()
        {
            if ( Vector3.Distance ( transform.position, player_transform.position ) <= view_distance )
            {
                Vector3 diffDirection = ( player_transform.position - transform.position ).normalized;
                float angle_between = Vector3.Angle ( transform.forward, diffDirection );
                if ( Mathf.Abs ( angle_between ) <= ( spot_angle + 10 ) / 2 )
                {
                    if ( !Physics.Linecast ( transform.position, player_transform.position, mask_layer ) )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        IEnumerator Patrol ()
        {
            transform.position = waypoints[0];
            int next_waypoint_index = 1;
            Vector3 target_point = waypoints[next_waypoint_index];
            transform.LookAt ( target_point );
            yield return new WaitForSeconds ( wait_time );

            // Sebastian
            while ( true )
            {
                transform.position = Vector3.MoveTowards ( transform.position, target_point, speed * Time.deltaTime );
                if ( Vector3.Equals ( transform.position, target_point ) )
                {
                    //rotate around 
                    next_waypoint_index = ++next_waypoint_index % waypoints.Length;
                    target_point = waypoints[next_waypoint_index];
                    yield return new WaitForSeconds ( wait_time );
                    yield return StartCoroutine ( LookTowardsTarget ( target_point ) );
                }
                yield return null;
            }

            #region My way
            //while (true)
            //{
            //    if ( Vector3.Distance ( transform.position, waypoints[next_waypoint_index] ) < 0.1f )
            //    {
            //        //Debug.Log ( "before " + current_waypoint_index );
            //        rigidbody.velocity = Vector3.zero;
            //        yield return new WaitForSeconds ( wait_time );
            //        current_waypoint_index = ++current_waypoint_index % waypoints.Length;
            //        next_waypoint_index = ++next_waypoint_index % waypoints.Length;
            //    }
            //    else
            //    {
            //        Vector3 direction = ( waypoints[current_waypoint_index] - waypoints[next_waypoint_index] ).normalized;
            //        rigidbody.velocity = ( -direction * speed * Time.deltaTime );
            //    }
            //    yield return null;
            //} 
            #endregion
        }

        IEnumerator LookTowardsTarget ( Vector3 target )
        {
            Vector3 direction = ( target - transform.position ).normalized;
            float angle_in_degrees = Mathf.Atan2 ( direction.x, direction.z ) * Mathf.Rad2Deg;

            while ( Mathf.Abs ( Mathf.DeltaAngle ( transform.eulerAngles.y, angle_in_degrees ) ) > 0.05f )
            {
                float angle_per_frame = Mathf.MoveTowardsAngle ( transform.eulerAngles.y, angle_in_degrees, turn_speed * Time.deltaTime );
                transform.eulerAngles = Vector3.up * angle_per_frame;
                yield return null;
            }
        }


        private void OnDrawGizmos ()
        {
            Vector3 start_position = path_holder.GetChild ( 0 ).position;
            Vector3 previous_position = start_position;
            foreach ( Transform item in path_holder )
            {
                Gizmos.DrawSphere ( item.position, 0.15f );
                Gizmos.DrawLine ( previous_position, item.position );
                previous_position = item.position;
            }

            Gizmos.DrawLine ( previous_position, start_position );

            Gizmos.DrawRay ( transform.position, transform.forward * view_distance );
        }
    } 
}
