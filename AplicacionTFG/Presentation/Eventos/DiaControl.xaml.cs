using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AplicacionTFG.Presentation.Eventos;
public sealed partial class DiaControl : UserControl
{
    public DiaControl()
    {
        this.InitializeComponent();
    }

    public DiaControl(int dia, List<Evento> eventos) : this()
    {
        this.DiaCampo.Text = dia > 0 ? dia.ToString() : "";
        EventosLista.ItemsSource = eventos;
    }
    public int Dia
    {
        get => int.Parse(DiaCampo.Text);
        set => DiaCampo.Text = value > 0 ? value.ToString() : "";
    }
    public List<Evento> Eventos
    {
        get => (List<Evento>)EventosLista.ItemsSource;
        set => EventosLista.ItemsSource = value;
    }

    private void UserControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
        Console.WriteLine(this.DataContext.GetType());
        Console.WriteLine(Dia);
    }
}
