using RPGGame.Characters;
using RPGGame.Enums;

namespace RPGGame.Items
{
    internal class Weapon : EquipableItem
    {

        public int Attack { get; set; }

        public override ItemEnum ItemType => ItemEnum.Weapon;
        
        // 무기 객체 생성 후 반환
        public override Item CreateItem()
        {
            return new Weapon { Name = Name, Price = Price, Attack = Attack };
        }

        // 플레이어의 스탯을 state 매개변수 상태에 따라 올려주거나 내림 (장착, 해제)
        protected override void OnApplyStat(Player player, bool state)
        {
            player.UpdateItemStats(atk: Attack, isEquip: state);
        }
    }
}
