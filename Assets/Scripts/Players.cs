using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class Players : MonoBehaviour
    {
        [SerializeField]
        private Text jugadores;

        private GameManager manager = new GameManager();

        private string max;

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
            int maxPlayers = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.PlayerCount);

            for (int i = 0; i < maxPlayers; i++)
            {
                Debug.Log(PhotonNetwork.PlayerList[i].CustomProperties["InBus"]);
                if (PhotonNetwork.PlayerList[i].CustomProperties["InBus"].Equals(true)) playersdone++;
                Debug.Log(playersdone);
            }
            return System.Convert.ToString(playersdone);
        }

        public void TerminarDia()
        {
            if (PhotonNetwork.IsMasterClient)
                manager.SwitchScenes(8);
        }
    }
}