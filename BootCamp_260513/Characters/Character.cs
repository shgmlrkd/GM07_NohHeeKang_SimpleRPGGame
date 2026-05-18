using RPGGame.Datas;
using RPGGame.Interface;

namespace RPGGame.Characters
{
    internal class Character
    {
        public GameCharacterData CharacterData { get; protected set; }
        public int Hp { get; protected set; }
        public int Mp { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }
        
        // 몬스터, 플레이어의 공통 스탯 초기화
        protected virtual void Init()
        {
            if (CharacterData != null)
            {
                Hp = CharacterData.Hp;
                Mp = CharacterData.Mp;
                Attack = CharacterData.Attack;
                Defense = CharacterData.Defense;
            }
        }

        // 정보 보여주기
        public virtual void ShowInfo(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write($"이름 : {CharacterData.Name}");

            Console.SetCursorPosition(x, y + 1);
            Console.Write($"체력 : {Hp}    마나 : {Mp}");

            Console.SetCursorPosition(x, y + 2);
            Console.Write($"공격력 : {Attack}    방어력 : {Defense}");
        }

        // 죽었는지 확인
        public bool IsDead()
        {
            return Hp <= 0;
        }

        // 데미지 주기
        public virtual int TakeDamage(int damage)
        {
            int totalDamage = damage - Defense;

            if (totalDamage < 0)
            {
                totalDamage = 0;
            }

            Hp -= totalDamage;

            return totalDamage;
        }
    }
}
