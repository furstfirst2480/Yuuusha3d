using UnityEngine;
using UnityEngine.UI;

public class item: MonoBehaviour
{
    public Sprite[] itemSprites; // Array of sprites representing different picture items
    private int currentItemIndex = 0; // Index of the current picture item
    public GameObject HealthPotion;
    public GameObject ManaPotion;
    public GameObject Skill;
    // Reference to the Image component to display the picture item
    private Image imageComponent;

    void Start()
    {
        // Get the Image component attached to this GameObject
        imageComponent = GetComponent<Image>();

        // Set the initial picture item
        if (itemSprites.Length > 0)
        {
            imageComponent.sprite = itemSprites[currentItemIndex];
        }
    }

    void Update()
    {
        // Check if Tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Increase the current item index
            currentItemIndex++;

            // If the index goes beyond the array length, wrap around to the beginning
            if (currentItemIndex >= itemSprites.Length)
            {
                currentItemIndex = 0;
            }

            // Update the displayed picture item
            imageComponent.sprite = itemSprites[currentItemIndex];
        }
        if(currentItemIndex == 0)
        {
            HealthPotion.SetActive(true);
            Skill.SetActive(false);

        }
        if (currentItemIndex == 1)
        {
            ManaPotion.SetActive(true);
            HealthPotion.SetActive(false);
        }
        if (currentItemIndex == 2)
        { 
            Skill.SetActive(true);
            ManaPotion.SetActive(false);
        }
    }
}
