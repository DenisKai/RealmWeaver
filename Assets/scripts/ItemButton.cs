using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public GameObject itemPrefab;

    private Button button;
    
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CreatePrefab);
    }

    private void CreatePrefab () {
        var buttonPos = transform.position - (transform.forward * 0.025f);
        Instantiate(itemPrefab, buttonPos, Quaternion.identity);
    }
}
