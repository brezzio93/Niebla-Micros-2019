﻿using Photon.Pun;
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
            foreach(Player p in PhotonNetwork.PlayerList)
                p.CustomProperties["llega" + Jugador.dias] = CalcularLlegada();
            //PhotonNetwork.LocalPlayer.CustomProperties["llega" + Jugador.dias] = CalcularLlegada();
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

        private bool CalcularLlegada()
        {
            Debug.Log("CalcularLlegada");
            int evasores = 0;
            bool paga;
            float pFalla, pLlega;
            double x;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                
                paga = System.Convert.ToBoolean(p.CustomProperties["pago" + Jugador.dias]);
                Debug.Log("DIA " + Jugador.dias+" "+ p.NickName+" paga:"+ p.CustomProperties["pago" + Jugador.dias]);
                if (paga==false) evasores++;
            }
            Debug.Log(" Evasores"+evasores+"/" + PhotonNetwork.CurrentRoom.PlayerCount);
            x = ((double)evasores/(double)PhotonNetwork.CurrentRoom.PlayerCount);
            pFalla = 1-(1/(1+ Mathf.Exp(13*((float)x-0.5f))));
            pLlega = 1 - pFalla;
            pLlega = pLlega * 100;
            Debug.Log(pLlega + "%");
            if (pLlega < Random.Range(0, 100))
                return false;
            return true;
        }
    }
}