using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theactualgame
{
    public class PlayerSpawner : MonoBehaviour
    {

        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private CinemachineVirtualCamera playerCamera = null;

        // Start is called before the first frame update
        void Start()
        {
            var player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            playerCamera.Follow = player.transform;
            playerCamera.Follow = player.transform;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


