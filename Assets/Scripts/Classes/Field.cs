namespace Classes
{
    public class Field
    {
        public int Type { get; set; }
        public int VerticalCoord { get; set; }
        public int HorizontalCoord { get; set; }
        public bool FlagIsActive { get; set; }

        public Field(int fieldType, int vert, int hor)
        {
            Type = fieldType;
            VerticalCoord = vert;
            HorizontalCoord = hor;
        }
    }
}