using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [Tooltip("Перетащите сюда префаб персонажа из папки Prefabs")]
    public GameObject playerPrefab;

    [Tooltip("Точка, где появится персонаж (если пусто — появится в координатах самого спавнера)")]
    public Transform spawnPoint;

    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Не назначен префаб персонажа в PlayerSpawner!");
            return;
        }

        // Определяем координаты и поворот для появления
        Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion rotation = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        // Создаем персонажа на сцене
        GameObject spawnedPlayer = Instantiate(playerPrefab, position, rotation);
        
        // Убираем приписку "(Clone)" из имени объекта в иерархии для порядка
        spawnedPlayer.name = playerPrefab.name;
    }
}