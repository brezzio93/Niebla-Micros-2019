using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class FinalResults : MonoBehaviour
    {
        [SerializeField]
        private Text nombreSala, pasajes, llegados, nombreGraficoLinea;

        [SerializeField]
        private Image graphPagados, graphLlegados, avatar;
        
        [SerializeField]
        private gameObject rojo, azul, amarillo, naranjo, verde, purpura, rosa;

        [SerializeField]
        private GameObject rojo, azul, amarillo, naranjo, verde, purpura, rosa;

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
            string sprite_name = PhotonNetwork.CurrentRoom.CustomProperties["Imagen"] as string;
            var face = GameManager.instance.GetAvatarFaces(sprite_name);

            avatar.sprite = face.happy;
            avatar.gameObject.SetActive(true);
        }

        /// <summary>
        /// Se llenan los gráficos de torta
        /// </summary>
        private void Llenar()
        {
            double lleno = System.Convert.ToDouble(Contar("pago"));
            graphPagados.fillAmount = (float)lleno / (float)(PhotonNetwork.CurrentRoom.PlayerCount * 10);
            lleno = System.Convert.ToDouble(Contar("llega"));
            graphLlegados.fillAmount = (float)lleno / (float)(PhotonNetwork.CurrentRoom.PlayerCount * 10);
        }

        /// <summary>
        /// Se cuenta cuantos jugadores han pagado/llegado a lo largo de los 10 días
        /// </summary>
        /// <param name="str">"paga/llega"</param>
        /// <returns>Cantidad de jugadores que han llegado/pagado</returns>
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

        public void MostrarGrafico()
        {
        }

        /*public void MostrarGrafico(nombre usuario clickeado, color usuario clickeado)
    {
        rojo=1;
        azul=2;
        amarillo=3;
        naranjo=4;
        verde=5;
        purpura=6;
        rosa=7;
        Al clickear avatar de usuario:
            if (coloravatar==rojo){
                if (rojo.enabled==true){
                    azul.enabled=true;
                    amarillo.enabled=true;
                    naranjo.enabled=true;
                    verde.enabled=true;
                    purpura.enabled=true;
                    rosa.enabled=true;
                    nombreGraficoLinea= " ";
                else
                    rojo.enabled=true;
                    azul.enabled=false;
                    amarillo.enabled=false;
                    naranjo.enabled=false;
                    verde.enabled=false;
                    purpura.enabled=false;
                    rosa.enabled=false;
                    nombreGraficoLinea= nombre usuario seleccionado;
                }
            }
            elseif (coloravatar=azul){
                if (azul.enabled==true){
                    rojo.enabled=true;
                    amarillo.enabled=true;
                    naranjo.enabled=true;
                    verde.enabled=true;
                    purpura.enabled=true;
                    rosa.enabled=true;
                    nombreGraficoLinea= " ";
                else
                    rojo.enabled=false;
                    azul.enabled=true;
                    amarillo.enabled=false;
                    naranjo.enabled=false;
                    verde.enabled=false;
                    purpura.enabled=false;
                    rosa.enabled=false;
                    nombreGraficoLinea= nombre usuario seleccionado;
                    }
            }
            elseif (coloravatar=amarillo){
                if (amarillo.enabled==true){
                    rojo.enabled=true;
                    azul.enabled=true;
                    naranjo.enabled=true;
                    verde.enabled=true;
                    purpura.enabled=true;
                    rosa.enabled=true;
                    nombreGraficoLinea= " ";
                else
                    rojo.enabled=false;
                    azul.enabled=false;
                    amarillo.enabled=true;
                    naranjo.enabled=false;
                    verde.enabled=false;
                    purpura.enabled=false;
                    rosa.enabled=false;
                    nombreGraficoLinea= nombre usuario seleccionado;
                    }
            }
            elseif (coloravatar=naranjo){
                if (naranjo.enabled==true){
                    rojo.enabled=true;
                    amarillo.enabled=true;
                    azul.enabled=true;
                    verde.enabled=true;
                    purpura.enabled=true;
                    rosa.enabled=true;
                    nombreGraficoLinea= " ";
                else
                    rojo.enabled=false;
                    azul.enabled=false;
                    amarillo.enabled=false;
                    naranjo.enabled=true;
                    verde.enabled=false;
                    purpura.enabled=false;
                    rosa.enabled=false;
                    nombreGraficoLinea= nombre usuario seleccionado;
                    }
            }
            elseif (coloravatar=verde){
                if (verde.enabled==true){
                    rojo.enabled=true;
                    amarillo.enabled=true;
                    naranjo.enabled=true;
                    azul.enabled=true;
                    purpura.enabled=true;
                    rosa.enabled=true;
                    nombreGraficoLinea= " ";
                else
                    rojo.enabled=false;
                    azul.enabled=false;
                    amarillo.enabled=false;
                    naranjo.enabled=false;
                    verde.enabled=true;
                    purpura.enabled=false;
                    rosa.enabled=false;
                    nombreGraficoLinea= nombre usuario seleccionado;
                    }
            }
            elseif (coloravatar=purpura){
                if (purpura.enabled==true){
                    rojo.enabled=true;
                    amarillo.enabled=true;
                    naranjo.enabled=true;
                    verde.enabled=true;
                    azul.enabled=true;
                    rosa.enabled=true;
                    nombreGraficoLinea= " ";
                else
                    rojo.enabled=false;
                    azul.enabled=false;
                    amarillo.enabled=false;
                    naranjo.enabled=false;
                    verde.enabled=false;
                    purpura.enabled=true;
                    rosa.enabled=false;
                    nombreGraficoLinea= nombre usuario seleccionado;
                    }
            }
            elseif (coloravatar=rosa){
                if (rosa.enabled==true){
                    rojo.enabled=true;
                    amarillo.enabled=true;
                    naranjo.enabled=true;
                    verde.enabled=true;
                    purpura.enabled=true;
                    azul.enabled=true;
                    nombreGraficoLinea= " ";
                else
                    rojo.enabled=false;
                    azul.enabled=false;
                    amarillo.enabled=false;
                    naranjo.enabled=false;
                    verde.enabled=false;
                    purpura.enabled=false;
                    rosa.enabled=true;
                    nombreGraficoLinea= nombre usuario seleccionado;
                    }
            }
      }*/
    }
}