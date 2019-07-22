using UnityEngine;

namespace RPG.Core
{
    // this was replaced by Cinemachine's follow camera - code not currently being used.
    public class FollowCamera : MonoBehaviour
    {
        // config params
        [SerializeField] Transform target;

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
