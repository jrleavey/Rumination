using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField]
    private CinemachineVirtualCamera _activeCamera;
    private void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _activeCamera.Priority = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _activeCamera.Priority = 0;

        }
    }
}
