using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
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

        [SerializeField]
        private LineRenderer rojo, azul, amarillo, naranjo, verde, purpura, rosa;

        private LineRenderer[] lineas;

        // Start is called before the first frame update
        private void Start()
        {
            Llenar();
            string[] sala = PhotonNetwork.CurrentRoom.Name.Split('#');
            nombreSala.text = sala[0];
            pasajes.text = Contar("pago") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * 10);
            llegados.text = Contar("llega") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * 10);
            //////////////////////////////Comienza mostrando todos los graficos de linea////////////////////////////////////////////////////////
            lineas = new LineRenderer[7];
            lineas[0] = rojo;
            lineas[1] = azul;
            lineas[2] = amarillo;
            lineas[3] = naranjo;
            lineas[4] = verde;
            lineas[5] = purpura;
            lineas[6] = rosa;
            if (PhotonNetwork.CurrentRoom.PlayerCount <= 7)
            {
                Debug.Log("algo");
                int numUsers = PhotonNetwork.CurrentRoom.PlayerCount;
                for (int i=0; i < numUsers; i++)
                {

                    lineas[i].gameObject.SetActive(true);
                    Debug.Log("algo2");
                }

            }
            pasajes.text = Contar("pago") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * GameManager.instance.maxDias);
            llegados.text = Contar("llega") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * GameManager.instance.maxDias);
            ContarPorcentajes("pago");
            ContarPorcentajes("llega");
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
            graphPagados.fillAmount = (float)lleno / (float)(PhotonNetwork.CurrentRoom.PlayerCount * GameManager.instance.maxDias);
            lleno = System.Convert.ToDouble(Contar("llega"));
            graphLlegados.fillAmount = (float)lleno / (float)(PhotonNetwork.CurrentRoom.PlayerCount * GameManager.instance.maxDias);
        }

        /// <summary>
        /// Se cuenta cuantos jugadores han pagado/llegado a lo largo de los 10 días
        /// </summary>
        /// <param name="opcion">"paga/llega"</param>
        /// <returns>Cantidad de jugadores que han llegado/pagado</returns>
        private string Contar(string opcion)
        {
            int cantidad = 0;
            bool test;
            for (int j = 1; j <= GameManager.instance.maxDias; j++)
            {
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    test = System.Convert.ToBoolean(p.CustomProperties[opcion + j]);
                    if (test) cantidad++;
                }
            }
            return System.Convert.ToString(cantidad);
        }

        public void ContarPorcentajes(string opcion)
        public void GraficoUsuario() //muestra solo el grafico del jugador seleccionado
        {
            int cantidadTotal = 0, cantidad = 0, index = 0;
            List<int> contados = new List<int>();
            List<double> contadosPorcentaje = new List<double>();
            bool test;
            for (int i = 1; i <= GameManager.instance.maxDias; i++)
            {
                cantidad = 0;
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    test = System.Convert.ToBoolean(p.CustomProperties[opcion + i]);
                    if (test)
                        cantidad++;
                }
                contados.Add(cantidad);
            }

            foreach (int contado in contados)
            {
                cantidadTotal += contado;
                contadosPorcentaje.Add((double)contado / (double)PhotonNetwork.CurrentRoom.PlayerCount);
                Debug.LogFormat("{0} {1}: {2}%", opcion, contado, contadosPorcentaje[index] * 100);
                index++;
            }
            Debug.LogFormat("Total {0}: {1}", cantidadTotal, opcion);
        }
            Debug.Log("algo3");
            lineas = new LineRenderer[7];
            lineas[0] = rojo;
            lineas[1] = azul;
            lineas[2] = amarillo;
            lineas[3] = naranjo;
            lineas[4] = verde;
            lineas[5] = purpura;
            lineas[6] = rosa;
            int numUsers = PhotonNetwork.CurrentRoom.PlayerCount;
            int crojo=0, cazul=1, camarillo=2, cnaranjo=3, cverde=4, cpurpura=5, crosa=6;
            int coloravatar=Random.Range(0, numUsers);            
            int i;
            if (coloravatar == crojo) {

                for (i = 0; i < numUsers; i++)
                {
                    lineas[i].gameObject.SetActive(false);
                }
                lineas[0].gameObject.SetActive(true);

            }
            else if (coloravatar == cazul) {

                for (i = 0; i < numUsers; i++)
                {
                    lineas[i].gameObject.SetActive(false);
                }
                lineas[1].gameObject.SetActive(true);
            }
            else if (coloravatar == camarillo) {

                for (i = 0; i < numUsers; i++)
                {
                    lineas[i].gameObject.SetActive(false);
                }
                lineas[2].gameObject.SetActive(true);
            }
            else if (coloravatar == cnaranjo) {

                for (i = 0; i < numUsers; i++)
                {
                    lineas[i].gameObject.SetActive(false);
                }
                lineas[3].gameObject.SetActive(true);

            }
            else if (coloravatar == cverde) {

                for (i = 0; i < numUsers; i++)
                {
                    lineas[i].gameObject.SetActive(false);
                }
                lineas[4].gameObject.SetActive(true);
            }
            else if (coloravatar == cpurpura) {

                for (i = 0; i < numUsers; i++)
                {
                    lineas[i].gameObject.SetActive(false);
                }
                lineas[5].gameObject.SetActive(true);
            }
            else if (coloravatar==crosa){

                    for (i = 0; i < numUsers; i++)
                    {
                        lineas[i].gameObject.SetActive(false);
                    }
                    lineas[6].gameObject.SetActive(true);
                }
            }
        public void MostrarTodos() //muestra los graficos de todos los jugadores juntos
        {
            lineas = new LineRenderer[7];
            int numUsers = PhotonNetwork.CurrentRoom.PlayerCount;
            int i;
            lineas[0] = rojo;
            lineas[1] = azul;
            lineas[2] = amarillo;
            lineas[3] = naranjo;
            lineas[4] = verde;
            lineas[5] = purpura;
            lineas[6] = rosa;
            for (i = 0; i < numUsers; i++)
            {
                lineas[i].gameObject.SetActive(true);
            }
            Debug.Log("Funcionan todas");
        }
    }
}

