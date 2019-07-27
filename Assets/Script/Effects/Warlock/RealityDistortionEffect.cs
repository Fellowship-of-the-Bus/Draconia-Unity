using System;

public class RealityDistortionEffect : DurationEffect {
  private Attributes attr;

  protected override void onActivate() {
    attachListener(owner, EventHook.activateEffect);
    attachListener(owner, EventHook.deactivateEffect);

    attr = owner.attrChange.clone();
    owner.attrChange = -attr;
  }

  protected override void onDeactivateEffects() {
    owner.attrChange = attr;
  }

  protected override void onDeactivateListeners() {
    detachListener(owner);
  }

  protected override void additionalEffect(Draconia.Event e) {
    owner.attrChange += attr;
    attr += owner.attrChange;
    owner.attrChange = -attr;
  }
}

