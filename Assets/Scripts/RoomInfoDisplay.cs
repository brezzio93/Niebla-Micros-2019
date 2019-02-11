using Photon.Pun;
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

        private RoomParameters parameters = new RoomParameters();

        private Scene currentScene;
        public static string SceneName;

        // Use this for initialization
        private void Start()
        {
            currentScene = SceneManager.GetActiveScene();
            SceneName = currentScene.name;
        }

        // Update is called once per frame
        private void Update()
        {
            //Debug.Log("Avatar " + PhotonNetwork.CurrentRoom.Name);
            if (SceneName == "05 Espera")
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (Sprite sprite in GameManager.instance.Avatars)
                    {
                        if (sprite.name == PhotonNetwork.CurrentRoom.CustomProperties["Imagen"] as string)
                        {
                            SpriteDueño.sprite = sprite;
                            SpriteDueño.gameObject.SetActive(true);
                        }
                    }

                    nombreSala.text = "Sala de: " + PhotonNetwork.CurrentRoom.Name;
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
            while (i != 0)
            {
                i--;
                Debug.Log("Players in Room: " + PhotonNetwork.PlayerList[i].NickName);
            }
        }

        /// <summary>
        /// El host actualiza los parametros de la sala y comienza el juego
        /// </summary>
        public void ComenzarJuego()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //parameters.SetRoomProperties();
                //Debug.Log("Monto (Sala): " + PhotonNetwork.CurrentRoom.CustomProperties["monto"]);
                LoadArena();
            }
        }

        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Player on the Room : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("06 Sala");
        }

        public void JoinSelectedRoom()
        {
            if (ServerManager.roomSelected != null)
                PhotonNetwork.JoinRoom(ServerManager.roomSelected);
            else PhotonNetwork.JoinRandomRoom();
        }
    }
}