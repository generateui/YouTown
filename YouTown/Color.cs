namespace YouTown
{
    /// <summary>
    /// Color used for resources and hexagons
    /// </summary>
    /// Wraps a string to ensure future extensibility and type safety
    /// Tiny type implementation pattern
    public class Color
    {
        public static readonly Color Red = new Color("color");
        public static readonly Color DarkYellow = new Color("darkyellow");
        public static readonly Color Purple = new Color("purple");
        public static readonly Color LightGreen = new Color("lightgreen");
        public static readonly Color DarkGreen = new Color("darkgreen");
        public static readonly Color LightYellow = new Color("lightyellow");
        public static readonly Color Blue = new Color("blue");

        private readonly string _color;
        private Color(string color)
        {
            _color = color;
        }

        protected bool Equals(Color other)
        {
            return string.Equals(_color, other._color);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Color) obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _color?.GetHashCode() ?? 0;
        }
    }
}
