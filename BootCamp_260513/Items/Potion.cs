using RPGGame.Enums;

namespace RPGGame.Items
{
    internal class Potion : Item
    {
        public PotionTypeEnum PotionType { get; set; }
        public int Heal { get; set; }

        public override ItemEnum ItemType => ItemEnum.Potion;

        // 포션 객체 생성 후 반환
        public override Item CreateItem()
        {
            return new Potion { Name = Name, Price = Price, Heal = Heal, PotionType = PotionType };
        }
    }
}