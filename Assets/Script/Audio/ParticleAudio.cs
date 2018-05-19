using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class ParticleAudio : MonoBehaviour {
  public AudioClip clip;
  public ParticleSystem part;
  public List<ParticleCollisionEvent> collisionEvents;

  void Start() {
    part = GetComponent<ParticleSystem>();
    collisionEvents = new List<ParticleCollisionEvent>();
  }

  void OnParticleCollision(GameObject other) {
    int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

    int i = 0;
    while (i < numCollisionEvents) {
      AudioSource.PlayClipAtPoint(clip, collisionEvents[i].intersection);
      i++;
    }
  }
}