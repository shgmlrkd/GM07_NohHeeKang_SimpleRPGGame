namespace BootCamp_260513
{
    /********************************************************
    [제네릭 제약]
    - 일반화 자료형을 선언할때 제약조건을 선언하여 사용당시 쓸 수 있는 자료형을 제한
    - 불필요한 타입이 들어가는 것을 방지하고 원하는 기능을 보장하는데 유용
    - 제네릭 타입 제한 "where T : 조건 형식"으로 사용하며 여러 개를 조합할 수도 있음

    제네릭은 타입을 나중에 결정하는 문법인데 아무타입이나 들어오면 문제가 생길 수 있음
    ex) 공격 기능을 실행해야하는데 int가 들어오거나 이름을 출력해야하는데 Name 프로퍼티가 없는 타입이 들어오면 안됨
        그래서 where 키워드를 사용해서 `이런 조건을 만족하는 타입만 들어올 수 있다`라고 제한함

        게임에서 공격 가능, 데미지 가능, 이동 가능, 상호작용 같은 기능 기반 설계를 할텐데
        Attacker<int>??     <- X
        Attacker<string>??  <- X
        
    class Attacker<T> where T : IAttackable => T에는 반드시 IAttackable을 구현한 타입만 넣을 수 있음

    유니티에서 제약을 사용하는 이유?
    1. 컴포넌트 기반 구조
     - 즉 어떤 기능을 가진 객체냐?
     - 데미지 받음, 이동 가능, 공격 가능, 상호작용 가능
    
    ex)
    class Player where T : Mnobehavior
    class Player where T : IDamageable
    class Player where T : Component

    2. GetComponent<T>() 같은 구조
    GetComponent<T> <=> where T : Component 와 같음

    3. 구조 안전성
    제약이 없으면 이상한 타입을 사용, 잘못된 객체 전달, 런타임 오류가 발생될 수 있음
    그래서 제약으로 사용 가능한 타입 범위를 제한

    where T : Monobehavior, IDamageable => 유니티 컴포넌트면서 데미지를 받을 수 있는 객체만 허용함
    ********************************************************/

    /********************************************************
    class Struct<T> where T : struct {} => T는 구조체만 사용 가능
    class Class<T> where T : class {} => T는 클래스만 사용 가능
    class New<T> where T : new() {} => T는 매개변수가 없는 생성자가 있는 자료형만 사용 가능
    class Parent<T> where T : Parent {} => T는 parent, 파생 클래스만 사용 가능
    class Interface<T> where T : IComparable {} => T는 인터페이스를 포함한 자료형만 사용 가능
    ********************************************************/
    
    // 클래스 타입 즉 참조 타입만 허용함
    // where T : class -> 참조 타입만 허용하고, int, bool, float 같은 값 타입은 사용 불가
    // class 제약은 내가 만든 클래스만 가능한게 아니라 참조 타입 전체를 뜻함
    class ReferenceOnly<T> where T : class
    {
        public T Data { get; set; }

        public ReferenceOnly(T data)
        {
            Data = data;
        }
    }

    // 구조체 타입 즉 값 타입만 허용함
    // where T : struct -> 값 타입만 허용하고 string이나 class같은 참조 타입은 사용 불가
    class ValueOnly<T> where T : struct
    {
        public T Value { get; set; }

        public ValueOnly(T value)
        {
            Value = value;
        }
    }

    // where T : new() -> 매개변수가 없는 public 기본 생성자가 있는 타입만 사용 가능
    // new() 있어야 제네릭 내부에서 new T() 사용 가능
    class Factory<T> where T : new()
    {
        public T CreateInstance()
        {
            return new T();
        }
    }

    class Player
    {
        public string Name { get; set; } = "홍길동";
    }

    class Character
    { 
        public string Name { get; set; }
    }

    class Warrior : Character { }
    class Mage { }

    // where T : Character -> Character 자기 자신 또는 상속한 자식 클래스만 사용 가능
    class CharacterManager<T> where T : Character
    {
        public void PrintName(T Character)
        {
            Console.WriteLine(Character.Name);
        }
    }

    //
    class GameObject { }

    interface IDamageble 
    {
        void TakeDamage();
    }

    class Enemy : GameObject, IDamageble
    {
        public void TakeDamage()
        {
            Console.WriteLine("a");
        }
    }

    // where T : GameObject, IDamageble -> GameObject를 상속하고 IDamageble 인터페이스도 구현한 타입만 사용 가능
    // 주의 : 클래스 제약은 먼저 써야함
    class DamageHandler<T> where T : GameObject, IDamageble
    { 
        public void ApplyDamage(T obj)
        {
            obj.TakeDamage();
        }
    }

    interface IAttackable
    {
        void Attack();
    }

    class Monster : IAttackable
    {
        public void Attack()
        {

        }
    }

    // where T : IAttackable -> 인터페이스를 구현한 타입만 사용 가능
    class Attacker<T> where T : IAttackable
    {
        public void Att(T att)
        {
            att.Attack();
        }
    }

    // IComparable 인터페이스 제한
    // IComparable : CompareTo()를 통해 두 값을 비교할 수 있게 해주는 인터페이스
    // where T : IComparable -> IComparable 인터페이스를 구현한 타입만 사용 가능
    class InterfaceT<T> where T : IComparable { }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 클래스(참조) 타입 
            ReferenceOnly<string> refInstance = new ReferenceOnly<string>("Hello");
            //ReferenceOnly<int> intInstance = new ReferenceOnly<int>(1234); 값 타입이라 컴파일 오류

            // 구조체(값) 타입
            ValueOnly<int> valueInstance = new ValueOnly<int>(100);
            //ValueOnly<string> stringInstance = new ValueOnly<string>("Hello"); 참조 타입이라 컴파일 오류

            // new 제약
            Factory<Player> factory = new Factory<Player>();
            Player p = factory.CreateInstance();

            // 특정 클래스 제한
            // 자기 자신이므로 사용 가능
            CharacterManager<Character> c1 = new CharacterManager<Character>();

            // Character를 상속 받았으므로 사용 가능
            CharacterManager<Warrior> c2 = new CharacterManager<Warrior>();

            // Character를 상속 받지 않았으므로 사용 불가
            // CharacterManager<Mage> c3 = new CharacterManager<Mage>(); 

            // 여러개 제한 조건
            // where T : GameObject, IDamageble
            DamageHandler<Enemy> enemy = new DamageHandler<Enemy>();

            // GameObject, IDamagable 둘 다 상속 받지 않았으므로 사용 불가
            // DamageHandler<string> stringHandler = new DamageHandler<string>();

            // GameObject만 상속 받고 IDamageble은 상속 받지 않았으므로 사용 불가
            // DamageHandler<GameObject> obj = new DamageHandler<GameObject>();

            // 인터페이스
            Attacker<Monster> m = new Attacker<Monster>();
            // Attacker<int> i = new Attacker<int>();

            // IComparable
            InterfaceT<int> interT = new InterfaceT<int>();
            InterfaceT<string> interString = new InterfaceT<string>();
        }
    }
}
