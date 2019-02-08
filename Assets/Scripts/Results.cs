using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class Results : MonoBehaviour
    {
        [SerializeField]
        private Text dia, message, ganancia;

        // Start is called before the first frame update
        private void Start()
        {
            dia.text = System.Convert.ToString(Jugador.dias);
            ResultadosViaje();
        }

        private void ResultadosViaje()
        {
            CalcularLlegada();

            if (System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["llega" + Jugador.dias]))
            {
                message.text = "¡Bien, has llegado a tu trabajo!";
                message.color = Color.green;
                ganancia.gameObject.SetActive(true);
                ganancia.text = PhotonNetwork.CurrentRoom.CustomProperties["ganancia"] as string;
            }
            else
            {
                message.text = "¡Oh no, tu micro se averió!";
                message.color = Color.red;
            }
        }

        private void CalcularLlegada()
        {
            Debug.Log("CalcularLlegada");
            int evasores = 0;
            bool paga, llega;
            float pFalla, pLlega;
            double x;

            ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                paga = System.Convert.ToBoolean(p.CustomProperties["pago" + Jugador.dias]);
                if (paga == false) evasores++;
            }
            Debug.Log(" Evasores" + evasores + " de " + PhotonNetwork.CurrentRoom.PlayerCount);
            x = ((double)evasores / (double)PhotonNetwork.CurrentRoom.PlayerCount);
            pFalla = 1 - (1 / (1 + Mathf.Exp(13 * ((float)x - 0.5f))));
            pLlega = 1 - pFalla;
            pLlega = pLlega * 100;
            Debug.Log(pLlega + "%");

            if (pLlega < Random.Range(0, 100))
                llega = false;
            else llega = true;
            CustomProps.Add("llega" + Jugador.dias, llega);            
            PhotonNetwork.LocalPlayer.SetCustomProperties(CustomProps);
        }
    }
}