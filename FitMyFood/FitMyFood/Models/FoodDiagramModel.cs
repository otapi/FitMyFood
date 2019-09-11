using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{
    public class FoodDiagramModel
    {
        public enum Types
        {
            Energy,
            Fat,
            Carbo,
            Protein
        }
        public double Actual { get; set; } = 0;
        public double Maximum { get; set; } = 0;
        public double WholeDay { get; set; } = 0;
        public Types _Type;
        public Types Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                switch (value)
                {
                    case Types.Energy:
                        ActualColor = Xamarin.Forms.Color.DimGray;
                        MaximumColor = Xamarin.Forms.Color.Gray;
                        WholeDayColor = Xamarin.Forms.Color.Black;
                        break;
                }
            }
        }
        public Xamarin.Forms.Color ActualColor { get; set; }
        public Xamarin.Forms.Color MaximumColor { get; set; }
        public Xamarin.Forms.Color WholeDayColor { get; set; }

        public FoodDiagramModel()
        {
            Type = Types.Energy;
        }
    }
}
