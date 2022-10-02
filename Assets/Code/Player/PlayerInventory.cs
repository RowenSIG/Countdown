using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int currentlyShowing = 0;

    public List<PlayerInventoryItem> collectableItems = new List<PlayerInventoryItem>();

    void Update()
    {
        if (Room.Instance.Mode == eMode.NORMAL)
        {
            CheckNextItem();
            CheckPrevItem();

            ShowCurrentItem();
        }
    }

    public void Reset()
    {
        currentlyShowing = 0;
        foreach (var item in collectableItems)
            item.Reset();
    }

    public void TryAddItem(CollectableItem zCollectable)
    {
        for (int i = 0; i < collectableItems.Count; i++)
        {
            var item = collectableItems[i];
            if (item.Type == zCollectable.Type)
            {
                item.Collected(zCollectable);
                currentlyShowing = i;
                break; ;
            }
        }
    }

    public bool CanCollect(eCollectableItem zItemType)
    {
        for (int i = 0; i < collectableItems.Count; i++)
        {
            var item = collectableItems[i];
            if (item.Type == zItemType)
            {
                return item.CanCollect();
            }
        }
        return true;
    }

    void ShowCurrentItem()
    {
        for (int i = 0; i < collectableItems.Count; i++)
        {
            var collectable = collectableItems[i];
            bool show = i == currentlyShowing && collectable.Have;

            if (show)
                collectable.Show();
            else
                collectable.Hide();

        }
    }

    void CheckNextItem()
    {
        var action = false;

        action |= Input.GetKeyDown(KeyCode.Period);
        action |= PlayerInputManager.GetButtonDown(0, ePadButton.RIGHT_BUMPER);

        if (action)
        {
            for (int i = 0; i < collectableItems.Count; i++)
            {
                var index = Wrap(currentlyShowing + i + 1);
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
        var action = false;

        action |= Input.GetKeyDown(KeyCode.Comma);
        action |= PlayerInputManager.GetButtonDown(0, ePadButton.LEFT_BUMPER);
        if (action)
        {
            for (int i = 0; i < collectableItems.Count; i++)
            {
                var index = Wrap(currentlyShowing - (i + 1));
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
