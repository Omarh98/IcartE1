using System.ComponentModel.DataAnnotations;

namespace IcartE1.Models
{
    public class ForecastingFilterViewModel
    {
        public CategoryEnum Category { get; set; }
        public int Days { get; set; }


        public enum CategoryEnum
        {
            [Display(Name = "Automotive")]
            automotive,
            [Display(Name = "Personal Care")]
            personalcare,
            [Display(Name = "Hardware")]
            hardware,
            [Display(Name = "Seafood")]
            seafood,
            [Display(Name = "Bakery")]
            bakery,
            [Display(Name = "Cleaning")]
            cleaning,
            [Display(Name = "Frozen Food")]
            frozenfood
        }
    }
}
