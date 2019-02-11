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

        private GameManager manager = new GameManager();

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            jugadores.text = CountDonePlayers() + " de " + PhotonNetwork.PlayerList.Length;
        }

        private string CountDonePlayers()
        {
            int playersdone = 0;
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                Debug.Log(p.NickName + " " + p.CustomProperties["InBus" + Jugador.dias]);

                bool inBus = System.Convert.ToBoolean(p.CustomProperties["InBus" + Jugador.dias]);

                if (p.CustomProperties["InBus" + Jugador.dias].Equals(true))
                {
                    playersdone++;
                }
            }
            return System.Convert.ToString(playersdone);
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