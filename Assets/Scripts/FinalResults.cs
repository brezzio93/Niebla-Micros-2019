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

		private List<double> pagoList, llegoList;
        private Window_Graph graficoLineas;

        // Start is called before the first frame update
        private void Start()
		{
			Llenar();
			string[] sala = PhotonNetwork.CurrentRoom.Name.Split('#');
			nombreSala.text = sala[0];
			pasajes.text = Contar("pago") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * 10);
			llegados.text = Contar("llega") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * 10);
			pasajes.text = Contar("pago") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * GameManager.instance.maxDias);
			llegados.text = Contar("llega") + "/" + (PhotonNetwork.CurrentRoom.PlayerCount * GameManager.instance.maxDias);
			int pagoVerde = 0; //para el gráfico de porcentaje de pagados
			int llegoAzul = 1; //para el gráfico de porcentaje de llegados
			pagoList = new List<double>();
			llegoList = new List<double>();
            graficoLineas= new Window_Graph();
            pagoList.AddRange(ContarPorcentajes("pago"));
            llegoList.AddRange(ContarPorcentajes("llega"));
            graficoLineas.GraficosLineas(pagoList, pagoVerde);
			graficoLineas.GraficosLineas(llegoList, llegoAzul);
          
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

		public List<double> ContarPorcentajes(string opcion)

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

			return contadosPorcentaje; //para llamar a la funcion de graficos de linea
		}

	}

}

