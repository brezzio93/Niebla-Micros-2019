using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class PlayerParameters : MonoBehaviour
    {
        public Image Avatar;
        public Image Chosen;
        public static string ChosenName;

        // Use this for initialization
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Se guardan las CustomProperties del jugador
        /// </summary>
        public void SetPlayerProperties()
        {
            ExitGames.Client.Photon.Hashtable pago = new ExitGames.Client.Photon.Hashtable();
            ExitGames.Client.Photon.Hashtable llega = new ExitGames.Client.Photon.Hashtable();

            for (int i = 1; i <= GameManager.instance.maxDias; i++)
            {
                pago.Add("pago" + i, false);
            }
            PhotonNetwork.SetPlayerCustomProperties(pago);
            PhotonNetwork.SetPlayerCustomProperties(llega);

            ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();

            CustomProps["Imagen"] = ChosenName;
            PhotonNetwork.SetPlayerCustomProperties(CustomProps);
        }

        /// <summary>
        /// Se obtiene el nombre del sprite que seleccionó el jugador
        /// </summary>
        public void SetPlayerAvatar()
        {
            ChosenName = Avatar.sprite.name;
            Chosen.sprite = Avatar.sprite;
            Chosen.gameObject.SetActive(true);
        }
    }
}