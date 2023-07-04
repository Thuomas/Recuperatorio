using System.ComponentModel.DataAnnotations;
using SegundoParcial.Models;

namespace SegundoParcial.ViewModels;
public class EquipoEditViewModel
{
    public int Id { get; set; }

    public string NumSerie { get; set; }

    public DateOnly FechaProd { get; set; }

    public DateOnly FechaVenta { get; set; }

    public string? Comentario { get; set; }
    [Display(Name="Deposito")]
    public int DepositoId { get; set; }

}