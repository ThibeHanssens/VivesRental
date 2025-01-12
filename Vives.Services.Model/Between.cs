namespace Vives.Services.Model
{
    //Can represent a range where a struct value must fall between

    public class Between<T>
        where T:struct
    {
        public T? From { get; set; }
        public bool IncludeFrom { get; set; } = true;
        public T? To { get; set; }
        public bool IncludeTo { get; set; } = false;
    }
}
