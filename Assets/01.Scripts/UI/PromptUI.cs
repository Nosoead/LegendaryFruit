using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptUI : UIBase
{
   [SerializeField] private WeaponSO weaponSo;
   [SerializeField] private TextMeshProUGUI gradeText;
   [SerializeField] private TextMeshProUGUI promptName;
   [SerializeField] private TextMeshProUGUI promptDescription;
   [SerializeField] private TextMeshProUGUI promptAttackPower;
   [SerializeField] private Image promptImage;
   private WeaponSO currentWeaponSo;

   public override void Open()
   {
      DataToPromptText();
      base.Open();
   }

   public void SetWeapon(WeaponSO newWeaponSo)
   {
      currentWeaponSo = newWeaponSo;
   }
   private void DataToPromptText()
   {
      gradeText.text = currentWeaponSo.gradeType.ToString();
      promptName.text = currentWeaponSo.weaponName;
      promptDescription.text = currentWeaponSo.description;
      //promptAttackPower.text = currentWeaponSo.attackPower.ToString();
      promptImage.sprite = currentWeaponSo.weaponSprite;


      // IInteractable로 무기정보 가져오기?

      //F 꾹누를때 fillamount 조정
   }
}