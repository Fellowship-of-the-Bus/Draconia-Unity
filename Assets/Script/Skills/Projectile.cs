using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading;
using System.Linq;

public enum ProjectileType {
  None,
  Arrow,
  Fireball
}

public enum ProjectileMovementType {
  Straight,
  Parabolic
}

public class Projectile {
  protected BattleCharacter source;
  protected Effected target;
  Action callback;
  GameObject projectile;
  Vector3 direction;

  static GameObject Arrow = Resources.Load("Projectiles/Arrow") as GameObject; //Must be a prefab containing an instance of the desired model named "Projectile"
  static GameObject Fireball = Resources.Load("Projectiles/Fireball") as GameObject;

  static Dictionary<ProjectileType,GameObject> Projectiles = new Dictionary<ProjectileType,GameObject>() {
    {ProjectileType.None, null},
    {ProjectileType.Arrow, Projectile.Arrow},
    {ProjectileType.Fireball, Projectile.Fireball}
  };

  public Projectile(BattleCharacter source, Effected target, ProjectileType projType, ProjectileMovementType moveType, float speed, Action callback) {
    this.source = source;
    this.target = target;
    this.callback = callback;
    if (Projectiles[projType]) {
      projectile = GameObject.Instantiate(Projectiles[projType], source.leftHand) as GameObject;
    }
    else projectile = null;
    direction = (target.transform.position - source.transform.position).normalized;
    GameManager.get.waitFor(GameManager.get.StartCoroutine(move(moveType, speed)));
  }

  IEnumerator move(ProjectileMovementType moveType, float speed) {
    const float height = 2f;
    if (projectile) {
      Transform proj = projectile.transform.Find("Projectile");
      projectile.transform.SetParent(null);
      Quaternion angle = Quaternion.LookRotation(direction + new Vector3(0,height,0));
      proj.rotation = angle;

      yield return GameManager.get.moveObject(
        projectile,
        speed,
        projectile.transform.position,
        target.transform.position,
        0,
        1,
        null,
        height,
        (t) => {
          const float totalRotation = 90f;
          proj.rotation = angle;
          proj.Rotate(Vector3.right,totalRotation*t);
        },
        false,
        moveType == ProjectileMovementType.Parabolic
      );
    }
    GameObject.Destroy(projectile);
    callback();
  }
}
