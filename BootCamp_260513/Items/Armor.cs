using RPGGame.Characters;
using RPGGame.Enums;

namespace RPGGame.Items
{
    // 장착 가능한 아이템 상속 받은 방어구(Armor)
    internal class Armor : EquipableItem
    {
        public int Defense { get; set; }

        public override ItemEnum ItemType => ItemEnum.Armor;

        // 방어구 객체 생성 후 반환
        public override Item CreateItem()
        {
            return new Armor { Name = Name, Price = Price, Defense = Defense };
        }

        // 플레이어의 스탯을 state 매개변수 상태에 따라 올려주거나 내림 (장착, 해제)
        protected override void OnApplyStat(Player player, bool state)
        {
            player.UpdateItemStats(def: Defense, isEquip: state);
        }
    }
}
