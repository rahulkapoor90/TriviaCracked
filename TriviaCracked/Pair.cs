namespace TriviaCracked {
    public class Pair <T, U> {
        public Pair() {
        }

        public Pair(T first, U second) {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    }
    public static class Pair {
        public static Pair<T1, T2> New<T1, T2>(T1 first, T2 second) {
            var pair = new Pair<T1, T2>(first, second);
            return pair;
        }
    }
    
}
