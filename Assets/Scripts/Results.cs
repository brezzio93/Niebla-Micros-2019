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

        /// <summary>
        /// Cambia los elementos del Canvas según si el jugador llegó o no al trabajo el día de hoy
        /// </summary>
        private void ResultadosViaje()
        {
            CalcularLlegada();
            bool llega = System.Convert.ToBoolean(PhotonNetwork.LocalPlayer.CustomProperties["llega" + Jugador.diaActual]);
            if (llega)
            {
                ConstruirAvatar(PhotonNetwork.LocalPlayer.CustomProperties["Imagen"] as string, llega);
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

                ConstruirAvatar(PhotonNetwork.LocalPlayer.CustomProperties["Imagen"] as string, llega);
                CargarSprite(bus, Buses[0]);
                placeholder.transform.Translate(Vector3.right * 200);//numero magico para centrar el avatar, ya q no hay panel del remuneracion.
                //Comento esta linea xq emparente el avatar al placeholder. Lazy~
                //avatar.transform.Translate(Vector3.right * 200);

                message.text = "¡Oh no, tu micro se averió!";
                message.color = Color.red;
            }
        }

        /// <summary>
        /// Se obtiene y activa el sprite solicitado
        /// </summary>
        /// <param name="image">imagen sobre la cual se asignarán los valores del sprite del parametro "sprite"</param>
        /// <param name="sprite">Sprite con la imagen del bus descompuesto o en buen estado</param>
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

        /// <summary>
        /// Toma todas las decisiones de los jugadores hasta el día actual para calcular la probabilidad
        /// de llegada de bus
        /// </summary>
        private void CalcularLlegada()
        {
            Debug.Log("CalcularLlegada");
            int evasores = 0;
            bool llega, juega;
            float pLlega;
            double razonEvasores;

            List<string> jugadoresSiJugaron = new List<string>();//Lista auxiliar para comprobar quienes no han jugado el día de hoy y por tanto automaticamente no llegarán

            ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();

            jugadoresSiJugaron.Clear();
            jugadoresSiJugaron.AddRange(GameManager.instance.JugadoresEnSala);
            foreach (var jugador in GameManager.instance.JugadoresJugados)
            {
                if (jugadoresSiJugaron.Contains(jugador))
                    jugadoresSiJugaron.Remove(jugador);
            }

            if (jugadoresSiJugaron.Contains(PhotonNetwork.LocalPlayer.NickName))
                juega = true;
            else
                juega = false;

            evasores = ContarEvasores();
            razonEvasores = ((double)evasores / ((double)PhotonNetwork.CurrentRoom.PlayerCount * (double)Jugador.diaActual));
            Debug.Log("Razon Evasores " + razonEvasores);
            pLlega = ProbabilidadLlegada(razonEvasores);

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

        /// <summary>
        /// Se cuenta la cantidad de evasores de pasaje de bus
        /// </summary>
        /// <returns>
        /// Cantidad de evasores
        /// </returns>
        private int ContarEvasores()
        {
            int evasores = 0;
            bool paga;
            for (int i = 1; i <= Jugador.diaActual; i++)
            {
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    paga = System.Convert.ToBoolean(p.CustomProperties["pago" + i]);//Se revisa si cada jugador pagó en "i" día
                    if (paga == false) evasores++;
                }
            }

            Debug.Log(" Evasores: " + evasores + " de " + PhotonNetwork.CurrentRoom.PlayerCount * Jugador.diaActual);

            return evasores;
        }

        /// <summary>
        /// Se utiliza la función de calculo de probabilidad de llegada para calcular la probabilidad de llegada del bus
        /// </summary>
        /// <param name="x">
        /// Razon de evasores de pasaje
        /// </param>
        /// <returns>
        /// Probabilidad de llegada del bus
        /// </returns>
        private float ProbabilidadLlegada(double x)
        {
            float probabilidadFalla, probabilidadLlega;
            probabilidadFalla = 1 - (1 / (1 + Mathf.Exp(13 * ((float)x - 0.5f))));
            probabilidadLlega = 1 - probabilidadFalla;
            probabilidadLlega = probabilidadLlega * 100;
            Debug.Log(probabilidadLlega + "%");

            return probabilidadLlega;
        }
    }
}