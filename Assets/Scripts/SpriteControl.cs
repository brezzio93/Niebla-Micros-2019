using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class SpriteControl : MonoBehaviour
    {
        [SerializeField]
        private JugadoresEnEspera playerTemplate;

        private List<JugadoresEnEspera> buttons;

        // Start is called before the first frame update
        private void Start()
        {
            buttons = new List<JugadoresEnEspera>();

            GameManager.instance.AlEntrarJugador += Instance_AlEntrarJugador;

            InstanciarBotones();
        }

        private void Instance_AlEntrarJugador(Player obj)
        {
            //throw new System.NotImplementedException();

            foreach (JugadoresEnEspera button in buttons)
            {
                if (!button.asignado)
                {
                    button.ConstruirAvatar(obj.CustomProperties["Imagen"] as string);

                    break;
                }
            }
        }

        public void InstanciarBotones()
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers - 1; i++)
            {
                JugadoresEnEspera button = Instantiate(playerTemplate) as JugadoresEnEspera;
                button.gameObject.SetActive(true);
                button.transform.SetParent(playerTemplate.transform.parent, false);
                buttons.Add(button);
            }
        }

        public void ListarJugadores()
        {
            Player[] playerNoHost = PhotonNetwork.PlayerList;

            List<Player> PlayerList = new List<Player>();

            for (int i = 0; i < playerNoHost.Length; i++)

                PlayerList.Add(playerNoHost[i]);

            PlayerList.Remove(PhotonNetwork.MasterClient);

            //if (buttons.Count > 0)
            //{
            //    foreach (JugadoresEnEspera button in buttons)
            //        Destroy(button.gameObject);
            //}
            //buttons.Clear();

            foreach (JugadoresEnEspera button in buttons)
            {
                //foreach (Player p in PlayerList)
                //{
                //    Sprite sprite = GameManager.instance.Avatars.FirstOrDefault(s => s.name == (p.CustomProperties["Imagen"] as string));
                //}
            }
        }
    }
}