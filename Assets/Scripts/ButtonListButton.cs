using UnityEngine;

using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class ButtonListButton : MonoBehaviour
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


        public void SetText(string str)
        {
            nombreSalaCompleto = str;
            nombreDuenno= str.Split('#');
            myText.text = nombreDuenno[0];
        }

        public void GetRoomName()
        {
            server.GetRoomName(nombreSalaCompleto);            
        }


    }
}