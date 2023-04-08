
namespace XboxAuthNet.Game.Test
{
    public record MockObject
    {
        public string? Name { get; init; }
        public int Age { get; init; }
        public char Type { get; init; }
        public bool IsObject { get; init; }
        public InnerMockObject? InnerData1 { get; init; }
        public InnerMockObject? InnerData2 { get; init; }
    }

    public record InnerMockObject(string text, int number);
}