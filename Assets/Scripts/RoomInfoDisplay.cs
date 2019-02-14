using Photon.Pun;
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
        private Image SpriteDueño;

        [SerializeField]
        private GameObject SpriteJugadoresTemplate;

        private List<GameObject> buttons;

        private RoomParameters parameters = new RoomParameters();

        private Scene currentScene;
        public static string SceneName;

        // Use this for initialization
        private void Start()
        {
            Jugador.dias = 0;
            currentScene = SceneManager.GetActiveScene();
            SceneName = currentScene.name;
            buttons = new List<GameObject>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (SceneName == "05 Espera")
            {
                if (PhotonNetwork.InRoom)
                {
                    string[] sala = PhotonNetwork.CurrentRoom.Name.Split('#');
                    foreach (Sprite sprite in GameManager.instance.Avatars)
                    {
                        if (sprite.name == PhotonNetwork.CurrentRoom.CustomProperties["Imagen"] as string)
                        {
                            SpriteDueño.sprite = sprite;
                            SpriteDueño.gameObject.SetActive(true);
                        }
                    }

                    nombreSala.text = sala[0];

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
                
                GameManager.instance.JugadoresEnSala.Clear();

                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    GameManager.instance.JugadoresEnSala.Add(p.NickName);
                }
                GameManager.instance.JugadoresJugados.AddRange(GameManager.instance.JugadoresEnSala);
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