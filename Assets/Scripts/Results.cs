using Photon.Pun;
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
            bool llego = System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["llega" + Jugador.dias]);
            if (llego)
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
    }
}