using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTFG.Presentation;
public class ViewModelBase: ObservableObject
{
    protected string _mensajeError = "Las contraseñas no coinciden";
    /// <summary>
    /// Valida un modelo utilizando Data Annotations.
    /// </summary>
    /// <param name="modelo"></param>
    /// <returns> True si los campos son válidos
    /// False si los campos no son válidos
    /// </returns>
    public bool ValidarModelo(object modelo)
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        var context = new ValidationContext(modelo);
        bool isValid = Validator.TryValidateObject(modelo, context, validationResults, true);

        // Limpiar errores previos
        _mensajeError = validationResults.Count > 0 ? validationResults[0].ErrorMessage : string.Empty;

        // Agregar nuevos errores si los hay
        //foreach (var validationResult in validationResults)
        //{
        //    Errores.Add(validationResult.ErrorMessage);
        //}

        return isValid;
    }
}
