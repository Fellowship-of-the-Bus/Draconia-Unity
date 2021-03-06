using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillButton : MonoBehaviour {
  [SerializeField]
  private Tooltip tooltip = null;
  [SerializeField]
  private Image image = null;
  [SerializeField]
  private Image cooldown = null;
  [SerializeField]
  private Button button = null;

  public void AddListener(UnityAction call) {
    button.onClick.AddListener(call);
  }

  public void clearSkill() {
    button.interactable = false;
    tooltip.tiptext = "";
    image.color = Color.white;
  }

  public void setSkill(ActiveSkill skill) {
    bool usable = skill.canUse();
    button.interactable = usable;
    tooltip.tiptext = skill.tooltip;

    Sprite skillImage = SkillList.get.skillImages[skill.GetType()];
    if (skillImage != null) {
      image.overrideSprite = skillImage;
    }

    if (usable) {
      cooldown.fillAmount = 0f;
    } else {
      if (skill.curCooldown != 0) {
        cooldown.fillAmount = (float)skill.curCooldown / (float)(skill.maxCooldown + 1);
      } else {
        cooldown.fillAmount = 1f;
      }
    }
  }

  public void disable() {
    button.interactable = false;
  }

  public void setSelected(bool selected) {
    if (selected) {
      image.color = Color.green;
    } else {
      image.color = Color.white;
    }
  }
}
