using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class Jugador : MonoBehaviourPunCallbacks
    {
        public static int billetera;

        [SerializeField]
        private Text t_dias, pasaje, saldo;

        private float probabilidad;

        private bool[] pago, llego;
        private int evasores = 0;
        private int precio, ganancia, monto_inicial;

        public static int diaActual = 0;

        // Start is called before the first frame update
        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = false;

            diaActual++;

            if (diaActual == 11)
            {
                SceneManager.LoadScene(0);
                PhotonNetwork.LeaveRoom();
            }
            precio = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["precio"]);
            ganancia = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["ganancia"]);
            monto_inicial = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["monto"]);

            if (diaActual==1) billetera = monto_inicial;
            else billetera = CalcularBilletera();

            t_dias.text = "Día " + System.Convert.ToString(diaActual);
            saldo.text = System.Convert.ToString(billetera);
            pasaje.text = System.Convert.ToString(-precio);
        }

        // Update is called once per frame
        private void Update()
        {
            saldo.text = System.Convert.ToString(billetera);
        }

        /// <summary>
        /// Revisa que el jugador pague su pasaje y en base a eso calcula la probabilidad de llegar junto con los
        /// calculos del pasaje de bus
        /// </summary>
        /// <param name="button"> Elección de pagar/no pagar</param>
        public void TomarBus(bool button)
        {
            if (diaActual <= GameManager.instance.maxDias)
            {
                Pagar(button);
                GameManager.instance.SwitchScenes(7);
            }
        }

        /// <summary>
        /// Se añade la selección del jugador a su historial de pago y se descuenta saldo de la billetera del jugador
        /// </summary>
        public void Pagar(bool button)
        {
            GameManager.LevantarEventos(GameManager.CodigoEventosJuego.JugadorJuega, PhotonNetwork.LocalPlayer.NickName, ReceiverGroup.All);

            ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();
            CustomProps.Add("pago" + diaActual, button);
            PhotonNetwork.LocalPlayer.SetCustomProperties(CustomProps);
        }

        /// <summary>
        /// Revisa los custom properties de pago/llegada para añadir fondos a la billetera
        /// </summary>
        /// <returns>retorna el monto actual de la billetera del jugador</returns>
        public int CalcularBilletera()
        {
            bool pago, llego;
            int wallet = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["monto"]);
            for (int i = 1; i < diaActual; i++)
            {
                pago = System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["pago" + i]);
                llego = System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["llega" + i]);

                if (pago) wallet = wallet - precio;
                if (llego) wallet = wallet + ganancia;
            }
            return wallet;
        }
    }
}