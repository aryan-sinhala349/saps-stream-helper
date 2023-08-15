namespace SAPS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            if (window == null)
                return null;

            window.Title = "S@PS Stream Helper";
            return window;
        }
    }
}