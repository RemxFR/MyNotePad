namespace MyNotePad.Services
{
    // Une méthode d'Extensions est toujorus public ET static.
    public static class Extensions_Methods
    {
        //<T> s'applique à tout type de liste.
        public static void Replace<T>(this List<T> list, T oldItem, T newItem)
        {
            var oldItemIndex = list.IndexOf(oldItem);
            list[oldItemIndex] = newItem;
        }
    }
}