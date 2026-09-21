
public class Define
{
    public enum EItemType
    {
        Equipment,
        Ingredient,
        Product
    }

    public enum EEquipmentType
    {
        Weapon,
        Armor,
    }

    public enum EInteractionType
    {
        Item,
        CraftingStation,
        SellingPortal,
        StorePortal,
        EntryPortal,
        ReturnPortal
    }

    public enum EEffectType
    {
        None,
        MuzzleFlash,
        Wave,
        NormalHit,
        BulletTrail,
    }
}
