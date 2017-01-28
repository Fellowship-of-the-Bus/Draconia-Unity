using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class Effected : EventManager {
    TypeMap<Heap<Effect>> effects = new TypeMap<Heap<Effect>>();

    public void applyEffect(Effect effect) {
    if (!effects.ContainsKey(effect)) {
      effects.Add(effect, new Heap<Effect>());
    }
    effect.apply(this);
    Heap<Effect> l = effects.Get(effect);

    //find max level of effects in list
    Effect maxEffect = l.getMax();

    //if newly applied effect is the highest level
    //activate it and deactivate the highest leveled one.
    if (maxEffect != null && effect > maxEffect) {
      maxEffect.deactivate();
      effect.activate();
    } else if (maxEffect == null) {
      effect.activate();
    }
    l.add(effect);
  }


  public void removeEffect(Effect effect) {
    Heap<Effect> l = effects.Get(effect);
    Debug.Assert(l.Count != 0);

    Effect maxEffect = l.getMax();
    l.remove(effect);
    if (effect == maxEffect) {
      effect.deactivate();
      if (l.getMax() != null) {
        l.getMax().activate();
      }
    }
    effect.remove();
  }
}
