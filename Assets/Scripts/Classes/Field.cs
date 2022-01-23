namespace Classes
{
    public class Field
    {
        public int Type { get; set; }
        public bool FlagIsActive { get; set; }

        public Field(int fieldType)
        {
            Type = fieldType;
        }
    }
}