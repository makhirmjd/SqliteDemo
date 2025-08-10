using SqliteDemo.ViewModels;

namespace SqliteDemo.Views;

public partial class MainPageView : ContentPage
{
    private readonly MainPageViewModel mainPageViewModel;

    public MainPageView(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
        this.mainPageViewModel = mainPageViewModel;
        BindingContext = mainPageViewModel;
    }
}