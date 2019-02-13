using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        private ServerManager server = new ServerManager();
        private RoomParameters parameters = new RoomParameters();
        private Scene currentScene;
        public static string SceneName;
        private ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();

        public List<Sprite> Avatars = new List<Sprite>();

        public static GameManager instance = null;

        [HideInInspector]
        public List<string> JugadoresEnSala = new List<string>();

        [HideInInspector]
        public List<string> JugadoresJugados = new List<string>();

        public List<string> RoomList = new List<string>();

        #region Eventos

        public event Action<string> SeJugo;

        public event Action<Player> AlEntrarJugador;

        public enum CodigoEventosJuego
        {
            JugadorJuega = 1,
            EsperarBus = 2,
            NuevoDia = 3
        }

        #endregion

        public enum colorAvatar
        {
            rojo = 1,
            azul = 2,
        }

        #region Struct AvatarFaces

        [Serializable]
        public struct AvatarFaces
        {
            public Sprite happy, sad;
            public colorAvatar color;
        }

        public List<AvatarFaces> caras;

        public AvatarFaces GetAvatarFaces(string nombreSprite)
        {
            foreach (var cara in caras)
            {
                if (cara.happy.name == nombreSprite || cara.sad.name == nombreSprite)
                {
                    return cara;
                }
            }
            return default(AvatarFaces);
        }

        public AvatarFaces GetAvatarFaces(int indiceSprite)
        {
            if (indiceSprite < caras.Count)
            {
                return caras[indiceSprite];
            }
            return default(AvatarFaces);
        }

        #endregion Struct AvatarFaces

        #region Photon Callbacks

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            SwitchScenes(5);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            if (AlEntrarJugador != null)
            {
                AlEntrarJugador(PhotonNetwork.LocalPlayer);
            }
            //SwitchScenes(5);
        }

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            if (AlEntrarJugador != null)
            {
                AlEntrarJugador(other);
            }

            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        #endregion Photon Callbacks

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);

            if (Application.isEditor)
                Application.runInBackground = true;
        }

        private void Start()
        {
            currentScene = SceneManager.GetActiveScene();
            SceneName = currentScene.name;
        }

        private void Update()
        {
        }

        #endregion MonoBehaviour Callbacks

        #region Public Methods

        public void LeaveRoom()
        {
            if (PhotonNetwork.InRoom)

                PhotonNetwork.LeaveRoom();
        }

        public void FinishGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    PhotonNetwork.CloseConnection(p);
                }
                PhotonNetwork.DestroyAll();
            }
            PhotonNetwork.LeaveRoom();
        }

        public void SwitchScenes(int idScene)
        {
            SceneManager.LoadScene(idScene);
        }

        /// <summary>
        /// Redirige al registro del jugador y guarda la selección de crear o unirse a sala
        /// </summary>
        /// <param name="create">
        /// Parametro booleano que indica si se creará o no una sala
        /// </param>
        public void GoLogin(bool create)
        {
            ServerManager.createRoom = create;
            SwitchScenes(1);
        }

        /// <summary>
        /// Función utilizada para saber si el jugador creará o se unirá a una sala
        /// </summary>
        public void CreateOrJoin()
        {
            server.CreateOrJoin();
        }

        public void OnEvent(EventData photonEvent)
        {
            CodigoEventosJuego codigoEventos = (CodigoEventosJuego)photonEvent.Code;
            Debug.LogFormat("OnEvent(): {0}, {1}", codigoEventos, photonEvent.CustomData);
            switch (codigoEventos)
            {
                case CodigoEventosJuego.JugadorJuega:
                    string JugadorNick = (string)photonEvent.CustomData;
                    if (SeJugo != null)
                    {
                        SeJugo(JugadorNick);
                    }
                    ConfirmarJugadores(JugadorNick);

                    break;

                case CodigoEventosJuego.EsperarBus:
                    SceneManager.LoadScene(8);
                    break;

                case CodigoEventosJuego.NuevoDia:
                    JugadoresJugados.Clear();
                    JugadoresJugados.AddRange(JugadoresEnSala);
                    if (Jugador.dias == 10)
                        SceneManager.LoadScene(10);
                    else
                    {
                        SceneManager.LoadScene(6);
                    }

                    break;

                default:
                    break;
            }
        }

        private void ConfirmarJugadores(string nickname)
        {
            if (JugadoresJugados.Contains(nickname))
            {
                JugadoresJugados.Remove(nickname);

                if (JugadoresJugados.Count == 0)
                {
                    LevantarEventos(CodigoEventosJuego.EsperarBus, null, ReceiverGroup.All);
                }
            }
        }

        public static void LevantarEventos(CodigoEventosJuego eventosJuego, object param, ReceiverGroup target)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = target };
            SendOptions sendOptions = new SendOptions { Reliability = true };

            PhotonNetwork.RaiseEvent((byte)eventosJuego, param, raiseEventOptions, sendOptions);
        }

        #endregion Public Methods
    }
}