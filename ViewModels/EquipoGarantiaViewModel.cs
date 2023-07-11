using System.ComponentModel.DataAnnotations;
using SegundoParcial.Models;

namespace SegundoParcial.ViewModels;
public class EquipoGarantiaViewModel
{
    public int Id { get; set; }

    public string NumSerie { get; set; }

    public DateOnly FechaProd { get; set; }

    public DateOnly FechaVenta { get; set; }

    public bool EstaEnGarantia { get; set; }

}