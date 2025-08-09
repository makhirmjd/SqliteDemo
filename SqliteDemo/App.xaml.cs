using SqliteDemo.Data;

namespace SqliteDemo
{
    public partial class App : Application
    {
        public static ApplicationDbContext DbContext { get; private set; } = default!;
        public App(ApplicationDbContext dbContext)
        {
            InitializeComponent();
            DbContext = dbContext;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}