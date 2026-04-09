using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _target;

    [Tooltip("The offset of the camera")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private void Start()
    {
        FindPlayer();
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            FindPlayer();
            return;
        }

        transform.position = _target.position + offset;
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            _target = player.transform;
        }
    }
}
