using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class SpritesLoader : MonoBehaviour
    {
        [SerializeField]
        private Image playerAvatar;

        public void SetAvatarIcon(Sprite sprite)
        {
            playerAvatar.sprite = sprite;
        }
    }
}