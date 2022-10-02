using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstructions : MonoBehaviour
{
    private CodeDefusalInstruction codeInstruction = new CodeDefusalInstruction();
    private WireCutDefusalInstruction wireInstruction = new WireCutDefusalInstruction();
    private LiquidDefusalInstruction liquidInstruction = new LiquidDefusalInstruction();
    private MagneticLockDefusalInstruction magneticLockInstruction = new MagneticLockDefusalInstruction();
    private ScrewDriverDefusalInstruction screwDriverInstruction = new ScrewDriverDefusalInstruction();
    private TurnyHandleDefusalInstruction turnyHandleDefusalInstruction = new TurnyHandleDefusalInstruction();

    private void Awake()
    {
        Reset();
    }
    public DefusalInstruction GetInstruction(eDefusalType zType)
    {
        switch (zType)
        {
            case eDefusalType.CODE: return codeInstruction;
            case eDefusalType.WIRE_CUT: return wireInstruction;
            case eDefusalType.LIQUID: return liquidInstruction;
            case eDefusalType.MAGNETIC_LOCK: return magneticLockInstruction;
            case eDefusalType.SCREW_DRIVER_PANEL: return screwDriverInstruction;
            case eDefusalType.TURNY_HANDLE: return turnyHandleDefusalInstruction;
        }
        return null;
    }
    public void ItemCollected(CollectableItem zItem)
    {
        var collectables = Player.Instance.inventory.collectableItems;
        var inventoryCollectable = collectables.Find(p => p.Type == zItem.Type);

        if (inventoryCollectable == null)
        {
            //doesn't make sense, but whatever
            return;
        }

        //pattern matching!
        switch (inventoryCollectable)
        {
            case PlayerInventoryBattery battery:
                magneticLockInstruction.battery1 = battery.battery1.voltage;
                if (battery.battery2 != null)
                    magneticLockInstruction.battery2 = battery.battery2.voltage;
                break;

            case PlayerInventoryBeaker beaker:
                liquidInstruction.haveBeaker = true;

                liquidInstruction.colourOrder.Clear();
                liquidInstruction.colourOrder.AddRange(beaker.colourOrder);
                break;

            default:
                switch (inventoryCollectable.Type)
                {
                    case eCollectableItem.SCREW_DRIVER:
                        screwDriverInstruction.haveScrewDriver = true;
                        break;

                    case eCollectableItem.WIRE_CUTTERS:
                        wireInstruction.haveWireCutters = true;
                        break;
                }
                break;

        }
    }

    public void Reset()
    {
        codeInstruction = new CodeDefusalInstruction();
        wireInstruction = new WireCutDefusalInstruction();
        liquidInstruction = new LiquidDefusalInstruction();
        magneticLockInstruction = new MagneticLockDefusalInstruction();
        screwDriverInstruction = new ScrewDriverDefusalInstruction();
        turnyHandleDefusalInstruction = new TurnyHandleDefusalInstruction();
    }
}
