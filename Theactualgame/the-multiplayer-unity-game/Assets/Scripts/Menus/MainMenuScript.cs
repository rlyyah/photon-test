﻿using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Theactualgame.Menus
{

    public class MainMenuScript : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject findOpponentPanel = null;
        [SerializeField] private GameObject waitingStatusPanel = null;
        [SerializeField] private TextMeshProUGUI waitingStatusText = null;

        private bool isConnecting = false;

        private const string GameVersion = "0.1";
        private const int MaxPlayersPerRoom = 2;

        // all players in the lobby needs to be synced
        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        public void FindOpponent()
        {
            isConnecting = true;

            findOpponentPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Master");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            findOpponentPanel.SetActive(true);

            Debug.Log($"Disconnected due to: {cause}");


        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No Clients are waiting for an opponent, creatng new room!");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client Successfully joined a room");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if (playerCount != MaxPlayersPerRoom)
            {
                waitingStatusText.text = "Waiting for opponent";
                Debug.Log("Client is waiting for an opponent");
            }
            else
            {
                waitingStatusText.text = "Opponent Found!";
                Debug.Log("Match is ready to begin!");
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;

                waitingStatusText.text = "Opponent Found!";
                Debug.Log("Match is ready to begin");

                PhotonNetwork.LoadLevel("Scene_Main");
            }
        }
    }
}


