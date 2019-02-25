using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
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

        [SerializeField]
        private List<Sprite> Buses;

        // Start is called before the first frame update
        private void Start()
        {
            GameManager.instance.SeJugo += Instance_SeJugo;

            PhotonNetwork.AutomaticallySyncScene = false;
            dia.text = "Día " + System.Convert.ToString(Jugador.diaActual);
            ResultadosViaje();
        }

        private void Instance_SeJugo(string obj)
        {
            //throw new NotImplementedException();
        }

        private void ResultadosViaje()
        {
            CalcularLlegada();
            string nombreSprite;
            bool llega = System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["llega" + Jugador.diaActual]);
            if (llega)
            {
                nombreSprite = GetNombreSprite(0);                
                ConstruirAvatar(nombreSprite, llega);
                CargarSprite(bus, Buses[1]);
                message.text = "¡Bien, has llegado a tu trabajo!";
                message.color = Color.green;
                ganancia.gameObject.SetActive(true);
                ganancia.text = PhotonNetwork.CurrentRoom.CustomProperties["ganancia"] as string;
            }
            else
            {
                obtenidoPlainText.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                nombreSprite = GetNombreSprite(1);
                ConstruirAvatar(nombreSprite, llega);
                CargarSprite(bus, Buses[0]);
                placeholder.transform.Translate(Vector3.right * 200);
                avatar.transform.Translate(Vector3.right * 200);

                message.text = "¡Oh no, tu micro se averió!";
                message.color = Color.red;
            }
        }

        private string GetNombreSprite(int num)
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

        private void CargarSprite(Image image, Sprite sprite)
        {

                    image.sprite = sprite;
                    image.gameObject.SetActive(true);
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
            bool paga, llega, juega;
            float pFalla, pLlega;
            double x;

            List<string> jugadoresSiJugaron = new List<string>();

            ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();

            jugadoresSiJugaron.Clear();
            jugadoresSiJugaron.AddRange(GameManager.instance.JugadoresEnSala);
            Debug.Log(GameManager.instance.JugadoresJugados.Count);
            foreach (var jugador in GameManager.instance.JugadoresJugados)
            {
                if (jugadoresSiJugaron.Contains(jugador))
                    jugadoresSiJugaron.Remove(jugador);
            }
            Debug.Log(jugadoresSiJugaron.Count);
            for (int i = 0; i < jugadoresSiJugaron.Count; i++)
            {
                Debug.Log(jugadoresSiJugaron[i]);
            }

            if (jugadoresSiJugaron.Contains(PhotonNetwork.LocalPlayer.NickName))
                juega = true;
            else
                juega = false;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                paga = System.Convert.ToBoolean(p.CustomProperties["pago" + Jugador.diaActual]);
                if (paga == false) evasores++;
            }
            Debug.Log(" Evasores: " + evasores + " de " + PhotonNetwork.CurrentRoom.PlayerCount);
            x = ((double)evasores / (double)PhotonNetwork.CurrentRoom.PlayerCount);
            pFalla = 1 - (1 / (1 + Mathf.Exp(13 * ((float)x - 0.5f))));
            pLlega = 1 - pFalla;
            pLlega = pLlega * 100;
            Debug.Log(pLlega + "%");

            if (pLlega >= UnityEngine.Random.Range(0, 100))
                llega = true;
            else llega = false;

            if (!juega)
            {
                Debug.Log(juega);
                CustomProps.Add("llega" + Jugador.diaActual, false);
            }
            else if (juega) CustomProps.Add("llega" + Jugador.diaActual, llega);
            PhotonNetwork.LocalPlayer.SetCustomProperties(CustomProps);
        }
    }
}