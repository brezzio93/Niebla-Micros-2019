using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class ServerManager : MonoBehaviourPunCallbacks
    {
        private List<string> roomName = new List<string>();
        private Dictionary<string, RoomInfo> cachedRoomList;
        private RoomParameters parameters = new RoomParameters();

        private AtlasLoader atlas = new AtlasLoader();

        private Scene currentScene;
        private string SceneName;

        [SerializeField]
        private GameObject buttonTemplate;

        [SerializeField]
        private Image avatar;

        private List<GameObject> buttons;
        public static string roomSelected;
        public static bool createRoom;

        // Use this for initialization
        public void Awake()
        {
            cachedRoomList = new Dictionary<string, RoomInfo>();
        }

        private void Start()
        {
            currentScene = SceneManager.GetActiveScene();
            SceneName = currentScene.name;
            buttons = new List<GameObject>();
        }

        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                //LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                //LoadArena();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            UpdateCachedRoomList(roomList);
        }

        #endregion Photon Callbacks

        public void SwitchScenes(int idScene)
        {
            SceneManager.LoadScene(idScene);
        }

        /// <summary>
        /// Función utilizada para saber si el jugador creará o se unirá a una sala
        /// </summary>
        public void CreateOrJoin()
        {
            Debug.Log(PlayerParameters.ChosenName);

            if (PlayerParameters.ChosenName != null)
            {
                if (PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.JoinLobby();
                    Debug.Log("CreateOrJoin " + createRoom);
                    if (createRoom == true) SwitchScenes(4);
                    else SwitchScenes(3);
                }
            }
        }

        /// <summary>
        /// Se crea la sala con los parametros ingresados por el Host
        ///  </summary>
        public void CrearSala()
        {
            Debug.Log("CrearSala()");
            int cantidad = System.Convert.ToInt32(RoomParameters.param.cantidad);

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.CustomRoomPropertiesForLobby = new string[1] { "Imagen" };
            roomOptions.MaxPlayers = System.Convert.ToByte(cantidad);
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Imagen", PlayerParameters.ChosenName } }; // add this line

            Debug.Log(PlayerParameters.ChosenName);

            if (cantidad <= 20)
            {
                PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, roomOptions);
                SwitchScenes(5);
                //parameters.SetRoomProperties();
            }
        }

        public void test()
        {
            if (PhotonNetwork.InRoom)
            {
                string str = PhotonNetwork.CurrentRoom.CustomProperties["Imagen"] as string;
                Debug.Log(str);
            }
        }

        public void GetRoomName(string textString)
        {
            roomSelected = textString;
            Debug.Log(roomSelected);
        }

        public void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            Debug.Log("UpdateCachedRoomList()");
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                        roomName.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRoomList.Add(info.Name, info);
                    roomName.Add(info.Name);
                }
            }
            if (SceneName == "03 Lobby")
            {
                ListarSalas(roomList);
            }
        }

        /// <summary>
        /// Se obtiene una lista de todas las salas existentes
        /// </summary>
        public void ListarSalas(List<RoomInfo> roomList)
        {
            SpriteAtlas sprite = new SpriteAtlas();
            

            Debug.Log(roomList.Count);

            if (buttons.Count > 0)
            {
                foreach (GameObject button in buttons)
                    Destroy(button.gameObject);
            }
            buttons.Clear();

            foreach (var room in roomList)
            {
                Debug.Log(room.Name);
                GameObject button = Instantiate(buttonTemplate) as GameObject;
                var btnlist = button.GetComponent<ButtonListButton>();

                button.SetActive(true);
                btnlist.SetText(room.Name);
                string sprite_name = room.CustomProperties["Imagen"] as string;

                btnlist.Img.sprite = sprite.GetSprite(sprite_name);
                //btnlist.Img.sprite = Resources.Load<Sprite>(sprite_name);

                button.transform.SetParent(buttonTemplate.transform.parent, false);
                buttons.Add(button.gameObject);
            }
        }
    }
}