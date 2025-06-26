using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace Examen_Mvvm.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string producto1 = string.Empty;

    [ObservableProperty]
    private string producto2 = string.Empty;

    [ObservableProperty]
    private string producto3 = string.Empty;

    [ObservableProperty]
    private string subtotal = "0.00";

    [ObservableProperty]
    private string descuento = "0.00";

    [ObservableProperty]
    private string totalAPagar = "0.00";

    [RelayCommand]
    private void Calcular()
    {
        try
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(Producto1) ||
                string.IsNullOrWhiteSpace(Producto2) ||
                string.IsNullOrWhiteSpace(Producto3))
            {
                Application.Current?.MainPage?.DisplayAlert("Error",
                    "Todos los campos de productos deben tener un valor", "OK");
                return;
            }

            // Convertir los valores a decimal
            if (!decimal.TryParse(Producto1, out decimal valor1) ||
                !decimal.TryParse(Producto2, out decimal valor2) ||
                !decimal.TryParse(Producto3, out decimal valor3))
            {
                Application.Current?.MainPage?.DisplayAlert("Error",
                    "Los valores deben ser números válidos", "OK");
                return;
            }

            // Validar que los valores sean positivos
            if (valor1 < 0 || valor2 < 0 || valor3 < 0)
            {
                Application.Current?.MainPage?.DisplayAlert("Error",
                    "Los valores deben ser positivos", "OK");
                return;
            }

            // Calcular subtotal
            decimal subtotalCalculado = valor1 + valor2 + valor3;
            Subtotal = subtotalCalculado.ToString("F2");

            // Calcular descuento según los rangos
            decimal porcentajeDescuento = 0;
            if (subtotalCalculado >= 1000 && subtotalCalculado <= 4999.99m)
            {
                porcentajeDescuento = 0.10m; // 10%
            }
            else if (subtotalCalculado >= 5000 && subtotalCalculado <= 9999.99m)
            {
                porcentajeDescuento = 0.20m; // 20%
            }
            else if (subtotalCalculado >= 10000)
            {
                porcentajeDescuento = 0.30m; // 30%
            }

            decimal montoDescuento = subtotalCalculado * porcentajeDescuento;
            Descuento = montoDescuento.ToString("F2");

            // Calcular total a pagar
            decimal total = subtotalCalculado - montoDescuento;
            TotalAPagar = total.ToString("F2");
        }
        catch (Exception ex)
        {
            Application.Current?.MainPage?.DisplayAlert("Error",
                $"Ocurrió un error: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private void Limpiar()
    {
        try
        {
            Producto1 = string.Empty;
            Producto2 = string.Empty;
            Producto3 = string.Empty;
            Subtotal = "0.00";
            Descuento = "0.00";
            TotalAPagar = "0.00";
        }
        catch (Exception ex)
        {
            Application.Current?.MainPage?.DisplayAlert("Error",
                $"Error al limpiar campos: {ex.Message}", "OK");
        }
    }
}