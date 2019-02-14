using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class FinalResults : MonoBehaviour
    {
        [SerializeField]
        private Text nombreSala, pasajes, llegados;

        [SerializeField]
        private Image graphPagados, graphLlegados, avatar;

        // Start is called before the first frame update
        private void Start()
        {
            Llenar();
            string[] sala = PhotonNetwork.CurrentRoom.Name.Split('#');
            nombreSala.text = sala[0];
            pasajes.text = Contar("pago") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * 10);
            llegados.text = Contar("llega") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * 10);
        }

        // Update is called once per frame
        private void Update()
        {
            CargarSprite();
        }

        private void CargarSprite()
        {
            foreach (Sprite sprite in GameManager.instance.Avatars)
            {
                if (sprite != null)
                {
                    if (sprite.name == PhotonNetwork.CurrentRoom.CustomProperties["Imagen"] as string)
                    {
                        avatar.sprite = sprite;
                        avatar.gameObject.SetActive(true);
                    }
                }
            }
        }

        private void Llenar()
        {
            double lleno = System.Convert.ToDouble(Contar("pago"));
            graphPagados.fillAmount = (float)lleno / (float)(PhotonNetwork.CurrentRoom.PlayerCount * 10);
            lleno = System.Convert.ToDouble(Contar("llega"));
            graphLlegados.fillAmount = (float)lleno / (float)(PhotonNetwork.CurrentRoom.PlayerCount * 10);
        }

        private string Contar(string str)
        {
            int cantidad = 0;
            bool test;
            for (int j = 1; j <= Jugador.dias; j++)
            {
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    test = System.Convert.ToBoolean(p.CustomProperties[str + j]);
                    if (test) cantidad++;
                }
            }

            return System.Convert.ToString(cantidad);
        }
    }
}