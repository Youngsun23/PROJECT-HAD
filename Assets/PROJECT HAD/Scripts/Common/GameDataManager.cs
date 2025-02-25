namespace HAD
{
    public partial class GameDataManager : SingletonBase<GameDataManager>
    {
        // 예시
        public CharacterSampleData characterSampleData = new CharacterSampleData();

        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }
    }
}
