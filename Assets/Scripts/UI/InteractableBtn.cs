using UnityEngine;
using UnityEngine.UI;

public class InteractableBtn : MonoBehaviour
{
    public Sprite activeSprite; // Спрайт для активной кнопки
    public Sprite disabledSprite; // Спрайт для неактивной кнопки

    private Button button;

    void Start()
    {
        button = GetComponent<Button>(); // Получаем компонент кнопки
        UpdateButtonImage(); // Инициализируем изображение кнопки при запуске
    }

    private void Update()
    {
        UpdateButtonImage(); // Обновляем изображение кнопки
    }

    void UpdateButtonImage()
    {
        if (button.interactable)
        {
            button.image.sprite = activeSprite; // Устанавливаем спрайт для активной кнопки
        }
        else
        {
            button.image.sprite = disabledSprite; // Устанавливаем спрайт для неактивной кнопки
        }
    }
}