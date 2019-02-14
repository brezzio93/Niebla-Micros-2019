﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class GraphResults : MonoBehaviour
    {
        [SerializeField]
        private Text dia, paga, llega, ganancia, montoInicial, montoActual, pagados, llegados, next;

        [SerializeField]
        private Image graphPagados, graphLlegados;

        // Start is called before the first frame update
        private void Start()
        {
            //PhotonNetwork.AutomaticallySyncScene = true;
            InicializarTexto();
        }

        /// <summary>
        /// Se inicializan todos los valores de todos los objetos Text 
        /// </summary>
        private void InicializarTexto()
        {
            dia.text = System.Convert.ToString("Día " + Jugador.dias);
            if (System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["pago" + Jugador.dias]) == true)
                paga.text = "Si";
            else paga.text = "No";
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
            pagados.text = Contar("pago") + "/" + PhotonNetwork.CurrentRoom.PlayerCount;
            llegados.text = Contar("llega") + "/" + PhotonNetwork.CurrentRoom.PlayerCount;
            Llenar();

            montoInicial.text = System.Convert.ToString(CalcularBilletera(Jugador.dias - 1));
            montoActual.text = System.Convert.ToString(CalcularBilletera(Jugador.dias));

            if (Jugador.dias == 10) next.text = "Ver Resultados Finales";
            else next.text = System.Convert.ToString("Día " + (Jugador.dias + 1));
        }

        /// <summary>
        /// El host de la sala llama al evento NuevoDia
        /// </summary>
        public void DiaSiguiente()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameManager.LevantarEventos(GameManager.CodigoEventosJuego.NuevoDia, null, ReceiverGroup.All);
            }
        }

        /// <summary>
        /// Se cuentan a los jugadores que han jugado hasta el momento
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string Contar(string str)
        {
            int i = 0;
            bool test;
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                //Debug.LogFormat("Día {0}:{1} {2}, {3}", Jugador.dias, p.NickName, str, p.CustomProperties[str + Jugador.dias]);
                test = System.Convert.ToBoolean(p.CustomProperties[str + Jugador.dias]);
                if (test) i++;
            }
            return System.Convert.ToString(i);
        }

        /// <summary>
        /// Se calcula la billetera del jugador local hasta el día actual
        /// </summary>
        /// <param name="dias"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Se llenan los gráficos de torta
        /// </summary>
        private void Llenar()
        {
            double lleno = System.Convert.ToDouble(Contar("pago"));
            graphPagados.fillAmount = (float)lleno / (float)PhotonNetwork.CurrentRoom.PlayerCount;
            lleno = System.Convert.ToDouble(Contar("llega"));
            graphLlegados.fillAmount = (float)lleno / (float)PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }
}