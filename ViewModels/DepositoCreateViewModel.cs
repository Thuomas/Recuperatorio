using SegundoParcial.Models;
namespace SegundoParcial.ViewModels;

public class DepositoCreateViewModel
{

    public int Id { get; set; }

    public string Nombre { get; set; }

    public virtual List<int> AreaIds { get; set; }
}