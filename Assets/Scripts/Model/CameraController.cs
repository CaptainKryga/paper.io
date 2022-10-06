using System;
using Model.Entity;
using UnityEngine;

namespace Model
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private PlayerMove player;
        [SerializeField] private Transform body;
        private bool isPlay;

        private void Start()
        {
            body = player.Body;
        }

        private void Update()
        {
            cam.transform.position = new Vector3(body.position.x, body.position.y, cam.transform.position.z);
        }
    }
}
