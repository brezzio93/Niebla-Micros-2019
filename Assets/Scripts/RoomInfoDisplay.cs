﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class RoomInfoDisplay : MonoBehaviour
    {
        [SerializeField]
        private Text jugadores, nombreSala;

        [SerializeField]
        private Image SpriteDuenno;

        private List<GameObject> buttons;

        [SerializeField]
        private Button comenzarButton;

        private RoomParameters parameters = new RoomParameters();

        private Scene currentScene;
        public static string SceneName;

        // Use this for initialization
        private void Start()
        {
            Jugador.diaActual = 0;
            currentScene = SceneManager.GetActiveScene();
            SceneName = currentScene.name;
            buttons = new List<GameObject>();
            if (SceneName == "05 Espera")
            {
                if (!PhotonNetwork.IsMasterClient) comenzarButton.gameObject.SetActive(false);
                else comenzarButton.gameObject.SetActive(true);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (SceneName == "05 Espera")
            {
                if (PhotonNetwork.InRoom)
                {
                    string[] sala = PhotonNetwork.CurrentRoom.Name.Split('#');
                    nombreSala.text = sala[0];
                    string sprite_name = PhotonNetwork.CurrentRoom.CustomProperties["Imagen"] as string;
                    var face = GameManager.instance.GetAvatarFaces(sprite_name);

                    SpriteDuenno.sprite = face.happy;
                    SpriteDuenno.gameObject.SetActive(true);

                    PlayersInRoom();
                }
            }
        }

        /// <summary>
        /// Se lista a los jugadores dentro de la sala actual
        /// </summary>
        private void PlayersInRoom()
        {
            int i = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.PlayerCount);
            jugadores.text = "Jugadores Conectados:\n" + System.Convert.ToString(PhotonNetwork.CurrentRoom.PlayerCount)
                                + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        }

        /// <summary>
        /// El host actualiza los parametros de la sala y comienza el juego
        /// </summary>
        public void ComenzarJuego()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameManager.LevantarEventos(GameManager.CodigoEventosJuego.NuevoJuego, null, ReceiverGroup.All);

                
                LoadArena(6);
            }
        }

        private void LoadArena(int scene)
        {
            PhotonNetwork.LoadLevel(scene);
        }

        public void JoinSelectedRoom()
        {
            if (ServerManager.roomSelected != null)
                PhotonNetwork.JoinRoom(ServerManager.roomSelected);
            else PhotonNetwork.JoinRandomRoom();
        }
    }
}