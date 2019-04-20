public interface PoolableObject {
  /** Called when object is allocated from the pool */
  void OnPoolInitialize();

  /** Called when object is returned to the pool */
  void OnPoolRelease();
}
