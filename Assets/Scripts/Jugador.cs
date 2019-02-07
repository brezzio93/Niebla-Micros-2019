using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class Jugador : MonoBehaviourPunCallbacks
    {
        private GameManager manager = new GameManager();

        public static int billetera;

        [SerializeField]
        private Text t_dias;

        [SerializeField]
        private Text pasaje;

        [SerializeField]
        private Text saldo;

        public static int dias = 1;
        private float probabilidad;

        private bool[] pago, llego;
        private int evasores = 0;
        private int precio, ganancia, monto_inicial;

        // Start is called before the first frame update
        private void Start()
        {
            PhotonNetwork.LocalPlayer.CustomProperties["InBus"] = false;
            if (dias == 11)
            {
                SceneManager.LoadScene(0);
                PhotonNetwork.LeaveRoom();
            }
            precio = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["precio"]);
            ganancia = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["ganancia"]);
            monto_inicial = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["monto"]);

            if (billetera == 0) billetera = monto_inicial;
            else billetera = CalcularBilletera();

            t_dias.text = "Día " + System.Convert.ToString(dias);
            saldo.text = System.Convert.ToString(billetera);
            pasaje.text = System.Convert.ToString(precio);
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
        public void OtroDia(bool button)
        {
            if (dias <= 10)
            {
                Pagar(button);
                manager.SwitchScenes(7);
            }
        }

        /// <summary>
        /// Se añade la selección del jugador a su historial de pago y se descuenta saldo de la billetera del jugador
        /// </summary>
        public void Pagar(bool button)
        {
            PhotonNetwork.LocalPlayer.CustomProperties["pago" + dias] = button;
            PhotonNetwork.LocalPlayer.CustomProperties["InBus"] = true;

            if (button) Debug.Log("Se pagó hoy" + PhotonNetwork.LocalPlayer.CustomProperties["pago" + dias]);
            else Debug.Log("No se pagó hoy" + PhotonNetwork.LocalPlayer.CustomProperties["pago" + dias]);
        }

        public int CalcularBilletera()
        {
            bool pago, llego;
            int wallet = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["monto"]);
            for (int i = 1; i < dias; i++)
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