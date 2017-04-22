using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class Effected : EventManager {
  TypeMap<Heap<Effect>> effects = new TypeMap<Heap<Effect>>();

  public virtual void applyEffect(Effect effect) {
    if (!effects.ContainsKey(effect)) {
      effects.Add(effect, new Heap<Effect>());
    }
    effect.apply(this);
    Heap<Effect> l = effects.Get(effect);

    //if stackable activate without doing any max checks
    if (effect.stackable) {
      l.add(effect);
      effect.activate();
      return;
    }

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

  public virtual void removeEffect(Effect effect) {
    Heap<Effect> l = effects.Get(effect);
    Debug.Assert(l.Count != 0);

    //if stackable remove it without doing any max checks
    if (effect.stackable) {
      l.remove(effect);
      effect.deactivate();
      effect.remove();
      return;
    }

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

  public EffectKind getEffect<EffectKind>() where EffectKind : class {
    Heap<Effect> heap = effects.Get(typeof(EffectKind));
    return heap != null ? heap.getMax() as EffectKind : null;
  }

  public List<Effect> getEffects() {
    List<Effect> ret = new List<Effect>();
    foreach (Type t in effects.dic.Keys) {
      Effect e = effects.Get(t).getMax();
      if (e != null) {
        if (e.stackable) {
          foreach (Effect e2 in effects.Get(t).heap) {
            ret.Add(e2);
          }
        } else ret.Add(e);
      }
    }
    return ret;
  }
}
