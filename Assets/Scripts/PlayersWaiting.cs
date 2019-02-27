using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class PlayersWaiting : MonoBehaviour
    {
        [SerializeField]
        private Text jugadores;

        [SerializeField]
        private Button mostrarResultadosButton;

        private GameManager manager = new GameManager();

        // Start is called before the first frame update
        private void Start()
        {
            jugadores.text = "";

            //Se oculta el botón que muestra los resultados del día a todos menos al host de la sala
            if (!PhotonNetwork.IsMasterClient) mostrarResultadosButton.gameObject.SetActive(false);
            else mostrarResultadosButton.gameObject.SetActive(true);
        }

        // Update is called once per frame
        private void Update()
        {
            //Se le muestra al host cuantos han jugado hasta ahora
            if (PhotonNetwork.IsMasterClient)
                jugadores.text = string.Format("{0} de {1}", CountDonePlayers(), GameManager.instance.JugadoresEnSala.Count);
        }

        /// <summary>
        /// Se cuenta la cantidad de jugadores que han jugador hasta ahora
        /// </summary>
        /// <returns>
        /// cantidad de jugadores que han jugado
        /// </returns>
        private int CountDonePlayers()
        {
            int playersdone = GameManager.instance.JugadoresEnSala.Count - GameManager.instance.JugadoresJugados.Count;
            Debug.LogFormat("{0}-{1}", GameManager.instance.JugadoresEnSala.Count, GameManager.instance.JugadoresJugados.Count);

            return playersdone;
        }

        /// <summary>
        /// Se pasa a la escena de resultados
        /// </summary>
        public void TerminarViaje()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameManager.LevantarEventos(GameManager.CodigoEventosJuego.TerminarEspera, null, ReceiverGroup.All);
            }
        }
    }
}