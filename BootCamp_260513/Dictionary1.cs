namespace BootCamp_260513
{
    /********************************************************
    [딕셔너리]
    - Dictionary는 'key(키)' - 'value(값)' 형태로 데이터를 저장하는 자료구조

    - key와 value가 한 쌍으로 저장
    - 검색속도가 굉장히 빠름
    - 내부적으로 Hash Table(해시 테이블) 사용
    - 제네릭 기반임 (최신)
    - 키 값은 중복 허용 X
    - 순서를 보장하지 않음

    [Hash Table (간략)]
    - '키 - 값'을 쌍으로 저장하는 자료 구조
    - 키를 해시 함수에 의해 계산된 해시 코드로 변환하여 데이터를 저장하고 검색함
    - 빠른 검색 속도를 제공
    - 해시 테이블은 object 기반임 (예전)
    - 키와 값에 object타입을 모두 사용하기 떄문에 타입 안전성이 떨어짐
    - 박싱, 언박싱 발생 -> GC 부담줌

    [해시 함수]
    - 키를 해시코드로 변환하여 인덱스를 계산, 빠른 데이터 접근을 가능하게 함
    - 쉽게 말해 데이터를 저장할 주소 번호를 계산하는 함수

    [해시 테이블의 주의점]
    - 해시 함수가 서로 다른 입력값에 대해 동일한 해시 테이블 주소를 반환
    - 모든 입력값에 대해 고유한 해시 값을 만드는 것은 불가능하며 충돌은 피할 수 없음
    - 이 충돌을 해결하기 위해 체이닝, 개방 주소법이 있음

    해시 테이블 주의점 - 중요
    해결 방안 : 체이닝, 개방 주소법
    체이닝, 개방 주소법이 무엇인지까지는 정리해야함

    [해시 테이블 vs 딕셔너리] - 중요

    [Dictionary 생성]
    Dictionary<키 타입, 값 타입> 변수명

    Add : 데이터 추가 (중복 키면 예외 발생)
    TryAdd : 안전하게 데이터 추가
    ContainsKey : 키가 있는지 확인
    ContainsValue : 값이 있는지 확인
    TryGetValue : 안전하게 값 조회
    Remove : 특정 키를 통해 값을 삭제
    Clear : 모든 데이터 삭제
    Keys : 모든 키 가져오기
    Values : 모든 값 가져오기

    Add와 인덱서의 차이
    Add는 같은 키 추가 시 예외 발생
    인덱서는 기존 키가 있으면 값 수정, 없다면 새로 추가함
    ********************************************************/
    internal class Dictionary1
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> players = new Dictionary<int, string>();

            players.Add(1, "전사");
            players.Add(2, "마법사");
            players.Add(3, "도적");

            // TryAdd - 안전하게 값을 추가하는 방법
            players.TryAdd(3, "도둑");

            Console.WriteLine(players[1]);

            // ContainsKey
            // key 존재 여부 확인
            if (players.ContainsKey(2))
            {
                Console.WriteLine("있음");
            }

            players[3] = "궁수";
            Console.WriteLine(players[3]);

            players[4] = "드워프";
            Console.WriteLine(players[4]);

            // 특정 키를 삭제하는 것
            players.Remove(2);
            Console.WriteLine(players.Count);

            Console.WriteLine("============ 전체 출력 ============");

            foreach(KeyValuePair<int, string> player in players)
            {
                Console.WriteLine(player.Key + " : " + player.Value);
            }

            int searchKey = 4;

            // 안전하게 키를 통해 값을 조회하는 역할
            if (players.TryGetValue(searchKey, out string value))
            {
                Console.WriteLine(searchKey + "번 플레이어 : " + value);
            }
            else
            {
                Console.WriteLine(searchKey + "번 플레이어가 없다");
            }

            //players.Clear();

            // 키값으로 정렬을 해줌
            // 균형 이진 트리 기반으로 만들어짐
            SortedDictionary<int, int> a = new SortedDictionary<int, int>();

            a.Add(1, 211);
            a.Add(4, 42);
            a.Add(3, 13);
            a.Add(13, 13);
            a.Add(5, 13);

            foreach(var aa in a)
            {
                Console.WriteLine(aa.Key);
            }
        }
    }
}
