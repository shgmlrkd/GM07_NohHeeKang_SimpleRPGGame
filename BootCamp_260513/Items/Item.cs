using RPGGame.Enums;
namespace RPGGame.Items
{
    internal abstract class Item
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; protected set; } = 1;

        public abstract ItemEnum ItemType { get; }

        // 장착 가능한건 무기, 방어구 불가능한건 포션
        // 따라서 get만 설정하고 가상 프로퍼티로 만듦
        // 이러면 오버라이딩 할 시 get만 설정 가능 set은 없음
        public virtual bool CanEquip
        {
            get
            {
                return false;
            }
        }

        // 아이템 구입에 필요한 구체적인 아이템 객체를 생성하는 추상 메서드
        public abstract Item CreateItem();

        // 포션 같은 아이템의 개수를 표현하기 위한 메서드 AddCount, RemoveCount
        public void AddCount(int amount)
        { 
            Count += amount;
        }

        public void RemoveCount(int amount) 
        { 
            Count -= amount;

            if (Count < 0)
            { 
                Count = 0; 
            }
        }
    }
}