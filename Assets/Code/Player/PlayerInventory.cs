using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int? currentlyShowing = null;

    public List<PlayerInventoryItem> collectableItems = new List<PlayerInventoryItem>();

    void Update()
    {
        CheckNextItem();
        CheckPrevItem();

        CheckInventory();
        ShowCurrentItem();
    }

    public void TryAddItem(CollectableItem zCollectable)
    {

    }

    void ShowCurrentItem()
    {
        if (currentlyShowing.HasValue)
        {
            for (int i = 0; i < collectableItems.Count; i++)
            {
                var collectable = collectableItems[i];
                collectable.visual.EnsureActive(i == currentlyShowing.Value);
            }
        }
    }

    void CheckInventory()
    {
        if (currentlyShowing.HasValue == false && collectableItems.Count > 0)
            currentlyShowing = 0;
    }

    void CheckNextItem()
    {
        var inputType = Player.Instance.playerInput;
        var action = false;

        switch (inputType)
        {
            case ePlayerInput.KEYBOARD:
                action = Input.GetKeyDown(KeyCode.Period);
                break;

            case ePlayerInput.GAMEPAD:
                action = PlayerInputManager.GetButtonDown(0, ePadButton.RIGHT_BUMPER);
                break;
        }

        if (action)
        {
            for (int i = 0; i < collectableItems.Count; i++)
            {
                var index = Wrap(currentlyShowing.Value + i);
                var collectable = collectableItems[index];
                if (collectable.Have)
                {
                    currentlyShowing = index;
                    return;
                }
            }
        }
    }

    void CheckPrevItem()
    {
        var inputType = Player.Instance.playerInput;
        var action = false;

        switch (inputType)
        {
            case ePlayerInput.KEYBOARD:
                action = Input.GetKeyDown(KeyCode.Period);
                break;

            case ePlayerInput.GAMEPAD:
                action = PlayerInputManager.GetButtonDown(0, ePadButton.RIGHT_BUMPER);
                break;
        }
        if (action)
        {
            for (int i = 0; i < collectableItems.Count; i++)
            {
                var index = Wrap(currentlyShowing.Value - i);
                var collectable = collectableItems[index];
                if (collectable.Have)
                {
                    currentlyShowing = index;
                    return;
                }
            }
        }
    }

    private int Wrap(int zIndex)
    {
        if (zIndex < 0)
            return zIndex + collectableItems.Count;
        if (zIndex >= collectableItems.Count)
            return zIndex - collectableItems.Count;
        return zIndex;
    }
}
