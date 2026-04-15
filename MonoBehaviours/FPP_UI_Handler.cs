using Il2CppTMPro;

using UnityEngine;
using UnityEngine.UI;

namespace FlatPlayerPlus.MonoBehaviours
{
    public class FPP_UI_Handler : MonoBehaviour
    {
        public TextMeshProUGUI ammoTextSmall;
        public TextMeshProUGUI ammoTextMedium;
        public TextMeshProUGUI ammoTextLarge;
        public TextMeshProUGUI healthText;
        public Image healthBar;

        public GameObject healthBarUIParent;
        public GameObject ammoUIParent;

        public enum UIPosition
        { Top, Bottom }

        public static UIPosition HealthBarUIPosition { get; set; } = UIPosition.Bottom;
        public static UIPosition AmmoUIPosition { get; set; } = UIPosition.Top;

        public float CurrHealth { get; set; }

        public float MaxHealth { get; set; }

        private void Start()
        {
            ammoTextSmall.text = "0";
            ammoTextMedium.text = "0";
            ammoTextLarge.text = "0";
            CurrHealth = 0f;
        }

        // Update is called once per frame
        private void Update()
        {
            // healthBar.fillAmount = health / maxHealth;
            var healthPercentage = Mathf.FloorToInt(MaxHealth > 0 ? (CurrHealth / MaxHealth) * 100 : 0);
            healthPercentage = Mathf.Clamp(healthPercentage, 0, 100);
            healthText.text = $"{healthPercentage}%";

            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, CurrHealth / MaxHealth, Time.deltaTime * 5f);
        }

        public void UpdateAmmoText(int lightAmmo, int mediumAmmo, int heavyAmmo)
        {
            ammoTextSmall.text = $"{lightAmmo}";
            ammoTextMedium.text = $"{mediumAmmo}";
            ammoTextLarge.text = $"{heavyAmmo}";
        }

        public void UpdateUIPosition(UIPosition healthBarPosition, UIPosition ammoPosition)
        {
            switch (healthBarPosition)
            {
                case UIPosition.Bottom:
                    healthBarUIParent.transform.localPosition = new(-840, -450, 0f);
                    break;

                case UIPosition.Top:
                    healthBarUIParent.transform.localPosition = new(-840, 510, 0f);
                    break;
            }

            switch (ammoPosition)
            {
                case UIPosition.Bottom:
                    ammoUIParent.transform.localPosition = new(750, -450, 0f);
                    break;

                case UIPosition.Top:
                    ammoUIParent.transform.localPosition = new(750, 440, 0f);
                    break;
            }
        }
    }
}