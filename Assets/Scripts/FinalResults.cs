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

	/*	private List<double> pagoList, llegoList;
        private Window_Graph graficoLineas;*/

        // Start is called before the first frame update
        private void Start()
		{
            Llenar();
          
			string[] sala = PhotonNetwork.CurrentRoom.Name.Split('#');
			nombreSala.text = sala[0];
            CargarSprite();
			pasajes.text = Contar("pago") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * GameManager.instance.maxDias);
            llegados.text = Contar("llega") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * GameManager.instance.maxDias);          
        }

        /// <summary>
        /// Se carga el icono del dueño de la sala
        /// </summary>
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
		/// Se cuenta cuantos jugadores han pagado/llegado a lo largo de los días
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

        /// <summary>
		/// Se cuenta el porcentaje de jugadores han pagado/llegado a lo largo de los días
		/// </summary>
		/// <param name="opcion">"paga/llega"</param>
		/// <returns>Lista con el porcentaje de jugadores que han llegado/pagado cada dia</returns>
		public List<double> ContarPorcentajes(string opcion)
		{
			int cantidadTotal = 0, cantidad, index = 0;
			List<int> contados = new List<int>(); //Contiene la cantidad de pagados/llegados de cada día
			List<double> contadosPorcentaje = new List<double>();
			bool test;

            //Se cuenta cuantos jugadores pagaron/llegaron cada día
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
			return contadosPorcentaje; //para llamar a la funcion de graficos de linea
		}

	}

}

