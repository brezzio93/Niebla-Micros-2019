using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    [RequireComponent(typeof(InputField))]
    public class RoomParameters : MonoBehaviour
    {
        [SerializeField]
        private InputField monto;

        [SerializeField]
        private InputField precio;

        [SerializeField]
        private InputField ganancia;

        [SerializeField]
        private InputField cantidad;

        [SerializeField]
        private Text Warning;

        private ExitGames.Client.Photon.Hashtable CustomProps = new ExitGames.Client.Photon.Hashtable();

        public struct Params
        {
            public string nombre, monto, precio, ganancia, cantidad;
            //public byte cantidad;

            public Params(string p1, string p2, string p3, string p4, string p5)
            {
                nombre = p1;
                monto = p2;
                precio = p3;
                ganancia = p4;
                cantidad = p5;
            }
        }

        static public Params param;

        // Use this for initialization
        private void Awake()
        {
            //DontDestroyOnLoad(this);
        }

        public void setGetInputs()
        {
            param.nombre = PhotonNetwork.LocalPlayer.NickName;
            param.monto = monto.text;
            param.precio = precio.text;
            param.ganancia = ganancia.text;
            param.cantidad = cantidad.text;

            int cant = System.Convert.ToInt32(param.cantidad);
            if (cant > 20) Warning.text = "Advertencia: La Sala no puede tener más de 20 Jugadores";
        }

        /// <summary>
        /// Se añaden los parametros de la sala al objeto Room
        /// </summary>
        public void SetRoomProperties()
        {
            CustomProps["monto"] = param.monto;
            CustomProps["precio"] = param.precio;
            CustomProps["ganancia"] = param.ganancia;
            CustomProps["Imagen"] = PhotonNetwork.MasterClient.CustomProperties["Imagen"];
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomProps);
        }

        /// <summary>
        /// Se crea la sala con los parametros ingresados por el Host
        ///  </summary>
        public void CrearSala()
        {
            Debug.Log("CrearSala()");
            int cantidad = System.Convert.ToInt32(param.cantidad);
            if (cantidad <= 20)
            {
                PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, new RoomOptions
                {
                    MaxPlayers = System.Convert.ToByte(cantidad),
                    IsVisible = true,
                });

                SwitchScenes(5);
            }
        }

        public void SwitchScenes(int idScene)
        {
            SceneManager.LoadScene(idScene);
        }
    }
}