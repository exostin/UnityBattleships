namespace Classes
{
    public class Field
    {
        public Field(int fieldType)
        {
            Type = fieldType;
        }

        public int Type { get; set; }
        public bool FlagIsActive { get; set; }
    }
}