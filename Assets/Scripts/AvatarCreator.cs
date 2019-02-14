using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class AvatarCreator : MonoBehaviour
    {
        public bool asignado;
        public Image Avatar;

        // Start is called before the first frame update
        private void Start()
        {
        }

        public void ConstruirAvatar(string nombreSprite)
        {
            var faces =GameManager.instance.GetAvatarFaces(nombreSprite);
            Avatar.gameObject.SetActive(true);
            Avatar.sprite = faces.happy;
            asignado = true;            
        }
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