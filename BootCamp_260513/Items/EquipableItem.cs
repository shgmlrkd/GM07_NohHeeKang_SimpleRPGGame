using RPGGame.Characters;
using RPGGame.Interface;

namespace RPGGame.Items
{
    internal abstract class EquipableItem : Item, IEquipable
    {
        public bool IsEquipped { get; set; }
        public override bool CanEquip // 장착 가능 아이템이므로 무조건 true
        {
            get 
            { 
                return true; 
            }
        }

        // 장착
        public void Equip(Player player)
        {
            SetEquipState(player, true);
        }

        // 해제
        public void UnEquip(Player player)
        {
            SetEquipState(player, false);
        }

        // 스탯 적용
        private void SetEquipState(Player player, bool state)
        {
            IsEquipped = state;
            OnApplyStat(player, state);
        }

        // Weapon, Armor의 스탯을 플레이어한테 전달하는 메서드
        protected abstract void OnApplyStat(Player player, bool state);
    }
}
