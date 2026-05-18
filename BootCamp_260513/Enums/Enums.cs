namespace RPGGame.Enums
{
    // RPGGame에 필요한 모든 enum을 모아둠
    enum JobTypeEnum                    // 직업
    {
        Warrior,
        Mage,
        Archer,
        JobLength
    }

    enum MonsterTypeEnum                // 몬스터 등급
    {
        Normal,     
        Elite,      
        Boss
    }

    enum MainMenuEnum                   // 메인 메뉴
    {
        Start,
        Exit
    }

    enum AttackTypeEnum                 // 공격 타입
    { 
        BaseAttack,
        SkillAttack,
        Back
    }

    enum HuntingActionEnum              // 사냥터에서의 액션 타입
    {
        Attack,
        Inventory,
        Run
    }

    enum ItemEnum                       // 아이템 종류 타입
    {
        Weapon,
        Armor,
        Potion,
        Back
    }

    enum PotionTypeEnum                 // 포션 타입
    {
        Hp,
        Mp
    }

    public enum InventoryActionEnum     // 인벤토리 장작/사용, 삭제
    {
        UseOrEquipUnEquip, 
        Drop, 
        Back
    }

    public enum StatSelectEnum          // 레벨업 시 선택할 스탯
    {
        Hp,
        Mp,
        Attack,
        Defense
    }
    enum SkillMenuEnum                  // 스킬 선택을 위한 enum
    {
        Skill1,
        Skill2,
        Skill3,
        Skill4,
        Back 
    }

    enum GameStateEnum                  // 전체적인 게임 상태
    {
        MainMenu,
        CreateCharacter,

        PlayMenu,
        Hunting,
        Shop,

        Exit
    }
}
