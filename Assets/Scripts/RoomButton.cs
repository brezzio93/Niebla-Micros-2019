using UnityEngine;

using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class RoomButton : MonoBehaviour
    {
        [SerializeField]
        private Text myText;

        [SerializeField]
        private ServerManager server;

        [SerializeField]
        private Image img;

        
        public Image Img
        {
            get
            {
                return img;
            }

        }

        private string nombreSalaCompleto;
        private string[] nombreDuenno;

        /// <summary>
        /// Se asignan los valores al botón con información de la sala abierta, para listar las salas en escena "03 Lobby".
        /// </summary>
        /// <param name="str">
        /// string con el nombre de la sala (que en este caso es el nombre del dueño, con el formato "NombreDueño #idSala"
        /// </param>
        public void SetText(string str)
        {
            nombreSalaCompleto = str;
            nombreDuenno= str.Split('#');//Se separa el nombre de la sala, quedando {NombreDueño,idSala}
            myText.text = nombreDuenno[0]; //Nombre visible de la sala, se muestra solo el nombre del dueño
        }

        /// <summary>
        /// Se obtiene el nombre de la sala a la que se le ha hecho click
        /// </summary>
        public void GetRoomName()
        {
            server.GetRoomName(nombreSalaCompleto);            
        }


    }
}