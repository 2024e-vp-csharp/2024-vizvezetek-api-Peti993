namespace Vizvezetek.API.DTos
{
    public class MunkalapDto
    {
        public int Id { get; set; }
        public DateTime Beadas_datum { get; set; }
        public DateTime Javitas_datum { get; set; }
        public string Helyszin { get; set; } = string.Empty;
        public string Szerelo { get; set; } = string.Empty;
        public int Munkaora { get; set; }
        public int Anyagar { get; set; }
    }
}
