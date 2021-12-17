using NameYourTame.Main;

namespace NameYourTame.Utils
{
    class NameReciever : TextReceiver
    {
        private Tameable instance;

        private string name;

        public NameReciever(ref Tameable instance, string name)
        {
            this.instance = instance;
            this.name = name;
        }

        public string GetText() => this.name;

        public void SetText(string text)
        {
            this.name = text;
            ChangeName.SetName(ref instance, ref text);
        }
    }
}
