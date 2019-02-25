using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class AvatarCreator : MonoBehaviour
    {
        [HideInInspector]
        public bool asignado; //Este valor se ocupa al momento de listar los jugadores en la sala, desde la instancia del Host

        public Image Avatar, textBalloon,placeHolder;
        public Text namePlayer;
        private string strName;

        [SerializeField]
        private List<Sprite> placeHolders;


        // Start is called before the first frame update
        private void Start()
        {
        }

        /// <summary>
        /// Se asignan los valores al Avatar que seleccionó el Jugador
        /// </summary>
        /// <param name="nombreSprite">
        /// Nombre del sprite del avatar seleccionado por los jugadores al comienzo del juego,
        /// con este se obtiene el sprite desde la lista de avatares en gamemanager
        /// </param>
        public void ConstruirAvatar(string nombreSprite)
        {
            var faces = GameManager.instance.GetAvatarFaces(nombreSprite);
            Avatar.gameObject.SetActive(true);
            Avatar.sprite = faces.happy;
            asignado = true;
            if(GameManager.instance.GetCurrentScene().name=="05 Espera")
                placeHolder.sprite = placeHolders[0];
        }

        /// <summary>
        /// Se asigna una cara triste o feliz al jugador, dependiendo de si llega o no al trabajo
        /// </summary>
        /// <param name="nombreSprite">Nombre del Sprite, se usa para obtener el sprite de AvatarFaces</param>
        /// <param name="llegada">Indica si el jugador llegó o no, factor que cambia el sprite mostrado</param>
        public void ConstruirAvatar(string nombreSprite, bool llegada)
        {
            var faces = GameManager.instance.GetAvatarFaces(nombreSprite);
            Avatar.gameObject.SetActive(true);
            if (llegada)
                Avatar.sprite = faces.happy;
            else Avatar.sprite = faces.sad;
            asignado = true;
        }

        /// <summary>
        /// Almacena el nombre del jugador que se está uniendo a la sala de espera
        /// </summary>
        /// <param name="nickname"></param>
        public void ObtenerNombre(string nickname)
        {
            Debug.Log(nickname);
            strName = nickname;
        }

        /// <summary>
        /// Muestra por pantalla el nombre del jugador que se está clickeando
        /// </summary>
        public void MostrarNombre()
        {
            if (strName == null)//Si se clickea una casilla vacía, no se mostrará nada
            {
                namePlayer.text = "";
                textBalloon.gameObject.SetActive(false);
            }
            else
            {
                if (namePlayer.text == strName)// Al hacer click en una casilla que esté mostrando el nombre del jugador, se ocultará el nombre
                {
                    namePlayer.text = "";
                    textBalloon.gameObject.SetActive(false);
                }
                else //Al hacer click en una casilla que no esté mostrando el nombre del jugador, se mostrará junto a un globo de texto
                {
                    namePlayer.text = strName;
                    textBalloon.gameObject.SetActive(true);
                }
            }
            
        }
    }
}