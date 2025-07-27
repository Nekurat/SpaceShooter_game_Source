using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Sprite[] backgrounds;
    private SpriteRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (backgrounds.Length > 0)
        {
            int index = Random.Range(0, backgrounds.Length);
            _renderer.sprite = backgrounds[index];
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
