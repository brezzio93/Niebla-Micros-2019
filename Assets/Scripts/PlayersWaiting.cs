using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class PlayersWaiting : MonoBehaviour
    {
        [SerializeField]
        private Text jugadores;

        private GameManager manager = new GameManager();

        // Start is called before the first frame update
        private void Start()
        {
            GameManager.instance.SeJugo += Instance_SeJugo;
            jugadores.text = "";
        }

        private void Instance_SeJugo(string nickname)
        {
            //throw new System.NotImplementedException();
            //
        }

        // Update is called once per frame
        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
                jugadores.text = string.Format("{0} de {1}", CountDonePlayers(), GameManager.instance.JugadoresEnSala.Count);
        }

        private int CountDonePlayers()
        {
            int playersdone = GameManager.instance.JugadoresEnSala.Count - GameManager.instance.JugadoresJugados.Count;
            Debug.LogFormat("{0}-{1}", GameManager.instance.JugadoresEnSala.Count, GameManager.instance.JugadoresJugados.Count);

            return playersdone;
        }

        public void TerminarDia()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                manager.SwitchScenes(8);
            }
        }
    }
}