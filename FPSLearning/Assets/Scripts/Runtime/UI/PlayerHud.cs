using Runtime.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField] private CanvasGroup hudGroup;
        [SerializeField] private Image healthImage;

        public void Initialized(PlayerController controller)
        {
            controller.OnChangedHealth += SetHealthImage;
            hudGroup.alpha = 1;
        }

        public void DestroyHud(PlayerController controller)
        {
            controller.OnChangedHealth -= SetHealthImage;
        }
        
        private void SetHealthImage(float health, float maxHealth)
        {
            healthImage.fillAmount = health / maxHealth;
        }
    }
}