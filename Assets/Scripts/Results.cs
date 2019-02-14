using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class Results : MonoBehaviour
    {
        [SerializeField]
        private Text dia, message, ganancia, obtenidoPlainText;

        [SerializeField]
        private Image placeholder, avatar, bus, panel;

        

        // Start is called before the first frame update
        private void Start()
        {
            dia.text = "Día " + System.Convert.ToString(Jugador.dias);
            ResultadosViaje();
        }

        private void ResultadosViaje()
        {
            CalcularLlegada();
            string nombreSprite;
            bool llega = System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["llega" + Jugador.dias]);
            if (llega)
            {
                nombreSprite = NombreSprite(0);
                //CargarSprite(avatar, nombreSprite);
                ConstruirAvatar(nombreSprite,llega);
                CargarSprite(bus, "atlas_2_8");

                message.text = "¡Bien, has llegado a tu trabajo!";

                message.color = Color.green;
                ganancia.gameObject.SetActive(true);
                ganancia.text = PhotonNetwork.CurrentRoom.CustomProperties["ganancia"] as string;
            }
            else
            {
                obtenidoPlainText.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                nombreSprite = NombreSprite(1);
                //CargarSprite(avatar, nombreSprite);
                ConstruirAvatar(nombreSprite, llega);
                CargarSprite(bus, "atlas_2_7");
                placeholder.transform.Translate(Vector3.right * 200);
                avatar.transform.Translate(Vector3.right * 200);

                message.text = "¡Oh no, tu micro se averió!";
                message.color = Color.red;
            }
        }

        private string NombreSprite(int num)
        {
            string name = PhotonNetwork.LocalPlayer.CustomProperties["Imagen"] as string;
            string[] parts = name.Split('_');
            parts[2] = System.Convert.ToString(System.Convert.ToInt32(parts[2]) + num);

            if (name == "atlas_1_20")
            {
                parts[2] = "23";
            }

            name = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                name = string.Concat(name, "_", parts[i]);
            }
            return name;
        }

        private void CargarSprite(Image image, string nombreSprite)
        {
            
            foreach (Sprite sprite in GameManager.instance.Avatars)
            {
                if (sprite.name == nombreSprite)
                {
                    image.sprite = sprite;
                    image.gameObject.SetActive(true);
                }
            }
        }

        public void ConstruirAvatar(string nombreSprite, bool llegada)
        {
            var faces = GameManager.instance.GetAvatarFaces(nombreSprite);
            if (llegada)
                avatar.sprite = faces.happy;
            else
                avatar.sprite = faces.sad;
            avatar.gameObject.SetActive(true);

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