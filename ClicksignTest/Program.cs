namespace ClicksignTest
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            Get();
        }

        private static void Get()
        {
            var clicksign = new Clicksign.Clicksign();

            var document = clicksign.Get("A5E6-E2F6-47E9-23D2");
        }
    }
}
