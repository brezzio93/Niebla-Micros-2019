using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class GraphResults : MonoBehaviour
    {
        [SerializeField]
        private Text dia, paga, llega, ganancia, montoInicial, montoActual, pagados, llegados, next;

        [SerializeField]
        private Image graphPagados, graphLlegados;

        private Jugador Jugador = new Jugador();

        // Start is called before the first frame update
        private void Start()
        {
            dia.text = System.Convert.ToString("Día " + Jugador.dias);

            if (System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["pago" + Jugador.dias]) == true)
                paga.text = "Si";
            else paga.text = "No";

            pagados.text = Contar("pago") + "/" + PhotonNetwork.CurrentRoom.PlayerCount;
            llegados.text = Contar("llega") + "/" + PhotonNetwork.CurrentRoom.PlayerCount;
            Llenar();

            if (System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["llega" + Jugador.dias]) == true)
            {
                llega.text = "Si";
                ganancia.text = PhotonNetwork.CurrentRoom.CustomProperties["ganancia"] as string;
            }
            else
            {
                llega.text = "No";
                ganancia.text = "0";
            }

            montoInicial.text = System.Convert.ToString(CalcularBilletera(Jugador.dias - 1));
            montoActual.text = System.Convert.ToString(CalcularBilletera(Jugador.dias));

            if (Jugador.dias == 10) next.text = "Ver Resultados Finales";
            else next.text = System.Convert.ToString("Día " + (Jugador.dias + 1));
        }

        public void DiaSiguiente()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (Jugador.dias == 10) SceneManager.LoadScene(10);
                else
                {
                    SceneManager.LoadScene(6);
                }
            }
        }

        private string Contar(string str)
        {
            int i = 0;
            bool test;
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                Debug.Log("Día" + Jugador.dias + ": " + p.NickName + " " + str + " " + p.CustomProperties[str + Jugador.dias]);
                test = System.Convert.ToBoolean(p.CustomProperties[str + Jugador.dias]);
                if (test) i++;
            }
            return System.Convert.ToString(i);
        }

        public int CalcularBilletera(int dias)
        {
            bool pago, llego;
            int precio = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["precio"]);
            int ganancia = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["ganancia"]);
            int wallet = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["monto"]);

            for (int i = 1; i <= dias; i++)
            {
                pago = System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["pago" + i]);
                llego = System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["llega" + i]);

                if (pago) wallet = wallet - precio;
                if (llego) wallet = wallet + ganancia;
            }
            return wallet;
        }

        private void Llenar()
        {
            double lleno = System.Convert.ToDouble(Contar("pago"));
            graphPagados.fillAmount = (float)lleno / (float)PhotonNetwork.CurrentRoom.PlayerCount;
            lleno = System.Convert.ToDouble(Contar("llega"));
            graphLlegados.fillAmount = (float)lleno / (float)PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }
}