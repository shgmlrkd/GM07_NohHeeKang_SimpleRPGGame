using RPGGame.Characters;

namespace RPGGame.Interface
{
    interface IEquipable
    {
        // 현재 이 아이템이 장착되어 있는지 여부
        bool IsEquipped { get; set; }

        // 장착
        void Equip(Player player);

        // 장착 해제
        void UnEquip(Player player);
    }
}
