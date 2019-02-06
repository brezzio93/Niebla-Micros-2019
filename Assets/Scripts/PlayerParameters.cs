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
        }

        private void Update()
        {
        }

        public void SetPlayerProperties()
        {
            Debug.Log(ChosenName);

            ExitGames.Client.Photon.Hashtable pago = new ExitGames.Client.Photon.Hashtable();
            ExitGames.Client.Photon.Hashtable llega = new ExitGames.Client.Photon.Hashtable();
            for (int i = 1; i <= 10; i++)
            {
                pago.Add("pago"+i, false);
                llega.Add("llega" + i, false);
            }            
            PhotonNetwork.SetPlayerCustomProperties(pago);
            PhotonNetwork.SetPlayerCustomProperties(llega);

            ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();
            //CustomProps["paga"] = new bool[10];
            //CustomProps["llega"] = new bool[10];
            CustomProps["InBus"] = false;
            CustomProps["Avatar"] = ChosenName;
            PhotonNetwork.SetPlayerCustomProperties(CustomProps);
        }

        public void SetPlayerAvatar()
        {
            ChosenName = Avatar.sprite.name;
            Chosen.sprite = Avatar.sprite;
            Chosen.gameObject.SetActive(true);
            Debug.Log("SetPlayerAvatar " + ChosenName);
        }
    }
}