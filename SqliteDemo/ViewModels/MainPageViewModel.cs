using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqliteDemo.Models;
using SqliteDemo.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SqliteDemo.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private List<Customer> customers = [];
    [ObservableProperty]
    private Customer? currentCustomer;
    private readonly BaseRepository<Customer> customerRepository;

    public MainPageViewModel(BaseRepository<Customer> customerRepository)
    {
        this.customerRepository = customerRepository;
        _ = Refresh();
        GenerateNewCustomer();
    }

    [RelayCommand]
    public async Task AddOrUpdate()
    {
        if (CurrentCustomer is not null)
        {
            if (CurrentCustomer.Id == 0)
            {
                await customerRepository.AddAsync(CurrentCustomer);
                Debug.WriteLine(customerRepository.LastOperationStatus.StatusMessage);
                if (customerRepository.LastOperationStatus.IsSuccess)
                {
                    await Refresh();
                    GenerateNewCustomer();
                }
            }
            else
            {
                await customerRepository.UpdateAsync(CurrentCustomer);
                Debug.WriteLine(customerRepository.LastOperationStatus.StatusMessage);
                if (customerRepository.LastOperationStatus.IsSuccess)
                {
                    await Refresh();
                    GenerateNewCustomer();
                }
            }
        }
    }

    [RelayCommand]
    public async Task Delete()
    {
        if (CurrentCustomer is not null && CurrentCustomer.Id > 0)
        {
            await customerRepository.DeleteAsync(CurrentCustomer);
            Debug.WriteLine(customerRepository.LastOperationStatus.StatusMessage);
            if (customerRepository.LastOperationStatus.IsSuccess)
            {
                await Refresh();
                GenerateNewCustomer();
            }
        }
    }

    private void GenerateNewCustomer()
    {
        CurrentCustomer = new Faker<Customer>()
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Address, f => f.Person.Address.Street)
            .Generate();
    }

    private async Task Refresh() => Customers = await customerRepository.GetAllAsync();
}
