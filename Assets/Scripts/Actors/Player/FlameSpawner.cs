using UnityEngine;

public class FlameSpawner : AbstractedObjectPool<Flame> {
    [SerializeField] private Flame flamePrefab;
    private void Awake() {
        InitPool(flamePrefab);
    }
}
