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

        private ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();

        public static GameManager instance = null;

        public List<Sprite> Avatars = new List<Sprite>();

        [HideInInspector]
        public List<string> JugadoresEnSala = new List<string>();

        [HideInInspector]
        public List<string> JugadoresJugados = new List<string>();

        public List<string> RoomList = new List<string>();

        public int dias;

        #region Eventos

        public event Action<string> SeJugo;

        public event Action<Player> AlEntrarJugador;

        public enum CodigoEventosJuego
        {
            JugadorJuega = 1,
            EsperarBus = 2,
            NuevoDia = 3,
            NuevoJuego = 4
        }

        public enum colorAvatar
        {
            rojo = 1,
            azul = 2,
            amarillo = 3,
            naranja = 4,
            verde = 5,
            purpura = 6,
            rosa = 7
        }

        #endregion Eventos

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

        private Scene currentScene;

        public Scene GetCurrentScene()
        {
            return SceneManager.GetActiveScene();
        }

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
            //if (AlEntrarJugador != null)
            //    AlEntrarJugador(PhotonNetwork.LocalPlayer);
            //SwitchScenes(5);
        }

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

        /// <summary>
        /// Saca a los jugadores de la sesión actual y luego abandona el juego
        /// </summary>
        public void FinishGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    PhotonNetwork.CloseConnection(p);
                }
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
                //Este caso cuenta a los jugadores que han tomado el bus al inicio del día
                case CodigoEventosJuego.JugadorJuega:
                    string JugadorNick = (string)photonEvent.CustomData;
                    if (SeJugo != null)
                    {
                        SeJugo(JugadorNick);
                    }
                    ConfirmarJugadores(JugadorNick);

                    break;

                //Carga la escena de llegada una vez que todos los jugadores toman el bus
                case CodigoEventosJuego.EsperarBus:
                    SceneManager.LoadScene(8);
                    break;
                
                //Se carga la escena 6 al terminar el día, si se está en el último día se muestran los resultados finales
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

                //Se reinician los datos del juego al comenzar un juego nuevo
                case CodigoEventosJuego.NuevoJuego:

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Se remueven a los jugadores que han jugado al comienzo del día
        /// </summary>
        /// <param name="nickname">nickname del jugador que ha jugado</param>
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

        /// <summary>
        /// Se encapsula el metodo RaiseEvent
        /// </summary>
        /// <param name="eventosJuego"></param>
        /// <param name="param"></param>
        /// <param name="target"></param>
        public static void LevantarEventos(CodigoEventosJuego eventosJuego, object param, ReceiverGroup target)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = target };
            SendOptions sendOptions = new SendOptions { Reliability = true };

            PhotonNetwork.RaiseEvent((byte)eventosJuego, param, raiseEventOptions, sendOptions);
        }

        #endregion Public Methods
    }
}