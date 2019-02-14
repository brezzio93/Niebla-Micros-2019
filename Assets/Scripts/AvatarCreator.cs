﻿using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class AvatarCreator : MonoBehaviour
    {
        public bool asignado; //Este valor se ocupa al momento de listar los jugadores en la sala, desde la instancia del Host           
        public Image Avatar;

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
            var faces =GameManager.instance.GetAvatarFaces(nombreSprite);
            Avatar.gameObject.SetActive(true);
            Avatar.sprite = faces.happy;
            asignado = true; 
        }

        /// <summary>
        /// Se asigna una cara triste o feliz al jugador, dependiendo de si llega o no al trabajo
        /// </summary>
        /// <param name="nombreSprite"></param>
        /// <param name="llegada"></param>
        public void ConstruirAvatar(string nombreSprite, bool llegada)
        {
            var faces = GameManager.instance.GetAvatarFaces(nombreSprite);
            Avatar.gameObject.SetActive(true);
            if (llegada)
                Avatar.sprite = faces.happy;
            else Avatar.sprite = faces.sad;
            asignado = true;
        }
    }
}