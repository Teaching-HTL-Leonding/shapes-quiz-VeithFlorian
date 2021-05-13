using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shapes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<IShape> Shapes { get; set; } = new()
        {
            new Circle() { Sides = new() { new() {Name = "Radius r"} } },
            new Rectangle() { Sides = new() { new() { Name = "Seite a" }, new() { Name = "Seite b" } } },
            new Triangle() { Sides = new() { new() { Name = "Grundseite a" }, new() { Name = "Höhe h" } } },
        };

        public MainWindow()
        {
            CurrentShape = Shapes[0];
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public ObservableCollection<IShape> ShapeList { get; } = new();
        
        public double TotalArea => ShapeList.Sum(s => s.Area);

        private IShape CurrentShapeValue;
        public IShape CurrentShape
        {
            get => CurrentShapeValue;
            set
            {
                CurrentShapeValue = value;
                PropertyChanged?.Invoke(this, new(nameof(CurrentShape)));
            }
        }

        private void OnAddShape(object sender, RoutedEventArgs e)
        {
            var clone = (IShape) Activator.CreateInstance(CurrentShape.GetType());
            if (clone != null)
            {
                CurrentShape.Sides.ForEach(s => clone.Sides.Add(new Side() {Name = s.Name, Length = s.Length}));

                ShapeList.Add(clone);
                
                PropertyChanged?.Invoke(this, new(nameof(TotalArea)));
            }
        }
    }

    public interface IShape
    {
        public string Name { get; }
        public List<Side> Sides { get; set; }
        public double Area { get; }
    }

    public class Circle: IShape
    {
        public string Name => "Kreis";
        public List<Side> Sides { get; set; } = new();

        public double Area => Math.Pow(Sides[0].Length,2) * Math.PI;
    }
    
    public class Rectangle: IShape
    {
        public string Name => "Rechteck";
        public List<Side> Sides { get; set; } = new();        
        public double Area => Sides[0].Length * Sides[1].Length;

    }
    
    public class Triangle: IShape
    {
        public string Name => "Dreieck";
        public List<Side> Sides { get; set; } = new();        
        public double Area => Sides[0].Length * Sides[1].Length / 2;

    }

    public class Side
    {
        public string Name { get; set; }

        public double Length { get; set; }
    }
}
