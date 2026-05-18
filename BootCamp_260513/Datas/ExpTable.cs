namespace RPGGame.Datas
{
    internal static class ExpTable
    {
        public static int[] RequiredExp;

        // 경험치 테이블 생성자
        static ExpTable()
        {
            // 레벨 10까지만 하기위해 배열 길이를 10으로함
            RequiredExp = new int[10];

            // 1 레벨 경험치는 15로 시작
            int exp = 15;

            // 1.3배씩 증가해서 정수로 받고 RequiredExp 변수에 저장
            for (int level = 1; level < RequiredExp.Length; level++)
            {
                RequiredExp[level] = exp;

                exp = (int)(exp * 1.3f);
            }
        }
    }
}
