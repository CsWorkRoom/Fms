namespace Easyman.Dto
{
    public class Rolession
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public static Rolession For(string code, string name = "")
        {
            return new Rolession { Code = code, Name = name };
        }
    }
}
