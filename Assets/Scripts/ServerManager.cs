using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.MyCompany.MyGame
{
    public class ServerManager : MonoBehaviourPunCallbacks
    {
        private List<string> roomName = new List<string>();
        private Dictionary<string, RoomInfo> cachedRoomList;
        private RoomParameters parameters = new RoomParameters();

        private string currentScene;

        [SerializeField]
        private GameObject buttonTemplate;

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
            currentScene = GameManager.instance.GetCurrentScene().name;
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

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            GameManager.instance.SwitchScenes(5);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            GameManager.instance.SwitchScenes(5);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); 
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
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
            if (PlayerParameters.ChosenName != null)
            {
                if (PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.JoinLobby();
                    Debug.Log("CreateOrJoin " + createRoom);
                    if (createRoom == true) GameManager.instance.SwitchScenes(4);
                    else GameManager.instance.SwitchScenes(3);
                }
            }
        }

        /// <summary>
        /// Se crea la sala con los parametros ingresados por el Host
        ///  </summary>
        public void CrearSala()
        {
            int id_Sala = Random.Range(0, 9999);//Se añade este valor al nombre de la sala para que no genere conflico por crear varias salas con el mismo nombre
            Debug.Log("CrearSala()");
            int cantidad = System.Convert.ToInt32(RoomParameters.param.cantidad);
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.CustomRoomPropertiesForLobby = new string[1] { "Imagen" };
            roomOptions.MaxPlayers = System.Convert.ToByte(cantidad);
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
                { "Imagen", PlayerParameters.ChosenName },
                {"monto",RoomParameters.param.monto},
                {"precio",RoomParameters.param.precio},
                {"ganancia",RoomParameters.param.ganancia}
            };

            if (cantidad <= 20)
            {
                PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName + " #" + id_Sala, roomOptions);
            }
        }

        /// <summary>
        /// Obtiene el nombre de la sala seleccionada en el Lobby
        /// </summary>
        /// <param name="textString">
        /// Nombre de la sala
        /// </param>
        public void GetRoomName(string textString)
        {
            roomSelected = textString;
            Debug.Log(roomSelected);
        }

        /// <summary>
        /// Se llama cada vez que se actualiza la lista de salas
        /// </summary>
        /// <param name="roomList">
        /// Lista de salas en el momento
        /// </param>
        public void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            Debug.Log("UpdateCachedRoomList()");
            foreach (RoomInfo info in roomList)
            {
                //elimina de la lista todas las salas que no estén disponibles de momento
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                        GameManager.instance.RoomList.Remove(info.Name);
                    }

                    continue;
                }

                // Se actualizan las salas
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                else
                {
                    cachedRoomList.Add(info.Name, info);
                    GameManager.instance.RoomList.Add(info.Name);
                }
            }
            if (currentScene == "03 Lobby")
            {
                Debug.Log(roomList.Count + " Rooms");
                ListarSalas(roomList);
            }
        }

        /// <summary>
        /// Se obtiene una lista de todas las salas existentes
        /// </summary>
        public void ListarSalas(List<RoomInfo> roomList)
        {
            Debug.Log(roomList.Count);

            //Se limpian los botones 
            if (buttons.Count > 0)
            {
                foreach (GameObject button in buttons)
                    Destroy(button.gameObject);
            }

            buttons.Clear();

            //Se crea un botón por cada sala en la lista de salas
            foreach (var room in roomList)
            {
                Debug.Log(room.Name);
                GameObject button = Instantiate(buttonTemplate) as GameObject;
                var btnlist = button.GetComponent<ButtonListButton>();

                button.SetActive(true);
                btnlist.SetText(room.Name);
                string sprite_name = room.CustomProperties["Imagen"] as string;
                var face = GameManager.instance.GetAvatarFaces(sprite_name);

                btnlist.Img.sprite = face.happy;

                button.transform.SetParent(buttonTemplate.transform.parent, false);
                buttons.Add(button.gameObject);
            }
        }
    }
}