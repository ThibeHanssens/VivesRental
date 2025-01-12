namespace Vives.Services.Model
{
    public class Paging
    {
        public int Offset { get; set; } //How many will I skip?
        public int Limit { get; set; } //How many will I take?
    }
}
